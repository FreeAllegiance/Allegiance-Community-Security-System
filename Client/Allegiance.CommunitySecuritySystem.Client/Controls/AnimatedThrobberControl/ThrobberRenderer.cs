using System;
using System.Drawing;
using System.Drawing.Drawing2D;

// From a sample at: http://dev.nomad-net.info/articles/animatedthrobber

namespace Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl
{
  public enum ThrobberStyle
  {
    Custom,
    MacOSX,
    Firefox,
    IE7
  }

  public class ThrobberRenderEventArgs : EventArgs
  {
    public ThrobberRenderEventArgs(Graphics graphics, Rectangle bounds, int position, bool enabled)
    {
      Graphics = graphics;
      Bounds = bounds;
      Position = position;
      Enabled = enabled;
    }

    public readonly bool Enabled;
    public readonly Graphics Graphics;
    public readonly Rectangle Bounds;
    public int Position;
  }

  public class ThrobberRenderer
  {
    private const double NumberOfDegreesInCircle = 360f;
    private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;

    private const int MacOSXInnerCircleRadius = 5;
    private const int MacOSXOuterCircleRadius = 11;
    private const int MacOSXNumberOfSpoke = 12;
    private const int MacOSXSpokeThickness = 2;

    private const int FireFoxInnerCircleRadius = 6;
    private const int FireFoxOuterCircleRadius = 7;
    private const int FireFoxNumberOfSpoke = 9;
    private const int FireFoxSpokeThickness = 4;

    private const int IE7InnerCircleRadius = 8;
    private const int IE7OuterCircleRadius = 9;
    private const int IE7NumberOfSpoke = 24;
    private const int IE7SpokeThickness = 4;

    private int FInnerCircleRadius = 8;
    private int FOuterCircleRadius = 10;
    private int FNumberOfSpoke = 10;
    private int FSpokeThickness = 4;
    private Color FColor = Color.Gray;
    private Color[] FPalette;
    private double[] FSpokeAngles;
    private ThrobberStyle FStyle; // ThrobberStyle.Custom;

    public ThrobberRenderer()
    {
      GeneratePallete();
      GenerateSpokeAngles();
    }

    public Color Color
    {
      get { return FColor; }
      set
      {
        if (FColor == value)
          return;

        FColor = value;
        GeneratePallete();
      }
    }

    public int InnerCircleRadius
    {
      get { return FInnerCircleRadius; }
      set
      {
        if ((FInnerCircleRadius == value) || (value < 1))
          return;

        FInnerCircleRadius = value;
        FStyle = ThrobberStyle.Custom;
      }
    }

    public int OuterCircleRadius
    {
      get { return FOuterCircleRadius; }
      set
      {
        if ((FOuterCircleRadius == value) || (value < 1))
          return;

        FOuterCircleRadius = value;
        FStyle = ThrobberStyle.Custom;
      }
    }

    public int NumberOfSpoke
    {
      get { return FNumberOfSpoke; }
      set
      {
        if ((FNumberOfSpoke == value) || (value < 1))
          return;

        FNumberOfSpoke = value;
        FStyle = ThrobberStyle.Custom;
        GeneratePallete();
        GenerateSpokeAngles();
      }
    }

    public int SpokeThickness
    {
      get { return FSpokeThickness; }
      set
      {
        if ((FSpokeThickness == value) || (value < 1))
          return;

        FSpokeThickness = value;
        FStyle = ThrobberStyle.Custom;
      }
    }

    public ThrobberStyle Style
    {
      get { return FStyle; }
      set
      {
        if (FStyle == value)
          return;

        FStyle = value;
        switch (FStyle)
        {
          case ThrobberStyle.Firefox:
            FInnerCircleRadius = FireFoxInnerCircleRadius;
            FOuterCircleRadius = FireFoxOuterCircleRadius;
            FNumberOfSpoke = FireFoxNumberOfSpoke;
            FSpokeThickness = FireFoxSpokeThickness;
            break;
          case ThrobberStyle.IE7:
            FInnerCircleRadius = IE7InnerCircleRadius;
            FOuterCircleRadius = IE7OuterCircleRadius;
            FNumberOfSpoke = IE7NumberOfSpoke;
            FSpokeThickness = IE7SpokeThickness;
            break;
          case ThrobberStyle.MacOSX:
            FInnerCircleRadius = MacOSXInnerCircleRadius;
            FOuterCircleRadius = MacOSXOuterCircleRadius;
            FNumberOfSpoke = MacOSXNumberOfSpoke;
            FSpokeThickness = MacOSXSpokeThickness;
            break;
          default:
            break;
        }

        GeneratePallete();
        GenerateSpokeAngles();
      }
    }

