// TestCode.cs

/****************************************************************************
 * 
 * This program demonstrates the use of an interface in what is a real-life
 * scenario. In this program, we are "reading in" a series of medical records
 * from a database. The "reading in" part is simulated by generating the data
 * on the fly using the DataGenerator class. As each medical record is "read
 * in", the DataGenerator class is called to create ficticious data that is 
 * used to populate a new MedicalRecord object. This object is then stored in
 * an array.
 * 
 * After the data is generated, the list of medical records is displayed in 
 * its unordered state. The System.Array class has a Sort() method that will
 * sort any object provided that object implements the IComparable interface.
 * Our MedicalRecord class does implement that interface. It implements the 
 * CompareTo() method which compares the patient ID of two records. Based on
 * the outcome, CompareTo() returns an integer that indicates whether the 
 * first object is less than, equal to, or greater than the second object.
 * 
 ***************************************************************************/

using System;

namespace SortingRecords
{
    class TestClass
    {
        DataGenerator _dg;
        MedicalRecord[] _mrList;

        internal TestClass(int listSize)
        {
            _dg = new DataGenerator();
            _mrList = new MedicalRecord[listSize];
        }

        internal void ReadPatientRecords()
        {
            for (int i=0; i < _mrList.Length; i++)
            {
                // Create a new MedicalRecord record.
                MedicalRecord mr = new MedicalRecord();

                // Get the patient's ID.
                mr.PatientId = _dg.GetPositiveNumber(100000, 999999);

                // Get the patient's Name.
                string fName = _dg.GetString(8, true);
                string lName = _dg.GetString(12, true);
                mr.PatientName = lName + ", " + fName;

                // Get the patient's Address.
                int aNum = _dg.GetPositiveNumber(1, 99999);
                string aStreet = _dg.GetString (15, true);
                mr.PatientAddress = aNum + " " + aStreet;

                // Get the patient's phone number.
                int aCode = _dg.GetPositiveNumber(111, 999);
                int exchg = _dg.GetPositiveNumber(100, 999);
                int phnum = _dg.GetPositiveNumber(1, 9999);
                mr.PatientPhoneNumber = "(" + aCode.ToString() + ") " +
                    exchg.ToString() + "-" +
                    phnum.ToString("d4");

                // Get the patient's primary care physician.
                fName = _dg.GetString(8, true);
                lName = _dg.GetString(12, true);
                mr.PrimaryCarePhysician = fName + " " + lName;

                // Now add the new record to the list of records.
                _mrList[i] = mr;
            }
        }

        // This method prints the medical records to the screen.
        internal void PrintList()
        {
            for (int i=0; i < _mrList.Length; i++)
            {
                Console.WriteLine ("\n" + _mrList[i].ToString() + "\n");
            }
        }

        // This method simply calls the System.Array.Sort() method
        // passing in the list of medical records that was read in 
        // from the database.
        internal void SortList()
        {
            System.Array.Sort(_mrList);
        }

        static void Main()
        {
            // Prompt the user for how many records to "read in".
            Console.Write 
                ("How many patient records would you like to read in?: ");
            int listSize = int.Parse (Console.ReadLine());

            // Check to make sure the number of record desired in valid.
            if (listSize <= 0)
            {
                Console.WriteLine ("\nOkay, closing the database...");
            }
            else
            {
                // Create a new TestClass object.
                TestClass tc = new TestClass(listSize);

                // "Read" the data in from the database. This simulates
                // the reading by generating data on the fly.
                tc.ReadPatientRecords();

                // Display the patient records in their original, 
                // unsorted state.
                Console.WriteLine ("\nUNSORTED LIST:");
                tc.PrintList();

                // Sort the patient records.
                tc.SortList();

                // Reprint the list in its new sorted state.
                Console.WriteLine ("\nSORTED LIST:");
                tc.PrintList();
            }

            Console.Write ("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
