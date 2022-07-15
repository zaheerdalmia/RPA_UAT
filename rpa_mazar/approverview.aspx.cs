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
    public partial class approverview : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static DataTable dtTemp1 = null;
        static string strApproveList = string.Empty;
        static string strRejectList = string.Empty;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;
        //public StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["username"] == null))
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("22 \n", w);
                }
                try
                {
                    username.InnerText = Session["username"].ToString();
                    #region UserAccess Check
                    dtTemp = Common.AppUser.AccessModules(Session["username"].ToString());
                    if (dtTemp.Rows.Count > 0)
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("23 \n", w);
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
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("24 \n", w);
                        }

                        // Session.Clear();
                        //Common.Functions.ADFSSignout();
                        Response.Redirect("Dashboard.aspx");
                    }

                    if (dtTemp.Rows.Count > 0)
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("25 \n", w);
                        }

                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            HtmlAnchor anchor = (HtmlAnchor)Page.FindControl(dr["moduleelementid"].ToString());
                            if (anchor != null)
                                anchor.Visible = true;
                        }
                    }
                    else
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("26 \n", w);
                        }

                        Session.Clear();
                        Common.Functions.ADFSSignout();
                    }
                    #endregion
                }
                catch
                {
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log("27 \n", w);
                    }

                    Session.Clear();
                    //Common.Functions.ADFSSignout();
                    Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                }
            }
            else
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("Approver view 28 \n", w);
                }

                //Common.Functions.ADFSSignout();
                Session.Clear();
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            }

            if (!IsPostBack)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("29 \n", w);
                }

                //if (Request.QueryString["status"] != null)
                //{
                //    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                //    lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
                //    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                //}

                dtTemp = objSQL.DataTable_return("select ApprovalID from salesorderapprovalmaster where ApprovalEmail='" + Session["username"].ToString() + "';");
                if (dtTemp.Rows.Count > 0)
                    approverCode = dtTemp.Rows[0][0].ToString();//"10065";
                else
                    approverCode = "";

                setRequestIds();

                if (approverCode != string.Empty)
                {
                    GetDataForApproval();
                }
                else
                {
                    //Common.Functions.ADFSSignout();
                    //Session.Clear();
                    Response.Redirect("dashboard.aspx");
                }
            }
        }

        protected void gv_approvallist_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv_approvallist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GetDataForApproval();
            gv_approvallist.PageIndex = e.NewPageIndex;
            gv_approvallist.DataBind();
        }

        protected void GetDataForApproval()
        {
            dtTemp = objSQL.DataTable_return("Select distinct SPOCEmail, Approver, si.CaseCustomerID, Zone, StateName, Case when Division = 1 " +
            "then 'Cement' when Division = 2 then 'Clinker' when Division = 3 then 'Fly Ash' when Division = 4 then 'Power' when " +
            "Division = 5 then 'Scrap' when Division = 6 then 'NewBuildingSolutions' when Division = 7 then 'Limestone Chips' when Division = 8 " +
            "then 'Ready Mix Cement' when Division = 9 then 'Fly Ash Bricks' when Division = 10 then 'Slag' when Division = 11 then 'Asset' " +
            "when Division = 12 then 'Others' when Division = 13 then 'Coal' when Division = 14 then 'Bricks' when Division = 15 then " +
            "'Craft Beton' end as Division,CustGrp2, Case when PayTerms = 'ZS01' then 'PAYABLE IN 1 DAY' when PayTerms = 'ZS02' then 'PAYABLE IN 3 DAYS' " +
            "when PayTerms = 'ZS03' then 'PAYABLE IN 7 DAYS' when PayTerms = 'ZS04' then 'PAYABLE IN 15 DAYS' when PayTerms = 'ZS05' then " +
            "'PAYABLE IN 30 DAYS' when PayTerms = 'ZS06' then 'PAYABLE IN 40 DAYS' when PayTerms = 'ZS07' then 'PAYABLE IN 10 DAYS' " +
            "when PayTerms = 'ZS08' then 'PAYABLE IN 35 DAYS' when PayTerms = 'ZS19' then 'PAYABLE IN 21 DAYS' when PayTerms = 'ZS20' " +
            "then 'PAYABLE IN 60 DAYS' when PayTerms = 'ZS21' then 'PAYABLE IN 45 DAYS' when PayTerms = 'ZS25' then " +
            "'PAYABLE IN 75 DAYS' when PayTerms = 'ZS26' then 'PAYABLE IN 18 DAYS' when PayTerms = 'ZS27' then 'PAYABLE IN 80 DAYS' " +
            "when PayTerms = 'ZS28' then 'PAYABLE IN 100 DAYS' when PayTerms = 'ZS29' then 'PAYABLE IN 120 DAYS' when PayTerms = 'Z001' " +
            "then 'Advance' end as PayTerms,CustomerGroup1Description, Case when DistributionChannel = 1 then 'Trade' when DistributionChannel = 2 then " +
            "'Institutional' when DistributionChannel = 3 then 'Stock Transfer' when DistributionChannel = 4 then 'Self Consumption' when " +
            "DistributionChannel = 5 then 'Inter Company' when DistributionChannel = 6 then 'Export Sales' end as DistributionChannel, " +
            "Division, si.CustomerCode, Name, Category, KanikaDealer,  TotalCreditLimit, PremanentCreditLimit, TempCreditLimit, " +
            "SecurityDeposit, SSD, BankGuarantee, ChequeUnderClearance, TotalOutstanding, TotalNotDue, Days0To15, Days16To21, Days22To30, Days31To45, " +
            "Days46To60, ABOVE60DAYS, SOValue, DeliveryValue, `BlockedSalesOrder-Value`, TotalOsSoDelivery, STEP_1, STEP_2, NODBlock, " +
            "SPName, NetCreditAmount, SDUsed,DealerSlab,OpeningOutstanding,MTDCollectionsValue,MTDSalesValue from " +
            "(Select * From salesorderprocessing where caseCustomerID in (select caseCustomerID from salesorderapprovaltransaction where Approval_ID = " + approverCode + " " +
            "and Approval_status = 1 and level = 1 and caseCustomerID in (select caseCustomerID from salesorderinput where status = 2))) t, " +
            "salesorderinput si where si.CaseCustomerID = t.CaseCustomerID;");
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.DataBind();

        }

        protected void btn_SubmitData_ServerClick(object sender, EventArgs e)
        {
            strApproveList = string.Empty;
            strRejectList = string.Empty;

            foreach (GridViewRow gr in gv_approvallist.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox checkA = (CheckBox)gr.FindControl("chkAR");
                    if (checkA.Checked)
                    {
                        if (strApproveList == string.Empty)
                            strApproveList = gr.Cells[4].Text;
                        else
                            strApproveList = strApproveList + "," + gr.Cells[4].Text;
                    }

                    CheckBox checkR = (CheckBox)gr.FindControl("chkRR");
                    if (checkR.Checked)
                    {
                        if (strRejectList == string.Empty)
                            strRejectList = gr.Cells[4].Text;
                        else
                            strRejectList = strRejectList + "," + gr.Cells[4].Text;
                    }
                }
            }

            if (strApproveList != string.Empty)
            {
                ApproveRequests();
            }

            if (strRejectList != string.Empty)
            {
                RejectRequests();
            }

            //Response.Redirect("approverview.aspx?status=1");

            GetDataForApproval();
            setRequestIds();

            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
            lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
        }

        protected void ApproveRequests()
        {
            try
            {
                objSQL.ExecuteNonQuery_IUD("update salesorderapprovaltransaction set Approval_Status=2,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where level=1 and Approval_Status=1 and Approval_Id=" + approverCode + " and CaseCustomerID in (" + strApproveList + "); ");
                foreach (string strCCI in strApproveList.Split(','))
                {
                    dtTemp = objSQL.DataTable_return("select * from salesorderapprovaltransaction where level=0 and Approval_Status=0 and CaseCustomerID=" + strCCI + " order by Priority limit 1;");
                    dtTemp1 = objSQL.DataTable_return("select * from salesorderapprovaltransaction where level=1 and Approval_Status=1 and CaseCustomerID=" + strCCI + " order by Priority limit 1;");
                    if (dtTemp1.Rows.Count > 0)
                    {
                        // objSQL.ExecuteNonQuery_IUD("update salesorderapprovaltransaction set level = 1 where TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else if (dtTemp.Rows.Count > 0)
                    {
                        objSQL.ExecuteNonQuery_IUD("update salesorderapprovaltransaction set level = 1 where TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else
                    {
                        objSQL.ExecuteNonQuery_IUD("update salesorderinput set Status=3,`ApproverResponseDate`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where CaseCustomerID= " + strCCI + " and status=2;");
                    }
                }
                //GetDataForApproval();
                //lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                //lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + "\n\n" + ex.StackTrace, w);
                }
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Oops! Error while updating approve / reject.. Please refer errlog";
            }
        }

        protected void RejectRequests()
        {
            try
            {
                objSQL.ExecuteNonQuery_IUD("update salesorderapprovaltransaction set Approval_Status=3,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where level=1 and Approval_Status=1 and Approval_Id=" + approverCode + " and CaseCustomerID in (" + strRejectList + "); ");
                foreach (string strCCI in strRejectList.Split(','))
                {
                    objSQL.ExecuteNonQuery_IUD("update salesorderinput set Status=4,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', Remarks='Fail',Description='Rejection from Approver', ProcessedDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where CaseCustomerID= " + strCCI + " and Status=2;");
                }
                //GetDataForApproval();
                //lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                //lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + "\n\n" + ex.StackTrace, w);
                }
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Oops! Error while updating approve / reject.. Please refer errlog";
            }
        }

        protected void setRequestIds()
        {
            try
            {
                //remove hardcoded email id from query
                lbl_Message.InnerText = "";
                requestids.Items.Clear();
                dtTemp = objSQL.DataTable_return("select distinct(ReqID) from salesorderinput where CaseCustomerID in (select CaseCustomerID from salesorderapprovaltransaction where Approval_ID=" + approverCode + " and Approval_Status=1) and Status=2;");
                requestids.DataSource = dtTemp;
                requestids.DataTextField = "ReqID";
                requestids.DataValueField = "ReqID";
                requestids.DataBind();

                ListItem li = new ListItem();
                li.Text = "--- Select Request Id ---";
                li.Value = "0";
                requestids.Items.Insert(0, li);
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
            gv_approvallist.DataSource = null;
            dtTemp = objSQL.DataTable_return("Select distinct SPOCEmail, Approver, si.CaseCustomerID, Zone, StateName, Case when Division = 1 " +
            "then 'Cement' when Division = 2 then 'Clinker' when Division = 3 then 'Fly Ash' when Division = 4 then 'Power' when " +
            "Division = 5 then 'Scrap' when Division = 6 then 'NewBuildingSolutions' when Division = 7 then 'Limestone Chips' when Division = 8 " +
            "then 'Ready Mix Cement' when Division = 9 then 'Fly Ash Bricks' when Division = 10 then 'Slag' when Division = 11 then 'Asset' " +
            "when Division = 12 then 'Others' when Division = 13 then 'Coal' when Division = 14 then 'Bricks' when Division = 15 then " +
            "'Craft Beton' end as Division,CustGrp2, Case when PayTerms = 'ZS01' then 'PAYABLE IN 1 DAY' when PayTerms = 'ZS02' then 'PAYABLE IN 3 DAYS' " +
            "when PayTerms = 'ZS03' then 'PAYABLE IN 7 DAYS' when PayTerms = 'ZS04' then 'PAYABLE IN 15 DAYS' when PayTerms = 'ZS05' then " +
            "'PAYABLE IN 30 DAYS' when PayTerms = 'ZS06' then 'PAYABLE IN 40 DAYS' when PayTerms = 'ZS07' then 'PAYABLE IN 10 DAYS' " +
            "when PayTerms = 'ZS08' then 'PAYABLE IN 35 DAYS' when PayTerms = 'ZS19' then 'PAYABLE IN 21 DAYS' when PayTerms = 'ZS20' " +
            "then 'PAYABLE IN 60 DAYS' when PayTerms = 'ZS21' then 'PAYABLE IN 45 DAYS' when PayTerms = 'ZS25' then " +
            "'PAYABLE IN 75 DAYS' when PayTerms = 'ZS26' then 'PAYABLE IN 18 DAYS' when PayTerms = 'ZS27' then 'PAYABLE IN 80 DAYS' " +
            "when PayTerms = 'ZS28' then 'PAYABLE IN 100 DAYS' when PayTerms = 'ZS29' then 'PAYABLE IN 120 DAYS' when PayTerms = 'Z001' " +
            "then 'Advance' end as PayTerms,CustomerGroup1Description, Case when DistributionChannel = 1 then 'Trade' when DistributionChannel = 2 then " +
            "'Institutional' when DistributionChannel = 3 then 'Stock Transfer' when DistributionChannel = 4 then 'Self Consumption' when " +
            "DistributionChannel = 5 then 'Inter Company' when DistributionChannel = 6 then 'Export Sales' end as DistributionChannel, " +
            "si.CustomerCode, Name, Category, KanikaDealer,  TotalCreditLimit, PremanentCreditLimit, TempCreditLimit, " +
            "SecurityDeposit, SSD, BankGuarantee, ChequeUnderClearance, TotalOutstanding, TotalNotDue, Days0To15, Days16To21, Days22To30, Days31To45, " +
            "Days46To60, ABOVE60DAYS, SOValue, DeliveryValue, `BlockedSalesOrder-Value`, TotalOsSoDelivery, STEP_1, STEP_2, NODBlock,  " +
            "SPName, NetCreditAmount, SDUsed,DealerSlab,OpeningOutstanding,MTDCollectionsValue,MTDSalesValue from " +
            "(Select * From salesorderprocessing where caseCustomerID in (select caseCustomerID from salesorderapprovaltransaction where Approval_ID = " + approverCode + " " +
            "and Approval_status = 1 and level = 1 and caseCustomerID in (select caseCustomerID from salesorderinput where ReqID = " + requestids.SelectedValue + " and status = 2))) t, " +
            "salesorderinput si where si.CaseCustomerID = t.CaseCustomerID;");
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.DataBind();
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }
    }
}