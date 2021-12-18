using System;

namespace Dashboard.BLOBConstant
{
    public class BlobFileConstant
    {
        public const int MaxFileSize = 150 * 1024 * 1024;

        public static readonly int MaxFileSizeAsMegabytes = Convert.ToInt32(MaxFileSize / 1024 / 1024);
    }
}