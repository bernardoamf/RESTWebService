using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company;
using System.Data.Linq;
using System.Net;
using System.IO;
using System.Xml;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get Employee #1");
            GetRequest(1);
            Console.WriteLine();
            Console.WriteLine("Add new Employee");
            PostRequest("Juan", "Munoz", "CFO");
            Console.WriteLine();
            /*Console.WriteLine("Modify Employee");
            PUTRequest("Juan", "Munoz-Navarro", "CMO","3");
            Console.WriteLine();
            Console.WriteLine("Delete Employee");
            DeleteRequest(2);
            Console.WriteLine();*/
            Console.ReadLine();

        }

        static void testDC(string firstName, string lastName, string designation)
        {
            CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();

            Employee emp = new Employee();
            emp.FirstName = firstName;
            emp.LastName = lastName;
            emp.Designation = designation;
            db.Employees.InsertOnSubmit(emp);
            db.SubmitChanges();
        }

        static void GetRequest(int employeeID)
        {
            string url = "http://localhost/restwebservice/employee?id=" + employeeID.ToString();
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(url);
            GETRequest.Method = "GET";

            Console.WriteLine("Sending GET Request");
            HttpWebResponse GETResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream GETResponseStream = GETResponse.GetResponseStream();
            StreamReader sr = new StreamReader(GETResponseStream);

            Console.WriteLine("Response from server");
            Console.WriteLine(sr.ReadToEnd());
            Console.ReadLine();
        }

        static void DeleteRequest(int employeeID)
        {
            string url = "http://localhost/restwebservice/employee?id=" + employeeID.ToString();
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(url);
            GETRequest.Method = "DELETE";

            Console.WriteLine("Sending GET Request");
            HttpWebResponse GETResponse = (HttpWebResponse)GETRequest.GetResponse();
            Stream GETResponseStream = GETResponse.GetResponseStream();
            StreamReader sr = new StreamReader(GETResponseStream, Encoding.UTF8);

            Console.WriteLine("Response from server");
            Console.WriteLine(sr.ReadToEnd());
            Console.ReadLine();
        }

        static void PostRequest(string firstName, string lastName, string designation)
        {
            string url = "http://localhost/RestWebService/employee";
            byte[] employee = GenerateXMLEmployee(firstName, lastName, designation, "0");
            HttpWebRequest POSTRequest = (HttpWebRequest)WebRequest.Create(url);
            POSTRequest.Method = "POST";
            POSTRequest.ContentType = "text/xml";
            POSTRequest.KeepAlive = false;
            POSTRequest.Timeout = 5000;
            POSTRequest.ContentLength = employee.Length;

            Stream POSTstream = POSTRequest.GetRequestStream();
            POSTstream.Write(employee, 0, employee.Length);

            //Get the response
            HttpWebResponse POSTResponse = (HttpWebResponse)POSTRequest.GetResponse();
            StreamReader reader = new StreamReader(POSTResponse.GetResponseStream(), Encoding.UTF8);
            Console.WriteLine("Response");
            Console.WriteLine(reader.ReadToEnd().ToString());
        }

        static void PUTRequest(string firstName, string lastName, string designation, string employeeID)
        {
            string url = "http://localhost/RestWebService/employee";
            byte[] employee = GenerateXMLEmployee(firstName, lastName, designation, employeeID);
            HttpWebRequest POSTRequest = (HttpWebRequest)WebRequest.Create(url);
            POSTRequest.Method = "PUT";
            POSTRequest.ContentType = "text/xml";
            POSTRequest.KeepAlive = false;
            POSTRequest.Timeout = 5000;
            POSTRequest.ContentLength = employee.Length;

            Stream POSTstream = POSTRequest.GetRequestStream();
            POSTstream.Write(employee, 0, employee.Length);

            //Get the response
            HttpWebResponse POSTResponse = (HttpWebResponse)POSTRequest.GetResponse();
            StreamReader reader = new StreamReader(POSTResponse.GetResponseStream(), Encoding.UTF8);
            Console.WriteLine("Response");
            Console.WriteLine(reader.ReadToEnd().ToString());
        }
        static byte[] GenerateXMLEmployee(string firstName, string lastName, string designation, string empCode)
        {
            MemoryStream mStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(mStream, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Employee");
            xmlWriter.WriteStartElement("FirstName");
            xmlWriter.WriteString(firstName);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("LastName");
            xmlWriter.WriteString(lastName);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("EmpCode");
            xmlWriter.WriteString(empCode);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Designation");
            xmlWriter.WriteString(designation);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
            return mStream.ToArray();
        }
    }
}
