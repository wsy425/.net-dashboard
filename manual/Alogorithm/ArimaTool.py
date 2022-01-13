# -*- coding: utf-8 -*-
"""
此模块编写于2021年1月
用于主泵监测信号的ARIMA分析

@作者：GinasYang
"""

from statsmodels.tsa.stattools import pacf, adfuller
import statsmodels.graphics.tsaplots as sgt
from statsmodels.tsa.arima_model import ARIMA
import numpy as np
import pandas as pd


def data_prepare_mean(datain, windows):
    """
    原时间序列可能不满足平稳性要求，或者很难建立ARIMA模型
    所以需要对原始时间序列进行预处理
    该函数用于对原始时间序列进行加窗取平均，从而构造新的时间序列
    比如，我们可以将十分钟的原始数据取平均值，然后用新的值构造新的时间序列
    采样时间也就从原来的1s变成了360s，即重采样

    Parameters
    ----------
    datain : 类型：array of float64
             描述：为原始输入时间序列，shape为（n,）,n为时间序列的样本数
    windows : 类型：int
              描述：为窗口大小

    Returns
    -------
    newsamples : 类型: array of float64
                 描述：重采样后的新时间序列样本，采用的方法是窗口内数据求平均值

    """
    l = list()
    n = len(datain)
    for i in range((int)(n / windows)):
        l.append(sum(datain[i * windows:i * windows + windows - 1]) / windows)
    newsamples = np.asarray(l)
    return newsamples


def ts_analyse(datain, nlags=25, alpha=0.05):
    """
    此函数用于分析输入数据的平稳性，自相关函数和偏自相关函数
    
    Parameters
    ----------
    datain : 类型：array of float64
             描述：为原始输入时间序列，shape为（n,）,n为时间序列的样本数
    nlags : 类型: int
             描述：计算ACF和PACF的阶数，默认为25  
    alpha : 类型: float
             描述：拒绝检验参数，默认为0.05
    Returns
    -------
    ADF : 类型：float64
          描述：数列的ADF值，理论上越小越好
    p : 类型：float64
        描述：数据的p值，理论上小于输入参数alpha即表示数据平稳
    acf_x : 类型：array of float64
            描述：输入数据的自相关函数，shape为（nlags,）
    acon_lower : 类型：array of float64
                描述：在设定置信度下的自相关函数下界，shape为（nlags,）
    acon_upper : 类型：array of float64
                描述：在设定置信度下的自相关函数上界，shape为（nlags,）
    pacf_x : 类型：array of float64
            描述：输入数据的偏自相关函数，shape为（nlags,）
    pcon_lower : 类型：array of float64
                描述：在设定置信度下的偏自相关函数下界，shape为（nlags,）
    pcon_upper : 类型：array of float64
                描述：在设定置信度下的偏自相关函数上界，shape为（nlags,）
    """

    tem_result = adfuller(datain)

    ADF = tem_result[0]
    p = tem_result[1]
    acf_x, con = sgt.acf(datain, fft=False, alpha=alpha, nlags=nlags)
    acon_lower = con[:, 0] - acf_x
    acon_upper = con[:, 1] - acf_x
    pacf_x, confint = pacf(datain, nlags=nlags, alpha=alpha)
    pcon_lower = confint[:, 0] - pacf_x
    pcon_upper = confint[:, 1] - pacf_x
    return ADF, p, acf_x, acon_lower, acon_upper, pacf_x, pcon_lower, pcon_upper


def arimamodel(data, arorder, iorder, maorder):
    """
    此函数用来对输入时间序列按照给定的（p,i,q）参数建立ARIMA模型
    

    Parameters
    ----------
    data : TYPE: array of float64
        DESCRIPTION：为原始输入时间序列，shape为（n,）,n为时间序列的样本数.
    arorder : TYPE: int
        DESCRIPTION: 自回归模型（AR）系数阶数.
    iorder : TYPE：int
        DESCRIPTION：差分（I）阶数.
    maorder : TYPE：int
        DESCRIPTION: 平滑模型（MA）系数阶数.

    Returns
    -------
    model_fit : TYPE：ARIMAResults
        DESCRIPTION：所训练的ARIMA模型.
    summary : TYPE：str
        DESCRIPTION：模型的相关信息，包括系数、阶数、误差、方差等.
    residuals : TYPE：DataFrame
        DESCRIPTION：模型的残差，两组数据分别为残差序列和概率密度函数.
    predict : TYPE：array of float64
        DESCRIPTION：对于所构建模型在训练样本域中的预测估计.

    """
    model = ARIMA(data, order=(arorder, iorder, maorder))
    model_fit = model.fit(disp=-1)
    summary = model_fit.summary()
    residuals = pd.DataFrame(model_fit.resid)
    predict = model_fit.predict()
    return model_fit, summary, residuals, predict


def forecast(model, steps, alpha=0.05):
    """
    此函数用于将训练好的模型用来预测未来的时间序列

    Parameters
    ----------
    model : TYPE：ARIMAResults
        DESCRIPTION：训练好的ARIMA模型.
    steps : TYPE：int
        DESCRIPTION：向后预测的步数/点数.
    alpha : TYPE：float
        DESCRIPTION. 拒绝检验参数，默认为0.05.

    Returns
    -------
    fc_series : TYPE：Series
        DESCRIPTION：用训练的ARIMA模型预测得到的时间序列.
    lower_series : TYPE：Series
        DESCRIPTION：置信度参数下的预测下界.
    upper_series : TYPE：Series
        DESCRIPTION：置信度参属下的预测上届.

    """
    fc, se, conf = model.forecast(steps, alpha=alpha)
    fc_series = pd.Series(fc)
    lower_series = pd.Series(conf[:, 0])
    upper_series = pd.Series(conf[:, 1])
    return fc_series, lower_series, upper_series

