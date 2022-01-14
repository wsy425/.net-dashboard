using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.Parameters;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOBServices
{
    [RemoteService(false)]
    public class JsonFileService : ApplicationService,IJsonFileService
    {
        private readonly IConfiguration _configuration;
        private static int _count = 1;
        private static int _sub = 1;

        public JsonFileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FileBaseDto> CreateAsync(List<string> data)
        {
            var file = EnsureDirectory();
            file = file + "/error_" + _sub + ".txt";
            if (!File.Exists(file))
            {
                File.CreateText(file);
            }
            var res = new FileBaseDto();
            try
            {
                // 创建一个 StreamWriter 的实例来写入文件 
                // using 语句也能关闭 StreamReader
                using (var sr = new StreamWriter(file,true))
                {
                    foreach (var col in data)
                    {
                        await sr.WriteLineAsync(col);
                    }
                }
                // 计数存入文件次数
                // 满1000次则新开一个txt文件存储
                _count += 1;
                if (_count == 1000)
                {
                    _sub += 1;
                    _count = 0;
                }

                res.Code = 200;
                res.Info = "数据存储成功";
            }
            catch (Exception e)
            {
                res.Code = 400;
                res.Info = e.Message;
            }

            return res;
        }

        public BlobUploadInputDto DownLoadAsync()
        {
            var file = EnsureDirectory();
            file = file + "/error_" + _sub + ".txt";
            byte[] fileArray;
            try
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                var r = new BinaryReader(fs);
                fileArray = r.ReadBytes((int)fs.Length);
            }
            catch (Exception e)
            {
                throw new FileLoadException(e.Message);
            }
            return new BlobUploadInputDto
            {
                Bytes = fileArray,
                Name = _sub + ".txt"
            };
        }
        
        private string EnsureDirectory()
        {
            var directory = _configuration["Blobs:errors"];
            if (directory.EndsWith("/"))
            {
                directory.RemovePostFix("/");
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}