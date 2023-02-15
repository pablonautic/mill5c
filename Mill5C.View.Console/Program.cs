using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Settings;
using Mill5C.Core.Algorithm;

namespace Mill5C.View.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                SettingsNode[] sn = null;
                if (args.Length > 0)
                {
                    sn = new SettingsNode[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        sn[i] = SettingsNode.FromFile(args[i]);
                    }
                }
                else
                {
                    SettingsWizard sw = new SettingsWizard();
                    if (sw.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        sn = new SettingsNode[1];
                        sn[0] = sw.DataDefault;
                    }
                    else
                        return;
                }
                for (int i = 0; i < sn.Length; i++)
                {
                    Engine engine = new GenericObjectFactory<Engine>().Create(sn[i]);
                    engine.Start();
                    engine.Wait();
                    engine = null;
                    GC.Collect();
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
            }
        }
    }
}
