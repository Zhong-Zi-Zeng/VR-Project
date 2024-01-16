import os


dir = os.listdir(r"C:\Users\鐘子恒\Desktop\Side-Project\VR-Project\Python\Heng\map\PanoramaYoloSegmentation\panorama")
dir = [f for f in dir if f != 'id_map.png' and f != 'index_map.png']
dir.sort(key=lambda name: int(name.split(".")[0]))
print(dir)