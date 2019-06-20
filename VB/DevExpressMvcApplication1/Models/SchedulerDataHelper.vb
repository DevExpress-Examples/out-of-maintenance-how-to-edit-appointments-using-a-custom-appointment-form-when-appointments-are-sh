Imports System.Collections
Imports System.Linq
Imports DevExpressMvcApplication1
Imports DevExpressMvcApplication1.Models
Imports System.Collections.Generic
Imports System.Drawing
Imports System
Imports System.Web
Imports System.Web.Mvc.Html
Imports DevExpress.XtraScheduler.Xml
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraScheduler
Imports DevExpress.Web.ASPxScheduler

Public Module SchedulerDataHelper
    Public Function GetResources() As List(Of CustomResource)
        Dim resources As New List(Of CustomResource)()
        resources.Add(CustomResource.CreateCustomResource(1, "Max Fowler", Color.Yellow.ToArgb()))
        resources.Add(CustomResource.CreateCustomResource(2, "Nancy Drewmore", Color.Green.ToArgb()))
        resources.Add(CustomResource.CreateCustomResource(3, "Pak Jang", Color.LightPink.ToArgb()))
        Return resources
    End Function

    Private myRand As New Random()
    Public Function GetAppointments(ByVal resources As List(Of CustomResource)) As List(Of CustomAppointment)
        Dim appointments As New List(Of CustomAppointment)()
        For Each item As CustomResource In resources
            Dim subjPrefix As String = item.Name & "'s "
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix & "meeting", item.ResID, 2, 5, lastInsertedID))
            lastInsertedID += 1
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix & "travel", item.ResID, 3, 6, lastInsertedID))
            lastInsertedID += 1
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix & "phone call", item.ResID, 0, 10, lastInsertedID))
            lastInsertedID += 1
        Next item
        Return appointments
    End Function


    Public ReadOnly Property DataObject() As SchedulerDataObject
        Get

            Dim dataObject_Renamed As New SchedulerDataObject()


            If HttpContext.Current.Session("ResourcesList") Is Nothing Then
                HttpContext.Current.Session("ResourcesList") = GetResources()
            End If
            dataObject_Renamed.Resources = TryCast(HttpContext.Current.Session("ResourcesList"), List(Of CustomResource))

            If HttpContext.Current.Session("AppointmentsList") Is Nothing Then
                HttpContext.Current.Session("AppointmentsList") = GetAppointments(dataObject_Renamed.Resources)
            End If
            dataObject_Renamed.Appointments = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
            Return dataObject_Renamed
        End Get
    End Property


    Private defaultAppointmentStorage_Renamed As MVCxAppointmentStorage
    Public ReadOnly Property DefaultAppointmentStorage() As MVCxAppointmentStorage
        Get
            If defaultAppointmentStorage_Renamed Is Nothing Then
                defaultAppointmentStorage_Renamed = CreateDefaultAppointmentStorage()
            End If
            Return defaultAppointmentStorage_Renamed

        End Get
    End Property

    Private Function CreateDefaultAppointmentStorage() As MVCxAppointmentStorage
        Dim appointmentStorage As New MVCxAppointmentStorage()
        appointmentStorage.ResourceSharing = True
        appointmentStorage.AutoRetrieveId = True
        appointmentStorage.Mappings.AppointmentId = "ID"
        appointmentStorage.Mappings.Start = "StartTime"
        appointmentStorage.Mappings.End = "EndTime"
        appointmentStorage.Mappings.Subject = "Subject"
        appointmentStorage.Mappings.AllDay = "AllDay"
        appointmentStorage.Mappings.Description = "Description"
        appointmentStorage.Mappings.Label = "Label"
        appointmentStorage.Mappings.Location = "Location"
        appointmentStorage.Mappings.RecurrenceInfo = "RecurrenceInfo"
        appointmentStorage.Mappings.ReminderInfo = "ReminderInfo"
        appointmentStorage.Mappings.ResourceId = "OwnerId"
        appointmentStorage.Mappings.Status = "Status"
        appointmentStorage.Mappings.Type = "EventType"

        appointmentStorage.CustomFieldMappings.Add(New DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("AppointmentCustomField", "CustomInfo"))
        appointmentStorage.CustomFieldMappings.Add(New DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("TimeBeforeStart", "TimeBeforeStart"))
        Return appointmentStorage
    End Function


    Private defaultResourceStorage_Renamed As MVCxResourceStorage
    Public ReadOnly Property DefaultResourceStorage() As MVCxResourceStorage
        Get
            If defaultResourceStorage_Renamed Is Nothing Then
                defaultResourceStorage_Renamed = CreateDefaultResourceStorage()
            End If
            Return defaultResourceStorage_Renamed

        End Get
    End Property

    Private Function CreateDefaultResourceStorage() As MVCxResourceStorage
        Dim resourceStorage As New MVCxResourceStorage()
        resourceStorage.Mappings.ResourceId = "ResID"
        resourceStorage.Mappings.Caption = "Name"
        resourceStorage.Mappings.Color = "Color"
        Return resourceStorage
    End Function

    Public Function GetSchedulerSettings() As SchedulerSettings
        Return GetSchedulerSettings(Nothing)
    End Function

    Private Function CreateAppointmentRecurrenceFormSettings(ByVal container As DevExpress.Web.ASPxScheduler.AppointmentFormTemplateContainer) As AppointmentRecurrenceFormSettings
        Return New AppointmentRecurrenceFormSettings With {
            .Name = "appointmentRecurrenceForm",
            .Width = System.Web.UI.WebControls.Unit.Percentage(100),
            .IsRecurring = container.Appointment.IsRecurring,
            .DayNumber = container.RecurrenceDayNumber,
            .End = container.RecurrenceEnd,
            .Month = container.RecurrenceMonth,
            .OccurrenceCount = container.RecurrenceOccurrenceCount,
            .Periodicity = container.RecurrencePeriodicity,
            .RecurrenceRange = container.RecurrenceRange,
            .Start = container.Start,
            .WeekDays = container.RecurrenceWeekDays,
            .WeekOfMonth = container.RecurrenceWeekOfMonth,
            .RecurrenceType = container.RecurrenceType,
            .IsFormRecreated = container.IsFormRecreated
        }
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function GetSchedulerSettings(ByVal customHtml As System.Web.Mvc.HtmlHelper) As SchedulerSettings
        Dim settings As New SchedulerSettings()
        settings.Name = "scheduler"

        settings.InitClientAppointment = Sub(sched, evargs)
                                             evargs.Properties.Add(DevExpress.Web.ASPxScheduler.ClientSideAppointmentFieldNames.AppointmentType, evargs.Appointment.Type)
                                             evargs.Properties.Add(DevExpress.Web.ASPxScheduler.ClientSideAppointmentFieldNames.Subject, evargs.Appointment.Subject)
                                         End Sub

        settings.CallbackRouteValues = New With {
            Key .Controller = "Home",
            Key .Action = "SchedulerPartial"
        }
        settings.EditAppointmentRouteValues = New With {
            Key .Controller = "Home",
            Key .Action = "EditAppointment"
        }
        settings.CustomActionRouteValues = New With {
            Key .Controller = "Home",
            Key .Action = "CustomCallBackAction"
        }

        settings.Storage.Appointments.Assign(SchedulerDataHelper.DefaultAppointmentStorage)
        settings.Storage.Resources.Assign(SchedulerDataHelper.DefaultResourceStorage)

        settings.Storage.EnableReminders = True
        settings.GroupType = SchedulerGroupType.Resource
        settings.Views.DayView.Styles.ScrollAreaHeight = 400
        settings.Start = Date.Now

        settings.AppointmentFormShowing = Sub(sender, e)
                                              Dim scheduler = TryCast(sender, MVCxScheduler)
                                              If scheduler IsNot Nothing Then
                                                  e.Container = New CustomAppointmentTemplateContainer(scheduler)
                                              End If
                                          End Sub

        settings.OptionsForms.RecurrenceFormName = "appointmentRecurrenceForm"

        settings.OptionsForms.SetAppointmentFormTemplateContent(Sub(c)
                                                                    Dim container = CType(c, CustomAppointmentTemplateContainer)
                                                                    Dim modelAppointment As New ModelAppointment() With {
                                                                        .ID = If(container.Appointment.Id Is Nothing, -1, CInt((container.Appointment.Id))),
                                                                        .Subject = container.Appointment.Subject,
                                                                        .Location = container.Appointment.Location,
                                                                        .StartTime = container.Appointment.Start,
                                                                        .EndTime = container.Appointment.End,
                                                                        .AllDay = container.Appointment.AllDay,
                                                                        .Description = container.Appointment.Description,
                                                                        .EventType = CInt((container.Appointment.Type)),
                                                                        .Status = Convert.ToInt32(container.Appointment.StatusKey),
                                                                        .Label = Convert.ToInt32(container.Appointment.LabelKey),
                                                                        .CustomInfo = container.CustomInfo,
                                                                        .HasReminder = container.Appointment.HasReminder,
                                                                        .Reminder = container.Appointment.Reminder
                                                                    }
                                                                    Dim resourcesItems = TryCast(container.ResourceDataSource, DevExpress.Web.ListEditItemCollection)
                                                                    If resourcesItems IsNot Nothing Then
                                                                        For i As Integer = 0 To container.Appointment.ResourceIds.Count - 1
                                                                            modelAppointment.OwnerId &= resourcesItems.FindByValue(container.Appointment.ResourceIds(i)).Text
                                                                            If i < container.Appointment.ResourceIds.Count - 1 Then
                                                                                modelAppointment.OwnerId &= ", "
                                                                            End If
                                                                        Next i
                                                                    End If
                                                                    customHtml.ViewBag.DeleteButtonEnabled = container.CanDeleteAppointment
                                                                    customHtml.ViewBag.ResourceDataSource = container.ResourceDataSource
                                                                    customHtml.ViewBag.StatusDataSource = container.StatusDataSource
                                                                    customHtml.ViewBag.LabelDataSource = container.LabelDataSource
                                                                    customHtml.ViewBag.AppointmentRecurrenceFormSettings = CreateAppointmentRecurrenceFormSettings(container)
                                                                    customHtml.ViewBag.ReminderDataSource = container.ReminderDataSource
                                                                    customHtml.ViewBag.IsBaseAppointment = container.Appointment.Type = AppointmentType.Normal OrElse container.Appointment.Type = AppointmentType.Pattern
                                                                    customHtml.RenderPartial("CustomAppointmentFormPartial", modelAppointment)
                                                                End Sub)
        Return settings
    End Function

    Private lastInsertedID As Integer = 0

    Public Sub CorrectReminderInfo(ByVal appt As CustomAppointment)
        If appt.ReminderInfo <> "" AndAlso appt.TimeBeforeStart <> "" Then
            Dim reminders As New ReminderInfoCollection()
            ReminderInfoCollectionXmlPersistenceHelper.ObjectFromXml(reminders, appt.ReminderInfo)

            For i As Integer = 0 To reminders.Count - 1
                reminders(i).TimeBeforeStart = TimeSpan.Parse(appt.TimeBeforeStart)
                reminders(i).AlertTime = appt.StartTime.Subtract(reminders(i).TimeBeforeStart)
            Next i
            Dim helper As New ReminderInfoCollectionXmlPersistenceHelper(reminders)
            appt.ReminderInfo = helper.ToXml()
        End If
    End Sub

    ' CRUD operations implementation
    Public Sub InsertAppointments(ByVal appts() As CustomAppointment)
        If appts.Length = 0 Then
            Return
        End If

        Dim appointmnets As List(Of CustomAppointment) = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
        For i As Integer = 0 To appts.Length - 1
            If appts(i) IsNot Nothing Then
                appts(i).ID = lastInsertedID
                lastInsertedID += 1
                CorrectReminderInfo(appts(i))
                appointmnets.Add(appts(i))
            End If
        Next i
    End Sub

    Public Sub UpdateAppointments(ByVal appts() As CustomAppointment)
        If appts.Length = 0 Then
            Return
        End If

        Dim appointmnets As List(Of CustomAppointment) = TryCast(System.Web.HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
        For i As Integer = 0 To appts.Length - 1
            Dim sourceObject As CustomAppointment = appointmnets.First(Function(apt) apt.ID = appts(i).ID)
            appts(i).ID = sourceObject.ID
            appointmnets.Remove(sourceObject)

            CorrectReminderInfo(appts(i))
            appointmnets.Add(appts(i))
        Next i
    End Sub

    Public Sub RemoveAppointments(ByVal appts() As CustomAppointment)
        If appts.Length = 0 Then
            Return
        End If

        Dim appointmnets As List(Of CustomAppointment) = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
        For i As Integer = 0 To appts.Length - 1
            Dim sourceObject As CustomAppointment = appointmnets.First(Function(apt) apt.ID = appts(i).ID)
            appointmnets.Remove(sourceObject)
        Next i
    End Sub
End Module