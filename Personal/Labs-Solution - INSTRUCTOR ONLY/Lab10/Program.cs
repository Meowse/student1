using System;
using System.Collections.Generic;

namespace InterfacesLab
{
    internal class Employees
    {
        internal readonly int NumberOfEmployees;

        private List<Employee> employees;

        internal Employees(int numberOfEmployees)
        {
            NumberOfEmployees = numberOfEmployees;
            employees = new List<Employee>(NumberOfEmployees);
        }

        internal void PrintAllEmployees()
        {
            Console.WriteLine(
                "\n\n\n***  Printing Basic Info for All Employees  ***");

            for (int i = 0; i < NumberOfEmployees; i++)
            {
                Console.WriteLine("\n\nID No. {0}:{1}", i + 1,
                    employees[i].ToString());
            }
        }

        internal void PrintOnlyExecutiveEmployees()
        {
            ExecutiveEmployee ee;
            int executiveEmployeeCount = 0;

            Console.WriteLine(
                "\n\n\n**  Printing All Info for Executive Employees  **");

            for (int i = 0; i < NumberOfEmployees; i++)
            {
                ee = employees[i] as ExecutiveEmployee;

                if (ee != null)
                {
                    executiveEmployeeCount++;
                    Console.WriteLine("\n\n{0}.{1}",
                        executiveEmployeeCount, ee.ToString());
                }
            }
        }

        internal void AddEmployee(Employee e)
        {
            if (employees.Count < employees.Capacity)
            {
                employees.Add(e);
            }
        }
    }

    internal interface IEmployeeInfo
    {
        decimal PayCheck();
    }

    internal class Employee
    {
        private string _firstName;
        private string _lastName;
        private string _socialSecurityNumber;
        private string _employeeType;

        protected Employee(string firstName, string lastName, string ssn,
            string employeeType)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = ssn;
            EmployeeType = employeeType;
        }

        public string FirstName
        {
            get { return _firstName; }
            protected set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            protected set { _lastName = value; }
        }

        public string SocialSecurityNumber
        {
            get { return _socialSecurityNumber; }
            protected set { _socialSecurityNumber = value; }
        }

        public string EmployeeType
        {
            get { return _employeeType; }
            protected set { _employeeType = value; }
        }

        public override string ToString()
        {
            string output = "\n\tEmployee Type: " + EmployeeType +
                "\n\tLast name: " + LastName +
                "\n\tFirst name: " + FirstName +
                "\n\tSSN: " + SocialSecurityNumber;
            return output;
        }
    }

    internal class SalariedEmployee : Employee, IEmployeeInfo
    {
        private decimal _weeklySalary;

        public SalariedEmployee(string firstName, string lastName,
            string ssn, decimal weeklySalary)
            : this(firstName, lastName, ssn, weeklySalary,
                "Salaried Employee")
        { }

        public SalariedEmployee(string firstName, string lastName,
            string ssn, decimal weeklySalary, string employeeType)
            : base(firstName, lastName, ssn, employeeType)
        {
            WeeklySalary = weeklySalary;
        }

        public decimal WeeklySalary
        {
            get { return _weeklySalary; }
            set
            {
                if (value >= 0)
                {
                    _weeklySalary = value;
                }
                else
                {
                    _weeklySalary = 0;
                }
            }
        }

        // TODO: Revise the method below to be an explicit implementation
        //      of the PayCheck method of the IEmployeeInfo interface.
        decimal IEmployeeInfo.PayCheck()
        {
            return WeeklySalary;
        }

        // TODO: Revise the method below to fix the compile error that
        //      will occur after the TODO above is completed.
        public override string ToString()
        {
            string output = base.ToString();
            IEmployeeInfo ie = this as IEmployeeInfo;
            output += "\n\tWeekly Salary: " + ie.PayCheck().ToString("C");
            return output;
        }
    }

    sealed internal class HourlyEmployee : Employee, IEmployeeInfo
    {
        private decimal _hourlyWage;
        private decimal _hoursWorked;

        public HourlyEmployee(string firstName, string lastName, string ssn,
            decimal hourlyWage, decimal hoursWorked)
            : base(firstName, lastName, ssn, "Hourly Employee")
        {
            HourlyWage = hourlyWage;
            HoursWorked = hoursWorked;
        }

        public decimal HourlyWage
        {
            get { return _hourlyWage; }
            set
            {
                if (value >= 0)
                {
                    _hourlyWage = value;
                }
                else
                {
                    _hourlyWage = 0;
                }
            }
        }

        public decimal HoursWorked
        {
            get { return _hoursWorked; }
            set
            {
                if (value >= 0)
                {
                    _hoursWorked = value;
                }
                else
                {
                    _hoursWorked = 0;
                }
            }
        }

        public decimal Earnings
        {
            get
            {
                return HoursWorked * HourlyWage;
            }
        }

        decimal IEmployeeInfo.PayCheck()
        {
            return Earnings;
        }

        public override string ToString()
        {
            IEmployeeInfo ie = this;
            string output = base.ToString();
            output += "\n\tHourly pay: " + HourlyWage.ToString("C") +
                "\n\tHours worked: " + HoursWorked +
                "\n\tPaycheck amount: " + ie.PayCheck().ToString("C");
            return output;
        }
    }

    internal class ExecutiveEmployee : SalariedEmployee, IEmployeeInfo
    {
        private decimal _bonus;

        public ExecutiveEmployee(string firstName, string lastName,
            string ssn, decimal weeklySalary, decimal bonus) :
            base(firstName, lastName, ssn, weeklySalary, "Executive Employee")
        {
            Bonus = bonus;
        }

        public decimal Bonus
        {
            get { return _bonus; }
            set
            {
                if (value > 0)
                {
                    _bonus = value;
                }
            }
        }

        public new string ToString()
        {
            string output = base.ToString();
            output += "\n\tAve. Weekly salary including Bonus: " +
                PayCheck().ToString("C") +
                "\n\tBonus: " + Bonus.ToString("C");
            return output;
        }

        public new decimal PayCheck()
        {
            decimal averageWeeklyPay = (Bonus / 52) + WeeklySalary;
            return averageWeeklyPay;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Employees specifying 7 employees.
            Employees emps = new Employees(7);

            SalariedEmployee se = new SalariedEmployee("Jane", "Doe",
                "444-55-6666", 791.34m);
            emps.AddEmployee(se);

            HourlyEmployee he = new HourlyEmployee("Jim", "Smith",
                "777-88-9999", 12.38m, 40);
            emps.AddEmployee(he);

            ExecutiveEmployee ee = new ExecutiveEmployee("Laura",
                "Jones", "333-44-5555", 2813.71m, 25000);
            emps.AddEmployee(ee);

            he = new HourlyEmployee("Kyle", "McMasters",
                "111-55-3333", 15.49m, 39);
            emps.AddEmployee(he);

            ee = new ExecutiveEmployee("Thomas",
                "Franklin", "222-88-6666", 3157.71m, 29000);
            emps.AddEmployee(ee);

            se = new SalariedEmployee("Susan", "Mason",
                "555-33-7777", 842.25m);
            emps.AddEmployee(se);

            se = new SalariedEmployee("Janet", "Powell",
                "999-44-3333", 678.12m);
            emps.AddEmployee(se);

            // Print basic info for all employees.
            emps.PrintAllEmployees();

            // Print all info for executive employees.
            emps.PrintOnlyExecutiveEmployees();

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}

