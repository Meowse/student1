using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimpleFormsApp
{
    internal partial class _mainForm : Form
    {
        internal _mainForm()
        {
            InitializeComponent();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This message box appeared as a result of clicking the RUN button.", 
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}