import cv2
import pickle
import numpy as np

with open('obj_data/PanoramaYoloSegmentationSA/panorama.pkl', 'rb') as file:
    obj_data = pickle.load(file)

mask_arr = np.array([obj['mask'] for obj in obj_data])

all_mask = np.zeros((45, 2048, 4096, 3))
for i in range(mask_arr.shape[0]):
    all_mask[i] = np.repeat(mask_arr[i][..., None], 3, axis=-1).astype(np.float32) * np.random.random(3)

all_mask = np.sum(all_mask, axis=0)
all_mask = np.clip(all_mask * 255, 0, 255).astype(np.uint8)

panorama_img = cv2.imread('panorama.png')
panorama_img = cv2.addWeighted(panorama_img, 0.6, all_mask, 0.4, 5)
cv2.imwrite('mask.png', panorama_img)
mask = None