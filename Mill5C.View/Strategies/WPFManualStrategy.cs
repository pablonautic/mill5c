using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Strategies;
using System.Windows;
using System.Windows.Input;
using Mill5C.Core.Geometry;

namespace Mill5C.View.Window.Strategies
{
    public class WPFManualStrategy : BaseManualStrategy
    {
        public WPFManualStrategy(FrameworkElement inputSource)
        {
            inputSource.KeyDown += new System.Windows.Input.KeyEventHandler(InputSourceKeyDownHandler);
        }

        private void InputSourceKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            float speed = 5;

            Vector3D trs = new Vector3D();
            if (e.Key == Key.W)
                trs.X = speed;
            if (e.Key == Key.S)
                trs.X = -speed;
            if (e.Key == Key.A)
                trs.Y = speed;
            if (e.Key == Key.D)
                trs.Y = -speed;
            if (e.Key == Key.R)
                trs.Z = speed;
            if (e.Key == Key.F)
                trs.Z = -speed;

            TranslateCutter(trs);

        }
    }
}
