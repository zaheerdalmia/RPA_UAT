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
using rpa_mazar.Common;

namespace rpa_mazar
{
    public partial class RouteApprover : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string strApproveList = string.Empty;
        static string strRejectList = string.Empty;
        static bool IsPageAllowed = false;
        // string approverCode = "";
        //public StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true);
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com";
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;

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
                    //dtTemp = AppUser.AccessModules(Session["username"].ToString(), spoMasterTable: "routespocmaster", salesOrderApprovalMaster: "routeapprovalmaster");
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
                        //Response.Redirect("Dashboard.aspx");
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
                    Logger.Log("28 \n", w);
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

                GetDataForApproval();
                //dtTemp = objSQL.DataTable_return("select ApprovalID from routeapprovalmaster where ApprovalEmail='" + Session["username"].ToString() + "';");
                //if (dtTemp.Rows.Count > 0)
                //    approverCode = dtTemp.Rows[0][0].ToString(); //"10065";
                //else
                //    approverCode = "10065";

                ////setRequestIds();

                //if (approverCode != string.Empty)
                //{
                //    GetDataForApproval();
                //}
                //else
                //{
                //    Response.Redirect("dashboard.aspx");
                //}
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
            //dtTemp = objSQL.DataTable_return("SELECT distinct ncc.requestor_email,ncc.ReqID,ncc.ContractID ,ncc.Consider,cc_appro.Designation,ncc.zone,ncc.Region,ncc.Customer_Code,ncc.Customer_Name,ncc.Consignee_Code ,ncc.Payment_Terms_No_of_Days,ncc.qty, ncc.Cement_Type, ncc.`Source/Code` , ncc.`EX/FOR/FPD` , ncc.Floor_Price_Check, ncc.`Above/Below_Floor_Price`, ncc.is_price, ncc.trade_price, ncc.diferrence, ncc.is_ncr, ncc.VC, ncc.Contribution_PB_if_Any, ncc.IA_Comm, ncc.Validity_To, ncc.PO_No, ncc.PO_Date, ncc.FilePath FROM non_trade_price_contract_creation ncc inner join cc_approvaltransaction cc_appro on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID = ncc.ReqID where cc_appro.Approval_Status = 1 and cc_appro.Level = 1 and cc_appro.Approval_ID = '" + approverCode + "' and ncc.Status = 4;");
            dtTemp = objSQL.DataTable_return("SELECT distinct t2.TransactionID, t1.`RPAID`, t1.spocemail as 'SPOC',t1.`REQID`,t1.`ZONE`,t1.`STATE_PLANT`,t1.`SUPZONE`,t1.`RECEIVINGTZONE`,t1.`MODEOFTRANSPORT`,t1.`DISTANCE`, t1.`TRANSITTIME`,t1.`ROUTEDESCRIPTION`,t1.`ROUTEIDENTIFICATION`,t1.`COMMENTS` FROM `routeuserinput` t1 inner join `routeapprovaltransaction` t2 on t1.RPAID = t2.RPAID where t2.ApprovalStatus in (0,1) and level = 1 and t1.Status=2 and t2.ApprovalEmail='" + Convert.ToString(Session["username"]) + "'");
            // End
            //dtTemp = objSQL.DataTable_return("SELECT * FROM non_trade_price_contract_creation ncc  inner join  cc_approvaltransaction cc_appro on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID=ncc.ReqID where cc_appro.Approval_Status = 1 and cc_appro.Level = 1 and cc_appro.Approval_ID='" + approverCode + "' and ncc.Status=4;");
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.AutoGenerateColumns = false;
            gv_approvallist.DataBind();

            var dtRqIds = dtTemp.Clone();
            var addedRows = new List<string>();
            foreach (DataRow drtableOld in dtTemp.Rows)
            {
                if (addedRows.FirstOrDefault(x => x == drtableOld["ReqID"].ToString()) == null)
                {
                    addedRows.Add(drtableOld["REQID"].ToString());
                    dtRqIds.ImportRow(drtableOld);
                }
            }

            requestids.DataSource = dtRqIds;
            requestids.DataMember = "ReqID";
            requestids.DataValueField = "ReqID";
            requestids.DataBind();

