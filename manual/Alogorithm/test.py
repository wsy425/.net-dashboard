import requests
import FileOperate as fileRead
import json
import os
import numpy as np


def mkdir(directory):
    # 去除首位空格 尾部\符号
    directory = directory.strip().rstrip("\\")
    isExists = os.path.exists(directory)

    # 判断结果
    if not isExists:
        # 如果不存在则创建目录
        # 创建目录操作函数
        os.makedirs(directory)


def create(full_path, msg):
    with open(full_path, "w") as file:
        file.write(msg)


test = []
conf = fileRead.read_config_file("JsonFile/File.json")
path = conf["path"]
url = conf["url"]
txtName = "spectrum.txt"
mkdir(path)
filePath = path + "\\" + txtName
ss = list(np.random.rand(2, 4))
for i in range(2):
    test.append(list(ss[i]))
sss = json.dumps(test, ensure_ascii=False)
create(filePath, sss)
header = {"Content-Type": "application/json"}
d = {
    "request": {
        "state": 0,
        "name": 1.5
    }
}
data = json.dumps(d)
r = requests.post(url=url, headers=header, data=data)
print(r.text)
print(r)

