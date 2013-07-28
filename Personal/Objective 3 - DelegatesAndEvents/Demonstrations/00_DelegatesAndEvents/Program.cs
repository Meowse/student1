using System;

namespace SignalAndWarningSystem
{
    internal class HotWaterTransferSystem
    {
        private bool _powerOn;

        public HotWaterTransferSystem()
        {
            PowerOn = false;
        }

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
                        HWTSystem.PowerUp();
                        success = true;
                    }

                    break;

                case "2":   // Turn off the system.
                    if (HWTSystem != null)
                    {
                        HWTSystem.PowerDown();
                        success = false;
                    }

                    break;

                case "3":   // Purge hot water.
                    if (HWTSystem != null)
                    {
                        if (HWTSystem.TransferHotWaterOut())
                        {
                            success = true;
                        }
                    }

                    break;

                case "4":   // Fill cold water.
                    if (HWTSystem != null)
                    {
                        if (HWTSystem.TransferColdWaterIn())
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

            // Create the control room object.
            ControlRoom cr = new ControlRoom();

            // Continue to run until the user exits the application.
            while (!cr.ExitSystem)
            {
                // Display the control menu.
                cr.DisplayMenu();

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
