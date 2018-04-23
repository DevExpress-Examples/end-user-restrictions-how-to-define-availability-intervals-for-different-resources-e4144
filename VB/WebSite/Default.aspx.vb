Imports System
Imports System.Data
Imports System.Collections
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.XtraScheduler
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports System.Windows.Forms

Partial Public Class [Default]
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        ResourceFiller.FillResources(ASPxScheduler1.Storage, 3)
        ASPxScheduler1.DataBind()

        If Not IsPostBack Then
            ASPxScheduler1.Start = New Date(2012, 1, 1)
        End If
    End Sub

    Protected Sub ASPxScheduler1_HtmlTimeCellPrepared(ByVal handler As Object, ByVal e As ASPxSchedulerTimeCellPreparedEventArgs)
        If Not ResourcesAvailabilities.IsIntervalAvailableForResource(e.Resource.Id.ToString(), e.Interval) Then
            e.Cell.BackColor = ControlPaint.Dark(e.Cell.BackColor)
            'e.Cell.Style.Add("color", System.Drawing.ColorTranslator.ToHtml(ColorHelper.InvertColor(e.Cell.BackColor)));
            e.Cell.Style.Add("text-align", "center")
            e.Cell.Controls.Add(New LiteralControl("N/A"))
        End If
    End Sub

    Protected Sub ASPxScheduler1_BeforeExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)
        If e.CommandId = SchedulerCallbackCommandId.MenuView Then
            e.Command = New CustomMenuViewCallbackCommand(DirectCast(sender, ASPxScheduler))
        End If
    End Sub

    Protected Sub ASPxScheduler1_AppointmentChanging(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
        Dim apt As Appointment = CType(e.Object, Appointment)

        Dim currentCommandProhibited As Boolean = Not ResourcesAvailabilities.IsIntervalAvailableForResource(apt.ResourceId.ToString(), New TimeInterval(apt.Start, apt.End))

        If currentCommandProhibited Then
            AddHandler ASPxScheduler1.CustomJSProperties, AddressOf ASPxScheduler1_CustomJSProperties
        End If

        e.Cancel = currentCommandProhibited
    End Sub

    Private Sub ASPxScheduler1_CustomJSProperties(ByVal sender As Object, ByVal e As CustomJSPropertiesEventArgs)
        e.Properties.Add("cpWarning", ResourcesAvailabilities.WarningMessage)
    End Sub

    Protected Sub appointmentsDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
        e.ObjectInstance = New CustomEventDataSource(GetCustomEvents())
    End Sub

    Private Function GetCustomEvents() As CustomEventList

        Dim events_Renamed As CustomEventList = TryCast(Session("ListBoundModeObjects"), CustomEventList)
        If events_Renamed Is Nothing Then
            events_Renamed = New CustomEventList()
            Session("ListBoundModeObjects") = events_Renamed
        End If
        Return events_Renamed
    End Function

    Protected Sub ASPxScheduler1_AppointmentInserting(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
        SetAppointmentId(sender, e)
    End Sub

    Private Sub SetAppointmentId(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
        Dim storage As ASPxSchedulerStorage = DirectCast(sender, ASPxSchedulerStorage)
        Dim apt As Appointment = CType(e.Object, Appointment)
        storage.SetAppointmentId(apt, apt.GetHashCode())
    End Sub
End Class