using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.BLOBConstant;
using Dashboard.BLOBEntity;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Validation;

namespace Dashboard.BLOBService
{
    [RemoteService(false)]
    public class BlobService : ApplicationService, IBlobService
    {
        private readonly IBlobContainer<ProfilePictureContainer> _blobContainer;
        private readonly IRepository<Blob, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IConfiguration _configuration;

        public BlobService(
            IConfiguration configuration, 
            IGuidGenerator guidGenerator, 
            IBlobContainer<ProfilePictureContainer> blobContainer)
        {
            _configuration = configuration;
            _guidGenerator = guidGenerator;
            _blobContainer = blobContainer;
        }

        public async Task<BlobUploadResultDto> CreateAsync(BlobUploadInputDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                throw new AbpValidationException("Bytes of pictures can not be null or empty!");
            }
            
            if (input.Bytes.Length > BlobFileConstant.MaxFileSize)
            {
                throw new AbpValidationException(
                    $"File exceeds the maximum upload size {BlobFileConstant.MaxFileSizeAsMegabytes} MB !");
            }

            var uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss")
                                 + Path.GetFileNameWithoutExtension(input.Name)
                                 + Path.GetExtension(input.Name);

            var fileUrl = "api/blobs/files/web/" + uniqueFileName;

            await _blobContainer.SaveAsync(uniqueFileName,input.Bytes);

            await _repository.InsertAsync(new Blob
                {
                    Name = uniqueFileName,
                    WebUrl = fileUrl
                }
            );

            return new BlobUploadResultDto
            {
                Name = uniqueFileName,
                WebUrl = fileUrl
            };
        }

        public async Task<BlobFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            return new BlobFileDto
            {
                Bytes = await _blobContainer.GetAllBytesOrNullAsync(name) 
            };
        }

        public async Task<BlogDeleteDto> DeleteAsync(string name)
        {
            var isOk = await _blobContainer.DeleteAsync(name);
            await _repository.DeleteAsync(e => e.Name == name);
            return new BlogDeleteDto
            {
                Name = name,
                IsDeleteOk = isOk
            };
        }

        public List<string> GetListAsync()
        {
            var filePath = _configuration["Blobs:Picture"] + "/host";
            var directoryInfo = new DirectoryInfo(filePath);

            return directoryInfo.GetFiles().Select(file => file.Name).ToList();
        }
    }
}