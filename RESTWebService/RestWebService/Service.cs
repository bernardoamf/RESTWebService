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
using System.Data.Linq;


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
                    case "PUT":
                        UPDATE(context);
                        break;
                    case "DELETE":
                        DELETE(context);
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

            var employee = (from e in db.Employees
                           where e.Id == employeeCode
                           select e).First();

            Company.CompanyEmployee emp = new CompanyEmployee();

            emp.FirstName = employee.FirstName;
            emp.LastName = employee.LastName;
            emp.EmpCode = employee.Id;
            emp.Designation = employee.Designation;

            string serializedEmployee = Serialize(emp);
            context.Response.ContentType = "text/xml";
            WriteResponse(serializedEmployee);
        }

        private void CREATE(HttpContext context)
        {
            CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();
            byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
            string str = Encoding.UTF8.GetString(PostData);
            CompanyEmployee e = Deserialize(PostData);

            Employee emp = new Employee();
            emp.FirstName = e.FirstName;
            emp.LastName = e.LastName;
            emp.Id = e.EmpCode;
            emp.Designation = e.Designation;
            db.Employees.InsertOnSubmit(emp);
            db.SubmitChanges();

        }

        private void UPDATE(HttpContext context)
        {
            try
            {
                CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();
                byte[] PUTRequestByte = context.Request.BinaryRead(context.Request.ContentLength);
                context.Response.Write(PUTRequestByte);

                Company.CompanyEmployee emp = Deserialize(PUTRequestByte);


                var employee = (from e in db.Employees
                                where e.Id == emp.EmpCode
                                select e).First();

                employee.FirstName = emp.FirstName;
                employee.LastName = emp.LastName;
                employee.Designation = emp.Designation;

                db.SubmitChanges();

                WriteResponse("Employee Updated Successfully");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void DELETE(HttpContext context)
        {
            try
            {
                CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();
                int EmpCode = Convert.ToInt16(context.Request["id"]);
                var employee = (from e in db.Employees
                                where e.Id == EmpCode
                                select e);
                db.Employees.DeleteAllOnSubmit(employee);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion CRUD Functions

        #region Utility Functions

        private Company.CompanyEmployee Deserialize(byte[] xmlByteData)
        {
            try
            {
                XmlSerializer ds = new XmlSerializer(typeof(Company.Employee));
                MemoryStream memoryStream = new MemoryStream(xmlByteData);
                CompanyEmployee emp = new CompanyEmployee();
                emp = (CompanyEmployee)ds.Deserialize(memoryStream);
                return emp;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private static void WriteResponse(string strMessage)
        {
            HttpContext.Current.Response.Write(strMessage);
        }

        private String Serialize(CompanyEmployee emp)
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
