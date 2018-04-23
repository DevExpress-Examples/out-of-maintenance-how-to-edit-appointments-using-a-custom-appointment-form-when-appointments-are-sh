using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.Mvc;
using DevExpress.XtraScheduler;

namespace DevExpressMvcApplication1.Models {
    public class ModelAppointment {
        public ModelAppointment() { }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Subject { get; set; }

        public int Status { get; set; }
        public string Description { get; set; }
        public int Label { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public int EventType { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }

        [Required]
        [Display(Name = "Resource")]
        public string OwnerId { get; set; }
        public string CustomInfo { get; set; }
        public int ID { get; set; }

        public bool HasReminder { get; set; }

        public Reminder Reminder { get; set; }

        public TimeSpan TimeBeforeStart {
            get { return Reminder == null ? TimeSpan.FromMinutes(10) : Reminder.TimeBeforeStart; }
        }

        public ModelAppointment(CustomAppointment source) {
            if(source != null) {
                StartTime = source.StartTime;
                EndTime = source.EndTime;
                Subject = source.Subject;
                Status = source.Status;
                Description = source.Description;
                Label = source.Label;
                Location = source.Location;
                AllDay = source.AllDay;
                EventType = source.EventType;
                RecurrenceInfo = source.RecurrenceInfo;
                ReminderInfo = source.ReminderInfo;
                OwnerId = source.OwnerId;
                CustomInfo = source.CustomInfo;
                ID = source.ID;
                HasReminder = source.ReminderInfo.Trim() != "";
            }
        }
    }

    public class CustomAppointmentTemplateContainer : AppointmentFormTemplateContainer {
        public CustomAppointmentTemplateContainer(MVCxScheduler scheduler) : base(scheduler) { }

        public string CustomInfo {
            get {
                return Appointment.CustomFields["AppointmentCustomField"] != null ? Appointment.CustomFields["AppointmentCustomField"].ToString() : "";
            }
        }

        public override void DataBind() {
            base.DataBind();
        }

        protected override void DataBind(bool raiseOnDataBinding) {
            base.DataBind(raiseOnDataBinding);
        }

        protected override void DataBindChildren() {
            base.DataBindChildren();
        }

        protected override void LoadPostData(System.Web.UI.Control parent) {
            base.LoadPostData(parent);
        }

        protected override void LoadViewState(object savedState) {
            base.LoadViewState(savedState);
        }

        protected override object SaveControlState() {
            return base.SaveControlState();
        }

        protected override object SaveViewState() {
            return base.SaveViewState();
        }
    }
}