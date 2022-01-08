namespace Dashboard.Parameters
{
    public class BaseProperties
    {
        // 算法类型
        public string Algorithm { get; set; }
        // 数据筛选开始时间
        public string StartTime { get; set; }
        // 数据筛选结束时间
        public string EndTime { get; set; }
        // 时间间隔
        public int Interval { get; set; }
    }
}