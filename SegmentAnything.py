from __future__ import annotations
from segment_anything import SamPredictor, sam_model_registry
from typing import Union
import numpy as np


class SegmentAnything:
    def __init__(self, model_type: str = 'vit_h', device: str = 'cuda'):

        _model_type2path = {
            'vit_h': './weight/sam_vit_h.pth',
            'vit_l': './weight/sam_vit_l.pth',
            'vit_b': './weight/sam_vit_b.pth'
        }

        self._model = sam_model_registry[model_type](checkpoint=_model_type2path[model_type])
        self._model.to(device)
        self._predictor = SamPredictor(self._model)

    def show_mask(self, mask: np.ndarray, ax, random_color=True):
        """
            可視化mask
        """
        if random_color:
            color = np.concatenate([np.random.random(3), np.array([0.6])], axis=0)
        else:
            color = np.array([30 / 255, 144 / 255, 255 / 255, 0.6])
        h, w = mask.shape[-2:]

        mask_image = mask.reshape(h, w, 1) * color.reshape(1, 1, -1)
        ax.imshow(mask_image)

    def predict(self, img: np.ndarray,
                sa_mode: str,
                bbox: Union[np.ndarray, None] = None,
                multimask_output: bool = False,
                image_format='BGR') -> np.ndarray[np.bool]:
        """
            對傳入的image進行SA，可以選擇只用點或是box來生成

            Args:
                img: 圖片
                sa_mode: Segment-Anything的模式，可選擇 [point, box, both]
                bbox: 指定的box範圍，格式為 (x1, y1, x2, y2) 須為長度為4的array
                multimask_output:
                    是否要輸出多個mask，預設是True，則代表會輸出3張不同的mask
                    若設定False，則只會輸出一張mask
                image_format: 輸入影像的格式
            Returns:
                masks: 如果multimask_output設置為True，則返回(3, H, W)的mask，
                       如果multimask_output設置為False，則返回(1, H, W)的mask，
                       被mask的地方為True
        """
        self._predictor.set_image(img, image_format=image_format)

        if sa_mode == 'point':
            point_coords = np.array([[(bbox[0] + bbox[2]) / 2,
                                      (bbox[1] + bbox[3]) / 2]])
            point_labels = np.array([1])
            masks, scores, logits = self._predictor.predict(point_coords=point_coords,
                                                            point_labels=point_labels,
                                                            multimask_output=multimask_output,
                                                            )
        elif sa_mode == 'box':
            masks, scores, logits = self._predictor.predict(box=bbox,
                                                            multimask_output=multimask_output,
                                                            )
        elif sa_mode == 'both':
            point_coords = np.array([[(bbox[0] + bbox[2]) / 2,
                                      (bbox[1] + bbox[3]) / 2]])
            point_labels = np.array([1])
            masks, scores, logits = self._predictor.predict(point_coords=point_coords,
                                                            point_labels=point_labels,
                                                            box=bbox,
                                                            multimask_output=multimask_output,
                                                            )
        else:
            raise ValueError("Invalid sa_mode value")

        return masks[0]
