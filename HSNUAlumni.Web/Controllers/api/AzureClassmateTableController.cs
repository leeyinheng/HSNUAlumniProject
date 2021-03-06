﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HSNUAlumni.ModelLib;
using HSNUAlumni.DALLib;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using Geocoding;
using Geocoding.Google;

namespace HSNUAlumni.Web.Controllers.api
{
    public class AzureClassmateTableController : ApiController
    {
        private readonly DALLib.CloudTableOperation<Classmate> op = new DALLib.CloudTableOperation<Classmate>("classmates"); 
        
        [Route("api/classmates/{classId}")]
        [HttpGet]
        public IEnumerable<Classmate> GetClassmateList(string classId)
        {
            Helper.Guard.New().IsAuthentic().IsEmptyString(classId); 

            var list =   op.GetEntityListByPartiontionKey(classId).ToList();

            foreach(var item in list)
            {
                item.PhotoId = string.IsNullOrEmpty(item.PhotoId) ? "profile.png" : item.PhotoId; 
            }

            return list; 
        }

        [Route("api/classmates/recent")]
        [HttpGet]
        public IEnumerable<Classmate> GetRecentHSActive()
        {
            Helper.Guard.New().IsAuthentic();

            var list = op.GetAllContacts();

            // filter out by class Id 

            var items = list.Where(x => x.ClassId.StartsWith("6")).OrderByDescending(x => x.Timestamp).Take(10);

            return items; 
        }

        [Route("api/classmates/map")]
        [HttpGet]
        public IEnumerable<Classmate> GetHSMapUsers()
        {
            Helper.Guard.New().IsAuthentic();

            var list = op.GetAllContacts();

            var items = list.Where(x => x.ClassId.StartsWith("6") && x.HomeAddress != string.Empty).ToList();

            return items;
        }


        [Route("api/classmate/{classId}/{name}")]
        [HttpGet]
        public Classmate GetClassmateEntity(string classId, string name)
        {
            Helper.Guard.New().IsAuthentic().IsEmptyString(classId).IsEmptyString(name);

            return op.GetEntityByKeys(classId, name); 
        }

        [Route("api/classmate/AddorUpdate")]
        [HttpPost]
        public void AddOrUpdateClassmate(Classmate entity)
        {
            Helper.Guard.New().IsAuthentic();

            if (entity != null)
            {
                entity.PartitionKey = entity.ClassId;

                entity.RowKey = entity.Name;

                entity.ModifiedDate = DateTime.Now.ToShortDateString();

                entity.ModifiedUser = User.Identity.Name;

                var map = new GoogleMap();

                map.GetGeoCode(entity); 
                          
                op.AddorUpdateEntity(entity);

            }
         
        }

        
        [Route("api/classmates/AddorUpdateList")]
        [HttpPost]
        public void AddOrUpdateClassmate(List<Classmate> list)
        {
            Helper.Guard.New().IsAuthentic();

            foreach (var item in list)
            {
                item.ModifiedUser = User.Identity.Name;

                item.ModifiedDate = DateTime.Now.ToShortDateString(); 
            }

            op.AddorUpdateEntityList(list);
        }

        [Route("api/classmate/export")]
        [HttpPost]
        public HttpResponseMessage ExportToExcel(List<Classmate> list)
        {
            Helper.Guard.New().IsAuthentic();

            if (list.Count == 0) return null; 

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var grid = new GridView();
            grid.DataSource = list;
            grid.DataBind();
                        
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
                       
            byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());

            Stream stream = new MemoryStream(byteArray);
                        
            response.Content = new StreamContent(stream);

            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("content-disposition");

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/ms-excel");

            response.Content.Headers.ContentDisposition.FileName = "contacts.xls";

            return response;
        }

        [Route("api/classmate/delete/{classId}/{Name}")]
        [HttpPut]
        public void DeleteClassmate(string classId, string Name)
        {
            Helper.Guard.New().IsAuthentic().IsEmptyString(classId).IsEmptyString(Name); 

            op.DeleteEntityByKeys(classId,Name);
        }

        [Route("api/classmate/uploadphoto")]
        [HttpPost]
        public async Task<string> asyncUploadPhoto(HttpRequestMessage uploadedFile, string fileId)
        {

            Helper.Guard.New().IsAuthentic();

            if (!uploadedFile.Content.IsMimeMultipartContent()) return "no file";

            //string filename = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpg";

            string filename = fileId + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".jpg"; 
            
            var filePath = HttpContext.Current.Server.MapPath("~/Image/");

            var streamProvider = new MultipartFormDataStreamProvider(filePath);

            await uploadedFile.Content.ReadAsMultipartAsync(streamProvider);

            try
            {
                foreach (var file in streamProvider.FileData)
                {
                    Console.WriteLine(file.Headers.ContentDisposition.FileName);

                    if (System.IO.File.Exists(filePath + filename))
                    {
                        System.IO.File.Copy(file.LocalFileName, filePath + filename, true);

                        System.IO.File.Delete(file.LocalFileName); 
                    }
                    else
                    {
                        System.IO.File.Move(file.LocalFileName, filePath + filename);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          

            return filename;

        }

        [Route("api/classmate/vcard")]
        [HttpPost]
        public HttpResponseMessage vcard(Classmate entity)
        {
            Helper.Guard.New().IsAuthentic();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            string filepath = HttpContext.Current.Server.MapPath("~/Image/" + entity.PhotoId);

            var bytes = op.GenerateVcard(entity, filepath);

            Stream stream = new MemoryStream(bytes); 
            
            response.Content = new StreamContent(stream);

            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("content-disposition");
               
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/x-vcard");

            response.Content.Headers.ContentDisposition.FileName = "contact.vcf";

            return response;


        }
    }
}
