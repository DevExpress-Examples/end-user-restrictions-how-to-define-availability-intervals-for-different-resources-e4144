using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;

public class CustomMenuViewCallbackCommand : MenuViewCallbackCommand {
    bool currentCommandProhibited;

    public CustomMenuViewCallbackCommand(ASPxScheduler control)
        : base(control) {
    }

    protected override void ParseParameters(string parameters) {
        bool isNewAppointmentCommand = (parameters == "NewAppointment" ||
            parameters == "NewAllDayEvent" ||
            parameters == "NewRecurringAppointment" ||
            parameters == "NewRecurringEvent");

        currentCommandProhibited = isNewAppointmentCommand && !ResourcesAvailabilities.IsIntervalAvailableForResource(
            Control.SelectedResource.Id.ToString(), Control.SelectedInterval);

        base.ParseParameters(parameters);
    }

    protected override void ExecuteCore() {
        if (currentCommandProhibited) {
            Control.CustomJSProperties += new CustomJSPropertiesEventHandler(ASPxScheduler1_CustomJSProperties);
        }
        else {
            base.ExecuteCore();
        }
    }

    void ASPxScheduler1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties.Add("cpWarning", ResourcesAvailabilities.WarningMessage);
    }
}