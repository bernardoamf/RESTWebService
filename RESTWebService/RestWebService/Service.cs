using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Company;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace RestWebService
{
    public class Service : IHttpHandler
    {
        #region private Memebers
        
        //private string connString;

        #endregion
        
        #region Handler
        bool IHttpHandler.IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                string url = Convert.ToString(context.Request.Url);

                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        READ(context);
                        break;
                    case "POST":
                        CREATE(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Handler

        #region CRUD Functions

        private void READ(HttpContext context)
        {
            //HTTP Request - //http://server.com/virtual directory/employee?id={id}
            //http://localhost/RestWebService/employee

            int employeeCode = Convert.ToInt16(context.Request["id"]);
            CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();

            var employee = from e in db.Employees
                           where e.id = employeeCode
                           select e;
            string serializedEmployee = Serialize(employee);
            context.Response.ContentType = "text/xml";
            WriteResponse(serializedEmployee);
        }

        #endregion CRUD Functions

        #region Utility Functions

        private static void WriteResponse(string strMessage)
        {
            HttpContext.Current.Response.Write(strMessage);
        }

        private String Serialize(Employee emp)
        {
            String XmlizedString = null;
            XmlSerializer xs = new XmlSerializer(typeof(Company.Employee));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, emp);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        #endregion Utility Functions


    }
}
