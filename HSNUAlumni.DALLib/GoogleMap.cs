using Geocoding;
using Geocoding.Google;
using HSNUAlumni.ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.DALLib
{
    public class GoogleMap
    {
        public void GetGeoCode(Classmate entity)
        {
            if (entity != null && entity.HomeAddress == string.Empty)
            {
                entity.Lat = string.Empty;
                entity.Lng = string.Empty;
                return;
            }
            System.Threading.Thread.Sleep(200); // wait a bit to avoid over limit error 

            try
            {
                IGeocoder geocoder = new GoogleGeocoder();
                IEnumerable<Address> addresses = geocoder.Geocode(entity.HomeAddress);

                if (addresses.Count() > 0)
                {
                    entity.Lat = addresses.First().Coordinates.Latitude.ToString();
                    entity.Lng = addresses.First().Coordinates.Longitude.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
