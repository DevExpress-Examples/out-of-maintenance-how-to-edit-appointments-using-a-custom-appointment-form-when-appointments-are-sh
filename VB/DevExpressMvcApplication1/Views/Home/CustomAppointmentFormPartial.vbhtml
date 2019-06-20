@Imports System.Web.UI.WebControls
@ModelType DevExpressMvcApplication1.Models.ModelAppointment

@Code
    Html.EnableClientValidation()
    Html.EnableUnobtrusiveJavaScript()

    Dim TextBoxSettingsMethod As Action(Of MVCxFormLayoutItem) =
        Sub(i)
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox
            Dim TextBoxSettings = CType(i.NestedExtensionSettings, TextBoxSettings)
            TextBoxSettings.ShowModelErrors = True
            TextBoxSettings.Properties.ValidationSettings.Display = Display.Dynamic
            TextBoxSettings.Width = Unit.Percentage(100)
        End Sub
    Dim MemoSettingMethod As Action(Of MVCxFormLayoutItem) =
        Sub(i)
            i.ColSpan = 2
            i.Height = Unit.Pixel(55)

            i.NestedExtensionType = FormLayoutNestedExtensionItemType.Memo
            Dim MemoSettings = CType(i.NestedExtensionSettings, MemoSettings)
            MemoSettings.Properties.Rows = 2
            MemoSettings.Width = Unit.Percentage(100)
            MemoSettings.Height = Unit.Percentage(100)
        End Sub
    Dim DateTimeSettingsMethod As Action(Of MVCxFormLayoutItem) =
        Sub(i)
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit
            Dim DateEditSettings = CType(i.NestedExtensionSettings, DateEditSettings)
            DateEditSettings.Properties.EditFormat = EditFormat.DateTime
            DateEditSettings.ShowModelErrors = True
            DateEditSettings.Properties.ValidationSettings.Display = Display.Dynamic
            DateEditSettings.Width = Unit.Percentage(100)
        End Sub

    Dim SpinEditSettingsMethod As Action(Of MVCxFormLayoutItem) =
        Sub(i)
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit
            Dim SpinEditSettings = CType(i.NestedExtensionSettings, SpinEditSettings)
            SpinEditSettings.ShowModelErrors = True
            SpinEditSettings.Properties.DisplayFormatString = "{0:C}"
            SpinEditSettings.Properties.ValidationSettings.Display = Display.Dynamic
            SpinEditSettings.Width = Unit.Percentage(100)
        End Sub
End Code

