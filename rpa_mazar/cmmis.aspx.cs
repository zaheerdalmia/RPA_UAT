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
    public partial class cmmis : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;
        static string CFlagType = "";
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
                    dtTemp = Common.AppUser.AccessModules(Session["username"].ToString());
                    if (dtTemp.Rows.Count > 0)
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("Entered line No 39 CMMMIS", w);
                        }

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
                catch(Exception ex)
                {
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log(ex.Message, w);
                    }
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
                if (Request.QueryString["cid"] != null)
                {
                    CFlagType = Request.QueryString["cid"];
                    GetTop20();
                    setRequestIds();
                    //remove hardcoded email id from queries
                    pendingrequests.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where SpocEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
                    totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where SpocEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                    approvedrequets.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where SpocEmail='" + Session["username"].ToString() + "' and Status=5;").Rows[0][0].ToString();
                    rejectedrequests.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where SpocEmail='" + Session["username"].ToString() + "' and Status=7;").Rows[0][0].ToString();
                }
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from ntcontractupd_input where ContractFlag = '" + CFlagType + "' order by CreatedDate desc;");
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
                    dtTemp = objSQL.DataTable_return("select distinct(ReqID) from ntcontractupd_input where SpocEmail='" + Session["username"].ToString() + "' and ContractFlag = '" + CFlagType + "' order by CreatedDate desc limit 20;");
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
            try
            {
                //Zaheer Start Code
                string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("NTContractUpd_MISReport", w);
                }

                DataSet ds = new DataSet();
                using (MySqlConnection conn = new MySqlConnection(connect))
                {
                    MySqlCommand sqlComm = new MySqlCommand("NTContractUpd_MISReport", conn);
                    sqlComm.Parameters.AddWithValue("@vContractFlag", CFlagType);
                    sqlComm.Parameters.AddWithValue("@vReqID",Convert.ToInt32(requestids.SelectedValue));
                    sqlComm.Parameters.AddWithValue("@vStatus", Convert.ToInt32(1));

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                    gv_top20requests.DataSource = ds;
                    gv_top20requests.DataBind();
                }

                //Zaheer Start Code                
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("NTContractUpd_MISReport", w);
                }

                DataSet ds1 = new DataSet();
                using (MySqlConnection conn = new MySqlConnection(connect))
                {
                    MySqlCommand sqlComm = new MySqlCommand("NTContractUpd_MISReport", conn);
                    sqlComm.Parameters.AddWithValue("@vContractFlag", CFlagType);
                    sqlComm.Parameters.AddWithValue("@vReqID", Convert.ToInt32(requestids.SelectedValue));
                    sqlComm.Parameters.AddWithValue("@vStatus", Convert.ToInt32(2));
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds1);
                    gv_requeststatus.DataSource = ds1;
                    gv_requeststatus.DataBind();
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.StackTrace, w);
                }                
            }
        }
    }
}