using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.ModelLib
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class Agreement : TableEntity
    {
        public Agreement()
        {

        }

        public Agreement (string username)
        {
            this.PartitionKey = username;
            this.RowKey = DateTime.Now.ToLongDateString() + "|" + DateTime.Now.ToLongTimeString();
        }

        public string ClassId { get; set; }
    }
}
