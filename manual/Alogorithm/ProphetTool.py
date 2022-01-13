from fbprophet import Prophet


def ProphetPredict(df, freqs, period=50):
    m = Prophet()
    m.fit(df)
    future = m.make_future_dataframe(periods=period, freq=freqs)
    forecast = m.predict(future)
    return forecast
