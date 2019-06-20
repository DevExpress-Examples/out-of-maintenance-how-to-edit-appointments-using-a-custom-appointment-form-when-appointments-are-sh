<h2>Index</h2>

<script type="text/javascript">
    function OnAppointmentFormSave(s, e) {
        if (IsValidAppointment())
            scheduler.AppointmentFormSave();
    }

    function OnAppointmentFormCancel(s, e) {
        scheduler.AppointmentFormCancel();
    }

    function OnAppointmentFormDelete(s, e) {
        scheduler.AppointmentFormDelete();
    }

    function IsValidAppointment() {
        $.validator.unobtrusive.parse('form');
        return $("form").valid();
    }

    function OnTimeBeforeStartComboBoxInit(s, e) {
        s.SetEnabled(HasReminder.GetChecked());
    }

    function OnHasReminderCheckedChanged(s, e) {
        var reminderCB = ASPxClientComboBox.Cast("TimeBeforeStart");
        reminderCB.SetEnabled(HasReminder.GetChecked());
    }

    var textSeparator = ", ";
    function OnListBoxSelectionChanged(listBox, args) {
        UpdateText();
    }

    function UpdateText() {
        var selectedItems = OwnerId.GetSelectedItems();
        ddOwnerId.SetText(GetSelectedItemsText(selectedItems));
    }

    function SynchronizeListBoxValues(dropDown, args) {
        OwnerId.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = GetValuesByTexts(texts);
        OwnerId.SelectValues(values);
        UpdateText(); // for remove non-existing texts
    }

    function GetSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            texts.push(items[i].text);

        return texts.join(textSeparator);
    }

    function GetValuesByTexts(texts) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = OwnerId.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
</script>

@ModelType DevExpressMvcApplication1.Models.SchedulerDataObject
@Html.Partial("SchedulerPartial", Model)

