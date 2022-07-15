using MySql.Data.MySqlClient;
using rpa_mazar.Common;
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
    public partial class RouteMis : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com";
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;
            if (!(Session["username"] == null))
            {
                try
                {
                    username.InnerText = Session["username"].ToString();
                    #region UserAccess Check
                    //dtTemp = AppUser.AccessModules(Session["username"].ToString(), spoMasterTable: "routespocmaster", salesOrderApprovalMaster: "routeapprovalmaster");
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


            //approverCode = objSQL.DataTable_return("select SPOCID from spocmaster;").Rows[0][0].ToString();
            if (!IsPostBack)
            {
                GetTop20();
                setRequestIds();
                //remove hardcoded email id from queries
                pendingrequests.InnerHtml = objSQL.DataTable_return("select count(*) from routeuserinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=2;").Rows[0][0].ToString();
                totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from routeuserinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                approvedrequets.InnerHtml = objSQL.DataTable_return("select count(*) from routeuserinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=3;").Rows[0][0].ToString();
                rejectedrequests.InnerHtml = objSQL.DataTable_return("select count(*) from routeuserinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
            }
        }

        protected void btn_reportdownload_ServerClick(object sender, EventArgs e)
        {
            //dtTemp = objSQL.DataTable_return("Select sp.*, st.ApprovalEmail, st.Approval_Status, st.ApproverResponseDate From salesorderprocessing sp, salesorderapprovaltransaction st where sp.caseCustomerID in " +
            //    "(select distinct(caseCustomerID) from salesorderinput where SPOCEmail = '" + Session["username"].ToString() + "') and " +
            //    "sp.CaseCustomerID = st.CaseCustomerID and date(st.ApproverResponseDate) >= '" + Convert.ToDateTime(rfromdate.Value).ToString("yyyy-MM-dd") + "' and date(st.ApproverResponseDate) <= '" + Convert.ToDateTime(rtodate.Value).ToString("yyyy-MM-dd") + "'; ");


            //dtTemp = objSQL.DataTable_return("select sp.Name as `Customer Name`,t.* from (select si.ReqID as `Request ID`, si.CaseCustomerID,si.CreatedDate,si.ApproverResponseDate, " +
            //"si.ProcessedDate, GROUP_CONCAT(Concat(case when t.ApprovalEmail is null then '' else t.ApprovalEmail end,'-',case when t.Approval_Status = 2 then 'Approved' when t.Approval_Status = 1 then 'Pending' " +
            //"when t.Approval_Status = 3 then 'Rejected' when t.Approval_Status is null then 'Bot Pending' end),' ' order by t.Priority) as `Approval Flow` " +
            //", t.Approval_Status, si.Remarks from salesorderapprovaltransaction t right join salesorderinput si " +
            //"on si.CaseCustomerID = t.CaseCustomerID where si.SPOCEmail = '" + Session["username"].ToString() + "' and date(si.CreatedDate)>= '" + Convert.ToDateTime(rfromdate.Value).ToString("yyyy-MM-dd") + "' " +
            //"and date(si.CreatedDate)<= '" + Convert.ToDateTime(rfromdate.Value).ToString("yyyy-MM-dd") + "' group by si.ReqID) t left join salesorderprocessing sp on t.CaseCustomerID = sp.CaseCustomerID; ");


            //string attachment = "attachment; filename=report.xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/vnd.ms-excel";
            //string tab = "";
            //foreach (DataColumn dc in dtTemp.Columns)
            //{
            //    Response.Write(tab + dc.ColumnName);
            //    tab = "\t";
            //}
            //Response.Write("\n");
            //int i;
            //foreach (DataRow dr in dtTemp.Rows)
            //{
            //    tab = "";
            //    for (i = 0; i < dtTemp.Columns.Count; i++)
            //    {
            //        Response.Write(tab + dr[i].ToString());
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Response.End();
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }

        protected void GetTop20()
        {

            //dtTemp=objSQL.SP_DataTable_return("sp_getmisdata",)

            //dtTemp = objSQL.DataTable_return("select ReqID,'SO' as ProcessType, ProcessedDate as Submitted_Data_Time,status from `salesorderinput` where SPOCEmail = '" + Session["username"].ToString() + "'order by `ReqID` desc limit 20;");
            //gv_top20requests.DataSource = dtTemp;
            //gv_top20requests.DataBind();

            //gv_requeststatus.DataSource = dtTemp;
            //gv_requeststatus.DataBind();
        }


        protected void setRequestIds()
        {
            try
            {
                //remove hardcoded email id from query
                if (Session["username"].ToString() == "rpa-admin@dalmiabharat.com")
                {
                    requestids.Items.Clear();
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from routeuserinput order by CreatedDate desc limit 100;");
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from routeuserinput where SPOCEmail='" + Session["username"].ToString() + "' order by CreatedDate desc limit 20;");
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
            //dtTemp = objSQL.DataTable_return("SELECT distinct t1.`REQID`,t1.`RPAID`,t2.approvalemail AS 'APPROVAL EMAIL', t1.SPOCEMAIL AS 'SPOC EMAIL', case when t1.status = 0 then 'New Request' when t1.status = 2 then 'Pending for Approval' when t1.status = 3 then 'Approved' when t1.status = 4 then 'Route Created' when t1.Status = 5 then 'Route Determined' when t1.status = 6 then 'Rejected/Validation Failed' when t1.status = 7 then 'Route Processed Successfully' end as `CURRENT STATUS`, t1.Description AS 'DESCRIPTION ', t1.createdDate AS 'CREATED DATE', t1.processeddate AS 'PROCESSED DATE', t1.`ZONE`,t1.`STATE_PLANT`, t1.`ROUTEDESCRIPTION`,t1.`ROUTEIDENTIFICATION`,t1.`COMMENTS` FROM `routeuserinput` t1 left join `routeapprovaltransaction` t2 on t1.RPAID = t2.RPAID where t2.ApprovalStatus=1 and t2.level=1 and t1.reqid = " + requestids.SelectedValue + "; ");
            dtTemp = objSQL.DataTable_return("drop table if exists t2; " +
            "create temporary table t2 select distinct RPAID, ApprovalEmail, ApprovalStatus, Level from routeapprovaltransaction " +
            "where RPAID in (select RPAID from routeuserinput where ReqID = " + requestids.SelectedValue + " ) and approvalstatus = 1 and level = 1;" +
            "SELECT distinct t1.`REQID`,t1.`RPAID`,t1.SPOCEMAIL AS 'SPOC EMAIL', case when t1.status = 0 then 'New Request'" +
            "when t1.status = 2 then 'Pending for Approval' when t1.status = 3 then 'Approved' when t1.status = 4 then 'Route Created'" +
            "when t1.Status = 5 then 'Route Determined' when t1.status = 6 then 'Rejected/Validation Failed'" +
            "when t1.status = 7 then 'Route Processed Successfully' end as `CURRENT STATUS`, t1.Description, t1.createdDate 'CREATED DATE'," +
            "t1.processeddate 'PROCESSED DATE', t1.`ZONE`, t1.`STATE_PLANT`, t1.`ROUTEDESCRIPTION`,t1.`ROUTEIDENTIFICATION`,t1.`COMMENTS`," +
            "CASE WHEN t1.status = 2 and t2.ApprovalStatus=1 and t2.level=1 THEN t2.ApprovalEmail ELSE '' END AS `Current Approver`" +
            "FROM `routeuserinput` t1 LEFT JOIN t2 ON t1.RPAID = t2.RPAID left join `routeapprovaltransaction` t3 on t1.RPAID = t3.RPAID where t1.reqid = " + requestids.SelectedValue + "; ");

            gv_requeststatus.DataSource = dtTemp;
            gv_requeststatus.DataBind();
        }
    }
}