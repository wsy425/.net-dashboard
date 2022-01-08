namespace Dashboard
{
    public static class DashboardDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Dashboard";

        public static string DbSchema { get; set; } = null;

        public const string MySQLConnectionStringName = "MySQLDashboard";

        public const string MongoConnectionStringName = "MongoDashboard";
    }
}
