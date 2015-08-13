using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

// From a sample at: http://dev.nomad-net.info/articles/animatedthrobber

namespace Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl
{
  [Designer(typeof(AnimatedThrobberDesigner))]
  [DefaultProperty("Style")]
  public class AnimatedThrobber : Control
  {
    private const int WS_BORDER = 0x800000;
    private const int WS_EX_CLIENTEDGE = 0x200;

    private ThrobberRenderer Renderer;
    private Timer Timer;
    private int Position;
    private BorderStyle FBorderStyle;

    public AnimatedThrobber()
      : base()
    {
      SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
        ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

      Renderer = new ThrobberRenderer();
      Renderer.Color = ForeColor;

      Timer = new Timer();
      Timer.Tick += Timer_Tick;
      Timer.Enabled = Enabled;
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.ExStyle &= (~WS_EX_CLIENTEDGE);
        cp.Style &= (~WS_BORDER);

        switch (FBorderStyle)
        {
          case BorderStyle.Fixed3D:
            cp.ExStyle |= WS_EX_CLIENTEDGE;
            break;
          case BorderStyle.FixedSingle:
            cp.Style |= WS_BORDER;
            break;
        }

        return cp;
      }
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
      base.OnEnabledChanged(e);
      Timer.Enabled = Enabled;
      Invalidate();
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
      base.OnForeColorChanged(e);
      Renderer.Color = ForeColor;
    }

    protected override void OnPaddingChanged(EventArgs e)
    {
      base.OnPaddingChanged(e);
      PerformAutoSize();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      using (e)
      {
        Rectangle DrawBounds = Rectangle.FromLTRB(Padding.Left, Padding.Top,
          ClientSize.Width - Padding.Right, ClientSize.Height - Padding.Bottom);
        ThrobberRenderEventArgs RenderArgs = new ThrobberRenderEventArgs(e.Graphics, DrawBounds, Position, Enabled);
        Renderer.DrawThrobber(RenderArgs);
        Position = RenderArgs.Position;
      }
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
      proposedSize = Renderer.GetPreferredSize(proposedSize);
      proposedSize.Width += Padding.Horizontal;
      proposedSize.Height += Padding.Vertical;
      return proposedSize;
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
      if (AutoSize & ((specified & BoundsSpecified.Size) > 0))
      {
        Size NewSize = PreferredSize;
        width = NewSize.Width;
        height = NewSize.Height;
      }
      base.SetBoundsCore(x, y, width, height, specified);
    }

    private void PerformAutoSize()
    {
      if (AutoSize)
        Width = 0;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      Position = ++Position % NumberOfSpoke;
      Invalidate();
    }

    #region Hide unused inherited properties

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override Font Font
    {
      get { return base.Font; }
      set { base.Font = value; }
    }

    [DefaultValue(false)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool TabStop
    {
      get { return base.TabStop; }
      set { base.TabStop = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override string Text
    {
      get { return base.Text; }
      set { base.Text = value; }
    }

    #endregion

    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Browsable(true)]
    public override bool AutoSize
    {
      get { return base.AutoSize; }
      set
      {
        base.AutoSize = value;
        PerformAutoSize();
      }
    }

    [Category("Appearance")]
    [DefaultValue(typeof(BorderStyle), "None")]
    public BorderStyle BorderStyle
    {
      get { return FBorderStyle; }
      set
      {
        if (FBorderStyle == value)
          return;

        if (!Enum.IsDefined(typeof(BorderStyle), value))
          throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));

        FBorderStyle = value;
        UpdateStyles();
      }
    }

    [Category("Behavior")]
    [DefaultValue(100)]
    public int AnimationSpeed
    {
      get { return Timer.Interval; }
      set { Timer.Interval = value; }
    }

    #region Map ThrobberRenderer properties

    private bool ShouldSerializeInnerCircleRadius()
    {
      return Renderer.Style == ThrobberStyle.Custom;
    }

    [Category("Throbber")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public int InnerCircleRadius
    {
      get { return Renderer.InnerCircleRadius; }
      set
      {
        if (Renderer.InnerCircleRadius != value)
        {
          Renderer.InnerCircleRadius = value;
          Invalidate();
        }
      }
    }

    private bool ShouldSerializeOuterCircleRadius()
    {
      return Renderer.Style == ThrobberStyle.Custom;
    }

    [Category("Throbber")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public int OuterCircleRadius
    {
      get { return Renderer.OuterCircleRadius; }
      set
      {
        if (Renderer.OuterCircleRadius != value)
        {
          Renderer.OuterCircleRadius = value;
          PerformAutoSize();
          Invalidate();
        }
      }
    }

    private bool ShouldSerializeNumberOfSpoke()
    {
      return Renderer.Style == ThrobberStyle.Custom;
    }

    [Category("Throbber")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public int NumberOfSpoke
    {
      get { return Renderer.NumberOfSpoke; }
      set
      {
        if (Renderer.NumberOfSpoke != value)
        {
          Renderer.NumberOfSpoke = value;
          Invalidate();
        }
      }
    }

    private bool ShouldSerializeSpokeThickness()
    {
      return Renderer.Style == ThrobberStyle.Custom;
    }

    [Category("Throbber")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public int SpokeThickness
    {
      get { return Renderer.SpokeThickness; }
      set
      {
        if (Renderer.SpokeThickness != value)
        {
          Renderer.SpokeThickness = value;
          Invalidate();
        }
      }
    }

    private bool ShouldSerializeStyle()
    {
      return Renderer.Style != ThrobberStyle.Custom;
    }

    [Category("Throbber")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public ThrobberStyle Style
    {
      get { return Renderer.Style; }
      set
      {
        if (Renderer.Style != value)
        {
          Renderer.Style = value;
          PerformAutoSize();
          Invalidate();
        }
      }
    }

    #endregion
  }
}
