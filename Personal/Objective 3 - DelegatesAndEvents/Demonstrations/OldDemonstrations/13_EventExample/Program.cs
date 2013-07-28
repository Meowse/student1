using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleFormsApp
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new _mainForm());
        }
    }
}