using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Allegiance.CommunitySecuritySystem.Client
{
    /// <summary>
    /// GradientPanel is just like a regular panel except it optionally  
    /// shows a gradient.
    /// </summary>
    [ToolboxBitmap(typeof(Form))]
    public class GradientForm : Form
    {
        /// <summary>
        /// Property GradientColor (Color)
        /// </summary>
        private Color _gradientColor;
        public Color GradientColor
        {
            get { return this._gradientColor; }
            set { this._gradientColor = value; }
        }

        /// <summary>
        /// Property Rotation (float)
        /// </summary>
        private float _rotation;
        public float Rotation
        {
            get { return this._rotation; }
            set { this._rotation = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (e.ClipRectangle.IsEmpty) return; //why draw if non-visible?

            using (LinearGradientBrush lgb = new
                           LinearGradientBrush(this.ClientRectangle,
                      this.BackColor,
                      this.GradientColor,
                      this.Rotation))
            {
                //lgb.SetSigmaBellShape(0.8f);
                e.Graphics.FillRectangle(lgb, this.ClientRectangle);
            }

            base.OnPaint(e); //right, want anything handled to be drawn too.
        }
    }
}