    private Color Darken(Color color, int percent)
    {
      int intRed = color.R;
      int intGreen = color.G;
      int intBlue = color.B;
      return Color.FromArgb(percent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
    }

    private void GeneratePallete()
    {
      FPalette = GeneratePallete(FColor, FNumberOfSpoke);
    }

    private Color[] GeneratePallete(Color color, int spokeNumber)
    {
      Color[] objColors = new Color[NumberOfSpoke];

      // Value is used to simulate a gradient feel... For each spoke, the 
      // color will be darken by value in intIncrement.
      byte bytIncrement = (byte)(byte.MaxValue / NumberOfSpoke);

      //Reset variable in case of multiple passes
      byte PERCENTAGE_OF_DARKEN = 0;

      for (int I = 0; I < NumberOfSpoke; I++)
      {
        if (I == 0 || I < NumberOfSpoke - spokeNumber)
          objColors[I] = color;
        else
        {
          // Increment alpha channel color
          PERCENTAGE_OF_DARKEN += bytIncrement;

          // Ensure that we don't exceed the maximum alpha
          // channel value (255)
          if (PERCENTAGE_OF_DARKEN > byte.MaxValue)
            PERCENTAGE_OF_DARKEN = byte.MaxValue;

          // Determine the spoke forecolor
          objColors[I] = Darken(color, PERCENTAGE_OF_DARKEN);
        }
      }

      return objColors;
    }

    private void DrawLine(Graphics graphics, PointF pt1, PointF pt2, Color color, int lineThickness)
    {
      using (Pen LinePen = new Pen(color, lineThickness))
      {
        LinePen.StartCap = LineCap.Round;
        LinePen.EndCap = LineCap.Round;
        graphics.DrawLine(LinePen, pt1, pt2);
      }
    }

    private PointF GetCoordinate(PointF center, int radius, double angle)
    {
      double dblAngle = Math.PI * angle / NumberOfDegreesInHalfCircle;
      return new PointF(center.X + radius * (float)Math.Cos(dblAngle), center.Y + radius * (float)Math.Sin(dblAngle));
    }

    private void GenerateSpokeAngles()
    {
      FSpokeAngles = GenerateSpokeAngles(NumberOfSpoke);
    }

    private double[] GenerateSpokeAngles(int numberOfSpoke)
    {
      double[] Angles = new double[numberOfSpoke];
      double dblAngle = (double)NumberOfDegreesInCircle / numberOfSpoke;

      for (int I = 0; I < numberOfSpoke; I++)
        Angles[I] = (I == 0 ? dblAngle : Angles[I - 1] + dblAngle);

      return Angles;
    }

    public Size GetPreferredSize(Size proposedSize)
    {
      proposedSize.Width = (FOuterCircleRadius + FSpokeThickness) * 2;
      proposedSize.Height = proposedSize.Width;
      return proposedSize;
    }

    public void DrawThrobber(ThrobberRenderEventArgs e)
    {
      e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

      PointF CenterPoint = new PointF(e.Bounds.Left + e.Bounds.Width / 2, e.Bounds.Top + e.Bounds.Height / 2 - 1);

      for (int I = 0; I < FNumberOfSpoke; I++)
      {
        e.Position = e.Position % FNumberOfSpoke;
        DrawLine(e.Graphics,
          GetCoordinate(CenterPoint, FInnerCircleRadius, FSpokeAngles[e.Position]),
          GetCoordinate(CenterPoint, FOuterCircleRadius, FSpokeAngles[e.Position]),
          e.Enabled ? FPalette[I] : FColor, FSpokeThickness);
        e.Position++;
      }
    }
  }
}
