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
    public partial class ccmis : System.Web.UI.Page
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


            //approverCode = objSQL.DataTable_return("select SPOCID from spocmaster where Email='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
            if (!IsPostBack)
            {
                GetTop20();
                setRequestIds();
                //remove hardcoded email id from queries
                pendingrequests.InnerHtml = objSQL.DataTable_return("select count(*) from non_trade_price_contract_creation where requestor_email='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
                totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from non_trade_price_contract_creation where requestor_email='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                approvedrequets.InnerHtml = objSQL.DataTable_return("select count(*) from non_trade_price_contract_creation where requestor_email='" + Session["username"].ToString() + "' and Status=5;").Rows[0][0].ToString();
                rejectedrequests.InnerHtml = objSQL.DataTable_return("select count(*) from non_trade_price_contract_creation where requestor_email='" + Session["username"].ToString() + "' and Status=7;").Rows[0][0].ToString();
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from non_trade_price_contract_creation order by CaseCreatedDate desc;");
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from non_trade_price_contract_creation where requestor_email='" + Session["username"].ToString() + "' order by CaseCreatedDate desc limit 20;");
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
            "from(select ReqID as 'Request ID', 'Contract Creation' as 'Process Type', CaseCreatedDate as 'Submitted Date/Time', " +
            "requestor_email as SPOC, " +
            "(select count(*) from non_trade_price_contract_creation where ReqID = " + requestids.SelectedValue + ") as 'Total Records', " +
            "(select count(*) from non_trade_price_contract_creation where status = 7 and Description not in ('Rejected by approver','Contract Number Not Found for Customer') and ReqID =  " + requestids.SelectedValue + ") as 'Rejected in Validation', " +
            "(select count(*) from non_trade_price_contract_creation where status = 4 and ReqID =  " + requestids.SelectedValue + ") as 'Pending for Approval', " +
            "(select count(*) from non_trade_price_contract_creation where status = 7 and Description in ('Rejected by approver') and ReqID =  " + requestids.SelectedValue + ") as 'Records Rejected by Approver', " +
            "(select count(*) from non_trade_price_contract_creation where status = 7 and Description in ('Contract Number Not Found for Customer') and ReqID =  " + requestids.SelectedValue + ") as 'Records Unsuccessful in Processing', " +
            "(select count(*) from non_trade_price_contract_creation where Status = 8 and ReqID =  " + requestids.SelectedValue + ") as 'Records Successful in Processing' from non_trade_price_contract_creation where ReqID =  " + requestids.SelectedValue + ") t limit 1;");
            gv_top20requests.DataSource = dtTemp;
            gv_top20requests.DataBind();

            //dtTemp = objSQL.SP_DataTable_return("sp_getrequeststatus", new MySqlParameter("RID", requestids.SelectedValue)).Tables[0];
            //dtTemp = objSQL.DataTable_return("drop table if exists t1;  " +
            //"create temporary table t1 select distinct ncc.Customer_Code, cc_appro.ApprovalEmail, cc_appro.Approval_Status, cc_appro.Level from non_trade_price_contract_creation ncc inner join cc_approvaltransaction on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID = ncc.ReqID where CaseCustomerID in (select Customer_Code from non_trade_price_contract_creation " +
            //"where ReqID = " + requestids.SelectedValue + ") and cc_appro.approval_status = 1 and cc_appro.level = 1; " +
            //"select distinct s1.ReqId, S1.CaseCustomerID, s1.CustomerCode,s1.requestor_email, " +
            //"case when s1.status = 2 then 'Pending for Approval' when s1.status = 7 then 'Rejected / Validation Failed' when s1.status = 5 then 'Processed' when s1.status = 3 then 'Approved' when s1.status = 0 then 'New Request' end as `Current Status`, " +
            //"s1.Remarks, s1.Description, s1.CaseCreatedDate, Case when s1.status = 2 and t1.Approval_Status = 1 and t1.level = 1 then t1.ApprovalEmail else '' end as `Current Approver` " +
            //"from non_trade_price_contract_creation s1 left join t1 on s1.Customer_Code = t1.Customer_Code where s1.ReqID = " + requestids.SelectedValue + "; ");

            dtTemp = objSQL.DataTable_return("drop table if exists t1; create temporary table t1 select distinct contractid, ApprovalEmail, Approval_Status, Level from cc_approvaltransaction"+
                                              " where contractid in (select contractid from non_trade_price_contract_creation where ReqID = " + requestids.SelectedValue + " ) and approval_status = 1 and level = 1;" + 
                                              " select distinct s1.ReqId, S1.`ContractID`, s1.`Customer_Code` as 'Sold-to Code',s1.`Customer_Name` as 'Sold-to Name',s1.`Consignee_Code` as 'Ship-to Code',s1.`PO_No`," +
                                              " s1.`PO_Date`,s1.`IS_Price`, s1.`Plant`,s1.`Material_type`, s1.`Validity_From`,s1.`Validity_To`,s1.`ContractNumber`,s1.requestor_email as 'SPOC'," +
                                              " case when s1.status = 4 then 'Pending for Approval' when s1.status = 7 then 'Rejected / Validation Failed' when s1.status = 8 then 'Processed' when s1.status = 5 then 'Approved' when s1.status = 3 then 'New Request' end as `Current Status`," +
                                              " s1.Remark, s1.Description, s1.CaseCreatedDate, Case when s1.status = 4 and t1.Approval_Status = 1 and t1.level = 1 then t1.ApprovalEmail else '' end as `Current Approver` from non_trade_price_contract_creation s1 left join t1 on s1.ContractID = t1.ContractID where s1.ReqID = " + requestids.SelectedValue + "; ");

            gv_requeststatus.DataSource = dtTemp;
            gv_requeststatus.DataBind();
        }
    }
}