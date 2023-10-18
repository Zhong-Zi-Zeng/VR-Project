from __future__ import annotations
from typing import Optional, Dict, List
from abc import ABC, abstractmethod
from tools.PanoramaCubemapConverter import PanoramaCubemapConverter
from tqdm import tqdm
import numpy as np
import matplotlib.pyplot as plt
import os
import copy
import pickle


class MaskGenerationStrategy(ABC):
    @abstractmethod
    def generate_mask(self,
                      panorama_img: np.ndarray,
                      panorama_img_name: str) -> List[dict]:
        pass

    @staticmethod
    def _create_cubemap_with_mask(cubemap_mask: np.ndarray[bool], cube_w: int, dir: str) -> Dict[
        np.ndarray[bool]]:
        """
            為了將mask轉換到panorama，先將每一個mask放入到空白的cubemap中
        """
        direction = ['F', 'R', 'B', 'L', 'U', 'D']
        cubemap_temp = {d: np.zeros((cube_w, cube_w, 3), dtype=np.uint8) for d in direction}
        cubemap_temp[dir] = np.repeat(cubemap_mask[..., None], 3, axis=-1).astype(np.uint8) * 255

        return cubemap_temp

    @staticmethod
    def _c2p_image(cubemap_mask_dict: dict, hp: int, wp: int):
        return PanoramaCubemapConverter.c2p_image(cubemap_mask_dict, hp, wp)

    @staticmethod
    def _p2c_image(panorama_img: np.ndarray[np.uint8], cube_w: int, cube_format='dict') -> Dict[
        str, np.ndarray[np.uint8]]:
        return PanoramaCubemapConverter.p2c_image(panorama_img, cube_w, cube_format=cube_format)

    @staticmethod
    def _show_panorama_mask(panorama_mask: np.ndarray[bool],
                            panorama_img: np.ndarray[np.uint8],
                            random_color: bool = True):
        """
            可視化panorama mask的結果
        """
        H, W = panorama_img.shape[:2]

        copy_panorama = copy.deepcopy(panorama_img)
        plt.imshow(copy_panorama[..., ::-1])

        if random_color:
            color = np.concatenate([np.random.random(3), np.array([0.6])], axis=0)
        else:
            color = np.array([30 / 255, 144 / 255, 255 / 255, 0.6])

        mask_image = panorama_mask.reshape((H, W, 1)).astype(bool) * color.reshape((1, 1, -1))
        plt.gca().imshow(mask_image)
        plt.show()

    @staticmethod
    def _show_cubemap_bbox(cubemap_img: np.ndarray[np.uint8],
                           bbox_list: List[np.ndarray[float]]):
        plt.imshow(cubemap_img[..., ::-1])
        ax = plt.gca()

        for bbox in bbox_list:
            x1 = int(bbox[0])
            y1 = int(bbox[1])
            x2 = int(bbox[2])
            y2 = int(bbox[3])
            color = list(np.random.random(size=(3,)))

            ax.add_patch(plt.Rectangle((x1, y1), x2 - x1, y2 - y1,
                                       fill=False, color=color, linewidth=3))

        plt.show()

    @staticmethod
    def _show_cubemap_mask(cubemap_mask: np.ndarray[bool],
                           cubemap_img: np.ndarray[np.uint8],
                           random_color: bool = True):
        """
            可視化cubemap mask的結果
        """
        H, W = cubemap_mask.shape[:2]
        copy_cubemap = copy.deepcopy(cubemap_img)
        plt.imshow(copy_cubemap[..., ::-1])

        if random_color:
            color = np.concatenate([np.random.random(3), np.array([0.6])], axis=0)
        else:
            color = np.array([30 / 255, 144 / 255, 255 / 255, 0.6])

        mask_image = cubemap_mask.reshape((H, W, 1)).astype(bool) * color.reshape((1, 1, -1))
        plt.gca().imshow(mask_image)
        plt.show()

    @staticmethod
    def _store_mask(obj_data: List[dict], dir_path: str, panorama_img_name: str):
        obj_data_directory = os.path.join('obj_data', dir_path)

        if not os.path.isdir(obj_data_directory):
            os.makedirs(os.path.join('obj_data', dir_path))

        obj_data_path = os.path.join(obj_data_directory, panorama_img_name)

        with open(obj_data_path + '.pkl', 'wb') as file:
            pickle.dump(obj_data, file)


class CubemapYoloSA(MaskGenerationStrategy):
    def __init__(self, yolov8, sa, sa_mode, config):
        self.yolov8 = yolov8
        self.sa = sa
        self.sa_mode = sa_mode
        self.config = config

    def generate_mask(self,
                      panorama_img: np.ndarray[np.uint8],
                      panorama_img_name: str) -> List[dict]:

        hp, wp = panorama_img.shape[:2]
        cube_w = wp // 4
        cubemap_img = self._p2c_image(panorama_img, cube_w=cube_w, cube_format='dict')
        obj_data = []

        for dir, img in tqdm(cubemap_img.items()):
            bboxes, classes = self.yolov8.predict_obj(img=img)

            for bbox, cls, in zip(bboxes[0], classes[0]):
                label = self.config['LABELS'][int(cls)]
                cubemap_mask = self.sa.predict(img, sa_mode=self.sa_mode, bbox=bbox)
                cubemap_mask_dict = self._create_cubemap_with_mask(cubemap_mask, cube_w, dir)
                panorama_mask = self._c2p_image(cubemap_mask_dict, hp, wp)[..., 0].astype(bool)
                obj_data.append({'dir': dir, 'label': label, 'bbox': bbox, 'mask': panorama_mask})

        self._store_mask(obj_data, dir_path='CubemapYoloSA', panorama_img_name=panorama_img_name)

        return obj_data


