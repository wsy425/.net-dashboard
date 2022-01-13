# -*- coding: utf-8 -*-
"""
Created on Mon Jan 25 15:21:02 2021

@author: ginas
"""

import numpy as np
import pandas as pd


def resample(samples, window=1):
    """
    重采样

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    resamples : TYPE
        DESCRIPTION.

    """
    resamples = samples
    if (window > 1):
        mod = samples.size % window
        resamples = samples[:samples.size - mod].reshape(-1, window)
    return resamples


def mean(samples, window=1):
    """
    平均值
    mean=sum(x)/n

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.mean(temp, axis=1)
    elif (window==1):
        return samples
    else:
        return np.mean(samples)


def var(samples, window=1):
    """
    方差

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.var(temp, axis=1)
    elif (window==1):
        return samples
    else:
        return np.var(samples)


def std(samples, window=1):
    """
    标准差

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.std(temp, axis=1)
    elif (window==1):
        return samples
    else:
        return np.std(samples)


def rms(samples, window=1):
    """
    均方根（RMS），也被称为有效值
    RMS=sqrt(sum(x^2)/n)

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    result : TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.sqrt(np.sum(np.square(temp), axis=1) / window)
    elif (window==1):
        return samples
    else:
        return np.sqrt(np.sum(np.square(samples)) / samples.size)


def PtoP(samples, window=1):
    """
    计算峰峰值
    xmax-xmin

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.ptp(temp, axis=1)
    elif (window==1):
        return samples
    else:
        return np.ptp(samples)


def skew(samples, window=1):
    """
    计算偏度，即三阶标准距
    sum((x-μ)^3)/n

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        s = temp.shape[0]
        mean = np.mean(temp, axis=1)
        var = np.var(temp, axis=1)
        l = list()
        for i in range(s):
            l.append(np.mean((temp[i, :] - mean[i]) ** 3) / np.power(var[i], 1.5))
        return np.asarray(l)
    elif (window==1):
        return samples
    else:
        mean = np.mean(samples)
        var = np.mean(samples)
        return np.mean((samples - mean) ** 3) / np.power(var, 1.5)


def kurt(samples, window=1):
    """
    计算峭度
    μ^4/σ^4

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        s = temp.shape[0]
        mean = np.mean(temp, axis=1)
        var = np.var(temp, axis=1)
        l = list()
        for i in range(s):
            l.append(np.mean((temp[i, :] - mean[i]) ** 4) / var[i] ** 2)
        return np.asarray(l)
    elif (window==1):
        return samples
    else:
        mean = np.mean(samples)
        var = np.mean(samples)
        return np.mean((samples - mean) ** 4) / np.power(var, 2)


def pulsefactor(samples, window=1):
    """
    计算脉冲因子
    max(x)/abs(mean(x))

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE
        DESCRIPTION.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.max(temp, axis=1) / np.abs(np.mean(temp, axis=1))
    elif (window==1):
        return samples
    else:
        return np.max(samples) / np.abs(np.mean(samples))


def marginfactor(samples, window=1):
    """
    计算裕度因子
    max(x)/mean(x2)

    Parameters
    ----------
    samples : TYPE
        DESCRIPTION.
    window : TYPE, optional
        DESCRIPTION. The default is 1.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    if (window > 1):
        temp = resample(samples, window)
        return np.max(temp, axis=1) / np.mean(temp ** 2, axis=1)
    elif (window==1):
        return samples
    else:
        return np.max(samples) / np.mean(samples ** 2)


def FFT(samples, Ts):
    Y = np.fft.fft(samples)
    f = np.fft.fftfreq(samples.size, Ts)
    return Y, f


def powerspec(samplesFFT):
    ps = (samplesFFT.real ** 2 + samplesFFT.imag ** 2) / samplesFFT.size
    ps[0] = 0.0
    return ps


def amplitudespec(samplesFFT):
    ams = np.abs(samplesFFT) / samplesFFT.size
    ams[0] = 0.0
    return ams


def phasespec(samplesFFT):
    phase = np.arctan(samplesFFT.imag / samplesFFT.real)
    return phase


def Fc(Gf, f):
    """
    重心频率
    sum(f*Gf)/sum(Gf)

    Parameters
    ----------
    samplesFFT : TYPE
        DESCRIPTION.
    f : TYPE
        DESCRIPTION.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    fn = (int)(f.size)
    return np.sum(f[:fn] * Gf[:fn]) / np.sum(Gf[:fn])


def Ms(Gf, f):
    """
    均方频谱
    sum(f^2*Gf)/sum(Gf)

    Parameters
    ----------
    samplesFFT : TYPE
        DESCRIPTION.
    f : TYPE
        DESCRIPTION.

    Returns
    -------
    None.

    """
    fn = (int)(f.size)
    return np.sum((f[:fn] ** 2) * Gf[:fn]) / np.sum(Gf[:fn])


def Vf(Gf, Fd, f):
    """
    频率方差
    sum((f-fd)^2*Gf)/sum(Gf)

    Parameters
    ----------
    Gf : TYPE
        DESCRIPTION.
    Fd : TYPE
        DESCRIPTION.
    f : TYPE
        DESCRIPTION.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    fn = (int)(f.size)
    return np.sum(((f[:fn] - Fd) ** 2) * Gf[:fn]) / np.sum(Gf[:fn])


def Corrfcator(Gf, f):
    """
    相关因子
    sum(cos(2pi*f)*Gf)/sum(Gf)

    Parameters
    ----------
    Gf : TYPE
        DESCRIPTION.
    f : TYPE
        DESCRIPTION.

    Returns
    -------
    TYPE
        DESCRIPTION.

    """
    fn = (int)(f.size)
    return np.sum((np.cos(2 * np.pi * f[:fn]) * Gf[:fn]) / np.sum(Gf[:fn]))


def ToTS(samples):
    n = np.array(range(samples.size))
    df = pd.DataFrame({'ds': n, 'y': samples})
    return df


def Discriminate(feature, array, window=1):
    methods = {
        "mean": mean,
        "var": var,
        "std": std,
        "rms": rms,
        "ptop": PtoP,
        "skew": skew,
        "kurt": kurt,
        "pulsefactor": pulsefactor,
        "marginfactor": marginfactor
    }

    newArray = methods.get(feature)(array, window)
    return newArray

def Spectrum_Deal(Ps, Ams, Phase, f):
    minIndexTuple = np.where(f == np.min(f))
    minIndex = int(minIndexTuple[0][0])
    all_index = f.size - minIndex
    new_f = np.append(f[all_index:], f[0:all_index])
    new_Ps = np.append(Ps[all_index:], Ps[0:all_index])
    new_Ams = np.append(Ams[all_index:], Ams[0:all_index])
    new_Phase = np.append(Phase[all_index:], Phase[0:all_index])
    return new_f, new_Ps, new_Ams, new_Phase
