using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace rpa_mazar
{
    public partial class cmapprover : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string strApproveList = string.Empty;
        static string strRejectList = string.Empty;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com";
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;
            if (!(Session["username"] == null))
            {
                username.InnerText = Session["username"].ToString();
            }
            else
            {
                //Common.Functions.ADFSSignout();
                Session.Clear();
                Response.Redirect("login.aspx");
            }

            //Control myControl = (Control)Page.LoadControl("contractextension.ascx");
            //dvcontractextension.Controls.Add(myControl);
        }      

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }
    }
}