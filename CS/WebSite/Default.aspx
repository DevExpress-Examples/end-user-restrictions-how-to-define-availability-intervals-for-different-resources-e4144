<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.2, Version=15.2.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.2.Core, Version=15.2.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" AppointmentDataSourceID="appointmentDataSource" GroupType="Resource"
            OnAppointmentInserting="ASPxScheduler1_AppointmentInserting" OnHtmlTimeCellPrepared="ASPxScheduler1_HtmlTimeCellPrepared" 
            OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" OnAppointmentChanging="ASPxScheduler1_AppointmentChanging">

                <ClientSideEvents EndCallback="function(s, e) {
                    if (s.cpWarning) {
                        alert(s.cpWarning);
                        delete s.cpWarning;
                    }
                }" />
                
                <Storage>
                    <Appointments>
                        <Mappings 
                            AppointmentId = "Id"
                            Start = "StartTime"
                            End = "EndTime"
                            Subject = "Subject"
                            AllDay = "AllDay"
                            Description = "Description"
                            Label = "Label"
                            Location = "Location"
                            RecurrenceInfo = "RecurrenceInfo"
                            ReminderInfo = "ReminderInfo"
                            ResourceId = "OwnerId"
                            Status = "Status"
                            Type = "EventType"/>
                    </Appointments>
                </Storage>
                <Views>
                    <DayView >
                        <WorkTime End="00:00:00" Start="00:00:00" />
                    </DayView>
                    <WorkWeekView ShowFullWeek="True" >
                        <WorkTime End="00:00:00" Start="00:00:00" />
                    </WorkWeekView>
                </Views>
            </dxwschs:ASPxScheduler>
        <asp:ObjectDataSource ID="appointmentDataSource" runat="server" DataObjectTypeName="CustomEvent" TypeName="CustomEventDataSource" 
            DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler" InsertMethod="InsertMethodHandler" 
            UpdateMethod="UpdateMethodHandler" OnObjectCreated="appointmentsDataSource_ObjectCreated" />
    </form>
</body>
</html>