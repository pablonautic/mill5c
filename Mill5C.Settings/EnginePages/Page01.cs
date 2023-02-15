using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mill5C.Core.Strategies;
using Mill5C.Core.Strategies.Octree;
using Mill5C.Core.Materials;
using Mill5C.Core.Interpolators;
using Mill5C.Core.Geometry;
using Mill5C.Core.Algorithm;

namespace Mill5C.Settings.EnginePages
{
    public partial class Page01 : WizardPageBase
    {
        public Type ManualControlType { get; set; }
        public Type SingleThreadedType { get; set; }
        public Type MultiThreadedTypeNormal { get; set; }
        public Type MultiThreadedTypePE { get; set; }

        public Type MaterialType { get; set; }

        public Type InterpolatorType { get; set; }

        public object[] ManualControlArgs { get; set; }

        private string errors;

        public Page01()
        {
            InitializeComponent();

            ManualControlType = typeof(BaseManualStrategy);
            SingleThreadedType = typeof(BasicSingleThreadedStrategy);
            MultiThreadedTypeNormal = typeof(BasicMultiThreadedStrategy);
            MultiThreadedTypePE = typeof(ParallelExtStrategy);

            MaterialType = typeof(OctreeMaterial);
            InterpolatorType = typeof(LCInterpolator);
        }

        private void FilesButton_Click(object sender, EventArgs e)
        {
            if (openFileDialogFiles.ShowDialog() == DialogResult.OK)
            {
                var files = openFileDialogFiles.FileNames.ToList();
                files.Sort();
                FilesListBox.Items.AddRange(files.ToArray());
            }
        }

        private void DeleteFileButton_Click(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedIndex != -1)
                FilesListBox.Items.RemoveAt(FilesListBox.SelectedIndex);
        }

        private void UpFileButton_Click(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedIndex == -1 || FilesListBox.SelectedIndex == 0)
                return;

            MoveItem(-1);
        }

        private void DownFileButton_Click(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedIndex == -1 || FilesListBox.SelectedIndex == FilesListBox.Items.Count - 1)
                return;

            MoveItem(+1);
        }

        private void MoveItem(int offset)
        {
            int index = FilesListBox.SelectedIndex;
            var item = FilesListBox.Items[index];
            FilesListBox.Items.Remove(item);
            FilesListBox.Items.Insert(index  + offset, item);
            FilesListBox.SelectedIndex = index  + offset;
        }

        private bool TryGatherData()
        {
            errors = null;

            Point3D materialCenter;
            if (!Point3D.TryParse(CenterTextBox.Text, out materialCenter, ";"))
            {
                errors = "Invalid material origin point.";
                return false;
            }
            float edgeLength;
            if (!SingleExtension.TryParse(EdgeLengthTextBox.Text, out edgeLength) || edgeLength < 0)
            {
                errors = "Invalid material edge length.";
                return false;
            }
            float eps;
            if (!SingleExtension.TryParse(StopCondTextBox.Text, out eps) || eps < 0)
            {
                errors = "Invalid material stop condition.";
                return false;
            }
            float posDelta;
            if (!SingleExtension.TryParse(PositionDeltaTextBox.Text, out posDelta))
            {
                errors = "Invalid interpolator position delta.";
                return false;
            }
            float orientDelta;
            if (!SingleExtension.TryParse(OrientationDeltaTextBox.Text, out orientDelta))
            {
                errors = "Invalid interpolator orientation delta.";
                return false;
            }

            SettingsNode materialNode = new SettingsNode(materialCenter, edgeLength / 2, eps) 
            { 
                TypeObject = MaterialType.AssemblyQualifiedName 
            };
            SettingsNode interpolatorNode = new SettingsNode(posDelta, orientDelta) 
            { 
                TypeObject = InterpolatorType.AssemblyQualifiedName 
            };

            string[] fileNames = new string[FilesListBox.Items.Count];
            for (int i = 0; i < FilesListBox.Items.Count; i++)
                fileNames[i] = FilesListBox.Items[i].ToString();

            SettingsNode files = new SettingsNode { ValueObject = fileNames };

            SettingsNode strategy = null;

            if (ManualRadioButton.Checked)
            {
                strategy = new SettingsNode(ManualControlArgs ?? new object[0]) { TypeObject = ManualControlType.AssemblyQualifiedName };
            }
            else if (SinglethreadedRadioButton.Checked)
            {
                strategy = new SettingsNode(interpolatorNode ) { TypeObject = SingleThreadedType.AssemblyQualifiedName };
            }
            else if (MultithreadedNormalRadioButton.Checked)
            {
                strategy = new SettingsNode(interpolatorNode, new SettingsNode { ValueObject = (int)ThreadsNumericUpDown.Value }) 
                    { TypeObject = MultiThreadedTypeNormal.AssemblyQualifiedName };
            }
            else if (MultithreadedPERadioButton.Checked)
            {
                strategy = new SettingsNode(interpolatorNode, new SettingsNode { ValueObject = (int)ParallelDepthNumericUpDown.Value }) { TypeObject = MultiThreadedTypePE.AssemblyQualifiedName };
            }

            Data = new SettingsNode(strategy, materialNode, files) { TypeObject = typeof(Engine).AssemblyQualifiedName };

            return true;
        }

        public override string Validate2()
        {
            TryGatherData();
            return errors;
        }

        private void FilesListBox_DragDrop(object sender, DragEventArgs e)
        {

        }

    }
}
