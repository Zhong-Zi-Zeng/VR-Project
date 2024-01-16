import cv2
cap = cv2.VideoCapture(0)
while True:
    a, b = cap.read()
    print(b.shape)

    cv2.imshow("", b)
    cv2.waitKey(1)