# -*- coding = utf-8 -*-
# @Time : 2021/3/20 10:51
# @Author :
# @File : SignalR.py
# @Software : PyCharm


import FileOperate as FileRead
import logging
from signalrcore.hub_connection_builder import HubConnectionBuilder


def ini():
    config = FileRead.read_config_file("./JsonFile/SiganlR_Config.json")
    url = config['SignalR_URL']
    logging.basicConfig(format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    logger = logging.getLogger("manual")
    hub_connection = HubConnectionBuilder() \
        .with_url(url, options={
            "verify_ssl": False,
            "skip_negotiation": False,
            "enable_trace": True
    }) \
        .configure_logging(logging.INFO) \
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": config["keep_alive_interval"],
            "reconnect_interval": config["reconnect_interval"],
            "max_attempts": config["max_attempts"]
    }).build()
    hub_connection.on_open(lambda: logger.warning("SignalR has started"))
    hub_connection.on_close(lambda: logger.warning("SignalR client closed."))
    hub_connection.start()
    return hub_connection
