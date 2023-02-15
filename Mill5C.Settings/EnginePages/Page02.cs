using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mill5C.Settings.EnginePages
{
    public partial class Page02 : WizardPageBase
    {
        public Page02()
        {
            InitializeComponent();
        }

        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialogOutput.ShowDialog() == DialogResult.OK)
            {
                PathTextBox.Text = saveFileDialogOutput.FileName;
            }
        }

        public override string Validate2()
        {
            if (!string.IsNullOrEmpty(PathTextBox.Text))
                Data = new SettingsNode { ValueObject = PathTextBox.Text };
            else
                Data = null;

            return null;
        }
    }
}
