# Client
import socket
import json
import cv2
import numpy as np
import logging
from threading import Thread
from typing import Optional


class pythonConnect:
    def __init__(self, TCP_IP='127.0.0.1'):
        self.TCP_IP = TCP_IP

        self.send_port = 7777
        self.receive_port = 6666

    @staticmethod
    def encode_image(image: np.ndarray, image_format: str):
        """
            將照片轉換成bytes
        :param image: np.ndarray
        :param image_format: 可以為'.jpg' or '.png'
        :return: 被encode的image
        """
        assert image_format in ['.jpg', '.png'], 'The image\'s format was wrong.'
        return cv2.imencode(f'.{image_format}', image)[1].ravel().tolist()

    def send_data_to_unity(self,
                           panorama_with_mask: Optional[bytes] = None,
                           panorama: Optional[bytes] = None,
                           id_map: Optional[bytes] = None,
                           index_map: Optional[bytes] = None,
                           progress: Optional[int] = None,
                           text: Optional[str] = None
                           ):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((self.TCP_IP, self.send_port))

        data = {
            'panoramaWithMask': panorama_with_mask,
            'panorama': panorama,
            'idMap': id_map,
            'indexMap:': index_map,
            'progress': progress,
            'text': text
        }

        # 傳送給unity
        json_data = json.dumps(data)
        sock.sendall(bytes(json_data, encoding='utf-8'))
        sock.close()

        logging.info('Data sent to Unity')

    def listener(self, callback, *args, **kwargs):
        assert callable(callback), 'The callback function is not callable.'
        logging.info('start listen')

        while True:
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.connect((self.TCP_IP, self.receive_port))

            data = sock.recv(1024)  # 可更改buffer大小
            recv_dict = json.loads(data.decode('utf-8'))
            callback(recv_dict, *args, **kwargs)


if __name__ == '__main__':
    # 執行
    python_connector = pythonConnect()

    # 傳送格式
    panorama = python_connector.encode_image(cv2.imread('panorama.png'), image_format='.png')
    id_map = python_connector.encode_image(cv2.imread('id_map.png'), image_format='.png')
    index_map = python_connector.encode_image(cv2.imread('index_map.png'), image_format='.png')
    text_data = "Hello, this is some text."

    Thread(target=python_connector.send_data_to_unity,
           kwargs={"id_map": id_map, 'text': text_data, 'indexMap': index_map}).start()
    # Thread(target=python_connector.receive_data_from_unity).start()
