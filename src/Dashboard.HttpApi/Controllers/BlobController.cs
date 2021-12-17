using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace Dashboard.Controllers
{
    [Route("api/dashboard/blob/files")]
    public class BlobController : DashboardController
    {
        private readonly IBlobService _blobsAppService;

        public BlobController(IBlobService blobsAppService)
        {
            _blobsAppService = blobsAppService;
        }

        [HttpPost]
        public async Task<BlobUploadResultDto> CreateAsync(IFormFile file)
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var result = await _blobsAppService.CreateAsync(new BlobUploadInputDto
            {
                Name = file.FileName,
                Bytes = stream.ToArray()
            });
            
            return result;
        }

        [HttpGet]
        public async Task<FileResult> GetAsync(string name)
        {
            var file = await _blobsAppService.GetAsync(name);
            var extension = Path.GetExtension(name).RemovePreFix(".").ToLowerInvariant();
            string type = extension.Equals("pdf") ? MimeTypes.Application.Pdf : MimeTypes.GetByExtension(Path.GetExtension(name));
            return File(
                file.Bytes, 
                type,
                name.Substring(14)
            );
        }

        [HttpDelete]
        public async Task<BlogDeleteDto> DeleteAsync(string name)
        {
            return await _blobsAppService.DeleteAsync(name);
        }

        public List<string> GetListAsync()
        {
            return _blobsAppService.GetListAsync();
        }
        
        [HttpGet]
        [Route("json/{name}")]
        public async Task<FileResult> GetJsonAsync(string name)
        {
            var file = await _blobsAppService.GetAsync(name);
            return File(
                file.Bytes,
                MimeTypes.Application.Json,
                name.Substring(14)
            );
        }
    }
}