class CubemapYoloSegmentation(MaskGenerationStrategy):
    def __init__(self, yolov8, config: dict):
        self.yolov8 = yolov8
        self.config = config

    def generate_mask(self,
                      panorama_img: np.ndarray,
                      panorama_img_name: str) -> List[dict]:

        hp, wp = panorama_img.shape[:2]
        cube_w = wp // 4
        cubemap_img = self._p2c_image(panorama_img, cube_w=cube_w, cube_format='dict')
        obj_data = []

        for dir, img in tqdm(cubemap_img.items()):
            masks, bboxes, classes = self.yolov8.predict_seg(img=img)

            for mask, bbox, cls in zip(masks[0], bboxes[0], classes[0]):
                label = self.config['LABELS'][int(cls)]
                cubemap_mask_dict = self._create_cubemap_with_mask(mask, cube_w, dir)
                panorama_mask = self._c2p_image(cubemap_mask_dict, hp, wp)[..., 0].astype(bool)
                obj_data.append({'dir': dir, 'label': label, 'bbox': bbox, 'mask': panorama_mask})

        self._store_mask(obj_data, dir_path='CubemapYoloSegmentation', panorama_img_name=panorama_img_name)

        return obj_data


class PanoramaYoloSegmentation(MaskGenerationStrategy):
    def __init__(self, yolov8, config):
        self.yolov8 = yolov8
        self.config = config

    def generate_mask(self,
                      panorama_img: np.ndarray,
                      panorama_img_name: str) -> List[dict]:
        obj_data = []
        masks, bboxes, classes = self.yolov8.predict_seg(img=panorama_img)

        for mask, bbox, cls in zip(masks[0], bboxes[0], classes[0]):
            label = self.config['LABELS'][int(cls)]
            obj_data.append({'dir': None, 'label': label, 'bbox': bbox, 'mask': mask.astype(bool)})

        self._store_mask(obj_data, dir_path='PanoramaYoloSegmentation', panorama_img_name=panorama_img_name)

        return obj_data


class PanoramaYoloSegmentationSA(MaskGenerationStrategy):
    def __init__(self, yolov8, sa, sa_mode, config):
        self.yolov8 = yolov8
        self.sa = sa
        self.sa_mode = sa_mode
        self.config = config

    def _get_mask_fromSA(self,
                         img: np.ndarray[np.uint8],
                         bbox: np.ndarray[float]) -> np.ndarray[bool]:

        if self.sa_mode == 'point':
            mask = self.sa.predict(img=img,
                                   point_coords=np.array([[(bbox[0] + bbox[2]) / 2,
                                                           (bbox[1] + bbox[3]) / 2]]),
                                   point_labels=np.array([1])
                                   )
        elif self.sa_mode == 'box':
            mask = self.sa.predict(img=img, bbox=bbox)
        else:
            mask = self.sa.predict(img=img,
                                   point_coords=np.array([[(bbox[0] + bbox[2]) / 2,
                                                           (bbox[1] + bbox[3]) / 2]]),
                                   point_labels=np.array([1]),
                                   bbox=bbox
                                   )
        return mask

    def generate_mask(self,
                      panorama_img: np.ndarray[np.uint8],
                      panorama_img_name: str) -> List[dict]:

        obj_data_from_panorama = PanoramaYoloSegmentation(yolov8=self.yolov8,
                                                          config=self.config).generate_mask(panorama_img,
                                                                                            panorama_img_name)
        obj_data = []
        for obj in tqdm(obj_data_from_panorama):
            obj['mask'] = self.sa.predict(panorama_img, sa_mode=self.sa_mode, bbox=obj['bbox'])
            obj_data.append(obj)

        self._store_mask(obj_data, dir_path='PanoramaYoloSegmentationSA', panorama_img_name=panorama_img_name)
        return obj_data


class PanoramaCubemapYoloSegmentation(MaskGenerationStrategy):
    def __init__(self, yolov8, config):
        self.yolov8 = yolov8
        self.config = config

    def generate_mask(self,
                      panorama_img: np.ndarray,
                      panorama_img_name: str) -> List[dict]:
        # obj_data_from_cubemap = CubemapYoloSegmentation(yolov8=self.yolov8,
        #                                                 config=self.config).generate_mask(panorama_img,
        #                                                                                   panorama_img_name)
        # obj_data_from_panorama = PanoramaYoloSegmentation(yolov8=self.yolov8,
        #                                                   config=self.config).generate_mask(panorama_img,
        #                                                                                     panorama_img_name)
        with open('./obj_data/CubemapYoloSegmentation/panorama.pkl', 'rb') as file:
            obj_data_from_cubemap = pickle.load(file)

        with open('./obj_data/PanoramaYoloSegmentation/panorama.pkl', 'rb') as file:
            obj_data_from_panorama = pickle.load(file)

        obj_data = []

        return obj_data
