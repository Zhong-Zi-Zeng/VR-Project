import cv2
import pickle
import numpy as np
import json
import yaml
from ultralytics import YOLO

# with open('obj_data.json', 'r') as infile:
#     obj_data = json.load(infile)
#

img = cv2.imread('material/panorama.png')
model = YOLO('weight/yolov8x-seg.pt')
model.predict(img, imgsz=(2048, 4096), save=True)

