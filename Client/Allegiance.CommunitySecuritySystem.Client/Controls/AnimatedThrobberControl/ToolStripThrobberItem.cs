using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

// From a sample at: http://dev.nomad-net.info/articles/animatedthrobber

namespace Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl
{
  [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
  public class ToolStripThrobberItem : ToolStripItem
  {
    private ThrobberRenderer Renderer;
    private Timer Timer;
    private int Position;

    public ToolStripThrobberItem()
      : base()
    {
      Renderer = new ThrobberRenderer();
      Renderer.Color = ForeColor;

      Timer = new Timer();
      Timer.Tick += Timer_Tick;
      Timer.Enabled = Enabled;
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

    protected override void OnPaint(PaintEventArgs e)
    {
      using (e)
      {
        ToolStripItemRenderEventArgs ItemArgs = new ToolStripItemRenderEventArgs(e.Graphics, this);
        Parent.Renderer.DrawLabelBackground(ItemArgs);

        Rectangle DrawBounds = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical);
        ThrobberRenderEventArgs RenderArgs = new ThrobberRenderEventArgs(e.Graphics, DrawBounds, Position, Enabled);
        Renderer.DrawThrobber(RenderArgs);
        Position = RenderArgs.Position;
      }
    }

    public override Size GetPreferredSize(Size constrainingSize)
    {
      constrainingSize = Renderer.GetPreferredSize(constrainingSize);
      constrainingSize.Width += Padding.Horizontal;
      constrainingSize.Height += Padding.Vertical;
      return constrainingSize;
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
    public new bool AutoToolTip
    {
      get { return base.AutoToolTip; }
      set { base.AutoToolTip = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override ToolStripItemDisplayStyle DisplayStyle
    {
      get { return base.DisplayStyle; }
      set { base.DisplayStyle = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override Font Font
    {
      get { return base.Font; }
      set { base.Font = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new bool RightToLeftAutoMirrorImage
    {
      get { return base.RightToLeftAutoMirrorImage; }
      set { base.RightToLeftAutoMirrorImage = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override Image Image
    {
      get { return base.Image; }
      set { base.Image = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new ContentAlignment ImageAlign
    {
      get { return base.ImageAlign; }
      set { base.ImageAlign = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new int ImageIndex
    {
      get { return base.ImageIndex; }
      set { base.ImageIndex = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new string ImageKey
    {
      get { return base.ImageKey; }
      set { base.ImageKey = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new ToolStripItemImageScaling ImageScaling
    {
      get { return base.ImageScaling; }
      set { base.ImageScaling = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new Color ImageTransparentColor
    {
      get { return base.ImageTransparentColor; }
      set { base.ImageTransparentColor = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override string Text
    {
      get { return base.Text; }
      set { base.Text = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override ContentAlignment TextAlign
    {
      get { return base.TextAlign; }
      set { base.TextAlign = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new TextImageRelation TextImageRelation
    {
      get { return base.TextImageRelation; }
      set { base.TextImageRelation = value; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override ToolStripTextDirection TextDirection
    {
      get { return base.TextDirection; }
      set { base.TextDirection = value; }
    }

    #endregion

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
