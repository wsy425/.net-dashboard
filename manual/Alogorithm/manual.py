import numpy as np
import pandas as pd
import ArimaTool as arima
import GruTool as gru
import ProphetTool as pro
import FeatureExtract as fe
from datetime import datetime
import json
import os
import SignalR as signalr
import warnings
import FileOperate as fr
import logging
import requests
warnings.filterwarnings('ignore')


def ARI(datas):
    txtName = "arima.txt"
    filePath = directoryPath + "/" + txtName
    try:
        Stripdata = str(datas).strip('[').strip(']').strip("'")
        DeserializationData = json.loads(Stripdata)
        windows = DeserializationData[3]
        P = DeserializationData[4]
        I = DeserializationData[5]
        Q = DeserializationData[6]
        timelist = []
        for i in range(0, len(DeserializationData[1])):
            if i % windows == windows - 1:
                timelist.append(datetime.strptime(DeserializationData[1][i][:14], '%Y%m%d%H%M%S'))
        listToArr = np.array(DeserializationData[0], dtype=float)
        arr = fe.Discriminate(DeserializationData[2], listToArr, windows)
        logger.warning("Arima算法开始预测")
        Model, summary, residuals, predict = arima.arimamodel(arr, P, I, Q)
        fc_series, lower_series, upper_series = arima.forecast(Model, steps=arimaConfig['step'])
        logger.warning("Arima算法预测结束")
        state = True
        timeLast = timelist[-1]
        for i in range(arimaConfig['step']):
            timelist.append(timeLast + (i + 1) * (timelist[1] - timelist[0]))
        timeArr = np.array(timelist, dtype=str)
        result = [state, timeArr.tolist(), arr.tolist(), fc_series.values.tolist(),
                  lower_series.values.tolist(), upper_series.values.tolist()]
        data = json.dumps(result)
        create(filePath, data)
        request = {
            "state": 0,
            "name": 103
        }
        logger.warning("Arima算法回调成功")
        result.clear()
        timelist.clear()
        result.clear()
    except Exception as ex:
        logger.error("Arima算法调用异常")
        logger.error(ex)
        request = {
            "state": 1,
            "name": 103
        }
    requests.post(url=url, headers=header, json=request)
    return


def GRU(datas):
    txtName = "gru.txt"
    filePath = directoryPath + "/" + txtName
    try:
        Stripdata = str(datas).strip('[').strip(']').strip("'")
        DeserializationData = json.loads(Stripdata)
        windows = DeserializationData[3]
        timelist = []
        for i in range(0, len(DeserializationData[1])):
            if i % windows == windows - 1:
                timelist.append(datetime.strptime(DeserializationData[1][i][:14], '%Y%m%d%H%M%S'))
        listToArr = np.array(DeserializationData[0], dtype=float)
        arr = fe.Discriminate(DeserializationData[2], listToArr, windows)
        n = 4
        num_previous = DeserializationData[n]
        steps = DeserializationData[n + 1]
        num_future = DeserializationData[n + 2]
        batchs = DeserializationData[n + 3]
        echos = DeserializationData[n + 4]
        hiddens = tuple(DeserializationData[n + 5])
        os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
        logger.warning("GRU算法开始预测")
        model, fit, pre, train = gru.GRUPredict(arr, num_previous, steps, num_future, batchs, echos, *hiddens)
        logger.warning("GRU算法预测结束")
        state = True
        timeLast = timelist[-1]
        for i in range(0, len(pre)):
            timelist.append(timeLast + (i + 1) * (timelist[1] - timelist[0]))
        timelist = timelist[-(len(pre) + len(train)):]
        timeArr = np.array(timelist, dtype=str)
        result = [state, timeArr.tolist(), train, pre.tolist()]
        data = json.dumps(result)
        create(filePath, data)
        request = {
            "state": 0,
            "name": 102
        }
        logger.warning("GRU算法回调成功")
        timelist.clear()
        result.clear()
    except Exception as ex:
        logger.error("GRU算法调用异常")
        logger.error(ex)
        request = {
            "state": 1,
            "name": 102
        }
    requests.post(url=url, headers=header, json=request)
    return


