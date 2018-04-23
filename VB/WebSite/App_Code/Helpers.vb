Imports System
Imports System.Web
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports DevExpress.XtraScheduler
Imports DevExpress.Web.ASPxScheduler

Public NotInheritable Class ResourceFiller

    Private Sub New()
    End Sub

    Public Shared Users() As String = { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" }
    Public Shared Usernames() As String = { "sbrighton", "rfischer", "amiller" }

    Public Shared Sub FillResources(ByVal storage As ASPxSchedulerStorage, ByVal count As Integer)
        Dim resources As ResourceCollection = storage.Resources.Items
        storage.BeginUpdate()
        Try
            Dim cnt As Integer = Math.Min(count, Users.Length)
            For i As Integer = 1 To cnt
                resources.Add(storage.CreateResource(Usernames(i - 1), Users(i - 1)))
            Next i
        Finally
            storage.EndUpdate()
        End Try
    End Sub
End Class

Public NotInheritable Class ColorHelper

    Private Sub New()
    End Sub

    Public Shared Function InvertColor(ByVal color As Color) As Color
        Return System.Drawing.Color.FromArgb(color.A, CByte((Not color.R)), CByte((Not color.G)), CByte((Not color.B)))
    End Function
End Class

Public NotInheritable Class ResourcesAvailabilities

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property WarningMessage() As String
        Get
            Return "This time interval is not available for this resource."
        End Get
    End Property

    Public Shared Function IsIntervalAvailableForResource(ByVal resourceId As String, ByVal timeInterval As TimeInterval) As Boolean
        Dim availabilities As TimeIntervalCollection = GetAvailabilitiesForResource(resourceId.ToString())
        Dim result As Boolean = False

        For i As Integer = 0 To availabilities.Count - 1
            If availabilities(i).Contains(timeInterval) Then
                result = True
                Exit For
            End If
        Next i

        Return result
    End Function

    Private Shared Function GetAvailabilitiesForResource(ByVal resourceId As String) As TimeIntervalCollection
        Dim table As DataTable = GetAvailabilitiesTable()
        Dim view As New DataView(table, String.Format("ResourceId = '{0}'", resourceId), String.Empty, DataViewRowState.CurrentRows)
        Dim result As New TimeIntervalCollection()

        For i As Integer = 0 To view.Count - 1
            result.Add(New TimeInterval(Convert.ToDateTime(view(i)("StartTime")), Convert.ToDateTime(view(i)("EndTime"))))
        Next i

        Return result
    End Function

    Private Shared Function GetAvailabilitiesTable() As DataTable
        If HttpContext.Current.Session("AvailabilitiesTable") Is Nothing Then
            Using connection As New OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("DatabaseConnectionString").ConnectionString)
                Dim selectCommand As New OleDbCommand("SELECT * FROM ResourcesAvailabilities", connection)
                Dim dataAdapter As New OleDbDataAdapter(selectCommand)
                Dim dataTable As New DataTable("AvailabilitiesTable")

                dataAdapter.Fill(dataTable)
                dataTable.Constraints.Add("IDPK", dataTable.Columns("Id"), True)

                HttpContext.Current.Session("AvailabilitiesTable") = dataTable
                connection.Close()
            End Using
        End If

        Return DirectCast(HttpContext.Current.Session("AvailabilitiesTable"), DataTable)
    End Function
End Class