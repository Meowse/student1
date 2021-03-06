﻿using System;

namespace SignalAndWarningSystem
{
    internal delegate void DisplayMenu();
    internal delegate void ToggleSystemPower();
    internal delegate bool ManageWater();
    internal delegate void PowerUpComplete();

    internal class WarningEventArgs : EventArgs
    {
        private DateTime _currentTime;
        internal WarningEventArgs()
        {
            _currentTime = DateTime.Now;
        }

        internal string WarningTime
        {
            get
            {
                string time;
                time = _currentTime.Hour.ToString("00") + ":" +
                    _currentTime.Minute.ToString("00") + ":" +
                    _currentTime.Second.ToString("00") + "." +
                    _currentTime.Millisecond.ToString("000");
                return time;
            }
        }
    }

    internal class HotWaterTransferSystem
    {
        // The delegate that will be used to call target methods via the
        // event.
        internal delegate int RaiseWarningHandler
            (object sender, WarningEventArgs e);

        // Create the event mapping it to the RaiseWarningHandler
        // delegate.
        internal event RaiseWarningHandler OnRaiseWarning;

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

        // This is a new PowerUp method that takes two delegates, one
        // to call the real PowerUp method and one for the callback
        // method.
        internal void PowerUp(ToggleSystemPower powerUpProcess, 
            PowerUpComplete callbackMethod)
        {
            // Call the PowerUp process.
            powerUpProcess();

            // Now call the callback method to notify the client code
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

        internal int AlertTransferStationFloor
            (object sender, WarningEventArgs e)
        {
            Console.WriteLine
                ("*** {0} FLOOR - The HWT System is malfunctioning!",
                e.WarningTime);
            return 1;
        }

        internal int AlertControlRoom
            (object sender, WarningEventArgs e)
        {
            Console.WriteLine
                ("*** {0} CONTROL - The HWT System is malfunctioning!",
                e.WarningTime);
            return 2;
        }

        internal int AlertManagement
            (object sender, WarningEventArgs e)
        {
            Console.WriteLine
                ("*** {0} MANAGEMENT - The HWT System is malfunctioning!",
                e.WarningTime);
            return 3;
        }

        internal void RaiseWarningEvent()
        {
            if (OnRaiseWarning != null)
            {
                WarningEventArgs wea = new WarningEventArgs();

                // If we were to look at the integer return value that
                // we get back from OnRaiseWarning, it would be the 
                // value of the last method called...in this case 3.
                // Methods that are called through events typically
                // don't return a value. Instead they should be declared
                // returning void.
                OnRaiseWarning(this, wea);
            }
        }

        internal void ClearWarningList()
        {
            OnRaiseWarning = null;
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

        private void PowerUpProcessComplete()
        {
            Console.WriteLine("CALLBACK: Power Up process is complete.");
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
                        // The delegate used for the callback when powerup
                        // completes.
                        PowerUpComplete puc = 
                            new PowerUpComplete(PowerUpProcessComplete);

                        ToggleSystemPower togglePower = 
                            new ToggleSystemPower(HWTSystem.PowerUp);

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

                case "5":   // Correct system errors.
                    if (HWTSystem != null)
                    {
                        // See if the system is on. If not, turn it on!
                        if (!HWTSystem.PowerOn)
                        {
                            HWTSystem.PowerUp();
                        }

                        // Purge the hot water and then fill with cold water.
                        ManageWater manager =
                            new ManageWater(HWTSystem.TransferHotWaterOut);
                        if (manager())
                        {
                            manager = new 
                                ManageWater(HWTSystem.TransferColdWaterIn);
                            if (manager())
                            {
                                success = true;
                            }
                        }

                        // Turn the system off.
                        if (HWTSystem.PowerOn)
                        {
                            HWTSystem.PowerDown();
                        }

                        Console.WriteLine("System errors have been fixed " +
                            "and the system was shutdown successfully.");
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
            //RaiseWarning warning = null;
            bool status = true;
            bool emergencyOccured = false;
            bool controlRoomWarningAdded = false;
            bool managementWarningAdded = false;

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
                Console.WriteLine("\t5. Correct system errors");
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

                    // If this is the first emergency, create the warning 
                    // delegate and point it to the alert mechanism that 
                    // will notify the people working on the floor with
                    // the system hardware.
                    if (!emergencyOccured)
                    {
                        emergencyOccured = true;
                        cr.HWTSystem.OnRaiseWarning +=
                            new HotWaterTransferSystem.RaiseWarningHandler
                                (cr.HWTSystem.AlertTransferStationFloor);
                    }
                    else
                    {
                        // If the first warning was already posted, add in 
                        // another warning to the control room. Now both 
                        // the floor and the control room will be notified.
                        if (!controlRoomWarningAdded)
                        {
                            controlRoomWarningAdded = true;
                            cr.HWTSystem.OnRaiseWarning +=
                                new HotWaterTransferSystem.RaiseWarningHandler
                                    (cr.HWTSystem.AlertControlRoom);
                        }
                        else if 
                            (controlRoomWarningAdded && 
                            !managementWarningAdded)
                        {
                            // At this point, management needs to be notified
                            // because the warnings have not been cleared 
                            // yet. Add another method to the delegate's list
                            // of methods to call.
                            managementWarningAdded = true;
                            cr.HWTSystem.OnRaiseWarning +=
                                new HotWaterTransferSystem.RaiseWarningHandler
                                    (cr.HWTSystem.AlertManagement);
                        }
                    }

                    // Call the RaiseWarningEvent method. If there are no
                    // subscribers, this method will simply return.
                    cr.HWTSystem.RaiseWarningEvent();
                }
                else
                {
                    // All warnings have been corrected, or none have
                    // occured. Clear out the various flags and unload
                    // the multicast delegate by setting it to null.
                    emergencyOccured = false;
                    controlRoomWarningAdded = false;
                    managementWarningAdded = false;
                    cr.HWTSystem.ClearWarningList();
                }
            }
        }
    }
}
