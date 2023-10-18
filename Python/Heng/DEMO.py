import cv2
import pickle
import numpy as np

with open('obj_data/PanoramaYoloSegmentationSA/panorama.pkl', 'rb') as file:
    obj_data = pickle.load(file)

mask_arr = np.array([obj['mask'] for obj in obj_data])
mask = None
def show_mask(event,x,y,flags,userdata):
    global mask

    cls = np.argmax(mask_arr[:, y, x], axis=0)
    if mask_arr[cls, y, x] == 0:
        mask = None
        return

    mask = np.repeat(mask_arr[cls][..., None], 3, axis=-1).astype(np.float32) * np.array([30 / 255, 144 / 255, 255 / 255])
    mask = np.clip(mask * 255, 0, 255).astype(np.uint8)


while True:
    panorama_img = cv2.imread('panorama.png')
    # panorama_img = cv2.resize(panorama_img, (1024, 512))

    if mask is not None:
        panorama_img = cv2.addWeighted(panorama_img, 1, mask, 0.2, 5)

    cv2.imshow('panorama', panorama_img)
    cv2.setMouseCallback('panorama', show_mask)
    cv2.waitKey(1)