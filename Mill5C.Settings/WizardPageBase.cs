using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mill5C.Settings
{
    public class WizardPageBase : UserControl
    {
        public SettingsNode Data { get; protected set; }

        public virtual string Validate2()
        {
            return null;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WizardPageBase
            // 
            this.Name = "WizardPageBase";
            this.Size = new System.Drawing.Size(515, 320);
            this.ResumeLayout(false);

        }
    }
}
