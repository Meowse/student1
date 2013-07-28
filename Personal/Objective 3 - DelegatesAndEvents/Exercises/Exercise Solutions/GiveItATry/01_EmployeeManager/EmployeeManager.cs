/******************************************************************************
 * 
 *  Read the TODO comments and complete the code that each requests.
 * 
 *****************************************************************************/

using System;
using System.Text;
using System.Collections;

namespace EmployeeManager
{    
    internal class EmployeeCollection
    {
        private ArrayList _employeeList = new ArrayList();

        // TODO: Declare the delegate that will call comparison methods. This
        //       delegate should be named CompareItemsCallback.
        internal delegate int CompareItemsCallback(Employee emp1, Employee emp2);

        // This method will sort the list of employees.
        internal void Sort(CompareItemsCallback sortingMethod)
        {
            // Loop to compare numbers for sorting.
            for (int i=1; i < _employeeList.Count; i++)
            {
                int j = i;
                Employee e = (Employee)_employeeList[i];

                // Swap employees if they are out of order.
                while ((j>0) && sortingMethod((Employee)_employeeList[j-1], e) >= 0)
                {
                    _employeeList[j] = _employeeList[j-1];
                    j--;
                }

                _employeeList[j] = e;
            }
        }

        // This method is used to add a new employee record to the list
        // of employees.
        internal void AddNewEmployee(Employee e)
        {
            _employeeList.Add (e);
        }

        // This method is here to create an arbitrary list of employees.
        internal void CreateEmployeeList(int count)
        {
            Random employeeNameRand = new Random (DateTime.Now.Millisecond);
            Random employeeIdRand = new Random (DateTime.Now.Millisecond + DateTime.Now.Second);

            // Create as many employees as was passed in.
            for (int x=0; x < count; x++)
            {
                // All of our last names will be 7 characters long. We'll use a 
                // StringBuilder class to add characters to the underlying string.
                StringBuilder lastName = new StringBuilder(7);

                for (int i=0; i<7; i++)
                {
                    char letter;
                    if (i==0)
                    {
                        // Get the first letter of the last name from the capital letters.
                        letter = (char) employeeNameRand.Next(65, 90);
                    }
                    else
                    {
                        // Get the subsequent letters of the last name from the lower-case letters.
                        letter = (char) employeeNameRand.Next(97, 122);
                    }

                    // Place the character in the string.
                    lastName.Insert(i, letter);
                }

                // Create the employee ID.
                int tempId = employeeIdRand.Next (10000, 99999);

                // Create a new employee record.
                Employee e = new Employee(lastName.ToString(), tempId);

                // Add the new employee to the list.
                AddNewEmployee(e);
            }
        }

        internal void PrintEmployees(string title)
        {
            Console.WriteLine (title + "\n");

            for (int i=0; i < _employeeList.Count; i++)
            {
                Employee e = (Employee)_employeeList[i];
                PrintEmployee (e);
            }
        }

        internal void PrintEmployee(Employee e)
        {
            Console.WriteLine ("\tName: {0}, ID: {1}", e.Name, e.Id);
        }
    }

    internal class Employee
    {
        private string name;
        private int id;

        internal Employee(string name, int id)
        {
            this.name = name;
            this.id = id;
        }

        // TODO: Complete this method to compare the names of the employee and return the result.
        //       The result should be one of the following:
        //
        //          -1: name of e1 is less than name of e2.
        //           0: names are equal.
        //           1: name of e1 is greater than name of e2.
        //
        //       Check out the String class for help on comparing the two names.
        internal static int CompareByName(Employee e1, Employee e2)
        {
            // Compare the names of the employees for equality.
            int result = String.Compare (e1.name, e2.name);

            // Return the comparison result.
            return result;
        }

        // TODO: Complete this method to compare the IDs of the employee and return the result.
        //       The result should be one of the following:
        //
        //          -1: ID of e1 is less than ID of e2.
        //           0: IDs are equal.
        //           1: ID of e1 is greater than ID of e2.
        //
        //       The int type has a method named CompareTo that performs the same
        //       comparison and returns the same results as the String's Compare method.
        internal static int CompareById(Employee e1, Employee e2)
        {
            // Use the CompareTo() method that the int type supports to 
            // compare the two ID's.
            int result = e1.Id.CompareTo(e2.Id);

            // Return the comparison result.
            return result;
        }

        internal string Name
        {
            get { return name; }
            set { name = value; }
        }

        internal int Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    public class TestClass
    {
        // TODO: Complete this method by first calling Sort and then calling PrintEmployees.
        private static void PerformEmployeeMaintenance (EmployeeCollection ec, EmployeeCollection.CompareItemsCallback callBack, string title)
        {
            // Sort the list.
            ec.Sort(callBack);

            // Print the list.
            ec.PrintEmployees(title);
        }

        public static void Main(string[] args)
        {
            try
            {
                Console.Write ("How many employees do you want to generate? ");
                int count = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // Create the collection and populate it.
                EmployeeCollection ec = new EmployeeCollection();
                ec.CreateEmployeeList(count);

                // TODO: Create the delegate used to sort by name.
                EmployeeCollection.CompareItemsCallback callBack1 = new EmployeeCollection.CompareItemsCallback(Employee.CompareByName);

                // TODO: Sort the list based on the name of the employee. To
                //       do this, call PerformEmployee Maintenance().
                PerformEmployeeMaintenance(ec, callBack1, "List of Employees Sorted by Name");

                Console.WriteLine ();

                // TODO: Create the delegate used to sort by I.D.
                callBack1 = new EmployeeCollection.CompareItemsCallback(Employee.CompareById);

                // TODO: Sort the list based on the name of the employee.
                PerformEmployeeMaintenance(ec, callBack1, "List of Employees Sorted by ID");
            }
            catch (Exception e)
            {
                Console.WriteLine ("\n\n{0}", e.Message);
            }
            finally
            {
                Console.Write ("\n\nPress <ENTER> to end: ");
                Console.ReadLine();
            }
        }
    }
}
