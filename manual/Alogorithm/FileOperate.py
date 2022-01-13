# coding=utf-8
# pip install package -i https://pypi.tuna.tsinghua.edu.cn/simple
import json


def write_config_file(path, content):
    file = open(path, 'w')
    file.write(json.dumps(content) + '\n')
    file.close()


def read_config_file(path):
    file = open(path, 'r', encoding='utf-8')
    content = file.read()
    file.close()
    return json.loads(content)
