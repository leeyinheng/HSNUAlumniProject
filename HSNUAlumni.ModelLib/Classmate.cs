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

        public string Lat { get; set; }

        public string Lng { get; set; }
    }

    public enum ClassmateStatus
    {
        StayInTouch,
        Disconnected,
        Deceased
    }

    public interface IClassmate
    {
        string ClassId { get; set; }

        string SeatNumber { get; set; }

        string Name { get; set; }

        string EnglishName { get; set; }

        string Gender { get; set; }

        string Status { get; set; }

        string Birthday { get; set; }

        string Notes { get; set; }

        string CellPhone { get; set; }

        string HomePhone { get; set; }

        string OfficePhone { get; set; }

        string Email { get; set; }

        string FaceBookAccount { get; set; }

        string LineAccount { get; set; }

        string Company { get; set; }

        string Position { get; set; }

        string HomeAddress { get; set; }

        string OfficeAddrss { get; set; }

        string PhotoId { get; set; }

        string ModifiedUser { get; set; }

        string ModifiedDate { get; set; }
               

    }
}
