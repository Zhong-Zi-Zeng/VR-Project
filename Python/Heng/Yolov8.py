import numpy as np
from typing import Union
from ultralytics import YOLO
from torchvision import transforms

class Yolov8:
    def __init__(self,
                 obj_model_type: str = 'yolov5s',
                 seg_model_type: str = 'yolov5s',
                 device: str = 'cuda'):

        self._obj_model_type2path = {
            'yolov8s': './weight/yolov8s.pt',
            'yolov8m': './weight/yolov8m.pt',
            'yolov8x': './weight/yolov8x.pt'
        }

        self.obj_model = YOLO(self._obj_model_type2path[obj_model_type])
        self.obj_model.to(device)

        if seg_model_type is not None:
            self._seg_model_type2path = {
                'yolov8s': './weight/yolov8s-seg.pt',
                'yolov8m': './weight/yolov8m-seg.pt',
                'yolov8x': './weight/yolov8x-seg.pt'
            }

            self.seg_model = YOLO(self._seg_model_type2path[seg_model_type])
            self.seg_model.to(device)


    def predict_obj(self, img: Union[np.ndarray, list], image_format: str = 'BGR') -> tuple:
        """
            對傳入的照片進行物件檢測

            Args:
                img: 傳入的image可為單張或是多張，若是多張的話則需要用list的方式進行傳入，
                即[img_1, img_2, ...]，若為單張則要為nd.ndarray型態
                image_format: 輸入影像的格式

            Returns:
                (N為一張圖片裡的物件數量)

                bbox: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N,4)的array，
                其中包括 [x1, y1, x2, y2]

                cls: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N,)的array，
                其中包括 [cls]

                conf: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N,)的array，
                其中包括 [conf]
        """

        assert (isinstance(img, list) and all(isinstance(i, np.ndarray) for i in img)) or \
               (isinstance(img, np.ndarray) and len(
                   img.shape) != 4), 'You are trying to input multiple  images, but their type is not list.'

        if image_format == 'BGR' and isinstance(img, list):
            img = [bgr_img[..., ::-1] for bgr_img in img]
        elif image_format == 'BGR' and isinstance(img, np.ndarray):
            img = img[..., ::-1]

        if isinstance(img, list):
            H, W = img[0].shape[:2]
        elif isinstance(img, np.ndarray):
            H, W = img.shape[:2]

        results = self.obj_model.predict(img, imgsz=(H, W))

        bbox = [result.boxes.xyxy.cpu().numpy() for result in results]
        cls = [result.boxes.cls.cpu().numpy() for result in results]

        return bbox, cls

    def predict_seg(self, img: Union[np.ndarray, list], image_format: str = 'BGR') -> tuple:
        """
           對傳入的照片進行segmentation

           Args:
               img: 傳入的image可為單張或是多張，若是多張的話則需要用list的方式進行傳入，
               即[img_1, img_2, ...]，若為單張則要為nd.ndarray型態

               image_format: 輸入影像的格式

           Returns:
               (N為一張圖片裡的物件數量)
               mask: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N, H, W)的array

               cls: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N,)的array，
               其中包括 [cls]

               bbox: 假設傳入的照片數量為M，那則回傳一個長度為M的列表，每個元素為(N,4)的array，
                其中包括 [x1, y1, x2, y2]

       """
        assert (isinstance(img, list) and all(isinstance(i, np.ndarray) for i in img)) or \
               (isinstance(img, np.ndarray) and len(
                   img.shape) != 4), 'You are trying to input multiple  images, but their type is not list.'

        if isinstance(img, list):
            H, W = img[0].shape[:2]
        elif isinstance(img, np.ndarray):
            H, W = img.shape[:2]

        results = self.seg_model.predict(img, imgsz=(H, W), retina_masks=True)
        masks = [result.masks.data.cpu().numpy() for result in results]

        cls = [result.boxes.cls.cpu().numpy() for result in results]
        bbox = [result.boxes.xyxy.cpu().numpy() for result in results]

        return masks, bbox, cls
