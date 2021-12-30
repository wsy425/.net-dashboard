using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.BLOBConstant;
using Dashboard.BLOBEntity;
using Dashboard.ServiceExtensions;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Dashboard.BLOBServices
{
    [RemoteService(false)]
    public class BlobService : ApplicationService, IBlobService
    {
        private readonly IBlobContainer<ProfileBackgroundContainer> _blobContainer;
        private readonly IRepository<Blob, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IConfiguration _configuration;

        public BlobService(
            IConfiguration configuration, 
            IGuidGenerator guidGenerator, 
            IBlobContainer<ProfileBackgroundContainer> blobContainer, 
            IRepository<Blob, Guid> repository)
        {
            _configuration = configuration;
            _guidGenerator = guidGenerator;
            _blobContainer = blobContainer;
            _repository = repository;
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
                                 + "_"
                                 + Path.GetFileNameWithoutExtension(input.Name)
                                 + Path.GetExtension(input.Name);

            var fileUrl = "api/dashboard/blob/files/web/" + uniqueFileName;

            await _blobContainer.SaveAsync(uniqueFileName,input.Bytes);

            await _repository.InsertAsync(new Blob(_guidGenerator.Create())
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

        [UnitOfWork]
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
        /// <summary>
        /// 根据name的值返回包含其值的图片名及链接，若name为null则返回所有带png或jgp的图片
        /// </summary>
        /// <param name="field">需要图片包含的名称字段</param>
        /// <returns></returns>
        public async Task<List<GetBlobsDto>> GetBackGroundListAsync(string field)
        {
            var resultList = await _repository.GetListAsync();
            var resultDto = new List<GetBlobsDto>();
            var pngOrJgpEntity = resultList.Where(b => 
                b.Name.RemoveSeparateCode().IsContainPngOrJpgName(field)).ToList();
            if (pngOrJgpEntity.Count == 0)
            {
                return resultDto;
            }

            return ObjectMapper.Map(pngOrJgpEntity, resultDto);
        }
    }
}