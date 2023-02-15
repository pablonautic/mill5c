using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mill5C.View.Window.Views.XNA
{
    public class ArcBallCamera
    {
        public float DOLLYING_SPEED = 0.20f;
        public float ROTATION_SPEED = 0.50f;
        public float MOUSEWHEEL_SPEED = 0.50f;
        public float TRACKING_SPEED = 0.01f;

        public float offset;
        public Vector2 rotation;
        public Vector2 translate;
        public Vector3 position;
        public Vector3 target;
        public Vector3 viewDir;
        public Quaternion orientation;
        public Matrix View;

        private MouseState currentMouseState;
        private MouseState prevMouseState;

        public ArcBallCamera()
        {
            target = Vector3.Zero;
            offset = 300;
            orientation = Quaternion.CreateFromRotationMatrix(
                Matrix.CreateLookAt(new Vector3(200, 200, 200), Vector3.Zero, new Vector3(0, 0, 1)));
        }

        public void Update()
        {
            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            this.translate.X = this.translate.Y = 0.0f;
            this.rotation.X = this.rotation.Y = 0.0f;

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                dz = currentMouseState.Y - prevMouseState.Y;
                dz *= this.DOLLYING_SPEED;

                this.offset -= dz;
            }
            else if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                dx = currentMouseState.X - prevMouseState.X;
                dx *= this.ROTATION_SPEED;

                dy = currentMouseState.Y - prevMouseState.Y;
                dy *= this.ROTATION_SPEED;

                this.rotation.X = dy;
                this.rotation.Y = dx;
            }

            // Process mouse wheel scrolling.

            if (currentMouseState.ScrollWheelValue > prevMouseState.ScrollWheelValue)
                this.offset -= this.MOUSEWHEEL_SPEED;
            else if (currentMouseState.ScrollWheelValue < prevMouseState.ScrollWheelValue)
                this.offset += this.MOUSEWHEEL_SPEED;

            int deltaX = currentMouseState.X - prevMouseState.X;
            int deltaY = currentMouseState.Y - prevMouseState.Y;
            bool idle = deltaX == 0 && deltaY == 0;

            Quaternion rotation = Quaternion.Identity;
            float heading = MathHelper.ToRadians(this.rotation.Y);
            float pitch = MathHelper.ToRadians(this.rotation.X);

            if (heading != 0.0f)
            {
                rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, heading);
                Quaternion.Concatenate(ref rotation, ref this.orientation, out this.orientation);
            }

            if (pitch != 0.0f)
            {
                rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, pitch);
                Quaternion.Concatenate(ref this.orientation, ref rotation, out this.orientation);
            }

            Matrix.CreateFromQuaternion(ref this.orientation, out this.View);

            Vector3 xAxis = new Vector3(this.View.M11, this.View.M21, this.View.M31);
            Vector3 yAxis = new Vector3(this.View.M12, this.View.M22, this.View.M32);
            Vector3 zAxis = new Vector3(this.View.M13, this.View.M23, this.View.M33);

            this.target -= xAxis * this.translate.X;
            this.target -= yAxis * this.translate.Y;

            this.position = this.target + zAxis * this.offset;

            this.View.M41 = -Vector3.Dot(xAxis, this.position);
            this.View.M42 = -Vector3.Dot(yAxis, this.position);
            this.View.M43 = -Vector3.Dot(zAxis, this.position);

            Vector3.Negate(ref zAxis, out this.viewDir);
        }
    }
}
