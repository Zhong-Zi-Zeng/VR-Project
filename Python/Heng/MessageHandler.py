class Search:
    """
         將material資料夾下的所有照片和名稱都傳送給unity
         Send:
            1. panorama images
            2. panorama image's name
            3. progress
    """
    @staticmethod
    def handle(main, data, args, kwargs):
        print('Search')


class Generate:
    """
         將使用者指定的照片去生成mask後回傳給unity
         Send:
            1. panorama image with mask
            2. id_map
            3. progress
    """
    @staticmethod
    def handle(main, data, args, kwargs):
        print('Generate')


