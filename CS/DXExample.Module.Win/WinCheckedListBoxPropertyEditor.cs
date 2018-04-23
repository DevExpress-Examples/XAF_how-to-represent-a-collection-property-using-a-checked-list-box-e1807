using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Xpo;
using System.Windows.Forms;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.NodeWrappers;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace DXExample.Module.Win {
    [PropertyEditor(typeof(XPBaseCollection), false)]
    public class WinCheckedListBoxPropertyEditor : WinPropertyEditor, IComplexPropertyEditor {
        public WinCheckedListBoxPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override object CreateControlCore() {
            return new CheckedListBoxControl();
        }
        XPBaseCollection checkedItems;
        XafApplication application;
        protected override void ReadValueCore() {
            base.ReadValueCore();
            if (PropertyValue is XPBaseCollection) {
                Control.Items.Clear();
                checkedItems = (XPBaseCollection)PropertyValue;
                XPCollection dataSource = new XPCollection(checkedItems.Session, MemberInfo.ListElementType);
                IModelClass classInfo = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type);
                if (checkedItems.Sorting.Count > 0) {
                    dataSource.Sorting = checkedItems.Sorting;
                } else if (!String.IsNullOrEmpty(classInfo.DefaultProperty)) {
                    dataSource.Sorting.Add(new SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending));
                }
                Control.DataSource = dataSource;
                Control.DisplayMember = classInfo.DefaultProperty;
                foreach (object obj in checkedItems) {
                    Control.SetItemChecked(dataSource.IndexOf(obj), true);
                }
                Control.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(control_ItemCheck);
            }
        }
        void control_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e) {
            object obj = Control.GetItemValue(e.Index);
            switch (e.State) {
                case CheckState.Checked:
                    checkedItems.BaseAdd(obj);
                    break;
                case CheckState.Unchecked:
                    checkedItems.BaseRemove(obj);
                    break;
            }
        }
        public new CheckedListBoxControl Control {
            get {
                return (CheckedListBoxControl)base.Control;
            }
        }

        #region IComplexPropertyEditor Members

        public void Setup(IObjectSpace objectSpace, XafApplication application) {
            this.application = application;
        }

        #endregion
    }
}
