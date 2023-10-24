# Client
import socket
import json

TCP_IP = '127.0.0.1'
TCP_PORT = 7777

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((TCP_IP, TCP_PORT))
print('sock init')
sock.send('Hi'.encode('utf-8'))
sock.close()

def send_image_or_text(data, data_type):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((TCP_IP, TCP_PORT))

    if data_type not in ["image", "text"]:
        print("Unsupported data type")
        return

    data_dict = {'type': data_type, 'data': data}
    json_data = json.dumps(data_dict)
    print(json_data)

    sock.sendall(json_data.encode('utf-8'))
    print(f'Sent {data_type} data')
    sock.close()

# 傳圖片
image_path = "image.jpg"
with open(image_path, "rb") as image_file:
    img_data = image_file.read()
    send_image_or_text(list(img_data), 'image')
    print(list(img_data))
# 傳文字
text_data = "Hello, this is some text."
send_image_or_text(text_data, 'text')
