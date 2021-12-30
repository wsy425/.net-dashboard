using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.BLOBEntity;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace Dashboard.BLOBServices
{
    [RemoteService(false)]
    public class FileService : ApplicationService,IFileService
    {
        private readonly IRepository<File, Guid> _repository;

        public FileService(
            IRepository<File, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<BlobUploadResultDto> CreateAsync(BlobUploadInputDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                throw new AbpValidationException("Bytes of pictures can not be null or empty!");
            }

            var filter = (await _repository.GetListAsync())
                .Where(file=>file.Name.Equals(input.Name)).ToList();
            if (filter.Count != 0)
            {
                throw new AbpValidationException("Duplicate file name!");
            }

            var userId = CurrentUser.Id ?? Guid.Empty;
            var url = "api/dashboard/blob/file/web/" + input.Name;
            await _repository.InsertAsync(
                new File(GuidGenerator.Create(), input.Name, url, userId, input.Bytes));
            return new BlobUploadResultDto
            {
                Name = input.Name,
                WebUrl = url
            };
        }

        public async Task<BlobFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            var res = await _repository.GetAsync(f => f.Name == name);
            if (res == null)
            {
                throw new AbpValidationException("File is not existed!");
            }

            return new BlobFileDto
            {
                Bytes = res.Size
            };
        }
        
        public async Task<BlogDeleteDto> DeleteAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            await _repository.DeleteAsync(f => f.Name == name);
            return new BlogDeleteDto
            {
                IsDeleteOk = true,
                Name = name
            };
        }

        public async Task<List<string>> GetListAsync(string name)
        {
            var files = await _repository.GetListAsync();
            var result = new List<string>();
            if (files.Count==0)
            {
                return result;
            }
            result = files.Select(file => file.Name).ToList();
            return result;
        }
    }
}