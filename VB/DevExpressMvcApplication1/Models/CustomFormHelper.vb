Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraScheduler

Namespace DevExpressMvcApplication1.Models
	Public Class ModelAppointment
		Public Sub New()
		End Sub

		Private privateStartTime As DateTime
		<Required> _
		Public Property StartTime() As DateTime
			Get
				Return privateStartTime
			End Get
			Set(ByVal value As DateTime)
				privateStartTime = value
			End Set
		End Property

		Private privateEndTime As DateTime
		<Required> _
		Public Property EndTime() As DateTime
			Get
				Return privateEndTime
			End Get
			Set(ByVal value As DateTime)
				privateEndTime = value
			End Set
		End Property

		Private privateSubject As String
		<Required> _
		Public Property Subject() As String
			Get
				Return privateSubject
			End Get
			Set(ByVal value As String)
				privateSubject = value
			End Set
		End Property

		Private privateStatus As Integer
		Public Property Status() As Integer
			Get
				Return privateStatus
			End Get
			Set(ByVal value As Integer)
				privateStatus = value
			End Set
		End Property
		Private privateDescription As String
		Public Property Description() As String
			Get
				Return privateDescription
			End Get
			Set(ByVal value As String)
				privateDescription = value
			End Set
		End Property
		Private privateLabel As Integer
		Public Property Label() As Integer
			Get
				Return privateLabel
			End Get
			Set(ByVal value As Integer)
				privateLabel = value
			End Set
		End Property
		Private privateLocation As String
		Public Property Location() As String
			Get
				Return privateLocation
			End Get
			Set(ByVal value As String)
				privateLocation = value
			End Set
		End Property
		Private privateAllDay As Boolean
		Public Property AllDay() As Boolean
			Get
				Return privateAllDay
			End Get
			Set(ByVal value As Boolean)
				privateAllDay = value
			End Set
		End Property
		Private privateEventType As Integer
		Public Property EventType() As Integer
			Get
				Return privateEventType
			End Get
			Set(ByVal value As Integer)
				privateEventType = value
			End Set
		End Property
		Private privateRecurrenceInfo As String
		Public Property RecurrenceInfo() As String
			Get
				Return privateRecurrenceInfo
			End Get
			Set(ByVal value As String)
				privateRecurrenceInfo = value
			End Set
		End Property
		Private privateReminderInfo As String
		Public Property ReminderInfo() As String
			Get
				Return privateReminderInfo
			End Get
			Set(ByVal value As String)
				privateReminderInfo = value
			End Set
		End Property

		Private privateOwnerId As String
		<Required, Display(Name := "Resource")> _
		Public Property OwnerId() As String
			Get
				Return privateOwnerId
			End Get
			Set(ByVal value As String)
				privateOwnerId = value
			End Set
		End Property
		Private privateCustomInfo As String
		Public Property CustomInfo() As String
			Get
				Return privateCustomInfo
			End Get
			Set(ByVal value As String)
				privateCustomInfo = value
			End Set
		End Property
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property

		Private privateHasReminder As Boolean
		Public Property HasReminder() As Boolean
			Get
				Return privateHasReminder
			End Get
			Set(ByVal value As Boolean)
				privateHasReminder = value
			End Set
		End Property

		Private privateReminder As Reminder
		Public Property Reminder() As Reminder
			Get
				Return privateReminder
			End Get
			Set(ByVal value As Reminder)
				privateReminder = value
			End Set
		End Property

		Public ReadOnly Property TimeBeforeStart() As TimeSpan
			Get
				Return If(Reminder Is Nothing, TimeSpan.FromMinutes(10), Reminder.TimeBeforeStart)
			End Get
		End Property

		Public Sub New(ByVal source As CustomAppointment)
			If source IsNot Nothing Then
				StartTime = source.StartTime
				EndTime = source.EndTime
				Subject = source.Subject
				Status = source.Status
				Description = source.Description
				Label = source.Label
				Location = source.Location
				AllDay = source.AllDay
				EventType = source.EventType
				RecurrenceInfo = source.RecurrenceInfo
				ReminderInfo = source.ReminderInfo
				OwnerId = source.OwnerId
				CustomInfo = source.CustomInfo
				ID = source.ID
				HasReminder = source.ReminderInfo.Trim() <> ""
			End If
		End Sub
	End Class

	Public Class CustomAppointmentTemplateContainer
		Inherits AppointmentFormTemplateContainer
		Public Sub New(ByVal scheduler As MVCxScheduler)
			MyBase.New(scheduler)
		End Sub

		Public ReadOnly Property CustomInfo() As String
			Get
				Return If(Appointment.CustomFields("AppointmentCustomField") IsNot Nothing, Appointment.CustomFields("AppointmentCustomField").ToString(), "")
			End Get
		End Property

		Public Overrides Sub DataBind()
			MyBase.DataBind()
		End Sub

		Protected Overrides Sub DataBind(ByVal raiseOnDataBinding As Boolean)
			MyBase.DataBind(raiseOnDataBinding)
		End Sub

		Protected Overrides Sub DataBindChildren()
			MyBase.DataBindChildren()
		End Sub

		Protected Overrides Sub LoadPostData(ByVal parent As System.Web.UI.Control)
			MyBase.LoadPostData(parent)
		End Sub

		Protected Overrides Sub LoadViewState(ByVal savedState As Object)
			MyBase.LoadViewState(savedState)
		End Sub

		Protected Overrides Function SaveControlState() As Object
			Return MyBase.SaveControlState()
		End Function

		Protected Overrides Function SaveViewState() As Object
			Return MyBase.SaveViewState()
		End Function
	End Class
End Namespace