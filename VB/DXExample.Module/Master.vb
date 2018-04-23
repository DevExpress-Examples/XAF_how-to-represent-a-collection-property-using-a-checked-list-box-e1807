Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

Namespace DXExample.Module
	<DefaultClassOptions> _
	Public Class Master
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _MasterName As String
		Public Property MasterName() As String
			Get
				Return _MasterName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("MasterName", _MasterName, value)
			End Set
		End Property
		<Association("Master-Details")> _
		Public ReadOnly Property Details() As XPCollection(Of Detail)
			Get
				Return GetCollection(Of Detail)("Details")
			End Get
		End Property
	End Class
End Namespace