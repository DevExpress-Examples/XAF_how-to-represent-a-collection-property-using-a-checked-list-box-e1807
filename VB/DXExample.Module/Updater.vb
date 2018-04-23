Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.BaseImpl

Namespace DXExample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim master As Master = ObjectSpace.FindObject(Of Master)(New BinaryOperator("MasterName", "TestMaster"))
			If master Is Nothing Then
				master = ObjectSpace.CreateObject(Of Master)()
				master.MasterName = "TestMaster"
				CreateDetail("1")
				CreateDetail("2")
				master.Details.Add(CreateDetail("3"))
				CreateDetail("4")
				CreateDetail(Nothing)
				master.Save()
			End If
		End Sub
		Private Function CreateDetail(ByVal name As String) As Detail
			Dim detail As Detail = ObjectSpace.CreateObject(Of Detail)()
			detail.DetailName = name
			detail.Save()
			Return detail
		End Function
	End Class
End Namespace
