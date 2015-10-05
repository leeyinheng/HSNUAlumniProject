using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
namespace HSNUAlumni.Web.Helper
{
    public class Guard
    {
        private static Guard _guard; 

        public static Guard New()
       {
            _guard = new Guard();

            return _guard; 
       }
        
        public Guard  IsAuthentic()
        {
            if (System.Security.Principal.GenericPrincipal.Current.Identity.IsAuthenticated == false)
            {
                throw new Exception("Not Authenticated!"); 
            }

            return _guard; 
           
        }

        public Guard IsEmptyString(string entity)
        {
            if (string.IsNullOrEmpty(entity))
            {
                throw new Exception("Contains Empty String"); 
            }

            return _guard; 
        }

        
    }
}