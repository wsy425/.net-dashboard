from flask import request, Flask, jsonify
from signalrcore.hub_connection_builder import HubConnectionBuilder
from concurrent.futures import ThreadPoolExecutor
import threading
import FileOperate as fileRead
import logging
import time
import datetime
import json
import urllib3
import PCA_Monitor as PCA


class Job(threading.Thread):
    def __init__(self, *args, **kwargs):
        super(Job, self).__init__(*args, **kwargs)
        self.__flag = threading.Event()  # 用于暂停线程标识符
        self.__flag.set()    # 设置为True
        self.__running = threading.Event()   # 用于停止线程标识
        self.__running.set()  # 将running设置为True

    def run(self):
        while self.__running.is_set():
            self.__flag.wait()  # 为True时立即返回, 为False时阻塞直到内部的标识位为True后返回
            S1["Time"] = (datetime.datetime.now() +
                          datetime.timedelta(hours=config["add_hours"])).strftime("%Y-%m-%d %H:%M:%S")
            all_dict = json.dumps(S1)
            hub_connection.send("DeliverRawDataS1", [all_dict])
            time.sleep(config["timeSleep"])

    def pause(self):
        self.__flag.clear()  # 设置为False, 让线程阻塞

    def resume(self):
        self.__flag.set()  # 设置为True, 让线程停止阻塞

    def stop(self):
        self.__flag.set()  # 将线程从暂停状态恢复, 如何已经暂停的话
        self.__running.clear()


app = Flask(__name__)
app.config['JSON_AS_ASCII'] = False


@app.route('/start', methods=['POST'])
def start():
    symbol = int(request.form['symbol'])
    info = ""
    try:
        if symbol == 1:
            # connect()
            a.resume()
            info = {"result": SensorsConfig["Name"]}
    except Exception as e:
        logger.warning("SignalR data sender client has closed")
        info = {"result": "关闭成功"}
    finally:
        return jsonify(info), 200


@app.route('/stop', methods=['POST'])
def stop():
    symbol = int(request.form['symbol'])
    info = {"result": "Fail"}
    if symbol == 0:
        a.pause()
        info = {"result": "Success"}
    return jsonify(info), 200


def connect():
    while True:
        S1["Time"] = (datetime.datetime.now() +
                      datetime.timedelta(hours=config["add_hours"])).strftime("%Y-%m-%d %H:%M:%S")
        all_dict = json.dumps(S1)
        hub_connection.send("DeliverRawDataS1", [all_dict])
        time.sleep(config["timeSleep"])


if __name__ == '__main__':
    urllib3.disable_warnings()
    config = fileRead.read_config_file("JsonFile/SignalR_Config.json")
    SensorsConfig = fileRead.read_config_file("JsonFile/S1.json")
    S1 = dict()
    S1.setdefault(SensorsConfig['Time'])
    threadPool = ThreadPoolExecutor(max_workers=3, thread_name_prefix="test")
    for i in range(len(SensorsConfig['Parameters'])):
        S1.setdefault(SensorsConfig['Parameters'][i]['Name'], 0)
    url = config['URL']
    logging.basicConfig(format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    logger = logging.getLogger("S1")
    hub_connection = HubConnectionBuilder() \
        .with_url(url, options={
            "verify_ssl": False
        }) \
        .configure_logging(logging.INFO) \
        .build()
    hub_connection.on_open(lambda: logger.warning("SignalR data sender client has started"))
    hub_connection.on_close(lambda: logger.warning("SignalR data sender client is closing"))
    hub_connection.start()
    # 等待SignalR启动
    time.sleep(1)
    a = Job()
    a.start()
    a.pause()
    app.run(debug=False, host=SensorsConfig["host"], port=SensorsConfig["port"])

    # a = Job()
    # a.start()
    # time.sleep(3)
    # a.pause()
    # time.sleep(3)
    # a.resume()
    # time.sleep(3)
    # a.pause()
    # time.sleep(2)
    # a.stop()
