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
    public partial class approverviewco : System.Web.UI.Page
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
            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
            {
                Logger.Log("13 \n", w);
            }

            if (!(Session["username"] == null))
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("14 \n", w);
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
                            Logger.Log("15 \n", w);
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
                            Logger.Log("16 \n", w);
                        }

                        Response.Redirect("dashboard.aspx");
                        //Session.Clear();
                        //Common.Functions.ADFSSignout();
                    }

                    if (dtTemp.Rows.Count > 0)
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("17 \n", w);
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
                            Logger.Log("18 \n", w);
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
                        Logger.Log("19 \n", w);
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
                    Logger.Log("20 \n", w);
                }

                //Common.Functions.ADFSSignout();
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }

            if (!IsPostBack)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("21 \n", w);
                }

                //if (Request.QueryString["status"] != null)
                //{
                //    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                //    lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
                //    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                //}

                dtTemp = objSQL.DataTable_return("select ApprovalID from salesorderapprovalmaster where ApprovalEmail='" + Session["username"].ToString() + "';");
                if (dtTemp.Rows.Count > 0)
                    approverCode = dtTemp.Rows[0][0].ToString();
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
                    Session.Clear();
                    Response.Redirect("approverviewco.aspx");
                }
            }
        }

        protected void requestids_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_approvallist.DataSource = null;
            dtTemp = objSQL.DataTable_return("SELECT distinct t2.SPOCEmail,t3.Approval, t3.RPACLID,t2.ReqID,t2.CaseCreatedDate, t3.CustomerCode, " +
            "t3.CustomerName, t3.RequestType, t3.ProposedCreditLimitTemporary, t3.ValidityEndDate, " +
            "t3.ProposedTotalCreditLimit, t3.RegionName, t3.DealerType, t3.DOA_DOR, " +
            "t3.CategorySOP, t3.KanikaDealer, t3.CTSStatus, t3.CustomerStatus, t3.TotalCreditLimit, " +
            "t3.ParmanentCreditLimit, t3.TemporaryCreditLimit, t3.BankGurantee, t3.SD, " +
            "t3.SSD, t3.ChequeUnderClearance, t3.TotalOutstanding, t3.TotalDueAmt, " +
            "t3.TotalNotDue,t3.TotalSalesQty, " +
            "t3.TotalSalesAmt, t3.ProposedCreditLimitPermanent, t3.MinmumSD_1_Lac, t3.SDVsCL, " +
            "t3.SDVsCLTimes, t3.CLbucket, t3.PendingDiscountSouthOnly, t3.SalesValueClguide, " +
            "t3.SDSSD, t3.TotalECLSouth, t3.TotalECLWith150percent, t3.ValidFromDate, " +
            "t3.ValidToDate, t3.LiftedMonthOfSalesQty, t3.Zone, t3.CustGrp2, " +
            "t3.`CustomerGroup-1-Description`,t3.CustomerSegment,Case when PayTerms = 'ZS01' then 'PAYABLE IN 1 DAY' when PayTerms = 'ZS02' then 'PAYABLE IN 3 DAYS' " + "when PayTerms = 'ZS03' then 'PAYABLE IN 7 DAYS'  when PayTerms = 'ZS04' then 'PAYABLE IN 15 DAYS'  when PayTerms = 'ZS05' then " +
            "'PAYABLE IN 30 DAYS'  when PayTerms = 'ZS06' then 'PAYABLE IN 40 DAYS'  when  PayTerms = 'ZS07' then 'PAYABLE IN 10 DAYS' " + "when PayTerms = 'ZS08' then 'PAYABLE IN 35 DAYS'  when PayTerms = 'ZS19' then 'PAYABLE IN 21 DAYS'  when PayTerms = 'ZS20' " + "then 'PAYABLE IN 60  DAYS'  when PayTerms = 'ZS21' then 'PAYABLE IN 45  DAYS'  when PayTerms = 'ZS25' then " + "'PAYABLE IN 75  DAYS'  when PayTerms = 'ZS26' then 'PAYABLE IN 18 DAYS'  when PayTerms = 'ZS27' then 'PAYABLE IN 80 DAYS' " + "when PayTerms = 'ZS28' then 'PAYABLE IN 100 DAYS'  when PayTerms = 'ZS29' then 'PAYABLE IN 120 DAYS'  when PayTerms = 'Z001' " + "then 'Advance' end as PayTerms, " + "t3.PDAmountSouth, t3.PDCAmountNE, t3.SalesPromoter, t3.DealerSLAB, " +
            "t3.RiskClass, t3.`CLvsSD+SSDTimes` " +
            "FROM creditlimitapprovaltransaction t1 LEFT JOIN Creditlimitinput t2 ON t1.RPACLID = t2.RPACLID LEFT JOIN " +
            "CreditLimitProcessing t3 ON t3.RPACLID = t2.RPACLID WHERE t2.Status = 2 AND t1.Approval_Status = 1 " +
            "AND t1.Level = 1 AND t1.Approval_ID = " + approverCode + " AND t2.reqid = " + requestids.SelectedValue + "  order by t3.TotalDueAmt desc;  ");
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.DataBind();
        }

        protected void gv_approvallist_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

            //Response.Redirect("approverviewco.aspx?status=1");

            GetDataForApproval();
            setRequestIds();
            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
            lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
        }

        protected void GetDataForApproval()
        {
            dtTemp = objSQL.DataTable_return("SELECT distinct t2.SPOCEmail, t3.Approval,t3.RPACLID,t2.ReqID,t2.CaseCreatedDate, t3.CustomerCode, " +
            "t3.CustomerName, t3.RequestType, t3.ProposedCreditLimitTemporary, t3.ValidityEndDate, " +
            "t3.ProposedTotalCreditLimit, t3.RegionName, t3.DealerType, t3.DOA_DOR, " +
            "t3.CategorySOP, t3.KanikaDealer, t3.CTSStatus, t3.CustomerStatus, t3.TotalCreditLimit, " +
            "t3.ParmanentCreditLimit, t3.TemporaryCreditLimit, t3.BankGurantee, t3.SD, " +
            "t3.SSD, t3.ChequeUnderClearance, t3.TotalOutstanding, t3.TotalDueAmt, " +
            "t3.TotalNotDue,t3.TotalSalesQty, " +
            "t3.TotalSalesAmt, t3.ProposedCreditLimitPermanent, t3.MinmumSD_1_Lac, t3.SDVsCL, " +
            "t3.SDVsCLTimes, t3.CLbucket, t3.PendingDiscountSouthOnly, t3.SalesValueClguide, " +
            "t3.SDSSD, t3.TotalECLSouth, t3.TotalECLWith150percent, t3.ValidFromDate, " +
            "t3.ValidToDate, t3.LiftedMonthOfSalesQty, t3.Zone, t3.CustGrp2, " +
            "t3.`CustomerGroup-1-Description`,t3.CustomerSegment,Case when PayTerms = 'ZS01' then 'PAYABLE IN 1 DAY' when PayTerms = 'ZS02' then 'PAYABLE IN 3 DAYS' " + "when PayTerms = 'ZS03' then 'PAYABLE IN 7 DAYS'  when PayTerms = 'ZS04' then 'PAYABLE IN 15 DAYS'  when PayTerms = 'ZS05' then " +
            "'PAYABLE IN 30 DAYS'  when PayTerms = 'ZS06' then 'PAYABLE IN 40 DAYS'  when  PayTerms = 'ZS07' then 'PAYABLE IN 10 DAYS' " + "when PayTerms = 'ZS08' then 'PAYABLE IN 35 DAYS'  when PayTerms = 'ZS19' then 'PAYABLE IN 21 DAYS'  when PayTerms = 'ZS20' " + "then 'PAYABLE IN 60  DAYS'  when PayTerms = 'ZS21' then 'PAYABLE IN 45  DAYS'  when PayTerms = 'ZS25' then " + "'PAYABLE IN 75  DAYS'  when PayTerms = 'ZS26' then 'PAYABLE IN 18 DAYS'  when PayTerms = 'ZS27' then 'PAYABLE IN 80 DAYS' " + "when PayTerms = 'ZS28' then 'PAYABLE IN 100 DAYS'  when PayTerms = 'ZS29' then 'PAYABLE IN 120 DAYS'  when PayTerms = 'Z001' " + "then 'Advance' end as PayTerms, " + "t3.PDAmountSouth, t3.PDCAmountNE, t3.SalesPromoter, t3.DealerSLAB, " +
            "t3.RiskClass, t3.`CLvsSD+SSDTimes` " +
            "FROM creditlimitapprovaltransaction t1 LEFT JOIN Creditlimitinput t2 ON t1.RPACLID = t2.RPACLID LEFT JOIN " +
            "CreditLimitProcessing t3 ON t3.RPACLID = t2.RPACLID WHERE t2.Status = 2 AND t1.Approval_Status = 1 " +
            "AND t1.Level = 1 AND t1.Approval_ID = " + approverCode + "   order by t3.TotalDueAmt desc; ");
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.DataBind();

        }

        protected void ApproveRequests()
        {
            try
            {
                objSQL.ExecuteNonQuery_IUD("update creditlimitapprovaltransaction set Approval_Status=2,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where level=1 and Approval_Status=1 and Approval_Id=" + approverCode + " and RPACLID in (" + strApproveList + "); ");
                foreach (string strCCI in strApproveList.Split(','))
                {
                    dtTemp = objSQL.DataTable_return("select * from creditlimitapprovaltransaction where level=0 and Approval_Status=0 and RPACLID=" + strCCI + " order by Priority limit 1;");
                    dtTemp1 = objSQL.DataTable_return("select * from creditlimitapprovaltransaction where level=1 and Approval_Status=1 and RPACLID=" + strCCI + " order by Priority limit 1;");
                    if (dtTemp1.Rows.Count > 0)
                    {
                        // objSQL.ExecuteNonQuery_IUD("update salesorderapprovaltransaction set level = 1 where TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else if (dtTemp.Rows.Count > 0)
                    {
                        objSQL.ExecuteNonQuery_IUD("update creditlimitapprovaltransaction set level = 1 where TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else
                    {
                        objSQL.ExecuteNonQuery_IUD("update creditlimitinput set Status=3,`ApproverResponseDate`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where RPACLID= " + strCCI + " and status=2;");
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
                objSQL.ExecuteNonQuery_IUD("update creditlimitapprovaltransaction set Approval_Status=3,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where level=1 and Approval_Status=1 and Approval_Id=" + approverCode + " and RPACLID in (" + strRejectList + "); ");
                foreach (string strCCI in strRejectList.Split(','))
                {
                    objSQL.ExecuteNonQuery_IUD("update creditlimitinput set Status=4,ApproverResponseDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', Remarks='Fail',Description='Rejection from Approver', CaseProcessedDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where RPACLID= " + strCCI + " and status=2;");
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
                dtTemp = objSQL.DataTable_return("select distinct(ReqID) from creditlimitinput where RPACLID in (select RPACLID from creditlimitapprovaltransaction where Approval_ID=" + approverCode + " and Approval_Status=1) and Status=2;");
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

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }
    }
}