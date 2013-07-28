// MedicalRecords.cs
using System;

namespace SortingRecords
{
    internal class MedicalRecord : System.IComparable
    {
        private int _patientId;
        private string _patientName;
        private string _patientAddress;
        private string _patientPhoneNumber;
        private string _primaryCarePhysician;

        // Default constructor that initializes the member variables
        // to acceptable default values.
        internal MedicalRecord()
        {
            _patientId = -1;
            _patientName = "";
            _patientAddress = "";
            _patientPhoneNumber = "";
            _primaryCarePhysician = "";
        }

        // Customer constructor that accepts arguments and stores them
        // in the various member variables.
        internal MedicalRecord 
            (int id, string name, string address, string phone, 
            string physician) : this()
        {
            _patientId = id;
            _patientName = name;
            _patientAddress = address;
            _patientPhoneNumber = phone;
            _primaryCarePhysician = physician;
        }

        // Implementing this method meets the requirements of implementing
        // the IComparable interface. By implementing this method, any array
        // that stores objects of this class type (MedicalRecord) is able to
        // sort the objects it is storing.
        int System.IComparable.CompareTo (object o)
        {
            int result = 1;

            // Using the 'as' keyword, attempt to convert the object passed
            // in to a MedicalRecord object. This ensures that the object 
            // passed in is comparable to the current object.
            MedicalRecord mr = o as MedicalRecord;
            if (null != mr)
            {
                // The property PatientId returns an integer. The 
                // System.Int32 type supports a CompareTo() method as well 
                // so we'll just use it to perform the comparison.
                result = this.PatientId.CompareTo(mr.PatientId);
            }
            else
            {
                Console.WriteLine ("\nMEDICALRECORD: Object passed is in " +
                    "not a MedicalRecord object - cannot compare.");
            }

            return result;
        }

        // Overrides the System.Object.ToString method to convert the
        // member variables in a nice string format.
        public override string ToString()
        {
            string output;

            output = 
                "\tPatient ID:             " + PatientId + "\n" +
                "\tPatient Name:           " + PatientName + "\n" +
                "\tPatient Address:        " + PatientAddress + "\n" +
                "\tPatient Phone Number:   " + PatientPhoneNumber + "\n" +
                "\tPrimary Care Physician: " + PrimaryCarePhysician;

            return output;
        }

        /********************************************************
         * 
         * The following properties are used to provide access
         * to the object's member variables.
         * 
         *******************************************************/

        internal int PatientId
        {
            get { return _patientId; }
            set
            {
                if (value <= 0)
                {
                    Console.WriteLine 
                        ("\nMEDICALRECORD - Invalid patient ID of {0} " +
                        " entered.", value);
                }
                else
                {
                    _patientId = value;
                }
            }
        }

        internal string PatientName
        {
            get { return _patientName; }
            set
            {
                if (value.Length <= 0)
                {
                    Console.WriteLine 
                        ("\nMEDICALRECORD - Invalid patient name.");
                }
                else
                {
                    _patientName = value;
                }
            }
        }

        internal string PatientAddress
        {
            get { return _patientAddress; }
            set
            {
                if (value.Length <= 0)
                {
                    Console.WriteLine 
                        ("\nMEDICALRECORD - Invalid patient address.");
                }
                else
                {
                    _patientAddress = value;
                }
            }
        }

        internal string PatientPhoneNumber
        {
            get { return _patientPhoneNumber; }
            set
            {
                if (value.Length <= 0)
                {
                    Console.WriteLine 
                        ("\nMEDICALRECORD - Invalid patient phone number.");
                }
                else
                {
                    _patientPhoneNumber = value;
                }
            }
        }

        internal string PrimaryCarePhysician
        {
            get { return _primaryCarePhysician; }
            set
            {
                if (value.Length <= 0)
                {
                    Console.WriteLine ("\nMEDICALRECORD - Invalid patient " +
                        "primary care physician.");
                }
                else
                {
                    _primaryCarePhysician = value;
                }
            }
        }
    }
}