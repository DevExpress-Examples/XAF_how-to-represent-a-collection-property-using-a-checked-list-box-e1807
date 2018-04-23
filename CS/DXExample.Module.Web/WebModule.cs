using System;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace DXExample.Module.Web
{
    [ToolboxItemFilter("Xaf.Platform.Web")]
    public sealed partial class DXExampleAspNetModule : ModuleBase
    {
        public DXExampleAspNetModule()
        {
            InitializeComponent();
        }
    }
}
