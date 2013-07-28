/************************************************************************
 * 
 * Delegates and Events: Part 1
 * 
 * Core Topics:
 * 1. Declare and use a delegate.
 * 2. Use delegate inference as an alternate way of declaring and
 *          loading a method call into a delegate.
 **********************************************************************/

using System;

namespace SignalAndWarningSystem
{
    internal delegate void ToggleSystemPower(string controller);
    internal delegate bool ManageWater(string controller);

    internal class HotWaterTransferSystem
    {
        private bool _powerOn;

        public HotWaterTransferSystem()
        {
            PowerOn = false;
        }

        // Method called by delegate.
        internal void PowerUp(string controlRoomOperator)     
        {
            // Power up request came in and system is off.
            if (!PowerOn)
            {
                Console.WriteLine("{0}: Starting up the system...",
                    controlRoomOperator);

                // It takes time to power up the system. Simulate that
                // with a pause in the operation.
                System.Threading.Thread.Sleep(10000);

                PowerOn = true;

                Console.WriteLine("{0}: System is running.",
                    controlRoomOperator);
            }
            else
            {
                // Power up request came in and system is already on.
                Console.WriteLine("{0}: System is already running.",
                    controlRoomOperator);
            }
        }

        // Method called by delegate.
        internal void PowerDown(string controlRoomOperator)       
        {
            // Power down request came in and system is on.
            if (PowerOn)
            {
                Console.WriteLine("{0}: Shutting down the system...",
                    controlRoomOperator);

                // It takes time to power down the system. Simulate that
                // with a pause in the operation.
                System.Threading.Thread.Sleep(7000);

                PowerOn = false;

                Console.WriteLine("{0}: System is powered down.",
                    controlRoomOperator);
            }
            else
            {
                // Power down request came in and system is already off.
                Console.WriteLine("{0}: System is already shut down.",
                    controlRoomOperator);
            }
        }

        internal bool PowerOn
        {
            get { return _powerOn; }
            private set { _powerOn = value; }
        }

        // Method called by delegate.
        internal bool TransferHotWaterOut(string controlRoomOperator)
        {
            bool result = true;

            if (PowerOn)
            {
                Console.WriteLine("{0}: Purging HOT water...",
                    controlRoomOperator);

                // Simulate the transfer of hot water out of the system
                // with a pause in the operation
                System.Threading.Thread.Sleep(5000);

                Console.WriteLine("{0}: Hot water transfer complete.",
                    controlRoomOperator);
            }
            else
            {
                Console.WriteLine
                    ("{0}: System is not on. Hot water tranfer aborted.",
                        controlRoomOperator);

                result = false;
            }

            return result;
        }

        // Method called by delegate.
        internal bool TransferColdWaterIn(string controlRoomOperator)
        {
            bool result = true;

            if (PowerOn)
            {
                Console.WriteLine("{0}: Filling COLD water...",
                    controlRoomOperator);

                // Simulate the transfer of hot water out of the system
                // with a pause in the operation
                System.Threading.Thread.Sleep(5000);

                Console.WriteLine("{0}: Cold water transfer complete.",
                    controlRoomOperator);
            }
            else
            {
                Console.WriteLine
                    ("{0}: System is not on. Cold water tranfer aborted.",
                        controlRoomOperator);

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

        private void DisplayMenu()
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
        }

        private bool RunOperation(string operation, string controlOperator)
        {
            bool success = false;
            string systemOperation = operation.ToUpper();
            systemOperation = systemOperation.Substring(0, 1);

            switch (systemOperation)
            {
                case "1":   // Turn on the system.
                    if (HWTSystem != null)
                    {
                        ToggleSystemPower togglePower = 
                            new ToggleSystemPower(HWTSystem.PowerUp);
                        togglePower(controlOperator);
                        success = true;
                    }

                    break;

                case "2":   // Turn off the system.
                    if (HWTSystem != null)
                    {
                        // Traditional approach to creating an instance of a delegate.
                        ToggleSystemPower togglePower = 
                            new ToggleSystemPower(HWTSystem.PowerDown);
                        // Use delegate inference for creating an instance of a delegate
                        //      to simplify code.
                        //ToggleSystemPower togglePower = HWTSystem.PowerDown;
                        togglePower(controlOperator);
                        success = false;
                    }

                    break;

                case "3":   // Purge hot water.
                    if (HWTSystem != null)
                    {
                        ManageWater manager =
                            new ManageWater(HWTSystem.TransferHotWaterOut);
                        if (manager(controlOperator))
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
                        if (manager(controlOperator))
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
            string controlOperator;

            // Create the control room object.
            ControlRoom cr = new ControlRoom();

            // Get operator's name.
            Console.Write("Enter your name as the operator: ");
            controlOperator = Console.ReadLine();

            // Continue to run until the user exits the application.
            while (!cr.ExitSystem)
            {
                // Display the control menu.
                cr.DisplayMenu();

                // Get the option from the user.
                string option = Console.ReadLine();
                Console.WriteLine();

                // Process the option.
                status = cr.RunOperation(option, controlOperator);
                if (!status)
                {
                    Console.WriteLine
                        ("{0}: WARNING: Is there a problem in the system?",
                            controlOperator);
                }
            }
        }
    }
}
