using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using RabbitMQ;

namespace UploaderParserApi.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class XmlParserController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        public XmlParserController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{name}")]
        [ActionName("GetFile")]
        public ActionResult<string> GetFile(string name)
        {
            try
            {
                var contents = _fileProvider.GetDirectoryContents("Uploads").ToList();
                var file = contents.FirstOrDefault(x => x.Name == name);

                XmlDocument doc = new XmlDocument();
                doc.Load(file.PhysicalPath);
                RemoveAllNamespaces(doc.InnerXml);
                string measurementUnit = doc.GetElementsByTagName("MeasurementUnit")[0].InnerText;
                string processingDateTime = doc.GetElementsByTagName("processingDateTime")[0].InnerText;
                string softwareCreator = doc.GetElementsByTagName("softwareCreator")[0].InnerText;
                string softwareName = doc.GetElementsByTagName("softwareCreator")[0].InnerText;
                string softwareVersion = doc.GetElementsByTagName("softwareVersion")[0].InnerText;
                string text = name.ToLower().Replace(".xml", "") + ":::" + "<Description><MeasurementUnit> " + measurementUnit + " </MeasurementUnit><processingDateTime>" + processingDateTime + "</processingDateTime><softwareCreator>" + softwareCreator + "</softwareCreator><softwareName>" + softwareName + "</softwareName><softwareVersion>" + softwareVersion + "</softwareVersion></Description>";


                var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
                IConfigurationSection section = config.GetSection("RabbitMqConnection");

                RabbitMQPublisher mq = new RabbitMQPublisher();
                mq.SendMessage(section, text);

                return text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [ActionName("GetAllFiles")]
        public ActionResult<List<string>> GetAllFiles()
        {
            try
            {
                var contents = _fileProvider.GetDirectoryContents("Uploads").ToList();
                List<string> files = new List<string>();
                foreach (IFileInfo info in contents)
                {
                    files.Add(info.Name);
                }
                return files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [ActionName("UploadFile")]
        public async Task<HttpResponseMessage> UploadFile(IFormFile file)
        {
            var resp = new HttpResponseMessage();
            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    foreach (var formFile in form.Files)
                    {
                        string[] split = formFile.FileName.Split('\\');
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "Uploads",
                                    Guid.NewGuid() + "_" + split[split.Length - 1]);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);
                        }
                    }
                }
                HttpContent content = new StringContent("File Uploaded Successfully");
                resp.Content = content;
                resp.StatusCode = System.Net.HttpStatusCode.OK;
                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        
        
    }
}
