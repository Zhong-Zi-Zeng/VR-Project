# Client
import socket
import json
import cv2
import numpy as np

class pythonConnect:
    def __init__(self, TCP_IP='127.0.0.1', TCP_PORT_SEND=7777):
        self.TCP_IP = TCP_IP
        self.TCP_PORT_SEND = TCP_PORT_SEND

    def send_data_to_unity(self, json_path=None, image_path=None, text_data=None):

        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((self.TCP_IP, self.TCP_PORT_SEND))

        # 處理圖片
        if image_path is not None:
            image = cv2.imread(image_path)
            _, image_format = image_path.rsplit('.', 1)     # 自動偵測圖片格
            image_data = cv2.imencode(f'.{image_format}', image)[1].ravel().tolist()
        else:
            image_data = None

        # 處理json檔，裡面是list
        if json_path is not None:
            with open(json_path) as f:
                list_data = json.load(f)
        else:
            list_data = None

        dict_data = {
            'list': np.array(list_data).reshape(-1).tolist(),   # 檢查unity收到的資料 2048 * 4096 = 8388608
            'H': len(list_data) if list_data is not None else None,
            'W': len(list_data[0]) if list_data is not None else None,
            'image': image_data,
            'text': text_data,
        }
        json_data = json.dumps(dict_data)
        sock.sendall(bytes(json_data, encoding='utf-8'))
        sock.close()
        print('Data sent to Unity')

    def receive_data_from_unity(self):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((self.TCP_IP, self.TCP_PORT_SEND))

        while True:
            data = sock.recv(1024)  # 可更改buffer大小
            recv_dict = json.loads(data.decode('utf-8'))
            print('Data received from Unity:', recv_dict['parameter'])  # 接收參數

        sock.close()


# 傳送格式
json_path = 'id_map.json'
image_path = 'panorama.png'
text_data = "Hello, this is some text."

# 執行
python_connector = pythonConnect()
python_connector.send_data_to_unity(json_path=json_path, image_path=None, text_data=None)
python_connector.receive_data_from_unity()
