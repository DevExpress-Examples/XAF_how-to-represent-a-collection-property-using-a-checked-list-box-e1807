Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.ExpressApp
Imports DevExpress.XtraEditors
Imports DevExpress.Xpo
Imports System.Windows.Forms
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Model

Namespace DXExample.Module.Win
	Public Class WinCheckedListBoxPropertyEditor
		Inherits WinPropertyEditor
		Implements IComplexPropertyEditor
		Public Sub New(ByVal objectType As Type, ByVal model As IModelMemberViewItem)
			MyBase.New(objectType, model)
		End Sub
		Protected Overrides Function CreateControlCore() As Object
			Return New CheckedListBoxControl()
		End Function
		Private checkedItems As XPBaseCollection
		Private application As XafApplication
		Protected Overrides Sub ReadValueCore()
			MyBase.ReadValueCore()
			If TypeOf PropertyValue Is XPBaseCollection Then
				Control.Items.Clear()
				checkedItems = CType(PropertyValue, XPBaseCollection)
				Dim dataSource As New XPCollection(checkedItems.Session, MemberInfo.ListElementType)
				Dim classInfo As IModelClass = application.Model.BOModel.GetClass(MemberInfo.ListElementTypeInfo.Type)
				If checkedItems.Sorting.Count > 0 Then
					dataSource.Sorting = checkedItems.Sorting
				ElseIf (Not String.IsNullOrEmpty(classInfo.DefaultProperty)) Then
					dataSource.Sorting.Add(New SortProperty(classInfo.DefaultProperty, DevExpress.Xpo.DB.SortingDirection.Ascending))
				End If
				Control.DataSource = dataSource
				Control.DisplayMember = classInfo.DefaultProperty
				If (Not dataSource.DisplayableProperties.Contains(classInfo.DefaultProperty)) Then
					dataSource.DisplayableProperties &= ";" & classInfo.DefaultProperty
				End If
				For Each obj As Object In checkedItems
					Control.SetItemChecked(dataSource.IndexOf(obj), True)
				Next obj
				AddHandler Control.ItemCheck, AddressOf control_ItemCheck
			End If
		End Sub
		Private Sub control_ItemCheck(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs)
			Dim obj As Object = Control.GetItemValue(e.Index)
			Select Case e.State
				Case CheckState.Checked
					checkedItems.BaseAdd(obj)
				Case CheckState.Unchecked
					checkedItems.BaseRemove(obj)
			End Select
		End Sub
		Public Shadows ReadOnly Property Control() As CheckedListBoxControl
			Get
				Return CType(MyBase.Control, CheckedListBoxControl)
			End Get
		End Property

		#Region "IComplexPropertyEditor Members"

		Public Sub Setup(ByVal objectSpace As IObjectSpace, ByVal application As XafApplication) Implements IComplexPropertyEditor.Setup
			Me.application = application
		End Sub

		#End Region
	End Class
End Namespace
