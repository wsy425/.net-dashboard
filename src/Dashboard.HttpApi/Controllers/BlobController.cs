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
        [Route("web/{name}")]
        public async Task<FileResult> GetAsync(string name)
        {
            var file = await _blobsAppService.GetAsync(name);
            var extension = Path.GetExtension(name).RemovePreFix(".").ToLowerInvariant();
            return File(
                file.Bytes, 
                MimeTypes.GetByExtension(extension)
            );
        }

        [HttpGet]
        [Route("download/{name}")]
        public async Task<FileResult> DownloadAsync(string name)
        {
            var file = await _blobsAppService.GetAsync(name);
            var extension = Path.GetExtension(name).RemovePreFix(".").ToLowerInvariant();
            var type = GetFileExtensionType(extension);
            return File(
                file.Bytes, 
                type,
                name.Split("_")[1]
            );
        }

        [HttpDelete]
        public async Task<BlogDeleteDto> DeleteAsync(string name)
        {
            return await _blobsAppService.DeleteAsync(name);
        }

        [HttpGet]
        public List<string> GetListAsync(string name)
        {
            return _blobsAppService.GetListAsync(name);
        }
        
        // [HttpGet]
        // [Route("json/{name}")]
        // public async Task<FileResult> GetJsonAsync(string name)
        // {
        //     var file = await _blobsAppService.GetAsync(name);
        //     return File(
        //         file.Bytes,
        //         MimeTypes.Application.Json,
        //         name.Substring(14)
        //     );
        // }

        /**
         * 获取常用文件后缀
         */
        private string GetFileExtensionType(string extension)
        {
            switch (extension)
            {
                case "png":
                    return MimeTypes.Image.Png;
                case "jpg":
                case "jpeg":
                    return MimeTypes.Image.Jpeg;
                case "gif":
                    return MimeTypes.Image.Gif;
                case "pdf":
                    return MimeTypes.Application.Pdf;
                case "javascript":
                    return MimeTypes.Application.Javascript;
                case "xml":
                    return MimeTypes.Application.Xml;
                case "zip":
                    return MimeTypes.Application.Zip;
                case "mp4":
                    return MimeTypes.Video.Mp4;
                case "css":
                    return MimeTypes.Text.Css;
                case "csv":
                    return MimeTypes.Text.Csv;
                case "html":
                    return MimeTypes.Text.Html;
            }

            return MimeTypes.Application.OctetStream;
        }
    }
}