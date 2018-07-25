using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XmlToCSV.Models;

namespace XmlToCSV.Services
{
    public class ParserService : IParserService
    {
        private HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public ParserService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();
            _remoteServiceBaseUrl = config.GetSection("XmlParserApi")["uri"];
        }

        public async Task<bool> Parse(string name)
        {
            var uri = $"{_remoteServiceBaseUrl}" + "/GetFile/" + name;
            var responseString = await _httpClient.GetStringAsync(uri);
            if (!string.IsNullOrEmpty(responseString))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                var uri = $"{_remoteServiceBaseUrl}" + "/UploadFile";
                byte[] data;
                using (var br = new BinaryReader(file.OpenReadStream()))
                    data = br.ReadBytes((int)file.OpenReadStream().Length);

                ByteArrayContent bytes = new ByteArrayContent(data);


                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", file.FileName);

                var responseString = _httpClient.PostAsync(uri, multiContent).Result;

                if (responseString.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FileView>> GetFiles()
        {
            List<FileView> files = new List<FileView>();
            var uri = $"{_remoteServiceBaseUrl}" + "/GetAllFiles";
            var responseString = await _httpClient.GetStringAsync(uri);
            if (!string.IsNullOrEmpty(responseString))
            {
                string[] fileNames = responseString.TrimStart('[').TrimEnd(']').Split(',');
                for (int i = 0; i < fileNames.Length; i++)
                {
                    FileView fView = new FileView();
                    fView.FileName = fileNames[i].TrimStart('"').TrimEnd('"');
                    files.Add(fView);
                }
            }
            return files;
        }
    }
}
