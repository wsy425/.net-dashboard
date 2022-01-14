using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.Parameters;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Http;

namespace Dashboard.Controllers
{
    [Route("api/dashboard/json")]
    [RemoteService(Name = "Json")]
    public class JsonFileController : DashboardController
    {
        private readonly IJsonFileService _service;

        public JsonFileController(IJsonFileService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        public async Task<FileBaseDto> CreateAsync(List<string> data)
        {
            return await _service.CreateAsync(data);
        }

        [HttpGet]
        [Route("download")]
        public FileResult DownLoadAsync()
        {
            var load = _service.DownLoadAsync();
            return File(load.Bytes, MimeTypes.Text.RichText, load.Name);
        }
    }
}