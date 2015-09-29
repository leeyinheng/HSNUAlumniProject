using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.ModelLib
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class Classmate :  TableEntity
    {
        public Classmate()
        {

        }

        public Classmate(string classId, string name)
        {
            this.PartitionKey = classId;
            this.RowKey = name;
            this.ClassId = classId;
            this.Name = name; 
        }

        public string ClassId { get; set; }

        public string SeatNumber { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public string Gender { get; set; }

        public string Status { get; set; }

        public string Birthday { get; set; }

        public string Notes { get; set; }

        public string CellPhone { get; set; }

        public string HomePhone { get; set; }

        public string OfficePhone { get; set; }

        public string Email { get; set; }

        public string FaceBookAccount { get; set; }

        public string LineAccount { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public string HomeAddress { get; set; }

        public string OfficeAddrss { get; set; }

        public string PhotoId { get; set; }

        public string ModifiedUser { get; set; }

        public string ModifiedDate { get; set;}

        public bool Hide { get; set; }
    }

    public enum ClassmateStatus
    {
        StayInTouch,
        Disconnected,
        Deceased
    }
}
