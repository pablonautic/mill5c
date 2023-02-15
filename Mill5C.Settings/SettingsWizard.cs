using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mill5C.Settings.EnginePages;

namespace Mill5C.Settings
{
    public partial class SettingsWizard : Form
    {
        public List<WizardPageBase> Pages { get; private set; }

        public int CurrentPageIndex { get; private set; }
        public WizardPageBase CurrentPage { get { return Pages[CurrentPageIndex]; } }

        public Page01 Page01 { get; private set; }
        public Page02 Page02 { get; private set; }

        public SettingsNode DataDefault
        {
            get
            {
                SettingsNode sn = Page01.Data;
                if (Page02.Data != null)
                {
                    //make place for the new arg
                    SettingsNode[] newChildren = new SettingsNode[sn.Children.Length + 1];

                    //copy old args
                    for (int i = 0; i < sn.Children.Length; i++)
                        newChildren[i] = sn.Children[i];

                    //input files list is the last arg
                    newChildren[newChildren.Length - 1] = newChildren[newChildren.Length - 2];

                    //pre-last is the output file
                    newChildren[newChildren.Length - 2] = Page02.Data;

                    sn.Children = newChildren;
                }
                return sn;
            }
        }

        public SettingsWizard()
        {
            InitializeComponent();
            Pages = new List<WizardPageBase>();
            Page01 = new Page01();
            Pages.Add(Page01);
            Page02 = new Page02();
            Pages.Add(Page02);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateGui();
        }

        private void UpdateGui()
        {
            BackButton.Visible = !FirstPage;

            if (LastPage)
                NextFinishButton.Text = "Finish";
            else
                NextFinishButton.Text = "Next";

            ContentPanel.Controls.Clear();
            ContentPanel.Controls.Add(CurrentPage);
            CurrentPage.Dock = DockStyle.Fill;
        }

        private bool Validate2()
        {
            string msg = CurrentPage.Validate2();
            if (msg != null)
            {
                MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            CurrentPageIndex--;
            UpdateGui();
        }

        private void NextFinishButton_Click(object sender, EventArgs e)
        {
            if (!Validate2())
                return;

            if (!LastPage)
            {
                CurrentPageIndex++;
                UpdateGui();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public bool FirstPage { get { return CurrentPageIndex == 0; } }
        public bool LastPage { get { return CurrentPageIndex == Pages.Count - 1; } }
    }
}
