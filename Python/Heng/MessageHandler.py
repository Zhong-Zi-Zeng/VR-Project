from pathlib import Path
import os
import cv2


class reporter:
    def __init__(self, train_api) -> None:
        self.trans_api = train_api

    def __enter__(self):
        pass

    def __exit__(self, type, value, _):
        self.trans_api.send_data_to_unity(text="over")        


class Search:
    """
         將material資料夾下的所有照片和名稱都傳送給unity
         Send:
            1. panorama images
            2. panorama image's name
            3. progress
    """
    @staticmethod
    def handle(main, data, *args, **kwargs):
        with reporter(main.trans_api):
            for image_name in os.listdir('./material'):
                image = main.trans_api.encode_image(
                    cv2.imread(os.path.join('./material', image_name)),
                    image_format=Path(image_name).suffix
                )
                main.trans_api.send_data_to_unity(panorama=image, text=image_name)
        
class Generate:
    """
         將使用者指定的照片去生成mask後回傳給unity
         Send:
            1. panorama image with mask
            2. id_map
            3. index_map
            4. progress
    """
    @staticmethod
    def handle(main, data, *args, **kwargs):
        with reporter(main.trans_api):
            panorama_img_name_ext = data['imageName']
            panorama_with_mask, id_map, index_map = main.check_obj_data(panorama_img_name_ext)

            main.trans_api.send_data_to_unity(id_map=id_map)
            main.trans_api.send_data_to_unity(index_map=index_map)
            for mask_img in panorama_with_mask:
                main.trans_api.send_data_to_unity(panorama_with_mask=mask_img)
