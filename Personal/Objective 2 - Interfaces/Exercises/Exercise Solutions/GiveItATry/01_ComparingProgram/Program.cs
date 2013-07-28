using System;
using System.Collections;
using System.Text;

namespace ComparingProgram
{
    internal class ClientRecord : IEqualityComparer
    {
        private int _clientId;
        private string _clientName;

        public ClientRecord(int id, string name)
        {
            ClientId = id;
            ClientName = name;
        }

        public int ClientId
        {
            get { return _clientId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ClientId must be greater than 0.");
                }

                _clientId = value;
            }
        }

        public string ClientName
        {
            get { return _clientName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The client name cannot be empty.");
                }

                _clientName = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: {1}", ClientId.ToString(), ClientName);
            return (sb.ToString());
        }

        public new bool Equals(object a, object b)
        {
            bool result = false;

            // See if the objects can be converted to ClientRecord.
            ClientRecord cr1 = a as ClientRecord;
            ClientRecord cr2 = b as ClientRecord;

            // If either or both of the objects are null, we'll
            // assume that they are not equal.
            if (cr1 != null && cr2 != null)
            {
                // If the references are the same, then the objects
                // are equal.
                if (cr1 == cr2)
                {
                    result = true;
                }

                // See if the client IDs are the same. If the references
                // were the same above, this check wouldn't really be
                // needed. But it keeps the code clean.
                if (cr1.ClientId == cr2.ClientId)
                {
                    result = true;
                }
            }

            return result;
        }

        public int GetHashCode(object o)
        {
            ClientRecord cr = o as ClientRecord;
            int result = 0;

            if (cr != null)
            {
                result = cr.ClientId.GetHashCode();
            }

            return result;
        }
    }

    class Program
    {
        private static string AreEqual(IEqualityComparer iec1, IEqualityComparer iec2)
        {
            string operand;

            if (iec1.Equals(iec1, iec2))
            {
                operand = " == ";
            }
            else
            {
                operand = " != ";
            }

            return operand;
        }

        static void Main()
        {
            ClientRecord cr1 = new ClientRecord(1, "Joe");
            ClientRecord cr2 = new ClientRecord(2, "Jane");
            ClientRecord cr3 = new ClientRecord(3, "Tim");
            ClientRecord cr4 = new ClientRecord(4, "Sarah");
            ClientRecord cr5 = new ClientRecord(3, "Tim");
            ClientRecord cr6 = cr2;

            string operand;
            
            operand = AreEqual(cr1, cr2);
            Console.WriteLine("{0}{1}{2}", cr1, operand, cr2);

            operand = AreEqual(cr3, cr5);
            Console.WriteLine("{0}{1}{2}", cr3, operand, cr5);

            operand = AreEqual(cr2, cr6);
            Console.WriteLine("{0}{1}{2}", cr2, operand, cr6);

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
