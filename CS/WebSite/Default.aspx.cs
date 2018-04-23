using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxScheduler;
using System.Windows.Forms;

public partial class Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        ResourceFiller.FillResources(ASPxScheduler1.Storage, 3);
        ASPxScheduler1.DataBind();

        if (!IsPostBack)
            ASPxScheduler1.Start = new DateTime(2012, 1, 1);
    }

    protected void ASPxScheduler1_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e) {
        if (!ResourcesAvailabilities.IsIntervalAvailableForResource(e.Resource.Id.ToString(), e.Interval)) {
            e.Cell.BackColor = ControlPaint.Dark(e.Cell.BackColor);
            //e.Cell.Style.Add("color", System.Drawing.ColorTranslator.ToHtml(ColorHelper.InvertColor(e.Cell.BackColor)));
            e.Cell.Style.Add("text-align", "center");
            e.Cell.Controls.Add(new LiteralControl("N/A"));
        }
    }

    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {
        if (e.CommandId == SchedulerCallbackCommandId.MenuView)
            e.Command = new CustomMenuViewCallbackCommand((ASPxScheduler)sender);
    }

    protected void ASPxScheduler1_AppointmentChanging(object sender, PersistentObjectCancelEventArgs e) {
        Appointment apt = (Appointment)e.Object;

        bool currentCommandProhibited = !ResourcesAvailabilities.IsIntervalAvailableForResource(
            apt.ResourceId.ToString(), new TimeInterval(apt.Start, apt.End));

        if (currentCommandProhibited) {
            ASPxScheduler1.CustomJSProperties += new CustomJSPropertiesEventHandler(ASPxScheduler1_CustomJSProperties);
        }

        e.Cancel = currentCommandProhibited;
    }

    void ASPxScheduler1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties.Add("cpWarning", ResourcesAvailabilities.WarningMessage);
    }

    protected void appointmentsDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
        e.ObjectInstance = new CustomEventDataSource(GetCustomEvents());
    }

    private CustomEventList GetCustomEvents() {
        CustomEventList events = Session["ListBoundModeObjects"] as CustomEventList;
        if (events == null) {
            events = new CustomEventList();
            Session["ListBoundModeObjects"] = events;
        }
        return events;
    }

    protected void ASPxScheduler1_AppointmentInserting(object sender, PersistentObjectCancelEventArgs e) {
        SetAppointmentId(sender, e);
    }

    private void SetAppointmentId(object sender, PersistentObjectCancelEventArgs e) {
        ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
        Appointment apt = (Appointment)e.Object;
        storage.SetAppointmentId(apt, apt.GetHashCode());
    }
}