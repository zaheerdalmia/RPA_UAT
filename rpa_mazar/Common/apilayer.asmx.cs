using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace rpa_mazar.Common
{
    /// <summary>
    /// Summary description for apilayer
    /// </summary>
    [WebService(Namespace = "https://centralmdm.dalmiabharat.com/DalmiaRPAPortalV1/Common/apilayer.asmx/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class apilayer : System.Web.Services.WebService
    {
        MySQLHelper objSQL = new MySQLHelper();
        DataTable dtTemp = null;

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getDataSO()
        {
            dtTemp = objSQL.DataTable_return("SET sql_mode = ''; select count(month1) as total, MONTHNAME(STR_TO_DATE(month1,'%m')) as month1, year1 from (select date_format(createddate,'%m') as month1,date_format(createddate,'%y') as year1 from salesorderinput) t  group by month1, year1 limit 6;");
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

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(rows));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getDataCL()
        {
            dtTemp = objSQL.DataTable_return("SET sql_mode = ''; select count(month1) as total, MONTHNAME(STR_TO_DATE(month1,'%m')) as month1, year1 from (select date_format(casecreateddate,'%m') as month1,date_format(casecreateddate,'%y') as year1 from creditlimitinput) t  group by month1, year1 limit 6;");
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

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(js.Serialize(rows));
        }
    }
}
