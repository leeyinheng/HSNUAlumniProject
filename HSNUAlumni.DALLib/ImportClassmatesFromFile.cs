using Excel;
using HSNUAlumni.ModelLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSNUAlumni.DALLib
{
    public class ImportClassmatesFromFile
    {
        private readonly DALLib.CloudTableOperation<Classmate> op = new DALLib.CloudTableOperation<Classmate>("classmates");

        public void ImportData(string filepath, string user = "system")
        {
            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);

                //1. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                //2. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                var list = new List<Classmate>();

                var table = result.Tables[0];

                var columns = table.Columns;

                //3. Data Reader methods
                foreach (DataRow row in table.Rows)
                {
                    var item = new Classmate();

                    item.PartitionKey = row[columns["ClassId"]].ToString();

                    item.RowKey = row[columns["Name"]].ToString();

                    item.ClassId = row[columns["ClassId"]].ToString();

                    item.Name = row[columns["Name"]].ToString();

                    item.EnglishName = row[columns["EnglishName"]].ToString();

                    item.Birthday = row[columns["Birthday"]].ToString();

                    item.CellPhone = row[columns["CellPhone"]].ToString();

                    item.Company = row[columns["Company"]].ToString();

                    item.Email = row[columns["Email"]].ToString();

                    item.FaceBookAccount = row[columns["FaceBookAccount"]].ToString();

                    item.Gender = Convert.ToBoolean(row[columns["Gender"]].ToString());

                    item.HomeAddress = row[columns["HomeAddress"]].ToString();

                    item.HomePhone = row[columns["HomePhone"]].ToString();

                    item.LineAccount = row[columns["LineAccount"]].ToString();

                    item.Notes = row[columns["Notes"]].ToString();

                    item.ModifiedUser = user;

                    item.ModifiedDate = DateTime.Now.ToShortDateString();

                    item.OfficeAddrss = row[columns["OfficeAddrss"]].ToString();

                    item.OfficePhone = row[columns["OfficePhone"]].ToString();

                    item.Position = row[columns["Position"]].ToString();

                    item.SeatNumber = Convert.ToInt16(row[columns["SeatNumber"]].ToString());

                    item.Status = row[columns["Status"]].ToString();

                    list.Add(item);
                }

                //4. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                if (list.Count() > 0)
                {
                    //remove previous data 
                    op.DeleteEntityBySingleKey(list[0].PartitionKey); 
                    //insert new data 
                    op.AddorUpdateEntityList(list);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
