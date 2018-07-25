using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XmlToCSV.Models;

namespace XmlToCSV.Services
{

    public interface IParserService
    {
        Task<bool> UploadFile(IFormFile file);
        Task<bool> Parse(string name);
        Task<List<FileView>> GetFiles();
    }
}
