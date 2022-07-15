using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace rpa_mazar
{
    public partial class clmis : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["username"] == null))
            {
                try
                {
                    username.InnerText = Session["username"].ToString();
                    #region UserAccess Check
                    dtTemp = Common.AppUser.AccessModules(Session["username"].ToString());
                    if (dtTemp.Rows.Count > 0)
                    {
                        IsPageAllowed = false;
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (HttpContext.Current.Request.Url.AbsoluteUri.Substring(HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf("/") + 1, HttpContext.Current.Request.Url.AbsoluteUri.Length - HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf("/") - 1) == dr["modulepagename"].ToString())
                            {
                                IsPageAllowed = true;
                            }
                        }
                    }

                    if (IsPageAllowed == false)
                    {
                        Session.Clear();
                        Common.Functions.ADFSSignout();
                    }

                    if (dtTemp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            HtmlAnchor anchor = (HtmlAnchor)Page.FindControl(dr["moduleelementid"].ToString());
                            if (anchor != null)
                                anchor.Visible = true;
                        }
                    }
                    else
                    {
                        Session.Clear();
                        Common.Functions.ADFSSignout();
                    }
                    #endregion
                }
                catch
                {
                    Session.Clear();
                    Common.Functions.ADFSSignout();
                }
            }
            else
            {
                Common.Functions.ADFSSignout();
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                setRequestIds();
                //remove hardcoded email id from queries
                pendingrequests.InnerHtml = objSQL.DataTable_return("select count(*) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=2;").Rows[0][0].ToString();
                totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                approvedrequets.InnerHtml = objSQL.DataTable_return("select count(*) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=3;").Rows[0][0].ToString();
                rejectedrequests.InnerHtml = objSQL.DataTable_return("select count(*) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }

        protected void btn_reportdownload_ServerClick(object sender, EventArgs e)
        {
            //    //dtTemp = objSQL.DataTable_return("Select sp.*, st.ApprovalEmail, st.Approval_Status, st.ApproverResponseDate From creditlimitprocessing sp, creditlimitapprovaltransaction st " +
            //    //"where sp.RPACLID in (select distinct(RPACLID) from salesorderinput where SPOCEmail = 'rpa-admin@dalmiabharat.com') and " +
            //    //"sp.RPACLID = st.CaseCustomerID and date(st.ApproverResponseDate) >= '" + Convert.ToDateTime(rfromdate.Value).ToString("yyyy-MM-dd") + "' and date(st.ApproverResponseDate) <= '" + Convert.ToDateTime(rtodate.Value).ToString("yyyy-MM-dd") + "'; ");

            //    dtTemp = objSQL.DataTable_return("select sp.CustomerName as `Customer Name`,t.* from (select si.ReqID as `Request ID`, si.RPACLID,si.CaseCreatedDate,si.ApproverResponseDate, " +
            //    "si.CaseProcessedDate, GROUP_CONCAT(Concat(case when t.ApprovalEmail is null then '' else t.ApprovalEmail end,'-',case when t.Approval_Status = 2 then 'Approved' when t.Approval_Status = 1 then 'Pending' " +
            //    "when t.Approval_Status = 3 then 'Rejected' when t.Approval_Status is null then 'Bot Pending' end),' ' order by t.Priority) as `Approval Flow` " +
            //    ", t.Approval_Status, si.Remarks from creditlimitapprovaltransaction t right join creditlimitinput si " +
            //    "on si.RPACLID = t.RPACLID where si.SPOCEmail = '" + Session["username"].ToString() + "' and date(si.CaseCreatedDate)>= '" + Convert.ToDateTime(rfromdate.Value).ToString("yyyy-MM-dd") + "' " +
            //    "and date(si.CaseCreatedDate)<= '" + Convert.ToDateTime(rtodate.Value).ToString("yyyy-MM-dd") + "' group by si.ReqID) t left join creditlimitprocessing sp on t.RPACLID = sp.RPACLID; ");

            //    string attachment = "attachment; filename=report.xls";
            //    Response.ClearContent();
            //    Response.AddHeader("content-disposition", attachment);
            //    Response.ContentType = "application/vnd.ms-excel";
            //    string tab = "";
            //    foreach (DataColumn dc in dtTemp.Columns)
            //    {
            //        Response.Write(tab + dc.ColumnName);
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //    int i;
            //    foreach (DataRow dr in dtTemp.Rows)
            //    {
            //        tab = "";
            //        for (i = 0; i < dtTemp.Columns.Count; i++)
            //        {
            //            Response.Write(tab + dr[i].ToString());
            //            tab = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            //    Response.End();
        }

        protected void setRequestIds()
        {
            try
            {
                //remove hardcoded email id from query
                if (Session["username"].ToString() == "rpa-admin@dalmiabharat.com")
                {
                    requestids.Items.Clear();
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from creditlimitinput order by CaseCreatedDate desc limit 100;");
                    requestids.DataSource = dtTemp;
                    requestids.DataTextField = "ReqID";
                    requestids.DataValueField = "ReqID";
                    requestids.DataBind();

                    ListItem li = new ListItem();
                    li.Text = "--- Select Request Id ---";
                    li.Value = "0";
                    requestids.Items.Insert(0, li);
                }
                else
                {
                    requestids.Items.Clear();
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "' order by CaseCreatedDate desc limit 20;");
                    requestids.DataSource = dtTemp;
                    requestids.DataTextField = "ReqID";
                    requestids.DataValueField = "ReqID";
                    requestids.DataBind();

                    ListItem li = new ListItem();
                    li.Text = "--- Select Request Id ---";
                    li.Value = "0";
                    requestids.Items.Insert(0, li);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.StackTrace, w);
                }
                Console.WriteLine("Method Name: setRequestIds \n" + ex.Message);
            }
        }

        protected void requestids_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtTemp = null;
            //dtTemp= objSQL.SP_DataTable_return("sp_getmisdata", new MySqlParameter("RID",requestids.SelectedValue)).Tables[0];
            dtTemp = objSQL.DataTable_return("SELECT DISTINCT `Request ID`,`Process Type`,`Submitted Date/Time`, `SPOC`,`Total Records`,`Rejected in Validation`,`Pending for Approval`,`Total Records` - `Rejected in Validation` AS `Records approved in validation`, " +
            "`Records Rejected by Approver`,`Total Records` - `Rejected in Validation` - `Records Rejected by Approver` AS `Records Cleared by Approver`,`Records Unsuccessful in Processing`, " +
            "`Records Successful in Processing` FROM(SELECT ReqID AS 'Request ID', 'CL' AS 'Process Type', CaseCreatedDate AS 'Submitted Date/Time', SPOCEmail AS SPOC, " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE ReqID = "+ requestids.SelectedValue +") AS 'Total Records', " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE(Description like 'Not to be released%' or Description in ('Spoc invalid', 'Business Exception')) AND ReqID = " + requestids.SelectedValue + ") AS 'Rejected in Validation', " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE status = 2 AND ReqID = " + requestids.SelectedValue + ") AS 'Pending for Approval', " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE Description IN('Rejection from Approver') AND ReqID = " + requestids.SelectedValue + ") AS 'Records Rejected by Approver', " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE Description IN('Not Updated or customer not found') AND Status = 4  AND ReqID = " + requestids.SelectedValue + ") AS 'Records Unsuccessful in Processing', " +
            "(SELECT COUNT(*) FROM creditlimitinput WHERE Status = 5 AND ReqID = " + requestids.SelectedValue + ") AS 'Records Successful in Processing' FROM creditlimitinput WHERE ReqID = " + requestids.SelectedValue + ") t limit 1; ");
            gv_top20requests.DataSource = dtTemp;
            gv_top20requests.DataBind();

            //dtTemp = objSQL.SP_DataTable_return("sp_getrequeststatus", new MySqlParameter("RID", requestids.SelectedValue)).Tables[0];
            dtTemp = objSQL.DataTable_return("drop table if exists t2; " +
            "create temporary table t2 select distinct RPACLID, ApprovalEmail, Approval_Status, Level from creditlimitapprovaltransaction " +
            "where RPACLID in (select RPACLID from creditlimitinput where ReqID = " + requestids.SelectedValue + " ) and approval_status = 1 and level = 1; " +
            "SELECT s1.ReqId,S1.RPACLID,s1.CustomerCode,s2.Customername,s1.RequestType,  " +
            "CASE WHEN s1.RequestType = 'TCL' THEN s1.TemporaryCreditLimit WHEN s1.RequestType = 'PCL' THEN s1.PermanentCreditLimit END AS `ProposedCreditLimit`, " +
            "s1.SPOCEmail,CASE WHEN s1.status = 2 THEN 'Pending for Approval' WHEN s1.status = 4 THEN 'Rejected / Validation Failed'WHEN s1.status = 5 THEN 'Processed' " +
            "WHEN s1.status = 3 THEN 'Approved'WHEN s1.status = 0 THEN 'New Request'END AS `Current Status`, s1.Remarks,s1.Description,s1.CaseCreatedDate, " +
            "CASE WHEN s1.status = 2 AND t2.Approval_Status = 1 AND t2.level = 1 THEN t2.ApprovalEmail ELSE '' END AS `Current Approver` " +
            "FROM creditlimitinput s1 LEFT JOIN t2 ON s1.RPACLID = t2.RPACLID left join creditlimitprocessing s2 ON s1.RPACLID = s2.RPACLID WHERE s1.ReqID = " + requestids.SelectedValue + "; ");
            gv_requeststatus.DataSource = dtTemp;
            gv_requeststatus.DataBind();
        }
    }
}