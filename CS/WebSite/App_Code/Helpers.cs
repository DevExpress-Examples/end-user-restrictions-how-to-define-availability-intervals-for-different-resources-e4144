using System;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;

public static class ResourceFiller {
    public static string[] Users = new string[] { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" };
    public static string[] Usernames = new string[] { "sbrighton", "rfischer", "amiller" };

    public static void FillResources(ASPxSchedulerStorage storage, int count) {
        ResourceCollection resources = storage.Resources.Items;
        storage.BeginUpdate();
        try {
            int cnt = Math.Min(count, Users.Length);
            for (int i = 1; i <= cnt; i++) {
                resources.Add(storage.CreateResource(Usernames[i - 1], Users[i - 1]));
            }
        }
        finally {
            storage.EndUpdate();
        }
    }
}

public static class ColorHelper {
    public static Color InvertColor(Color color) {
        return Color.FromArgb(color.A, (byte)~color.R, (byte)~color.G, (byte)~color.B);
    }
}

public static class ResourcesAvailabilities {
    public static string WarningMessage { get { return "This time interval is not available for this resource."; } }
    
    public static bool IsIntervalAvailableForResource(string resourceId, TimeInterval timeInterval) {
        TimeIntervalCollection availabilities = GetAvailabilitiesForResource(resourceId.ToString());
        bool result = false;

        for (int i = 0; i < availabilities.Count; i++) {
            if (availabilities[i].Contains(timeInterval)) {
                result = true;
                break;
            }
        }

        return result;
    }

    private static TimeIntervalCollection GetAvailabilitiesForResource(string resourceId) {
        DataTable table = GetAvailabilitiesTable();
        DataView view = new DataView(table, string.Format("ResourceId = '{0}'", resourceId), string.Empty, DataViewRowState.CurrentRows);
        TimeIntervalCollection result = new TimeIntervalCollection();

        for (int i = 0; i < view.Count; i++) {
            result.Add(new TimeInterval(Convert.ToDateTime(view[i]["StartTime"]), Convert.ToDateTime(view[i]["EndTime"])));
        }

        return result;
    }

    private static DataTable GetAvailabilitiesTable() {
        if (HttpContext.Current.Session["AvailabilitiesTable"] == null) {
            using (OleDbConnection connection = new OleDbConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString)) {
                OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM ResourcesAvailabilities", connection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(selectCommand);
                DataTable dataTable = new DataTable("AvailabilitiesTable");

                dataAdapter.Fill(dataTable);
                dataTable.Constraints.Add("IDPK", dataTable.Columns["Id"], true);

                HttpContext.Current.Session["AvailabilitiesTable"] = dataTable;
                connection.Close();
            }
        }

        return (DataTable)HttpContext.Current.Session["AvailabilitiesTable"];
    }
}