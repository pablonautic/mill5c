using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mill5C.Settings
{
    public class GenericObjectFactory<T>
    {
        public T Create(SettingsNode rootNode)
        {
            return (T)EvalNode(rootNode);
        }

        private object EvalNode(SettingsNode node)
        {
            if (node.TypeObject == null)
                return node.ValueObject;

            object[] args = new object[node.Children.Length];

            for (int i = 0; i < args.Length; i++)
                args[i] = EvalNode((SettingsNode)node.Children[i]);

            Type t = Type.GetType(node.TypeObject);

            return Activator.CreateInstance(t, args);
        }
    }
}
