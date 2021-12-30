using System;
using Dashboard.BLOBConstant;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Dashboard.BLOBEntity
{
    public class File : Entity<Guid>
    {
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public Guid UserId { get; set; }
        public byte[] Size { get; set; }
        
        public File(Guid id,[NotNull]string name,[NotNull]string url,Guid userId,[NotNull]byte[] size)
            : base(id)
        {
            Name = name;
            WebUrl = url;
            UserId = userId;
            Size = CheckContentLength(size);
        }

        protected File()
        {
            
        }

        private byte[] CheckContentLength(byte[] size)
        {
            Check.NotNull(size, nameof(size));
            if (size.Length >= BlobFileConstant.MaxFileSize)
            {
                throw new AbpException($"Blob content size cannot be more than {BlobFileConstant.MaxFileSizeAsMegabytes} MB.");
            }

            return size;
        }
    }
}