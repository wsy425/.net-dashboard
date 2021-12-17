using System;

namespace Dashboard.BLOBConstant
{
    public class BlobFileConstant
    {
        public static int MaxFileSize = 150 * 1024 * 1024;
        
        public static int MaxFileSizeAsMegabytes = Convert.ToInt32(MaxFileSize / 1024 / 1024);
    }
}