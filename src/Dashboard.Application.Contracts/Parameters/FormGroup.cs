using System.Collections.Generic;

namespace Dashboard.Parameters
{
    /// <summary>
    /// 定义各个算法的所需传入的属性结构
    /// </summary>
    public class FormGroup
    {
        // 算法类型
        public string Algorithm { get; set; }
        // 数据筛选开始时间
        public string StartTime { get; set; }
        // 数据筛选结束时间
        public string EndTime { get; set; }
        // 时间间隔
        public int Interval { get; set; }
        // 数据源，如果为null则是所有数据
        public string Parameter { get; set; }
        // 特征函数
        public string Feature { get; set; }
        // 特征提取窗口大小
        public int Windows { get; set; }
        // Prophet参数    预测长度，默认为50步
        public int PredictStep { get; set; } = 50;
        // GRUPredict参数   用多少个数据来预测
        public int NumPrevious { get; set; }
        // GRUPredict参数   向后预测多少步,默认为50步
        public int Steps { get; set; } = 50;
        // GRUPredict参数   向后预测多少步，默认为单步预测，即默认值为10
        public int NumFuture { get; set; } = 10;
        // GRUPredict参数   训练时送入模型训练样本的批大小,默认值为30
        public int Batches { get; set; } = 30;
        // GRUPredict参数   训练循环次数,默认值为200
        public int Echos { get; set; } = 200;
        // GRUPredict参数   神经网络隐含层的设置
        public List<int> Hidden { get; set; }
        // Arima算法P
        public int P { get; set; }
        // Arima算法I
        public int I { get; set; }
        // Arima算法Q
        public int Q { get; set; }
    }
}