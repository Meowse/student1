using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceDemo
{
    class NewCollection : ISortable
    {
        private string[] _internalList;
        private const int MAX_WORD_SIZE = 5;

        public NewCollection(int size)
        {
            _internalList = new string[size];
            GenerateStrings();
        }

        public void Sort()
        {
            // Just like with integers, we can allow the Array class
            // to sort strings for us. No need for us to implement our
            // own sorting algorithm.
            Array.Sort(_internalList);
        }

        private void GenerateStrings()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            StringBuilder word = null;

            for (int i = 0; i < _internalList.Length; i++)
            {
                word = new StringBuilder();

                for (int j = 0; j < MAX_WORD_SIZE; j++)
                {
                    word.Append((char)r.Next(97, 122));
                    System.Threading.Thread.Sleep(71);
                }

                _internalList[i] = word.ToString();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _internalList.Length; i++)
            {
                sb.AppendFormat("\t{0}\n", _internalList[i]);
            }

            return sb.ToString();
        }
    }
}