def Pro(datas):
    txtName = "prophet.txt"
    filePath = directoryPath + "/" + txtName
    try:
        Stripdata = str(datas).strip('[').strip(']').strip("'")
        DeserializationData = json.loads(Stripdata)
        windows = DeserializationData[3]
        period = DeserializationData[4]
        freq = windows * period
        timelist = []
        for i in range(0, len(DeserializationData[1])):
            if i % windows == windows - 1:
                timelist.append(DeserializationData[1][i][:14])
        listToArr = np.array(DeserializationData[0], dtype=float)
        arr = fe.Discriminate(DeserializationData[2], listToArr, windows)
        startTime = str(datetime.strptime(timelist[-1], '%Y%m%d%H%M%S'))
        logger.warning("Prophet算法开始预测")
        # 构造假的时间戳
        t = pd.date_range(start="1992-1-1", end="2015-12-1", freq='H')
        # 重新构造需要返回的时间戳
        t_new = pd.date_range(start=startTime, periods=period + 1, freq=str(freq) + 'S')
        N = len(arr)
        t1 = t[:N]
        df = pd.DataFrame({'ds': t1, 'y': arr})
        da = pro.ProphetPredict(df, 'H', period)
        # 这里时间戳要换成真实的时间戳t_new
        for i in range(0, len(timelist)):
            timelist[i] = str(datetime.strptime(timelist[i], '%Y%m%d%H%M%S'))
        for i in range(1, len(t_new)):
            timelist.append(str(t_new[i]))
        yhat = da.yhat.values
        y_low = da.yhat_lower.values
        y_up = da.yhat_upper.values
        logger.warning("Prophet算法预测结束")
        state = True
        result = [state, timelist, yhat.tolist(), y_low.tolist(), y_up.tolist(), arr.tolist()]
        data = json.dumps(result)
        create(filePath, data)
        request = {
            "state": 0,
            "name": 101
        }
        logger.warning("Prophet算法回调成功")
        timelist.clear()
        result.clear()
    except Exception as ex:
        logger.error("Prophet算法调用异常")
        logger.error(ex)
        request = {
            "state": 1,
            "name": 101
        }
    requests.post(url=url, headers=header, json=request)
    return


def Spec(datas):
    txtName = "spectrum.txt"
    filePath = directoryPath + "/" + txtName
    try:
        Stripdata = str(datas).strip('[').strip(']').strip("'")
        DeserializationData = json.loads(Stripdata)
        windows = DeserializationData[2]
        data_list = []
        for i in range(1, len(DeserializationData[0]), 2):
            data_list.append(DeserializationData[0][i])
        arr = np.array(data_list, dtype=float)
        arr = fe.Discriminate(DeserializationData[1], arr, windows)
        logger.warning("Spectrum算法开始预测")
        Y, f = fe.FFT(arr, windows)
        Ps = fe.powerspec(Y)
        Ams = fe.amplitudespec(Y)
        Phase = fe.phasespec(Y)
        logger.warning("Spectrum算法预测结束")
        state = True
        totalCount = specConfig['Ns']
        for index in range(totalCount):
            if f[index] < 0:
                totalCount = index
                break
        result = [state, f[:totalCount].tolist(), Ps[:totalCount].tolist(),
                  Ams[:totalCount].tolist(), Phase[:totalCount].tolist()]
        data = json.dumps(result)
        create(filePath, data)
        request = {
            "state": 0,
            "name": 100
        }
        logger.warning("Spectrum算法调用成功")
        data_list.clear()
        result.clear()
    except Exception as ex:
        logger.error("Spectrum算法调用异常")
        logger.error(ex)
        request = {
            "state": 1,
            "name": 100
        }
    requests.post(url=url, headers=header, json=request)
    return


def mkdir(path):
    isExists = os.path.exists(path)
    # 判断结果
    if not isExists:
        os.makedirs(path)


def create(full_path, msg):
    with open(full_path, "w") as file:
        file.write(msg)


if __name__ == "__main__":
    warnings.filterwarnings('ignore')
    logging.basicConfig(format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    logger = logging.getLogger("manual")
    arimaConfig = fr.read_config_file("./JsonFile/ArimaConfig.json")
    logger.warning("The arima configuration file has been load.")
    specConfig = fr.read_config_file("./JsonFile/SpectrumConfig.json")
    logger.warning("The spectrum configuration file has been load.")
    fileConfig = fr.read_config_file("./JsonFile/File.json")
    logger.warning("The other configuration files has been load.")

    directoryPath = fileConfig["path"].strip().rstrip("/")
    url = fileConfig['url']
    mkdir(directoryPath)
    logger.warning("The directory of manual result files has been created.")
    header = {
        "Content-Type": "application/json"
    }

    warnings.filterwarnings("ignore")

    hub_connection = signalr.ini()
    hub_connection.on("ARIMAPredict", ARI)
    hub_connection.on("GRUPredict", GRU)
    hub_connection.on("ProphetPredict", Pro)
    hub_connection.on("Spectrum", Spec)
    hub_connection.on("AutoPCAS1", str)
    hub_connection.on("AutoPCAS2", str)
    hub_connection.on("AutoPCAS3", str)
    hub_connection.on("Front", str)
    hub_connection.on("RawDataComeS1", str)
    hub_connection.on("RawDataComeS2", str)
    hub_connection.on("RawDataComeS3", str)

    temp = 1
    while True:
        temp = 1
