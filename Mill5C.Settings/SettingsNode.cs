using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Mill5C.Settings
{
    [Serializable]
    public class SettingsNode
    {
        public string TypeObject { get; set; }
        public object ValueObject { get; set; }

        public SettingsNode[] Children { get; set; }

        public SettingsNode()
        {
        }

        public SettingsNode(params SettingsNode[] children)
        {
            Children = children;
        }

        public SettingsNode(params object[] valueTypeChildren)
        {
            Children = new SettingsNode[valueTypeChildren.Length];
            for (int i = 0; i < valueTypeChildren.Length; i++)
            {
                Children[i] = new SettingsNode { ValueObject = valueTypeChildren[i] };
            }
        }

        public void Save(System.IO.Stream stream)
        {
            XmlSerializer xms = Create();
            xms.Serialize(stream, this);
        }

        public void Save(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            Save(fs);
            fs.Close();
        }

        private static XmlSerializer Create()
        {
            return new XmlSerializer(typeof(SettingsNode), new Type[] { 
                typeof(Mill5C.Core.Geometry.Point3D), typeof(Mill5C.Core.Geometry.Vector3D), typeof(string[]) });
        }

        public static SettingsNode FromFile(System.IO.Stream stream)
        {
            XmlSerializer xms = Create();
            return (SettingsNode)xms.Deserialize(stream);
        }

        public static SettingsNode FromFile(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            SettingsNode sn = FromFile(fs);
            fs.Close();
            return sn;
        }
    }
}
