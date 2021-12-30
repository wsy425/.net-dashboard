using System;

namespace Dashboard.BLOBConstant
{
    public static class BlobFileConstant
    {
        /// <summary>
        /// Default value: 15MB
        /// </summary>
        public const int MaxFileSize = 15 * 1024 * 1024;

        public static readonly int MaxFileSizeAsMegabytes = Convert.ToInt32(MaxFileSize / 1024 / 1024);
    }
}