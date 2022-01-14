from flask import request, Flask, jsonify
from flask_cors import *
from signalrcore.hub_connection_builder import HubConnectionBuilder
import threading
import FileOperate as fileRead
import logging
import time
import datetime
import json
import urllib3
import numpy as np
import PCA_Monitor as PCA


class JobS1(threading.Thread):
    def __init__(self, *args, **kwargs):
        super(JobS1, self).__init__(*args, **kwargs)
        self.__flag = threading.Event()  # 用于暂停线程标识符
        self.__flag.set()  # 设置为True
        self.__running = threading.Event()  # 用于停止线程标识
        self.__running.set()  # 将running设置为True

    def run(self):
        while self.__running.is_set():
            self.__flag.wait()  # 为True时立即返回, 为False时阻塞直到内部的标识位为True后返回
            S1["Time"] = (datetime.datetime.now() +
                          datetime.timedelta(hours=globalConfig["add_hours"])).strftime("%Y-%m-%d %H:%M:%S")
            controlS1 = fileRead.read_config_file("JsonFile/scope.json")["S1"]
            for index in range(len(S1Config['Parameters'])):
                S1[S1Config['Parameters'][index]['Name']] = np.random.random() * controlS1["multiple"] + controlS1["offset"]
            all_dict = json.dumps(S1)
            hub_connection.send("DeliverRawDataS1", [all_dict])
            array = converter(S1)
            [_, scores, custom_threshold] = p1.CustomPredict(p1.Model.threshold_ * PCAS1Config['multiple'], array)
            res = [S1['Time'], scores[0], custom_threshold]
            auto = json.dumps(res)
            hub_connection.send("AutoPCADeliverS1", [auto])
            time.sleep(globalConfig["timeSleep"])

    def pause(self):
        self.__flag.clear()  # 设置为False, 让线程阻塞

    def resume(self):
        self.__flag.set()  # 设置为True, 让线程停止阻塞

    def stop(self):
        self.__flag.set()  # 将线程从暂停状态恢复, 如何已经暂停的话
        self.__running.clear()

    def status(self):
        return self.__flag.is_set()  # True为线程运行


class JobS2(threading.Thread):
    def __init__(self, *args, **kwargs):
        super(JobS2, self).__init__(*args, **kwargs)
        self.__flag = threading.Event()  # 用于暂停线程标识符
        self.__flag.set()  # 设置为True
        self.__running = threading.Event()  # 用于停止线程标识
        self.__running.set()  # 将running设置为True

    def run(self):
        while self.__running.is_set():
            self.__flag.wait()  # 为True时立即返回, 为False时阻塞直到内部的标识位为True后返回
            S2["Time"] = (datetime.datetime.now() +
                          datetime.timedelta(hours=globalConfig["add_hours"])).strftime("%Y-%m-%d %H:%M:%S")
            controlS2 = fileRead.read_config_file("JsonFile/scope.json")["S2"]
            for index in range(len(S2Config['Parameters'])):
                S2[S2Config['Parameters'][index]['Name']] = np.random.random() * controlS2["multiple"] + controlS2["offset"]
            all_dict = json.dumps(S2)
            hub_connection.send("DeliverRawDataS2", [all_dict])
            array = converter(S2)
            [_, scores, custom_threshold] = p2.CustomPredict(p2.Model.threshold_ * PCAS2Config['multiple'], array)
            res = [S2['Time'], scores[0], custom_threshold]
            auto = json.dumps(res)
            hub_connection.send("AutoPCADeliverS2", [auto])
            time.sleep(globalConfig["timeSleep"])

    def pause(self):
        self.__flag.clear()  # 设置为False, 让线程阻塞

    def resume(self):
        self.__flag.set()  # 设置为True, 让线程停止阻塞

    def stop(self):
        self.__flag.set()  # 将线程从暂停状态恢复, 如何已经暂停的话
        self.__running.clear()

    def status(self):
        return self.__flag.is_set()  # True为线程运行


class JobS3(threading.Thread):
    def __init__(self, *args, **kwargs):
        super(JobS3, self).__init__(*args, **kwargs)
        self.__flag = threading.Event()  # 用于暂停线程标识符
        self.__flag.set()  # 设置为True
        self.__running = threading.Event()  # 用于停止线程标识
        self.__running.set()  # 将running设置为True

    def run(self):
        while self.__running.is_set():
            self.__flag.wait()  # 为True时立即返回, 为False时阻塞直到内部的标识位为True后返回
            S3["Time"] = (datetime.datetime.now() +
                          datetime.timedelta(hours=globalConfig["add_hours"])).strftime("%Y-%m-%d %H:%M:%S")
            controlS3 = fileRead.read_config_file("JsonFile/scope.json")["S3"]
            for index in range(len(S3Config['Parameters'])):
                S3[S3Config['Parameters'][index]['Name']] = np.random.random() * controlS3["multiple"] + controlS3["offset"]
            all_dict = json.dumps(S3)
            hub_connection.send("DeliverRawDataS3", [all_dict])
            array = converter(S3)
            [_, scores, custom_threshold] = p3.CustomPredict(p3.Model.threshold_ * PCAS3Config['multiple'], array)
            res = [S3['Time'], scores[0], custom_threshold]
            auto = json.dumps(res)
            hub_connection.send("AutoPCADeliverS3", [auto])
            time.sleep(globalConfig["timeSleep"])

    def pause(self):
        self.__flag.clear()  # 设置为False, 让线程阻塞

    def resume(self):
        self.__flag.set()  # 设置为True, 让线程停止阻塞

    def stop(self):
        self.__flag.set()  # 将线程从暂停状态恢复, 如何已经暂停的话
        self.__running.clear()

    def status(self):
        return self.__flag.is_set()  # True为线程运行


