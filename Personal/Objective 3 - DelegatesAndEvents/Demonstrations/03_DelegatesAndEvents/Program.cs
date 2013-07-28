/************************************************************************
 * 
 * Delegates and Events: Part 3
 * 
 * Core Topics:
 * 1. Use a delegate to implement a callback to notify the client
 *          when the called method is complete.
 * 2. Identifies the steps involved in setting up and 
 *          implementing a callback.
 **********************************************************************/

using System;

namespace SignalAndWarningSystem
{
//1.  Declare delegates
    internal delegate void DisplayMenu();
    internal delegate void ToggleSystemPower();
    internal delegate bool ManageWater();
    internal delegate void PowerUpComplete();

    internal class HotWaterTransferSystem
    {
        private bool _powerOn;

        public HotWaterTransferSystem()
        {
            PowerOn = false;
        }

//3.  Implement target method to be called by the delegate.
        internal void PowerUp()
        {
            // Power up request came in and system is off.
            if (!PowerOn)
            {
                Console.WriteLine("Starting up the system...");

                // It takes time to power up the system. Simulate that
                // with a pause in the operation.
                System.Threading.Thread.Sleep(10000);

                PowerOn = true;

                Console.WriteLine("System is running.");
            }
            else
            {
                // Power up request came in and system is already on.
                Console.WriteLine("System is already running.");
            }

        }

//2.  Define the method that takes the delegate(s) and calls them.
        // This is a new PowerUp method that takes two delegates, one
        // to call the real PowerUp method and one for the callback
        // method.
        internal void PowerUp(ToggleSystemPower powerUpProcess, 
            PowerUpComplete callbackMethod)
        {
            // Call the PowerUp process using the Invoke method of the Delegate class.
            powerUpProcess();

            // Now call the callback method, using the Invoke method of the
            // delegate class, to notify the client code
            // that the reading process is done.
            callbackMethod();
        }

        internal void PowerDown()
        {
            // Power down request came in and system is on.
            if (PowerOn)
            {
                Console.WriteLine("Shutting down the system...");

                // It takes time to power down the system. Simulate that
                // with a pause in the operation.
                System.Threading.Thread.Sleep(7000);

                PowerOn = false;

                Console.WriteLine("System is powered down.");
            }
            else
            {
                // Power down request came in and system is already off.
                Console.WriteLine("System is already shut down.");
            }
        }

        internal bool PowerOn
        {
            get { return _powerOn; }
            private set { _powerOn = value; }
        }

        internal bool TransferHotWaterOut()
        {
            bool result = true;

            if (PowerOn)
            {
                Console.WriteLine("Purging HOT water...");

                // Simulate the transfer of hot water out of the system
                // with a pause in the operation
                System.Threading.Thread.Sleep(5000);

                Console.WriteLine("Hot water transfer complete.");
            }
            else
            {
                Console.WriteLine
                    ("System is not on. Hot water tranfer aborted.");

                result = false;
            }

            return result;
        }

        internal bool TransferColdWaterIn()
        {
            bool result = true;

            if (PowerOn)
            {
                Console.WriteLine("Filling COLD water...");

                // Simulate the transfer of hot water out of the system
                // with a pause in the operation
                System.Threading.Thread.Sleep(5000);

                Console.WriteLine("Cold water transfer complete.");
            }
            else
            {
                Console.WriteLine
                    ("System is not on. Cold water tranfer aborted.");

                result = false;
            }

            return result;
        }
    }

    class ControlRoom
    {
        bool _exitSystem;
        HotWaterTransferSystem _hwtSystem;

        public ControlRoom()
        {
            ExitSystem = false;
            HWTSystem = new HotWaterTransferSystem();
        }

        private HotWaterTransferSystem HWTSystem
        {
            get { return _hwtSystem; }
            set { _hwtSystem = value; }
        }

        private bool ExitSystem
        {
            get { return _exitSystem; }
            set { _exitSystem = value; }
        }

//3.  Implement target method to be called by the delegate.
        private void PowerUpProcessComplete()
        {
            Console.WriteLine("CALLBACK: Power Up process is complete.");
        }

        //private void DisplayMenu()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("Hot Water Transfer System Control Menu");
        //    Console.WriteLine();
        //    Console.WriteLine("\t1. Turn on system");
        //    Console.WriteLine("\t2. Turn off system");
        //    Console.WriteLine("\t3. Purge hot water from system");
        //    Console.WriteLine("\t4. Fill system with cold water");
        //    Console.WriteLine("\tX. Exit HWTS control program");
        //    Console.WriteLine();
        //    Console.Write("Enter option: ");
        //}

        private bool RunOperation(string operation)
        {
            bool success = false;
            string systemOperation = operation.ToUpper();
            systemOperation = systemOperation.Substring(0, 1);

            switch (systemOperation)
            {
                case "1":   // Turn on the system.
                    if (HWTSystem != null)
                    {
//4. Create instance of delegate in the client code.
                        // The delegate used for the callback when powerup
                        // completes.
                        PowerUpComplete puc = 
                            new PowerUpComplete(PowerUpProcessComplete);

//4.  Create instance of delegate in the client code.
                        ToggleSystemPower togglePower = 
                            new ToggleSystemPower(HWTSystem.PowerUp);

//5.  Pass the instances of delegates to a method that will call them.
                        // Call the PowerUp method that takes the two
                        // delegates.
                        HWTSystem.PowerUp(togglePower, puc);
                        success = true;
                    }

                    break;

                case "2":   // Turn off the system.
                    if (HWTSystem != null)
                    {
                        ToggleSystemPower togglePower = 
                            new ToggleSystemPower(HWTSystem.PowerDown);
                        togglePower();
                        success = false;
                    }

                    break;

                case "3":   // Purge hot water.
                    if (HWTSystem != null)
                    {
                        ManageWater manager =
                            new ManageWater(HWTSystem.TransferHotWaterOut);
                        if (manager())
                        {
                            success = true;
                        }
                    }

                    break;

                case "4":   // Fill cold water.
                    if (HWTSystem != null)
                    {
                        ManageWater manager =
                            new ManageWater(HWTSystem.TransferColdWaterIn);
                        if (manager())
                        {
                            success = true;
                        }
                    }

                    break;

                case "X":   // Exit the control program.
                    ExitSystem = true;
                    success = true;
                    break;

                default:
                    Console.WriteLine("Menu option {0} is not valid.",
                        operation);
                    break;
            }

            return success;
        }

        static void Main(string[] args)
        {
            bool status = true;

            // Create the DisplayMenu delegate and use an anonymous method
            // to contain the code.
            DisplayMenu menu = delegate()
            {
                Console.WriteLine();
                Console.WriteLine("Hot Water Transfer System Control Menu");
                Console.WriteLine();
                Console.WriteLine("\t1. Turn on system");
                Console.WriteLine("\t2. Turn off system");
                Console.WriteLine("\t3. Purge hot water from system");
                Console.WriteLine("\t4. Fill system with cold water");
                Console.WriteLine("\tX. Exit HWTS control program");
                Console.WriteLine();
                Console.Write("Enter option: ");
            };

            // Create the control room object.
            ControlRoom cr = new ControlRoom();

            // Continue to run until the user exits the application.
            while (!cr.ExitSystem)
            {
                // Display the control menu.
                //cr.DisplayMenu();
                menu();

                // Get the option from the user.
                string option = Console.ReadLine();
                Console.WriteLine();

                // Process the option.
                status = cr.RunOperation(option);
                if (!status)
                {
                    Console.WriteLine
                        ("WARNING: Is there a problem in the system?");
                }
            }
        }
    }
}
