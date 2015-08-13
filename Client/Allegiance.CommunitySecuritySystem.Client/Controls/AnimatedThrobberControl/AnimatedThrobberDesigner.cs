using System.Windows.Forms;
using System.Windows.Forms.Design;

// From a sample at: http://dev.nomad-net.info/articles/animatedthrobber

namespace Allegiance.CommunitySecuritySystem.Client.Controls.AnimatedThrobberControl
{
  public class AnimatedThrobberDesigner : ControlDesigner
  {
    public override SelectionRules SelectionRules
    {
      get { return base.SelectionRules & ~(((Control)Component).AutoSize ? SelectionRules.AllSizeable : 0); }
    }
  }
}