app = Flask(__name__)
CORS(app, resources=r'/*')
app.config['JSON_AS_ASCII'] = False


@app.route('/start', methods=['POST'])
def start():
    try:
        symbol = int(request.form['symbol'])
        if symbol == 1:
            if a.status():
                info = {"code": 400, "result": "S1发数器重复运行"}
            else:
                a.resume()
                info = {"code": 200, "result": "S1发数器已启动"}
        elif symbol == 2:
            if b.status():
                info = {"code": 400, "result": "S2发数器重复运行"}
            else:
                b.resume()
                info = {"code": 200, "result": "S2发数器已启动"}
        elif symbol == 3:
            if c.status():
                info = {"code": 400, "result": "S3发数器重复运行"}
            else:
                c.resume()
                info = {"code": 200, "result": "S3发数器已启动"}
        else:
            info = {"code": 400, "result": "发数器启动失败"}
    except Exception as e:
        info = {"code": 500, "result": "请求失败"}
    rst = jsonify(info)
    rst.headers['Access-Control-Allow-Origin'] = '*'
    rst.headers['Access-Control-Allow-Method'] = 'POST'
    rst.headers['Access-Control-Allow-Headers'] = 'x-requested-with,content-type'
    return rst


@app.route('/stop', methods=['POST'])
def stop():
    try:
        symbol = int(request.form['symbol'])
        if symbol == 1:
            if a.status():
                a.pause()
                info = {"code": 200, "result": "S1发数器暂停成功"}
            else:
                info = {"code": 400, "result": "S1发数器已暂停"}
        elif symbol == 2:
            if b.status():
                b.pause()
                info = {"code": 200, "result": "S2发数器暂停成功"}
            else:
                info = {"code": 400, "result": "S2发数器已暂停"}
        elif symbol == 3:
            if c.status():
                c.pause()
                info = {"code": 200, "result": "S3发数器暂停成功"}
            else:
                info = {"code": 400, "result": "S3发数器已暂停"}
        else:
            info = {"code": 400, "result": "发数器停止失败"}
    except Exception as e:
        info = {"code": 500, "result": "请求失败"}
    rst = jsonify(info)
    rst.headers['Access-Control-Allow-Origin'] = '*'
    rst.headers['Access-Control-Allow-Method'] = 'POST'
    rst.headers['Access-Control-Allow-Headers'] = 'x-requested-with,content-type'
    return rst


def converter(dict_data):
    value = list(dict_data.values())
    return np.array(value[1:], dtype=float).reshape(1, -1)


if __name__ == '__main__':
    urllib3.disable_warnings()
    SignalRConfig = fileRead.read_config_file("JsonFile/SignalR_Config.json")
    S1Config = fileRead.read_config_file("JsonFile/S1.json")
    S2Config = fileRead.read_config_file("JsonFile/S2.json")
    S3Config = fileRead.read_config_file("JsonFile/S3.json")
    globalConfig = fileRead.read_config_file("JsonFile/conf.json")
    PCAS1Config = fileRead.read_config_file("JsonFile/PCAS1.json")
    PCAS2Config = fileRead.read_config_file("JsonFile/PCAS2.json")
    PCAS3Config = fileRead.read_config_file("JsonFile/PCAS3.json")
    controlConfig = fileRead.read_config_file("JsonFile/scope.json")

    p1 = PCA.PCA_Monitor()
    p2 = PCA.PCA_Monitor()
    p3 = PCA.PCA_Monitor()
    p1.LoadModel("models/" + PCAS1Config['model'])
    p2.LoadModel("models/" + PCAS2Config['model'])
    p3.LoadModel("models/" + PCAS3Config['model'])

    S1 = dict()
    S1.setdefault(S1Config['Time'])
    for i in range(len(S1Config['Parameters'])):
        S1.setdefault(S1Config['Parameters'][i]['Name'], 0)

    S2 = dict()
    S2.setdefault(S2Config['Time'])
    for i in range(len(S2Config['Parameters'])):
        S2.setdefault(S2Config['Parameters'][i]['Name'], 0)

    S3 = dict()
    S3.setdefault(S3Config['Time'])
    for i in range(len(S3Config['Parameters'])):
        S3.setdefault(S3Config['Parameters'][i]['Name'], 0)

    logging.basicConfig(format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    logger = logging.getLogger("dataSender")

    hub_connection = HubConnectionBuilder() \
        .with_url(SignalRConfig['URL'], options={
            "verify_ssl": False
        }) \
        .configure_logging(logging.INFO) \
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": SignalRConfig["keep_alive_interval"],
            "reconnect_interval": SignalRConfig["reconnect_interval"],
            "max_attempts": SignalRConfig["max_attempts"]
        }).build()
    hub_connection.on_open(lambda: logger.warning("SignalR data sender client has started"))
    hub_connection.on_close(lambda: logger.warning("SignalR data sender client is closing"))
    hub_connection.start()

    hub_connection.on('Front', str)
    hub_connection.on('Spectrum', str)
    hub_connection.on('ProphetPredict', str)
    hub_connection.on('GRUPredict', str)
    hub_connection.on('ARIMAPredict', str)

    # 等待SignalR启动
    time.sleep(1)
    a = JobS1()
    a.start()
    a.pause()

    b = JobS2()
    b.start()
    b.pause()

    c = JobS3()
    c.start()
    c.pause()
    app.run(debug=False, host=globalConfig["host"], port=globalConfig["port"])
