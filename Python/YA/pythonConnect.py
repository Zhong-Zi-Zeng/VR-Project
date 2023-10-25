import threading
import socket
import json
import cv2


# Client：傳送
def send_data():
    TCP_IP = '127.0.0.1'
    TCP_PORT = 7777

    # 要傳的圖片
    image_path = "panorama.png"
    image = cv2.imread(image_path)
    image_data = cv2.imencode('.png', image)[1].ravel().tolist()  # 要改為自動偵設圖片格式
    # [1]：cv2.imencode 傳回一個包含兩個值的元組，第一個值是一個布林值，指示編碼是否成功，而第二個值是包含編碼影像資料的字節或數組。 透過選擇索引1，提取包含編碼圖像資料的部分
    # ravel()：多維數組（圖片）轉換為一維數組

    # 要傳的文字
    text_data = "Hello, this is some text."

    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))

    while True:

        dict_data = {'image': image_data, 'text': text_data}
        json_data = json.dumps(dict_data)

        # sock_send = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # sock_send.connect((TCP_IP, TCP_PORT_SEND))

        sock_send.sendall(bytes(json_data, encoding='utf-8'))  # 只能傳 byte,string
        print('Data sent to Unity')
    sock_send.close()


# Server：接收
def receive_data():
    TCP_IP = '127.0.0.1'
    TCP_PORT = 8888

    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((TCP_IP, TCP_PORT))
    s.listen(1)

    print('Waiting for response from Unity...')
    connection, address = sock_recv.accept()

    while True:
        data = connection.recv(1024).decode('utf-8')  # 假設響應資料大小不超過 1024 字節，您可以根據實際需求更改
        # connection.close()
        if not data:
            break
        print("Received data: " + data)

    connection.close()


# 創建兩個執行緒，分別執行接收數據和傳送數據的函數
receive_thread = threading.Thread(target=receive_data)
send_thread = threading.Thread(target=send_data)

# 啟動執行緒
receive_thread.start()
send_thread.start()

# 等待執行緒完成
receive_thread.join()
send_thread.join()





