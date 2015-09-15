using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HSNUAlumni.Test
{
    [TestClass]
    public class TestGetMethods
    {
        [TestMethod]
        public void TestGetClassmate()
        {
            var op = new HSNUAlumni.DALLib.CloudTableOperation<HSNUAlumni.ModelLib.Classmate>("classmates");

            var r = op.GetEntityByKeys("618", "Ken"); 

            Assert.AreEqual("618", r.ClassId);

        }


        [TestMethod]
        public void TestGetClassmateList()
        {
            var op = new HSNUAlumni.DALLib.CloudTableOperation<HSNUAlumni.ModelLib.Classmate>("classmates");

            var list = op.GetEntityListByPartiontionKey("618"); 

            
        }
    }

  
}
