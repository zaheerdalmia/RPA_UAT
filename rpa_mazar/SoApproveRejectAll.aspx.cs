using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rpa_mazar
{
    public partial class SoApproveRejectAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["username"] == null))
            {
                try
                {
                    string message = "";
                    int approvalstatus = Convert.ToInt32(Request.QueryString["status"]);
                    string workFlowId = Convert.ToString(Request.QueryString["wid"]);
                    string userId = Convert.ToString(Session["username"]);
                    String pvConnectionString = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                    MySqlConnection conn = new MySqlConnection(pvConnectionString);
                    MySqlCommand cmd = new MySqlCommand();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SO_ApproveAll_RejectAll";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@inApproval_Status", approvalstatus);
                    cmd.Parameters["@inApproval_Status"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@inWorkflowID", workFlowId);
                    cmd.Parameters["@inWorkflowID"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@Emailid", userId);
                    cmd.Parameters["@Emailid"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@return_message", MySqlDbType.VarChar);
                    cmd.Parameters["@return_message"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    message = Convert.ToString(cmd.Parameters["@return_message"].Value);
                    conn.Close();
                    lblMessage.Text = message;
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
        }
    }
}