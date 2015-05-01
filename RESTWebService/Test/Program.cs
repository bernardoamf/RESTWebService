using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company;
using System.Data.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CompanyLinqDataContextDataContext db = new CompanyLinqDataContextDataContext();

            var employeeList = from e in db.Employees
                            select e;


            foreach (var e in employeeList)
            {
                Console.WriteLine("Name: {0}", e.FirstName);
                Console.WriteLine("Last Name: {0}", e.LastName);
            }
            Console.Read();
	{
		 
	}
        }
    }
}
