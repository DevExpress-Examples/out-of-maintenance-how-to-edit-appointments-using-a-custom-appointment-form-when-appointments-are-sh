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

        <Required>
        Public Property StartTime() As Date

        <Required>
        Public Property EndTime() As Date

        <Required>
        Public Property Subject() As String

        Public Property Status() As Integer
        Public Property Description() As String
        Public Property Label() As Integer
        Public Property Location() As String
        Public Property AllDay() As Boolean
        Public Property EventType() As Integer
        Public Property RecurrenceInfo() As String
        Public Property ReminderInfo() As String

        <Required, Display(Name:="Resource")>
        Public Property OwnerId() As String
        Public Property CustomInfo() As String
        Public Property ID() As Integer

        Public Property HasReminder() As Boolean

        Public Property Reminder() As Reminder

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