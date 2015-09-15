using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSNUAlumni.ModelLib;
using System.Collections.Generic;

namespace HSNUAlumni.Test
{
    [TestClass]
    public class TestClassmateTable
    {
        [TestMethod]
        public  void TestClassmateOperation()
        {
            var newClassmate = new Classmate("618", "Ken")
            {
                Gender = true,       
                CellPhone = "901-990-0011", 
                Status = ClassmateStatus.StayInTouch.ToString(), 
                Notes = "la la lal al al la l l"

            };

            
            

           // newClassmate.CellPhone = "832-372-7188";
           // newClassmate.Company = "Loomis";
           //// newClassmate.Status = ClassmateStatus.StayInTouch;
           // newClassmate.SeatNumber = 75;

            CustomerEntity customer = new CustomerEntity("333", "test123")
            {
                Email = "Walter@contoso.com",
                PhoneNumber = "425-555-0101", 
                Test = "test"
            };

            var op = new HSNUAlumni.DALLib.CloudTableOperation<HSNUAlumni.ModelLib.Classmate>("classmates");

            op.AddorUpdateEntity(newClassmate); 

        }


        [TestMethod]
        public void TestInsertClassmateList()
        {
            var list = new List<ModelLib.Classmate>();

            list.Add(new Classmate("618", "AAA") { Birthday = "08/09/1973", Gender = false });
            list.Add(new Classmate("618", "BBB") { Birthday = "08/09/1973", Gender = false });
            list.Add(new Classmate("618", "CCC") { Birthday = "08/09/1973", Gender = false });
            list.Add(new Classmate("618", "DDD") { Birthday = "08/09/1973", Gender = false });
            list.Add(new Classmate("618", "EEE") { Birthday = "08/09/1973", Gender = false });

            var op = new DALLib.CloudTableOperation<HSNUAlumni.ModelLib.Classmate>("classmates");

            op.AddorUpdateEntityList(list); 
        }

       



    }
}
