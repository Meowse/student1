/*********************************************************************************
 * 
 *  This program is another demonstration of how a delegate might be used. In 
 *  typical IT departments within a company, it is the responsibility of the 
 *  staff to ensure that all operations are running smoothly. In the event 
 *  something goes awry, staff needs to be notified so that they can handle the
 *  problem. Depending on the time of day, the notification may come in different
 *  forms. For example, if the current time is during a week day, most of the 
 *  staff will be at work, many with email open. So the notificate can just eb
 *  sent to email addresses. On weekday evenings, the staff is required to carry
 *  a pager so that notifications can reach them. Before the staff leaves for
 *  the weekend, each individual must specify an actual phone number that they
 *  can be reached when a problem is encountered. 
 * 
 *  This program demonstrates how delegates can be used to send notifications
 *  to different destinations. In addition, if a new destination is discovered,
 *  like video phone for example, that code can be easily dropped in an added
 *  to the delegate.
 * 
 ********************************************************************************/

using System;

// Declare the delegate that will be used to call the various types of 
// alarm signaling methods. Notice the return type on this delegate.
// Because it's void, this delegate is a multicast delegate.
internal delegate void RaiseAlarm (string text);

// This class signals an alarm via an email.
internal class EmailAlarm
{
    internal void SendEmail(string text)
    {
        Console.WriteLine("\n   EMAIL: Sending message [ " + text + " ].");
    }
}

// This class signals an alarm via a pager.
internal class PagerAlarm
{
    private string _phoneNumber;

    internal PagerAlarm()
    {
        Console.Write ("\nEnter the pager number: ");
        _phoneNumber = Console.ReadLine();
    }

    internal void SignalPager(string text)
    {
        Console.WriteLine("\n   PAGER: Sending message [ " + text + " ] to pager an number " +
            _phoneNumber + ".");
    }
}

// This method signals an alarm via a phone.
internal class PhoneAlarm
{
    private string _phoneNumber;

    internal PhoneAlarm()
    {
        Console.Write ("Enter the phone number: ");
        _phoneNumber = Console.ReadLine();
    }

    internal void SignalPhone(string text)
    {
        Console.WriteLine("\n   PHONE: Sending message [ " + text + " ] to pager an number " +
            _phoneNumber + ".");
    }
}

internal class AlarmInitializer
{
    static void Main(string[] args)
    {
        RaiseAlarm notifier = null;
        EmailAlarm ea = null;
        PagerAlarm pa = null;
        PhoneAlarm ha = null;

        // Prompt the user for the type of alarm they will want to raise.
        Console.WriteLine ("Please choose a level of alarm notification for this evening.\n");
        Console.WriteLine ("   1. Not urgent (Email only)");
        Console.WriteLine ("   2. Semi-urgent (Email and Pager only)");
        Console.WriteLine ("   3. Extremely urgent (Email, Pager, and Phone)");
        Console.WriteLine ("   0. Abort notification setup.");
        Console.Write ("\nEnter your choice: ");
        string choice = Console.ReadLine();

        // Determine what choice the user made.
        switch (choice)
        {
            case "1":
                // Set up the delegate to just send an email.
                ea = new EmailAlarm();
                notifier = new RaiseAlarm(ea.SendEmail);
                break;

            case "2":
                // Set up the delegate to send email and page a number.
                ea = new EmailAlarm();
                pa = new PagerAlarm();
                notifier = new RaiseAlarm(pa.SignalPager);
                notifier += new RaiseAlarm(ea.SendEmail);
                break;

            case "3":
                // Set up the delegate to send email, send a page, and make a call.
                ea = new EmailAlarm();
                pa = new PagerAlarm();
                ha = new PhoneAlarm();
                notifier = new RaiseAlarm(ha.SignalPhone);
                notifier += new RaiseAlarm(pa.SignalPager);
                notifier += new RaiseAlarm(ea.SendEmail);
                break;

            case "0":
                // The user wishes to exit.
                Console.WriteLine ("\nAborting alarm initialization.");
                break;

            default:
                // The menu choice entered is not valid.
                Console.WriteLine ("\nYour choice of " + choice + " is not valid - ABORTING!");
                break;
        }

        // Check to see if the notifier delegate was created.
        if (notifier != null)
        {
            // Prompt for the text to send.
            Console.Write ("\nEnter the text you wish to send: ");
            string text = Console.ReadLine();

            // Notify the appropriate target.
            Console.Write("Please wait...");
            System.Threading.Thread.Sleep(5000);
            notifier(text);
        }

        Console.Write("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
