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
    public partial class somis : System.Web.UI.Page
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


            //approverCode = objSQL.DataTable_return("select SPOCID from spocmaster;").Rows[0][0].ToString();
            if (!IsPostBack)
            {
                GetTop20();
                setRequestIds();
                //remove hardcoded email id from queries
                pendingrequests.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=2;").Rows[0][0].ToString();
                totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                approvedrequets.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=3;").Rows[0][0].ToString();
                rejectedrequests.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from salesorderinput order by CreatedDate desc limit 100;");
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' order by CreatedDate desc limit 20;");
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
            dtTemp = objSQL.DataTable_return("select distinct `Request ID`, `Process Type`, `Submitted Date/Time`, `SPOC`, `Total Records`, `Rejected in Validation`,`Pending for Approval`, " +
            "`Total Records`-`Rejected in Validation` as `Records approved in validation`,`Records Rejected by Approver`, " +
            "`Total Records`-`Rejected in Validation` - `Records Rejected by Approver` as `Records Cleared by Approver`,`Records Unsuccessful in Processing`, " +
            "`Records Successful in Processing` " +
            "from(select ReqID as 'Request ID', 'SO Release' as 'Process Type', CreatedDate as 'Submitted Date/Time', " +
            "SPOCEmail as SPOC, " +
            "(select count(*) from salesorderinput where ReqID = " + requestids.SelectedValue + ") as 'Total Records', " +
            "(select count(*) from salesorderinput where Description in ('Business Exception', 'SPOC invalid', 'Not to be Released') and ReqID =  " + requestids.SelectedValue + ") as 'Rejected in Validation', " +
            "(select count(*) from salesorderinput where status = 2 and ReqID =  " + requestids.SelectedValue + ") as 'Pending for Approval', " +
            "(select count(*) from salesorderinput where Description in ('Rejection from Approver') and ReqID =  " + requestids.SelectedValue + ") as 'Records Rejected by Approver', " +
            "(select count(*) from salesorderinput where Description in ('RecordNotFoundInSAP', 'ID is not maintained in tcode ZSD_CRM') and Status = 4 and ReqID =  " + requestids.SelectedValue + ") as 'Records Unsuccessful in Processing', " +
            "(select count(*) from salesorderinput where Status = 5 and ReqID =  " + requestids.SelectedValue + ") as 'Records Successful in Processing' from salesorderinput where ReqID =  " + requestids.SelectedValue + ") t limit 1;");
            gv_top20requests.DataSource = dtTemp;
            gv_top20requests.DataBind();

            dtTemp = objSQL.DataTable_return("drop table if exists t1;  " +
           "create temporary table t1 select distinct CaseCustomerID, ApprovalEmail, Approval_Status, Level from salesorderapprovaltransaction where CaseCustomerID in (select CaseCustomerID from salesorderinput " +
           "where ReqID = " + requestids.SelectedValue + ") and approval_status = 1 and level = 1; " +
           "select s1.ReqId, S1.CaseCustomerID, s1.CustomerCode,s2.Name, s1.SalesOrderNo,s1.SPOCEmail, " +
           "case when s1.status = 2 then 'Pending for Approval' when s1.status = 4 then 'Rejected / Validation Failed' when s1.status = 5 then 'Processed' when s1.status = 3 then 'Approved' when s1.status = 0 then 'New Request' end as `Current Status`, " +
           "s1.Remarks, s1.Description, s1.CreatedDate, Case when s1.status = 2 and t1.Approval_Status = 1 and t1.level = 1 then t1.ApprovalEmail else '' end as `Current Approver` " +
           "from salesorderinput s1 left join t1 on s1.CaseCustomerID = t1.CaseCustomerID left join salesorderprocessing s2 on s1.CaseCustomerID = s2.CaseCustomerID where s1.ReqID = " + requestids.SelectedValue + "; ");
            gv_requeststatus.DataSource = dtTemp;
            gv_requeststatus.DataBind();
        }
    }
}