Imports Microsoft.VisualBasic
Imports System
Namespace DXExample.Win
	Partial Public Class DXExampleWindowsFormsApplication
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
			Me.module3 = New DXExample.Module.DXExampleModule()
			Me.module4 = New DXExample.Module.Win.DXExampleWindowsFormsModule()
			Me.module5 = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.module6 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
			Me.module7 = New DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' module1
			' 
			Me.module1.AdditionalExportedTypes.Add(GetType(DevExpress.Xpo.XPObjectType))
			' 
			' module5
			' 
			Me.module5.AllowValidationDetailsAccess = True
			' 
			' DXExampleWindowsFormsApplication
			' 
			Me.ApplicationName = "DXExample"
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.module6)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Modules.Add(Me.module5)
			Me.Modules.Add(Me.module7)
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.DXExampleWindowsFormsApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub

		#End Region

		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
		Private module3 As DXExample.Module.DXExampleModule
		Private module4 As DXExample.Module.Win.DXExampleWindowsFormsModule
		Private module5 As DevExpress.ExpressApp.Validation.ValidationModule
		Private module6 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
		Private module7 As DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule
	End Class
End Namespace
