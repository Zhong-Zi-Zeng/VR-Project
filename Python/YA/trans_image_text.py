# Client
import socket
import json
import cv2

TCP_IP = '127.0.0.1'
TCP_PORT = 7777

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((TCP_IP, TCP_PORT))
print('sock init')
sock.send('Hi'.encode('utf-8'))
sock.close()

# 要傳的圖片
image_path = "panorama.png"
image = cv2.imread(image_path)
image_data = cv2.imencode('.png', image)[1].ravel().tolist()    # 要改為自動偵設圖片格式

# 要傳的文字
text_data = "Hello, this is some text."

# 要傳的list

while True:
    dict_data = {'image': image_data, 'text': text_data}
    json_data = json.dumps(dict_data)

    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((TCP_IP, TCP_PORT))

    sock.sendall(bytes(json_data, encoding='utf-8'))    # 只能傳 byte,string
    print('sock sent')
    sock.close()




