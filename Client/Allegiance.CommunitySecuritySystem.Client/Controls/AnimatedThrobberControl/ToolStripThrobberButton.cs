using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

// From a sample at: http://dev.nomad-net.info/articles/animatedthrobber

namespace Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl
{
  [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
  public class ToolStripThrobberButton : ToolStripButton
  {
    private ThrobberRenderer Renderer;
    private Timer Timer;
    private int Position;

    public ToolStripThrobberButton()
      : base()
    {
      Renderer = new ThrobberRenderer();

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

    protected override void OnPaint(PaintEventArgs e)
    {
      using (e)
      {
        switch (DisplayStyle)
        {
          case ToolStripItemDisplayStyle.Image:
          case ToolStripItemDisplayStyle.ImageAndText:
            Rectangle ThrobberBounds = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical);
            Rectangle TextBounds;

            if (DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)
            {
              Size ThrobberSize = Renderer.GetPreferredSize(Size);
              TextBounds = ThrobberBounds;

              switch (TextImageRelation)
              {
                case TextImageRelation.ImageAboveText:
                  ThrobberBounds.Height = ThrobberSize.Height;
                  TextBounds.Y += ThrobberSize.Height;
                  TextBounds.Height -= ThrobberSize.Height;
                  break;
                case TextImageRelation.TextAboveImage:
                  ThrobberBounds.Y = ThrobberBounds.Bottom - ThrobberSize.Height;
                  ThrobberBounds.Height = ThrobberSize.Height;
                  TextBounds.Height -= ThrobberSize.Height;
                  break;
                case TextImageRelation.ImageBeforeText:
                  ThrobberBounds.Width = ThrobberSize.Width;
                  TextBounds.X += ThrobberSize.Width;
                  TextBounds.Width -= ThrobberSize.Width;
                  break;
                case TextImageRelation.TextBeforeImage:
                  ThrobberBounds.X = ThrobberBounds.Right - ThrobberSize.Width;
                  ThrobberBounds.Width = ThrobberSize.Width;
                  TextBounds.Width -= ThrobberSize.Width;
                  break;
              }
            }
            else
              TextBounds = Rectangle.Empty;

            ToolStripItemRenderEventArgs ItemArgs = new ToolStripItemRenderEventArgs(e.Graphics, this);
            Parent.Renderer.DrawButtonBackground(ItemArgs);

            ThrobberRenderEventArgs RenderArgs = new ThrobberRenderEventArgs(e.Graphics, ThrobberBounds, Position, Enabled);
            Renderer.DrawThrobber(RenderArgs);
            Position = RenderArgs.Position;

            if (!TextBounds.IsEmpty)
            {
              ToolStripItemTextRenderEventArgs TextArgs = new ToolStripItemTextRenderEventArgs(e.Graphics, this, Text,
                TextBounds, ForeColor, Font, TextAlign);
              Parent.Renderer.DrawItemText(TextArgs);
            }

            break;
          default:
            base.OnPaint(e);
            break;
        }
      }
    }

    public override Size GetPreferredSize(Size constrainingSize)
    {
      switch (DisplayStyle)
      {
        case ToolStripItemDisplayStyle.Image:
          constrainingSize = Renderer.GetPreferredSize(constrainingSize);
          break;
        case ToolStripItemDisplayStyle.ImageAndText:
          Size ThrobberSize = Renderer.GetPreferredSize(constrainingSize);
          Size ButtonSize = base.GetPreferredSize(constrainingSize);
          ButtonSize.Width -= Padding.Horizontal;
          ButtonSize.Height -= Padding.Vertical;

          switch (TextImageRelation)
          {
            case TextImageRelation.TextAboveImage:
            case TextImageRelation.ImageAboveText:
              constrainingSize = new Size(Math.Max(ThrobberSize.Width, ButtonSize.Width),
                ButtonSize.Height + ThrobberSize.Height);
              break;
            case TextImageRelation.TextBeforeImage:
            case TextImageRelation.ImageBeforeText:
              constrainingSize = new Size(ButtonSize.Width + ThrobberSize.Width,
                Math.Max(ThrobberSize.Height, ButtonSize.Height));
              break;
            default:
              constrainingSize = new Size(Math.Max(ThrobberSize.Width, ButtonSize.Width),
                Math.Max(ThrobberSize.Height, ButtonSize.Height));
              break;
          }

          break;
        default:
          return base.GetPreferredSize(constrainingSize);
      }

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

    #endregion

    [Category("Behavior")]
    [DefaultValue(100)]
    public int AnimationSpeed
    {
      get { return Timer.Interval; }
      set { Timer.Interval = value; }
    }

    #region Map ThrobberRenderer properties

    [Category("Throbber")]
    [DefaultValue(typeof(Color), "Gray")]
    public Color ThrobberColor
    {
      get { return Renderer.Color; }
      set
      {
        if (Renderer.Color != value)
        {
          Renderer.Color = value;
          Invalidate();
        }
      }
    }

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