            var li = new ListItem();
            li.Text = "--- Select Request Id ---";
            li.Value = "0";
            requestids.Items.Insert(0, li);
        }

        protected void btn_SubmitData_ServerClick(object sender, EventArgs e)
        {
            strApproveList = string.Empty;
            strRejectList = string.Empty;

            try
            {

                foreach (GridViewRow gr in gv_approvallist.Rows)
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox checkA = (CheckBox)gr.FindControl("chkAR");
                        if (checkA.Checked)
                        {
                            if (strApproveList == string.Empty)
                                //strApproveList = gv_approvallist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                                strApproveList = gr.Cells[4].Text;// gr.Cells[1].Text;

                            else
                                strApproveList = strApproveList + "," + gr.Cells[4].Text;
                        }

                        CheckBox checkR = (CheckBox)gr.FindControl("chkRR");
                        if (checkR.Checked)
                        {
                            if (strRejectList == string.Empty)
                                strRejectList = gr.Cells[4].Text;// gr.Cells[1].Text;
                            else
                                strRejectList = strRejectList + "," + gr.Cells[4].Text;
                        }
                    }
                }
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
            // setRequestIds();

            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
            lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
        }

        protected void ApproveRequests()
        {
            try
            {
                //17-Jan-2022 Start
                //objSQL.ExecuteNonQuery_IUD("update cc_approvaltransaction set Approval_Status=2, ApproverResponseDate=now() where level=1 and Approval_Status=1 and Approval_Id=" + approverCode + " and ContractID in (" + strApproveList + "); ");
                objSQL.ExecuteNonQuery_IUD("update routeapprovaltransaction set APPROVALSTATUS=2, APPROVALRESPONSEDATE=now() where LEVEL=1 and APPROVALSTATUS in (0,1) and APPROVALEMAIL='" + Convert.ToString(Session["username"]) + "' and RPAID in (" + strApproveList + "); ");
                //END
                foreach (string strRPAID in strApproveList.Split(','))
                {
                    dtTemp = objSQL.DataTable_return("select * from routeapprovaltransaction where level=0 and APPROVALSTATUS=0 and RPAID=" + strRPAID + ";");
                    if (dtTemp.Rows.Count > 0)
                    {
                        objSQL.ExecuteNonQuery_IUD("update routeapprovaltransaction set level = 1 where TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else
                    {
                        objSQL.ExecuteNonQuery_IUD("update routeuserinput set Status=3,ApproverResponseDate=now(),remarks='Approved',description='Approved by approver' where RPAID= " + strRPAID + " and Status = 2 ;");
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
                objSQL.ExecuteNonQuery_IUD("update routeapprovaltransaction set APPROVALSTATUS=3, APPROVALRESPONSEDATE=now() where LEVEL=1 and APPROVALSTATUS in (0,1) and APPROVALEMAIL='" + Convert.ToString(Session["username"]) + "' and RPAID in (" + strRejectList + "); ");

                //objSQL.ExecuteNonQuery_IUD("update cc_approvaltransaction set Approval_Status=3, ApproverResponseDate=now() where level=1 and Approval_Status in (0,1) and Approval_Id=" + approverCode + " and ContractID in (" + strRejectList + "); ");
                foreach (string strRPAID in strRejectList.Split(','))
                    objSQL.ExecuteNonQuery_IUD("update routeuserinput set Status=6, ApproverResponseDate=now(), ProcessedDate=now(), remarks='Fail',description='Rejected by approver' where RPAID= " + strRPAID + " and status = 2 ;");
                
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

        protected void requestids_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_approvallist.DataSource = null;
            if (requestids.SelectedValue == "0")
                GetDataForApproval();
            else
            {
                //17-Jan-22 Start
                //dtTemp = objSQL.DataTable_return("SELECT distinct ncc.requestor_email,ncc.ReqID,ncc.ContractID ,ncc.Consider,cc_appro.Designation,ncc.zone,ncc.Region,ncc.Customer_Code,ncc.Customer_Name,ncc.Consignee_Code ,ncc.Payment_Terms_No_of_Days,ncc.qty, ncc.Cement_Type, ncc.`Source/Code` , ncc.`EX/FOR/FPD` , ncc.Floor_Price_Check, ncc.`Above/Below_Floor_Price`, ncc.is_price, ncc.trade_price, ncc.diferrence, ncc.is_ncr, ncc.VC, ncc.Contribution_PB_if_Any, ncc.IA_Comm, ncc.Validity_To, ncc.PO_No, ncc.PO_Date, ncc.FilePath FROM non_trade_price_contract_creation ncc  inner join  cc_approvaltransaction cc_appro on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID=ncc.ReqID where cc_appro.Approval_Status = 1 and cc_appro.Level = 1 and ncc.ReqId='" + requestids.SelectedValue + "' and cc_appro.Approval_ID='" + approverCode + "' and ncc.Status=4;");
                dtTemp = objSQL.DataTable_return("SELECT distinct t2.TransactionID,t1.`RPAID`,t1.spocemail as 'SPOC',t1.`REQID`,t1.`ZONE`,t1.`STATE_PLANT`,t1.`SUPZONE`,t1.`RECEIVINGTZONE`,t1.`MODEOFTRANSPORT`,t1.`DISTANCE`, t1.`TRANSITTIME`,t1.`ROUTEDESCRIPTION`,t1.`ROUTEIDENTIFICATION`,t1.`COMMENTS` FROM `routeuserinput` t1 inner join `routeapprovaltransaction` t2 on t1.RPAID = t2.RPAID where t2.ApprovalStatus in (0,1) and level = 1 and t1.Status=2 and t2.ApprovalEmail='" + Convert.ToString(Session["username"]) + "' and t1.`REQID`='" + requestids.SelectedValue + "'");
                //END

                gv_approvallist.DataSource = dtTemp;
                gv_approvallist.DataBind();
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