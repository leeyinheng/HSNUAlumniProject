using HSNUAlumni.ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.DALLib
{
    public class AddAgreement
    {
        private readonly DALLib.CloudTableOperation<Agreement> op = new DALLib.CloudTableOperation<Agreement>("agreement");

        public void Add(Agreement item)
        {
            op.AddorUpdateEntity(item); 
        }
    }
}
