using System;

namespace InterfacesExercise
{
    /// <summary>
    /// The IFormatter inferface containing the Format() method.
    /// </summary>
    internal interface IFormatter
    {
        void Format ();
    }

    /// <summary>
    /// The PhoneNumber class which implements the IFormatter
    /// interface. Phone numbers will be formatted to look like
    /// (NNN) NNN-NNNN.
    /// </summary>
    internal class PhoneNumber : IFormatter
    {
        private int _areaCode;
        private int _exchangePrefix;
        private int _phoneNumber;
        private string _formattedPhoneNumber;

        // This constructor doesn't do a lot of checking for
        // validation. For example, what would happen if the
        // user entered less than 10 numbers or letters instead
        // of numbers?
        internal PhoneNumber(string phoneNumber)
        {
            _areaCode = int.Parse(phoneNumber.Substring (0,3));
            _exchangePrefix = int.Parse(phoneNumber.Substring (3,3));
            _phoneNumber = int.Parse(phoneNumber.Substring (6,4));
            _formattedPhoneNumber = null;
        }

        // The implemented Format() method.
        public void Format ()
        {
            _formattedPhoneNumber = "(" + _areaCode.ToString() + ") " +
                _exchangePrefix.ToString() + "-" + _phoneNumber.ToString();
        }

        public override string ToString ()
        {
            if (null == _formattedPhoneNumber)
            {
                Format();
            }

            return _formattedPhoneNumber;
        }
    }


    /// <summary>
    /// The SocialSecurityNumber class which implements the 
    /// IFormatter interface. SSNs will be formatted to look 
    /// like NNN - NN - NNNN.
    /// </summary>
    internal class SocialSecurityNumber : IFormatter
    {
        private int _groupOne;
        private int _groupTwo;
        private int _groupThree;
        private string _formattedSsn;

        // This constructor doesn't do a lot of checking for
        // validation. For example, what would happen if the
        // user entered less than 9 numbers or letters instead
        // of numbers?
        internal SocialSecurityNumber(string ssn)
        {
            _groupOne = int.Parse(ssn.Substring (0,3));
            _groupTwo = int.Parse(ssn.Substring (3,2));
            _groupThree = int.Parse(ssn.Substring (5,4));
            _formattedSsn = null;
        }

        // The implemented Format() method.
        public void Format ()
        {
            _formattedSsn = _groupOne + " - " + _groupTwo + " - " + _groupThree;
        }

        public override string ToString ()
        {
            if (null == _formattedSsn)
            {
                Format();
            }

            return _formattedSsn;
        }
    }

    /// <summary>
    /// This testing class will display a menu to the user where
    /// they can decide what kind of record to create.
    /// </summary>
    class TestInterface
    {
        // This method takes an object that supports the IFormatter
        // interface. It will call that object's Format() method.
        private static void FormatRecord (IFormatter record)
        {
            record.Format();
        }

        // This method displays the menu to the user and gets their
        // input.
        private static string GetRecordType ()
        {
            Console.WriteLine ("\nWhat kind of record do you wish to make?\n");
            Console.WriteLine ("\t1. Phone Number");
            Console.WriteLine ("\t2. Social Security Number");
            Console.WriteLine ("\t0. None.");
            Console.Write ("\nEnter your choice: ");

            string recordType = Console.ReadLine();
            return recordType;
        }

        static void Main()
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                string userChoice = GetRecordType();

                switch (userChoice)
                {
                    case "0":
                        keepGoing = false;
                        break;

                    case "1":
                        Console.Write ("\nEnter the phone number to store: ");
                        string pn = Console.ReadLine();
                        PhoneNumber phoneNumber = new PhoneNumber (pn);
                        FormatRecord((IFormatter)phoneNumber);
                        Console.WriteLine ("\nYou entered {0} as the phone number.", phoneNumber.ToString());
                        break;

                    case "2":
                        Console.Write ("\nEnter the social security number to store: ");
                        string ssn = Console.ReadLine();
                        SocialSecurityNumber ssNumber = new SocialSecurityNumber (ssn);
                        FormatRecord((IFormatter)ssNumber);
                        Console.WriteLine ("\nYou entered {0} as the social security number.", ssNumber.ToString());
                        break;

                    default:
                        Console.WriteLine ("\nOption {0} is not valid. Try again.", userChoice);
                        break;
                }
            }
        }
    }
}