@Using Html.BeginForm()

    @Html.DevExpress().FormLayout(
                    Sub(settings)
                        settings.Name = "Schedule"
                        settings.Width = Unit.Percentage(100)
                        settings.ColCount = 2

                        settings.Items.Add(Function(m) m.Subject,
                           Sub(i)
                               i.Caption = "Appointment name"
                               i.ColSpan = 2
                               TextBoxSettingsMethod(i)
                           End Sub)

                        settings.Items.Add(Function(m) m.Location, TextBoxSettingsMethod)

                        settings.Items.Add(Function(m) m.OwnerId,
                           Sub(item)
                               item.NestedExtensionType = FormLayoutNestedExtensionItemType.DropDownEdit
                               Dim ddeSettings = CType(item.NestedExtensionSettings, DropDownEditSettings)

                               ddeSettings.Name = "ddOwnerId"
                               ddeSettings.Width = 210

                               ddeSettings.Enabled = ViewBag.IsBaseAppointment

                               ddeSettings.SetDropDownWindowTemplateContent(
                                Sub(c)
                                    Html.DevExpress().ListBox(Sub(ListBoxSettings)
                                                                  ListBoxSettings.Name = "OwnerId"

                                                                  ListBoxSettings.ControlStyle.Border.BorderWidth = 0
                                                                  ListBoxSettings.ControlStyle.BorderBottom.BorderWidth = 1
                                                                  ListBoxSettings.Width = Unit.Percentage(100)

                                                                  ListBoxSettings.Properties.SelectionMode = ListEditSelectionMode.CheckColumn
                                                                  ListBoxSettings.Properties.TextField = "Text"
                                                                  ListBoxSettings.Properties.ValueField = "Value"
                                                                  ListBoxSettings.Properties.ValueType = GetType(Int32)

                                                                  ListBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = "OnListBoxSelectionChanged"
                                                              End Sub).BindList(ViewBag.ResourceDataSource).Render()
                                    ViewContext.Writer.Write("<div style='padding: 6px; height: 24px;'>")
                                    Html.DevExpress().Button(Sub(ButtonSettings)
                                                                 ButtonSettings.Name = "buttonClose"
                                                                 ButtonSettings.Text = "Close"
                                                                 ButtonSettings.Style.Add("float", "right")
                                                                 ButtonSettings.Style.Add("padding", "0px 2px")
                                                                 ButtonSettings.ClientSideEvents.Click = "function(s, e){ ddOwnerId.HideDropDown(); }"
                                                                 ButtonSettings.Height = 26
                                                             End Sub).Render()
                                    ViewContext.Writer.Write("</div>")
                                End Sub)
                               ddeSettings.Properties.AnimationType = AnimationType.None
                               ddeSettings.Properties.ClientSideEvents.TextChanged = "SynchronizeListBoxValues"
                               ddeSettings.Properties.ClientSideEvents.DropDown = "SynchronizeListBoxValues"
                               ddeSettings.Properties.ClientSideEvents.Init = "SynchronizeListBoxValues"
                           End Sub)

                        settings.Items.Add(Function(m) m.StartTime, DateTimeSettingsMethod)
                        settings.Items.Add(Function(m) m.EndTime, DateTimeSettingsMethod)

                        settings.Items.Add(Function(m) m.HasReminder,
                                           Sub(item)
                                               item.NestedExtensionType = FormLayoutNestedExtensionItemType.CheckBox
                                               Dim cbSettings = CType(item.NestedExtensionSettings, CheckBoxSettings)
                                               cbSettings.Properties.ClientSideEvents.CheckedChanged = "OnHasReminderCheckedChanged"
                                           End Sub)

                        settings.Items.Add(Function(m) m.TimeBeforeStart,
                                           Sub(item)
                                               item.SetNestedContent(
                                                Sub()
                                                    Html.DevExpress().ComboBoxFor(Function(m) m.TimeBeforeStart,
                                                                                  Sub(cbSettings)
                                                                                      cbSettings.Width = Unit.Percentage(100)
                                                                                      cbSettings.Properties.ValueType = GetType(TimeSpan)
                                                                                      cbSettings.Properties.ValueField = "Value"
                                                                                      cbSettings.Properties.TextField = "Text"
                                                                                      cbSettings.Properties.ClientSideEvents.Init = "OnTimeBeforeStartComboBoxInit"
                                                                                  End Sub).BindList(ViewBag.ReminderDataSource).Render()
                                                End Sub)
                                           End Sub)

                        settings.Items.Add(Function(m) m.Description, MemoSettingMethod)

                        settings.Items.Add(Sub(i)
                                               i.ColSpan = 2
                                               i.ShowCaption = DefaultBoolean.False
                                               i.ClientVisible = ViewBag.IsBaseAppointment
                                           End Sub).SetNestedContent(
                                            Sub()
                                                Html.DevExpress().AppointmentRecurrenceForm(ViewBag.AppointmentRecurrenceFormSettings).Render()
                                            End Sub)

                        settings.Items.Add(Sub(i)
                                               i.ColSpan = 2
                                               i.ShowCaption = DefaultBoolean.False
                                               i.HorizontalAlign = FormLayoutHorizontalAlign.Center
                                               i.ParentContainerStyle.Paddings.PaddingTop = Unit.Pixel(7)
                                           End Sub).SetNestedContent(
                                            Sub()
                                                ViewContext.Writer.Write("<div style='width: 400px; text-align:center'>")
                                                Html.DevExpress().Button(Sub(btnSettings)
                                                                             btnSettings.Name = "Apply"
                                                                             btnSettings.Text = "Ok"
                                                                             btnSettings.Width = Unit.Pixel(91)
                                                                             btnSettings.ClientSideEvents.Click = "OnAppointmentFormSave"
                                                                         End Sub).Render()
                                                Html.DevExpress().Button(Sub(btnSettings)
                                                                             btnSettings.Name = "Cancel"
                                                                             btnSettings.Text = "Cancel"
                                                                             btnSettings.Width = Unit.Pixel(91)
                                                                             btnSettings.Style(HtmlTextWriterStyle.Margin) = "0 10px"
                                                                             btnSettings.ClientSideEvents.Click = "OnAppointmentFormCancel"
                                                                         End Sub).Render()
                                                Html.DevExpress().Button(Sub(btnSettings)
                                                                             btnSettings.Name = "Delete"
                                                                             btnSettings.Text = "Delete"
                                                                             btnSettings.Width = Unit.Pixel(91)
                                                                             settings.Enabled = ViewBag.DeleteButtonEnabled
                                                                             btnSettings.ClientSideEvents.Click = "OnAppointmentFormDelete"
                                                                         End Sub).Render()
                                                ViewContext.Writer.Write("</div>")
                                            End Sub)

                        settings.Items.Add(
                            Sub(i)
                                i.ColSpan = 2
                                i.ShowCaption = DefaultBoolean.False
                            End Sub).SetNestedContent(
                            Sub()
                                Html.DevExpress().SchedulerStatusInfo(
                                 Sub(siSettings)
                                     siSettings.Name = "schedulerStatusInfo"
                                     siSettings.Priority = 1
                                     siSettings.SchedulerName = "scheduler"
                                 End Sub).Render()
                            End Sub)
                    End Sub).GetHtml()
End Using