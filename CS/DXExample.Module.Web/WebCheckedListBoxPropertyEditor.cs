using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Xpo;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Web;
using DevExpress.Web.ASPxEditors;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.NodeWrappers;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace DXExample.Module.Web {
    [PropertyEditor(typeof(XPBaseCollection), false)]
    public class WebCheckedListBoxPropertyEditor : ASPxPropertyEditor, IComplexPropertyEditor {
        public WebCheckedListBoxPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        private TableEx table;
        XafApplication application;
        protected override WebControl CreateEditModeControlCore() {
            return CreateTable();
        }
        protected override WebControl CreateViewModeControlCore() {
            return CreateTable();
        }
        private Table CreateTable() {
            table = RenderHelper.CreateTable();
            table.ID = "MainTable";
            table.CssClass = "GroupContent";
            return table;
        }
        protected override void SetupControl(WebControl control) {
            base.SetupControl(control);
            if (ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.View) {
                control.Enabled = false;
            }
        }
        LightDictionary<ASPxCheckBox, object> items;
        XPBaseCollection checkedItems;
        protected override void ReadEditModeValueCore() {
            base.ReadEditModeValueCore();
            GenerateContent();
        }
        protected override void ReadViewModeValueCore() {
            base.ReadViewModeValueCore();
            GenerateContent();
        }
        protected override void SetImmediatePostDataScript(string script) { }
        protected override void SetImmediatePostDataCompanionScript(string script) { }
        private void GenerateContent() {
            table.Rows.Clear();
            items = new LightDictionary<ASPxCheckBox, object>();
            if (PropertyValue is XPBaseCollection) {
                checkedItems = (XPBaseCollection)PropertyValue;
                XPCollection dataSource = new XPCollection(checkedItems.Session, MemberInfo.ListElementType);
                IMemberInfo defaultMember = null;
                ITypeInfo typeInfo = MemberInfo.ListElementTypeInfo;
                IModelClass classInfo = application.Model.BOModel.GetClass(typeInfo.Type);
                if (!String.IsNullOrEmpty(classInfo.DefaultProperty)) {
                    defaultMember = typeInfo.FindMember(classInfo.DefaultProperty);
                }
                if (checkedItems.Sorting.Count > 0) {
                    dataSource.Sorting = checkedItems.Sorting;
                } else if (!String.IsNullOrEmpty(classInfo.DefaultProperty)) {
                    dataSource.Sorting.Add(new SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending));
                }
                foreach (object obj in dataSource) {
                    CreateRow(obj, defaultMember);
                }
                foreach (object obj in checkedItems) {
                    CheckItem(obj);
                }
            }
        }
        private void CreateRow(object obj, IMemberInfo defaultMember) {
            TableRow row = new TableRow();
            row.ID = GetId("Row", obj);
            table.Rows.Add(row);
            TableCell valueCell = new TableCell();
            valueCell.CssClass = "Caption";
            valueCell.Width = Unit.Percentage(95);
            Literal value = new Literal();
            string displayText = String.Empty;
            if (defaultMember != null && !typeof(PersistentBase).IsAssignableFrom(defaultMember.MemberType)) {
                displayText = Convert.ToString(defaultMember.GetValue(obj));
            } else {
                displayText = obj.ToString();
            }
            value.Text = displayText;
            valueCell.Controls.Add(value);
            TableCell checkBoxCell = new TableCell();
            checkBoxCell.ID = GetId("CheckBoxCell", obj);
            ASPxCheckBox checkBox = RenderHelper.CreateASPxCheckBox();
            checkBox.ID = GetId("CheckBox", obj);
            checkBox.CheckedChanged += checkBox_CheckedChanged;
            items.Add(checkBox, obj);
            checkBoxCell.Controls.Add(checkBox);
            row.Cells.Add(checkBoxCell);
            row.Cells.Add(valueCell);
        }
        private void CheckItem(object obj) {
            ASPxCheckBox checkBox = items.FindKeyByValue(obj);
            checkBox.CheckedChanged -= checkBox_CheckedChanged;
            checkBox.Checked = true;
            checkBox.CheckedChanged += checkBox_CheckedChanged;
        }

        void checkBox_CheckedChanged(object sender, EventArgs e) {
            ASPxCheckBox editor = (ASPxCheckBox)sender;
            object obj = items[editor];
            if (editor.Checked) {
                checkedItems.BaseAdd(obj);
            } else {
                checkedItems.BaseRemove(obj);
            }
            OnControlValueChanged();
        }
        private string GetId(string elementName, object obj) {
            return elementName + MemberInfo.ListElementTypeInfo.KeyMember.GetValue(obj).ToString();
        }
        #region IComplexPropertyEditor Members

        public void Setup(IObjectSpace objectSpace, XafApplication application) {
            this.application = application;
        }

        #endregion
    }
}
