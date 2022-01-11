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

app = Flask(__name__)
app.config['JSON_AS_ASCII'] = False


@app.route('/start', methods=['POST'])
def start():
    symbol = int(request.form['symbol'])
    info = ""
    try:
        if symbol == 1:
            # connect()
            future = threadPool.submit(connect)
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
        hub_connection.stop()
        ss = threading.currentThread()
        time.sleep(config["reconnect_interval"])
        hub_connection.start()
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
    app.run(debug=False, host=SensorsConfig["host"], port=SensorsConfig["port"])
