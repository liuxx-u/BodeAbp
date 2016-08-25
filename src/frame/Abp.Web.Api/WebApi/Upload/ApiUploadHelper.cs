using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abp.WebApi.Upload
{
    /// <summary>
    /// api文件上传工具类
    /// </summary>
    public static class ApiUploadHelper
    {
        private static readonly string ServerHost = ConfigurationManager.AppSettings["ServerHost"];

        /// <summary>
        /// api文件上传
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<List<string>> Upload(HttpContent content)
        {
            //文件存储地址
            string path = string.Format("UploadFile/{0}/", DateTime.Today.ToString("yyyyMMdd"));
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + path;

            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            // 采用MultipartMemoryStreamProvider
            List<string> files = new List<string>();
            var provider = new CustomMultipartFormDataStreamProvider(dirPath);
            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.  
                await content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    var serverPath = ServerHost + path + Path.GetFileName(file.LocalFileName);
                    files.Add(serverPath);
                }
                return files;
            }
            catch { return new List<string>(); }
        }
    }
}
