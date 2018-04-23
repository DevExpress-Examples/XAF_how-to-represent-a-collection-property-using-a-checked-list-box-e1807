using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace DXExample.Module.Win
{
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class DXExampleWindowsFormsModule : ModuleBase
    {
        public DXExampleWindowsFormsModule()
        {
            InitializeComponent();
        }
    }
}
