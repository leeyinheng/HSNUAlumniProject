using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HSNUAlumni.ModelLib;
using HSNUAlumni.DALLib; 

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
            op.AddorUpdateEntity(entity); 
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
    }
}
