using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventsLab
{
    // Custom EventArgs class to contain the UserId.
    //TODO: Declare the PasswordAlertEventArgs class.  Include
    //      the commented block of code below in the class.
    
    
        //private string _userID;

        //public PasswordAlertEventArgs(string userId)
        //{
        //    UserID = userId;
        //}

        //public string UserID
        //{
        //    get { return _userID; }
        //    set { _userID = value; }
        //}
    
    
    internal class LoginClass
    {
        int _passwordTriesInt;
        const string _PASSWORD_String = "EventsLab"; 

        // This is the delegate that encapsulates the target methods 
        // in other objects. When an object wishes to subscribe to the 
        // event in this class, this delegate will be used to call the 
        // other objects' event handler methods when the event is raised.
        internal delegate
            void PasswordAlertHandler(object sender, PasswordAlertEventArgs e);

        // This is the event (aka the multicast delegate) that will be 
        // raised. The event that can be subscribed to is OnPasswordAlert.
        internal event PasswordAlertHandler OnPasswordAlert;

        // This method is called everytime the user enters a password.
        internal bool Login(string userID, string password)
        {
            if (password == _PASSWORD_String)
            {
                Console.WriteLine("\nCongratulations!!  You logged in successfully!!");
                return true;
            }

            ++_passwordTriesInt;

            if (_passwordTriesInt > 2)
            {
                // Now there have been three failed attempts to enter the correct password.
                //      It's time to raise the alert message.
                RaisePasswordAttemptsExceededEvent(userID);
            }

            // Another failed attempt to enter the correct password.
            return false;
        }

        // Internal helper method that checks to see if any
        // objects are subscribed to the event. If any are,
        // their event handler method will be called through
        // the multicast delegate.
        private void RaisePasswordAttemptsExceededEvent(string userID)
        {
            // Make sure to check that there is at least one subscriber!
            if (OnPasswordAlert != null)
            {
                // TODO: Raise the OnPasswordAlert event by passing in an instance of
                //      the PasswordAlertEventArgs class.  Be sure
                //      to store the user id in the instance.  The user id is contained in the
                //      variable "userID".

            
            }
        }
    }

    class Program
    {
        private static bool _passwordAlertBool = false;
        
        private void DisplayPasswordAlert(object sender, PasswordAlertEventArgs e)
        {
            // Save the current color of the characters.
            ConsoleColor originalForeGroundColor = Console.ForegroundColor;

            // Change the color of the characters to Red.
            Console.ForegroundColor = ConsoleColor.Red;

            // TODO: Complete the Console.Writeline method below by passing in the user id
            //      that is stored in the PasswordAlertEventArgs parameter.
            Console.WriteLine(
                "ALERT to {0}: Three attempts have been made to crack your password!!"
                ,);

            _passwordAlertBool = true;

            // Reset the color of the characters to their original color.
            Console.ForegroundColor = originalForeGroundColor;
        }

        static void Main()
        {
            bool passwordOKBool = false;
            string password = null;
            string userID;
            
            // Clear the console window.
            Console.Clear();

            // Get the user ID.
            Console.Write("Enter your user ID: ");
            userID = Console.ReadLine();

            // Create an instance of this class (the user).
            Program program = new Program();

            // Create an instance of the service class that is used
            //      to verify the password.
            LoginClass loginVerify = new LoginClass();

            //  Subscribe the event handler that is to be called when
            //      three failed attempts were made to enter the correct password.
            loginVerify.OnPasswordAlert +=
                new LoginClass.PasswordAlertHandler
                (program.DisplayPasswordAlert);

            // Loop until either the correct password is entered or
            //      three failed attempts were made.
            while (!passwordOKBool)
            {
                // Get the password.
                Console.Write("Enter your password: ");
                password = Console.ReadLine();

                // Call the method to verify the password.
                passwordOKBool = loginVerify.Login(userID, password);

                // Flag used when alert was made after three failed attempts 
                //      at entering a password.
                if (_passwordAlertBool)
                {
                    break;
                }
            }

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
