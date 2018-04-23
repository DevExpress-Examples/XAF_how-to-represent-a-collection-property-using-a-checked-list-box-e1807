Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.ExpressApp.Web.Editors.ASPx
Imports DevExpress.Xpo
Imports System.Web.UI.WebControls
Imports DevExpress.ExpressApp.Web
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.Model.NodeWrappers
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Model

Namespace DXExample.Module.Web
	Public Class WebCheckedListBoxPropertyEditor
		Inherits ASPxPropertyEditor
		Implements IComplexPropertyEditor
		Public Sub New(ByVal objectType As Type, ByVal model As IModelMemberViewItem)
			MyBase.New(objectType, model)
		End Sub
		Private table As TableEx
		Private application As XafApplication
		Protected Overrides Function CreateEditModeControlCore() As WebControl
			Return CreateTable()
		End Function
		Protected Overrides Sub SetImmediatePostDataScript(ByVal script As String)
		End Sub
		Protected Overrides Sub SetImmediatePostDataCompanionScript(ByVal script As String)
		End Sub
		Protected Overrides Function CreateViewModeControlCore() As WebControl
			Return CreateTable()
		End Function
		Private Function CreateTable() As Table
			table = RenderHelper.CreateTable()
			table.ID = "MainTable"
			table.CssClass = "GroupContent"
			Return table
		End Function
		Protected Overrides Sub SetupControl(ByVal control As WebControl)
			MyBase.SetupControl(control)
			If ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View Then
				control.Enabled = False
			End If
		End Sub
		Private items As LightDictionary(Of ASPxCheckBox, Object)
		Private checkedItems As XPBaseCollection
		Protected Overrides Sub ReadEditModeValueCore()
			MyBase.ReadEditModeValueCore()
			GenerateContent()
		End Sub
		Protected Overrides Sub ReadViewModeValueCore()
			MyBase.ReadViewModeValueCore()
			GenerateContent()
		End Sub
		Private Sub GenerateContent()
			table.Rows.Clear()
			items = New LightDictionary(Of ASPxCheckBox, Object)()
			If TypeOf PropertyValue Is XPBaseCollection Then
				checkedItems = CType(PropertyValue, XPBaseCollection)
				Dim dataSource As New XPCollection(checkedItems.Session, MemberInfo.ListElementType)
				Dim defaultMember As IMemberInfo = Nothing
				Dim typeInfo As ITypeInfo = MemberInfo.ListElementTypeInfo
				Dim classInfo As IModelClass = application.Model.BOModel.GetClass(typeInfo.Type)
				If (Not String.IsNullOrEmpty(classInfo.DefaultProperty)) Then
					defaultMember = typeInfo.FindMember(classInfo.DefaultProperty)
				End If
				If checkedItems.Sorting.Count > 0 Then
					dataSource.Sorting = checkedItems.Sorting
				ElseIf (Not String.IsNullOrEmpty(classInfo.DefaultProperty)) Then
					dataSource.Sorting.Add(New SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending))
				End If
				For Each obj As Object In dataSource
					CreateRow(obj, defaultMember)
				Next obj
				For Each obj As Object In checkedItems
					CheckItem(obj)
				Next obj
			End If
		End Sub
		Private Sub CreateRow(ByVal obj As Object, ByVal defaultMember As IMemberInfo)
			Dim row As New TableRow()
			row.ID = GetId("Row", obj)
			table.Rows.Add(row)
			Dim valueCell As New TableCell()
			valueCell.CssClass = "Caption"
			valueCell.Width = Unit.Percentage(95)
			Dim value As New Literal()
			Dim displayText As String = String.Empty
			If defaultMember IsNot Nothing AndAlso (Not GetType(PersistentBase).IsAssignableFrom(defaultMember.MemberType)) Then
				displayText = Convert.ToString(defaultMember.GetValue(obj))
			Else
				displayText = obj.ToString()
			End If
			value.Text = displayText
			valueCell.Controls.Add(value)
			Dim checkBoxCell As New TableCell()
			checkBoxCell.ID = GetId("CheckBoxCell", obj)
			Dim checkBox As ASPxCheckBox = RenderHelper.CreateASPxCheckBox()
			checkBox.ID = GetId("CheckBox", obj)
			AddHandler checkBox.CheckedChanged, AddressOf checkBox_CheckedChanged
			items.Add(checkBox, obj)
			checkBoxCell.Controls.Add(checkBox)
			row.Cells.Add(checkBoxCell)
			row.Cells.Add(valueCell)
		End Sub
		Private Sub CheckItem(ByVal obj As Object)
			Dim checkBox As ASPxCheckBox = items.FindKeyByValue(obj)
			RemoveHandler checkBox.CheckedChanged, AddressOf checkBox_CheckedChanged
			checkBox.Checked = True
			AddHandler checkBox.CheckedChanged, AddressOf checkBox_CheckedChanged
		End Sub

		Private Sub checkBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim editor As ASPxCheckBox = CType(sender, ASPxCheckBox)
			Dim obj As Object = items(editor)
			If editor.Checked Then
				checkedItems.BaseAdd(obj)
			Else
				checkedItems.BaseRemove(obj)
			End If
			OnControlValueChanged()
		End Sub
		Private Function GetId(ByVal elementName As String, ByVal obj As Object) As String
			Return elementName & MemberInfo.ListElementTypeInfo.KeyMember.GetValue(obj).ToString()
		End Function
		#Region "IComplexPropertyEditor Members"

		Public Sub Setup(ByVal objectSpace As IObjectSpace, ByVal application As XafApplication) Implements IComplexPropertyEditor.Setup
			Me.application = application
		End Sub

		#End Region
	End Class
End Namespace
