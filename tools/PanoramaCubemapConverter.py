from __future__ import annotations
import numpy as np
import py360convert
import cv2


class PanoramaCubemapConverter:
    def __init__(self):
        self.top_matrix = self._rotation_matrix(pitch=-90, roll=0, yaw=0)
        self.down_matrix = self._rotation_matrix(pitch=90, roll=0, yaw=0)
        self.left_matrix = self._rotation_matrix(pitch=0, roll=0, yaw=90)
        self.right_matrix = self._rotation_matrix(pitch=0, roll=0, yaw=-90)
        self.front_matrix = self._rotation_matrix(pitch=0, roll=0, yaw=0)
        self.back_matrix = self._rotation_matrix(pitch=0, roll=0, yaw=180)

    @staticmethod
    def _rotation_matrix(pitch, roll, yaw):
        """
            生成旋轉矩陣
        """
        yaw = yaw * np.pi / 180.
        yaw_m = np.array([[np.cos(yaw), -np.sin(yaw), 0],
                          [np.sin(yaw), np.cos(yaw), 0],
                          [0, 0, 1]])

        pitch = pitch * np.pi / 180.  # rotate around x, angle from y+ to z+
        pitch_m = np.array([[1, 0, 0],
                            [0, np.cos(pitch), -np.sin(pitch)],
                            [0, np.sin(pitch), np.cos(pitch)]])

        roll = roll * np.pi / 180.  # rotate around y, angle from x+ to z+
        roll_m = np.array([[np.cos(roll), 0, -np.sin(roll)],
                           [0, 1, 0],
                           [np.sin(roll), 0, np.cos(roll)]])

        R = pitch_m @ roll_m @ yaw_m

        return R

    @staticmethod
    def _get_panorama_coord(P: np.ndarray, Wp: int, Hp: int) -> list[int, int]:
        """
            將給定的點轉換到Panorama座標系下
        """
        longitude = np.arctan2(P[0], P[1])
        latitude = np.arctan2(P[2], np.sqrt(P[0] ** 2 + P[1] ** 2))

        I = int((Wp * (longitude + np.pi) / (2 * np.pi)))
        J = int((Hp * (latitude + np.pi / 2) / np.pi))

        return [I, J]

    def c2p_point(self, x, y, direction, cube_w, Wp, Hp):
        """
             將Cubemap image 的一點轉換到 Panorama上的一點
             Args:
                 x: Cubemap 中的 x
                 y: Cubemap 中的 y
                 direction: 此Cubemap的方向 ['F', 'R', 'B', 'L', 'U', 'D']
                 cube_w: Cubemap 的寬
                 Wp: Panorama image的寬
                 Hp: Panorama image的高

             Return:
                 i : Panorama中的 x
                 j : Panorama中的 y
        """
        x = 2 * x / cube_w - 1
        y = 2 * y / cube_w - 1

        P = np.array([[x],
                      [1],
                      [y]])

        if direction == 'U':
            return self._get_panorama_coord(self.top_matrix @ P, Wp, Hp)
        elif direction == 'D':
            return self._get_panorama_coord(self.down_matrix @ P, Wp, Hp)
        elif direction == 'L':
            return self._get_panorama_coord(self.left_matrix @ P, Wp, Hp)
        elif direction == 'R':
            return self._get_panorama_coord(self.right_matrix @ P, Wp, Hp)
        elif direction == 'F':
            return self._get_panorama_coord(self.front_matrix @ P, Wp, Hp)
        elif direction == 'B':
            return self._get_panorama_coord(self.back_matrix @ P, Wp, Hp)

    @staticmethod
    def p2c_image(panorama_img: np.ndarray, cube_w: int, cube_format='dict') -> dict[np.uint8] | list[np.uint8]:
        """
            將Panorama image轉換到Cubemap
            Args:
                panorama_img: Panorama image (H, W, C)
                cube_w: Cubemap的寬度
                cube_format: 要以怎樣的型態回傳Cubemap. ['F', 'R', 'B', 'L', 'U', 'D']

            Return:
                Cubemap image
        """

        result = py360convert.e2c(panorama_img, face_w=cube_w, cube_format=cube_format)

        if cube_format == 'dict':
            result['R'] = result['R'][:, ::-1, :]
            result['B'] = result['B'][:, ::-1, :]
            result['U'] = result['U'][::-1, :, :]

        return result

    @staticmethod
    def c2p_image(cubemap: dict[np.ndarray[bool]] | list[np.uint8], h: int, w: int) -> np.ndarray[np.uint8]:
        """
            將Cubemap轉換到Panorama image
            Args:
                cubemap: Cubemap的圖片，可為dict或是list，但是順序一定要滿足['F', 'R', 'B', 'L', 'U', 'D']
                h: Panorama的高
                w: Panorama的寬

            Returns:
                Panorama image
        """
        if isinstance(cubemap, dict):
            cubemap['R'] = cubemap['R'][:, ::-1, :]
            cubemap['B'] = cubemap['B'][:, ::-1, :]
            cubemap['U'] = cubemap['U'][::-1, :, :]

            return py360convert.c2e(cubemap, h=h, w=w, cube_format='dict').astype(np.uint8)
        elif isinstance(cubemap, list):
            return py360convert.c2e(cubemap, h=h, w=w, cube_format='list').astype(np.uint8)

