using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.ModelLib
{
    public class VCard
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string ImageLink { get; set; }
    }
}
