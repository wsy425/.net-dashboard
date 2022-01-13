FROM python:3.7
ADD . .

WORKDIR /Alogorithm

RUN pip3 install -i https://pypi.tuna.tsinghua.edu.cn/simple -r requirements.txt \
    && pip3 install fbprophet==0.6 -i https://pypi.tuna.tsinghua.edu.cn/simple \
    && pip3 install tensorflow==1.14.0 -i https://pypi.tuna.tsinghua.edu.cn/simple

CMD [ "python", "manual.py" ]
