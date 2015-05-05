using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company
{
    public class CompanyEmployee
    {
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        private int _empCode;

        public int EmpCode
        {
            get { return _empCode; }
            set { _empCode = value; }
        }
        private string _designation;

        public string Designation
        {
            get { return _designation; }
            set { _designation = value; }
        }

        public string getEmployeeName()
        {
            string fullName = FirstName + " " + LastName;
            return fullName;
        }
    }
}
