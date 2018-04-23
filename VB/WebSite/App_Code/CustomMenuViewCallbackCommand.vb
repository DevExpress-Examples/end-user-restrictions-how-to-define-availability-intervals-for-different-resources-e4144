Imports System
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal

Public Class CustomMenuViewCallbackCommand
    Inherits MenuViewCallbackCommand

    Private currentCommandProhibited As Boolean

    Public Sub New(ByVal control As ASPxScheduler)
        MyBase.New(control)
    End Sub

    Protected Overrides Sub ParseParameters(ByVal parameters As String)
        Dim isNewAppointmentCommand As Boolean = (parameters = "NewAppointment" OrElse parameters = "NewAllDayEvent" OrElse parameters = "NewRecurringAppointment" OrElse parameters = "NewRecurringEvent")

        currentCommandProhibited = isNewAppointmentCommand AndAlso Not ResourcesAvailabilities.IsIntervalAvailableForResource(Control.SelectedResource.Id.ToString(), Control.SelectedInterval)

        MyBase.ParseParameters(parameters)
    End Sub

    Protected Overrides Sub ExecuteCore()
        If currentCommandProhibited Then
            AddHandler Control.CustomJSProperties, AddressOf ASPxScheduler1_CustomJSProperties
        Else
            MyBase.ExecuteCore()
        End If
    End Sub

    Private Sub ASPxScheduler1_CustomJSProperties(ByVal sender As Object, ByVal e As CustomJSPropertiesEventArgs)
        e.Properties.Add("cpWarning", ResourcesAvailabilities.WarningMessage)
    End Sub
End Class