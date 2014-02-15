using UnityEngine;

namespace Assets.Plugins
{
    public class ToolTip : PropertyAttribute
    {
        #region Members
     
        public readonly string tooltip;
     
        #endregion
     
        #region Exposed
        public ToolTip(string tooltip)
        {
            this.tooltip = tooltip;
        }
        #endregion
    }
}