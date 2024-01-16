from __future__ import annotations
import logging
import time
import matplotlib.pyplot as plt
import numpy as np
import cv2
import os
import yaml
import json
import matplotlib
from typing import Optional
from pythonConnect import pythonConnect
from Yolov8 import Yolov8
from strategies import *
from MessageHandler import *
from SegmentAnything import SegmentAnything
from tools.PanoramaCubemapConverter import PanoramaCubemapConverter

logging.basicConfig(level=logging.INFO)


class Main:
    def __init__(self,
                 yolov8_obj_model_type: str = 'yolov8x',
                 yolov8_seg_model_type: str = 'yolov8x',
                 sa_model_type: str = 'vit_h',
                 sa_mode: str = 'box',
                 threshold: float = 0.8,
                 device: str = 'cuda'):
        """
            Args:
                yolov8_obj_model_type: Yolo-V8的模型，可選擇 [yolov8s、yolov8m、yolov8x]
                yolov8_seg_model_type: Yolo-V8的segmentation模型，可選擇 [yolov8s、yolov8m、yolov8x]
                sa_model_type: Segment-Anything的模型，可選擇 [vit_h、vit_l、vit_b]
                sa_mode: Segment-Anything的模式，可選擇 [point, box, both]
                threshold: 用來合併mask
                device: 運行裝置，可選擇 [cpu、cuda]
        """

        assert yolov8_obj_model_type in ['yolov8s', 'yolov8m', 'yolov8x'], \
            '\"yolov8_obj_model_type\" should uses yolov8s、yolov8m or yolov8x.'
        assert yolov8_seg_model_type in ['yolov8s', 'yolov8m', 'yolov8x', None], \
            '\"yolov8_seg_model_type\" should uses yolov8s、yolov8m 、 yolov8x or None.'
        assert sa_model_type in ['vit_h', 'vit_l', 'vit_b'], '\"sa_mode\" should uses vit_h、vit_l or vit_b.'
        assert sa_mode in ['point', 'box', 'both'], '\"sa_mode\" should uses point、box or both.'

        self.yolov8 = Yolov8(obj_model_type=yolov8_obj_model_type,
                             seg_model_type=yolov8_seg_model_type,
                             device=device)

        self.sa = SegmentAnything(model_type=sa_model_type, device=device)
        self.sa_mode = sa_mode
        self.method = None

        self.converter = PanoramaCubemapConverter()
        self.threshold = threshold
        self.method_name = [
            'CubemapYoloSegmentation',
            'CubemapYoloSA',
            'PanoramaYoloSegmentation',
            'PanoramaCubemapYoloSegmentation',
            'PanoramaYoloSegmentationSA'
        ]

        with open('./yolov8_label.yaml', 'r') as file:
            self.config = yaml.load(file, Loader=yaml.CLoader)

        logging.info('Now use {} for inference.'.format(device))

        self.trans_api = pythonConnect()
        self.trans_api.listener(self.callback)

    def callback(self, data, *args, **kwargs):
        """
            針對收到的訊息執行對應的工作
        """
        if data['task'] == 'Search':
            Search.handle(self, data, args, kwargs)
        elif data['task'] == 'Generate':
            Generate.handle(self, data, args, kwargs)

    def check_obj_data(self, panorama_img_name_ext: str, method: int = 2) -> List[List[bytes], bytes, bytes]:
        """
            在收到指定的圖片名稱後，先去資料夾下查詢是否已經存在，否則則執行generate_mask後再返回數據

            Args:

                method: mask執行方法，目前可選擇4 [0, 1, 2, 3]
                    0: 直接將每一個Cubemap 送入Yolo-V8的Segmentation (尚未完成合併斷掉的mask)
                    1: 直接將每一個Cubemap 送入Yolo-V8的Object detection後再送入SA (尚未完成合併斷掉的mask)
                    2: 直接將Panorama送入Yolo-V8的Segmentation (已完成)
                    3: 將Cubemap和Panoramac都送入Yolo-V8的Segmentation，再去做後處理 (尚未完成合併斷掉的mask)
                    4: 將Cubemap和Panoramac都送入Yolo-V8的Segmentation後，再利用SA去refine (已完成，但是最後的mask可能會有重複的狀況)
                panorama_img_name_ext: 指定的panorama圖片名稱(包含副檔名)
            Return:
                [
                    panorama_with_mask, id_map, label_map
                ]
        """
        self.method = method
        panorama_img_name = os.path.splitext(panorama_img_name_ext)[0].lower()  # 不包含副檔名的
        obj_data_directory = os.path.join('map', self.method_name[method], panorama_img_name)

        # 檢查該圖片的mask是否存在在id_map資料夾下
        if not os.path.isdir(obj_data_directory):
            panorama_img = cv2.imread(os.path.join('material', panorama_img_name_ext))
            self._generate_mask(panorama_img=panorama_img, panorama_img_name=panorama_img_name)

        # 讀取id_map
        id_map = self.trans_api.encode_image(
            cv2.imread(os.path.join('map', self.method_name[method], panorama_img_name, 'id_map.png')),
            image_format='.png')

        # 讀取index_map
        index_map = self.trans_api.encode_image(
            cv2.imread(os.path.join('map', self.method_name[method], panorama_img_name, 'index_map.png')),
            image_format='.png')

        # 讀取所有masked圖片
        file_path_list = [mask_name for mask_name in os.listdir(obj_data_directory) if
                          mask_name != 'id_map.png' and mask_name != 'index_map.png']
        file_path_list.sort(key=lambda name: int(name.split(".")[0]))

        panorama_with_mask = [
            self.trans_api.encode_image(cv2.imread(os.path.join(obj_data_directory, mask_name)), image_format='.png')
            for mask_name in file_path_list]

        return [panorama_with_mask, id_map, index_map]

    def _generate_mask(self,
                       panorama_img: np.ndarray[np.uint8],
                       panorama_img_name: str):

        if self.method == 0:
            mask_generator = CubemapYoloSegmentation(yolov8=self.yolov8,
                                                     config=self.config)
        elif self.method == 1:
            mask_generator = CubemapYoloSA(yolov8=self.yolov8,
                                           sa=self.sa,
                                           sa_mode=self.sa_mode,
                                           config=self.config)
        elif self.method == 2:
            mask_generator = PanoramaYoloSegmentation(yolov8=self.yolov8,
                                                      config=self.config)
        elif self.method == 3:
            mask_generator = PanoramaCubemapYoloSegmentation(yolov8=self.yolov8,
                                                             config=self.config)
        elif self.method == 4:
            mask_generator = PanoramaYoloSegmentationSA(yolov8=self.yolov8,
                                                        sa=self.sa,
                                                        sa_mode=self.sa_mode,
                                                        config=self.config)
        else:
            raise ValueError("Invalid method value")

        mask_generator.generate_mask(panorama_img, panorama_img_name)


# img = cv2.imread('./material/panorama.png')
# img = cv2.resize(img, (1024, 512))
m = Main()
# m.check_obj_data(panorama_img_name_ext="panorama.png")
# m._check_obj_data(2, panorama_img_name_ext='panorama.png')
# m.generate_mask(img, panorama_img_name='panorama')
