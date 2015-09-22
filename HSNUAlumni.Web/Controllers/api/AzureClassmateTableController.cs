using System;
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

namespace HSNUAlumni.Web.Controllers.api
{
    public class AzureClassmateTableController : ApiController
    {
        private readonly DALLib.CloudTableOperation<Classmate> op = new DALLib.CloudTableOperation<Classmate>("classmates"); 
        
        [Route("api/classmates/{classId}")]
        [HttpGet]
        public IEnumerable<Classmate> GetClassmateList(string classId)
        {
            var list =   op.GetEntityListByPartiontionKey(classId).ToList();

            foreach(var item in list)
            {
                item.PhotoId = string.IsNullOrEmpty(item.PhotoId) ? "profile.png" : item.PhotoId; 
            }

            return list; 
        }

        [Route("api/classmate/{classId}/{name}")]
        [HttpGet]
        public Classmate GetClassmateEntity(string classId, string name)
        {
            return op.GetEntityByKeys(classId, name); 
        }

        [Route("api/classmate/AddorUpdate")]
        [HttpPost]
        public void AddOrUpdateClassmate(Classmate entity)
        {
            if (entity != null)
            {
                entity.PartitionKey = entity.ClassId;

                entity.RowKey = entity.Name;

                entity.ModifiedDate = DateTime.Now.ToShortDateString();

                entity.ModifiedUser = User.Identity.Name; 

                op.AddorUpdateEntity(entity);

            }
         
        }

        [Route("api/classmates/AddorUpdateList")]
        [HttpPost]
        public void AddOrUpdateClassmate(List<Classmate> list)
        {
            foreach(var item in list)
            {
                item.ModifiedUser = User.Identity.Name;

                item.ModifiedDate = DateTime.Now.ToShortDateString(); 
            }

            op.AddorUpdateEntityList(list);
        }

        [Route("api/classmate/delete/{classId}/{Name}")]
        [HttpPut]
        public void DeleteClassmate(string classId, string Name)
        {
            op.DeleteEntityByKeys(classId,Name);
        }

        [Route("api/classmate/uploadphoto")]
        [HttpPost]
        public async Task<string> asyncUploadPhoto(HttpRequestMessage uploadedFile, string fileId)
        {
            
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
    }
}
