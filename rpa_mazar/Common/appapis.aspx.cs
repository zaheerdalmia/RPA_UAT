using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace rpa_mazar.Common
{
    public partial class appapis : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        DataTable dtTemp = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected void getData()
        {
            dtTemp = objSQL.DataTable_return("select count(month1) as total, month1, year1 from (select monthname(createddate) as month1, year(createddate) as year1 from salesorderinput) t group by month1, year1 limit 6;");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            List<string> lstMonth = new List<string>();
            List<int> lstData = new List<int>();

            foreach (DataRow dr in dtTemp.Rows)
            {
                lstMonth.Add(dr["month1"].ToString() + " " + dr["year1"].ToString());
            }

            foreach (DataRow dr in dtTemp.Rows)
            {
                lstData.Add(Convert.ToInt32(dr["total"]));
            }

            row = new Dictionary<string, object>();
            row.Add("Labels", lstMonth);
            //rows.Add(row);
            row.Add("data", lstData);
            rows.Add(row);
            Response.Write(serializer.Serialize(rows));
        }
    }
}