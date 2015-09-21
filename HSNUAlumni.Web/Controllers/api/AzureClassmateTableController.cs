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
            return op.GetEntityListByPartiontionKey(classId); 
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

                op.AddorUpdateEntity(entity);

            }
         
        }

        [Route("api/classmates/AddorUpdateList")]
        [HttpPost]
        public void AddOrUpdateClassmate(List<Classmate> list)
        {
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
        public async Task<string> asyncUploadPhoto(HttpRequestMessage uploadedFile)
        {

            if (!uploadedFile.Content.IsMimeMultipartContent()) return "no file";
                                

            string filename = String.Format("{0:yyyyMMddHHmm}", DateTime.Now) + ".jpg";

            var filePath = HttpContext.Current.Server.MapPath("~/Image/");

            var streamProvider = new MultipartFormDataStreamProvider(filePath);

            await uploadedFile.Content.ReadAsMultipartAsync(streamProvider);

            // var bianrystring = streamProvider.FormData[0];

            //   MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(bianrystring));

            //  using (FileStream file = new FileStream(filePath + filename, FileMode.Create, FileAccess.Write))
            //{
            //    mStrm.WriteTo(file);
            //}

            foreach (var key in streamProvider.FormData.AllKeys)
            {
                foreach (var val in streamProvider.FormData.GetValues(key))
                {
                    Console.WriteLine(string.Format("{0}: {1}\n", key, val));
                }
            }

            foreach (var file in streamProvider.FileData)
            {
                Console.WriteLine(file.Headers.ContentDisposition.FileName);
                //using (FileStream filestr = new FileStream(filePath + filename, FileMode.Create, FileAccess.Write))
                //{
                //    await file.CopyToAsync(filestr);
                //}

                  System.IO.File.Move(file.LocalFileName, filePath + filename); 
            }

            return filename;

        }
    }
}
