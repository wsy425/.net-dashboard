# -*- coding: utf-8 -*-
"""
Created on Wed Jan 27 21:12:15 2021

@author: ginas
"""

import tensorflow.keras as keras
from tensorflow.keras import layers
import numpy as np
from sklearn.preprocessing import MinMaxScaler


def DataSet_scale(samples, scal=None):
    if (scal == None):
        scaler = MinMaxScaler()
        return scaler, scaler.fit_transform(samples.reshape(-1, 1))
    else:
        return scal.transform(samples.reshape(-1, 1))


def Train_DataSet_build(scaled_samples, num_previous, num_future):
    X_train, Y_train = [], []
    for i in range(scaled_samples.shape[0] - (num_previous + num_future)):
        X_train.append(np.array(scaled_samples[i:i + num_previous]))
        Y_train.append(np.array(scaled_samples[i + num_previous:i + num_previous + num_future]))
    return np.array(X_train).reshape(-1, num_previous), np.array(Y_train).reshape([-1, num_future])


def Inverse_Data(data, scaler):
    if (scaler != None and data.shape[1] == 1):
        return scaler.inverse_transform(data)


# 将Params前的*去掉，与hiddens类型一致
def GRUModelBuild(input_dim, output_dim, Params):
    model = keras.models.Sequential()
    NumP = len(Params)
    model.add(layers.Reshape(((input_dim, 1)), input_shape=(input_dim,)))
    for i in range(NumP):
        if (NumP > 1 and i < NumP - 1):
            model.add(layers.GRU(Params[i], return_sequences=True))
        else:
            model.add(layers.GRU(Params[i]))
    model.add(layers.Dense(output_dim))
    model.summary()
    return model


def GRUModelTrain(grumodel, trainx, trainy, batchs, epochs):
    grumodel.compile(loss='mean_squared_error', optimizer='adam')
    return grumodel.fit(trainx, trainy, batch_size=batchs, epochs=epochs)


def GRUModelPrediction(grumodel, testx, steps=1, scaler=None):
    l = list(testx)
    n = grumodel.input_shape[1]
    if (n == testx.size):
        for i in range(steps):
            predict = grumodel.predict(np.array(l[-n:]).reshape(1, -1))
            l.append(predict[0][0])
    if (scaler != None):
        return Inverse_Data(np.array(l[n:]).reshape(-1, 1), scaler)
    else:
        return np.array(l[n:]).reshape(-1, 1)


def Rolling_forecast(input_data, predict_len, model, scaler):
    """"
    此函数用于将输入数据划分为模型输入数据及标签
            对模型输入数据进行滚动预测 predict_len 步 以及 返回标签
    Params
    --------------------
        input_data :  list类型， 输入数据
        predict_len : int类型， 预测步数
        model:        object类型， 已训练好的预测模型
        scaler:       object类型, 对训练数据进行处理的定标器

    Returns
    --------------------
        predict_list: list类型，len=predict_len，对输入数据进行滚动预测的结果

    """

    X_data = input_data

    num_previous = len(X_data)
    data = np.array(X_data).reshape(-1, 1)  # (num_previous,1)
    data_trans = scaler.transform(data).reshape(-1, num_previous)  # 数据预处理

    predict_list = []
    for i in range(predict_len):
        predict = model.predict(data_trans)
        predict = float(predict[0])
        predict_list.append(predict)

        data_trans = np.column_stack((data_trans, [predict]))  # 将预测值添加至输入
        data_trans = np.delete(data_trans, obj=0, axis=1)  # 去掉原输入第一个数

    predict_np = np.array(predict_list).reshape(-1, 1)
    predict = Inverse_Data(predict_np, scaler)

    return predict


def GRUPredict(data, num_previous=40, steps=10, num_future=1, batchs=30, echos=1000, *hiddens):
    scaler, ys = DataSet_scale(data)
    trainx, trainy = Train_DataSet_build(ys, num_previous, num_future)
    model = GRUModelBuild(num_previous, num_future, hiddens)
    fit = GRUModelTrain(model, trainx, trainy, batchs, echos)
    pre = GRUModelPrediction(model, ys[-num_previous:].flatten(), steps, scaler)
    # new
    data_show_list = ys[-num_previous:].flatten().tolist()
    data_show_np = np.array(data_show_list).reshape(-1, 1)
    data_show = Inverse_Data(data_show_np, scaler).flatten().tolist()
    return model, fit, pre.flatten(), data_show
