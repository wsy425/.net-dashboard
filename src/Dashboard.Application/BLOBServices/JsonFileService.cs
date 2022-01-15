using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.BLOB;
using Dashboard.BLOB.Dto;
using Dashboard.Parameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOBServices
{
    [RemoteService(false)]
    public class JsonFileService : ApplicationService,IJsonFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JsonFileService> _logger;
        private static int _count = 1;
        private static int _sub = 1;

        public JsonFileService(
            IConfiguration configuration, 
            ILogger<JsonFileService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<FileBaseDto> CreateAsync(List<string> data)
        {
            var directory = _configuration["Blobs:errors"];
            var file = EnsureDirectory(directory);
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

        public FileBaseDto CreateAndUpdateAsync(Config config)
        {
            try
            {
                JsonConvert.DeserializeObject<Dictionary<string,Object>>(config.Content);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new FileBaseDto
                {
                    Code = 400,
                    Info = "传入参数有误，请检查参数格式"
                };
            }
            var directory = _configuration["Blobs:files"] + "\\" + "configs";
            EnsureDirectory(directory);
            var filePath = directory + "\\" + config.ConfigName.ToLowerInvariant() + ".txt";
            using var file = File.CreateText(filePath);
            var serializer = new JsonSerializer();
            serializer.Serialize(file, config.Content);
            return new FileBaseDto
            {
                Code = 200,
                Info = "配置文件更新成功"
            };
        }

        public BlobUploadInputDto DownLoadAsync()
        {
            var directory = _configuration["Blobs:errors"];
            var file = EnsureDirectory(directory);
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

        public string GetConfigAsync(string name)
        {
            var directory = _configuration["Blobs:files"] + "\\" + "configs";
            EnsureDirectory(directory);
            var file = new DirectoryInfo(directory).GetFiles().FirstOrDefault(file => file.Name.Contains(name));
            if (file != null)
            {
                var content = JsonConvert.DeserializeObject<string>(File.ReadAllText(file.FullName));
                return content;
            }

            return null;
        }

        private string EnsureDirectory(string directory)
        {
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