<!-- default file list -->
*Files to look at*:

* [CustomEvents.cs](./CS/WebSite/App_Code/CustomEvents.cs) (VB: [CustomEvents.vb](./VB/WebSite/App_Code/CustomEvents.vb))
* [CustomMenuViewCallbackCommand.cs](./CS/WebSite/App_Code/CustomMenuViewCallbackCommand.cs) (VB: [CustomMenuViewCallbackCommand.vb](./VB/WebSite/App_Code/CustomMenuViewCallbackCommand.vb))
* [Helpers.cs](./CS/WebSite/App_Code/Helpers.cs) (VB: [Helpers.vb](./VB/WebSite/App_Code/Helpers.vb))
* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
<!-- default file list end -->
# End-User Restrictions - How to define "availability" intervals for different resources


<p>This example illustrates how to define different time intervals that are available for scheduling. This information is stored in the <strong>~\App_Data\ResourcesAvailabilities.mdb</strong> database. It has the <strong>ResourcesAvailabilities </strong>table with the following schema:</p><p><strong>Id: Integer, <br />
ResourceId: String, <br />
StartTime: DateTime, <br />
EndTime: DateTime</strong></p><p>So, generally, each resource can have independent availability intervals. This information is loaded and exposed via the <strong>ResourcesAvailabilities </strong>class.</p><p>First of all, we use this information to modify the default appearance of the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument3835"><u>Time Cells</u></a> by handling the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_HtmlTimeCellPreparedtopic"><u>ASPxScheduler.HtmlTimeCellPrepared Event</u></a>. Then, we disallow appointment creation and modification actions if their execution is initiated outside available intervals. For this, we substitute the regular <strong>MenuViewCallbackCommand </strong>with a custom one (see <a href="http://documentation.devexpress.com/#AspNet/CustomDocument5462"><u>Callback Commands</u></a>), and handle the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_AppointmentChangingtopic"><u>ASPxScheduler.AppointmentChanging Event</u></a>. Note that we use a technique from the <a href="https://www.devexpress.com/Support/Center/p/Q353824">How to show user friendly message from the server on a callback</a> ticket to show a warning if the end-user tries to schedule an appointment in the restricted area. In other words, we handle the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_CustomJSPropertiestopic"><u>ASPxScheduler.CustomJSProperties Event</u></a> on the server side to pass a warning message on the client side. This message is intercepted in the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerScriptsASPxClientScheduler_EndCallbacktopic"><u>ASPxClientScheduler.EndCallback Event</u></a>.</p><p>Here is a screenshot that illustrates a sample application in action:</p><p><img src="https://raw.githubusercontent.com/DevExpress-Examples/end-user-restrictions-how-to-define-availability-intervals-for-different-resources-e4144/13.1.8+/media/54f13999-9b8b-42d6-a021-19590a8a2980.png"></p><p><strong>See Also:</strong><br />
<a href="https://www.devexpress.com/Support/Center/p/E3499">End-User Restrictions - How to allow appointment creation or deletion only for specific users</a><br />
<a href="https://www.devexpress.com/Support/Center/p/E3790">End-User Restrictions - How to allow appointment modification or deletion depending on custom field values</a><br />
<a href="https://www.devexpress.com/Support/Center/p/E3999">End-User Restrictions - How to implement a client-side confirmation on deleting an appointment</a></p>

<br/>


