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
    public partial class contractextension : System.Web.UI.UserControl
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static string strApproveList = string.Empty;
        static string strRejectList = string.Empty;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;
        string ContractFlag = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com";
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;
                    
            if (!(Session["username"] == null))
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log("Line no 35 contrat extension \n", w);
                }
                try
                {
                    //username.InnerText = Session["username"].ToString(); //zaheer
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
                catch(Exception ex)
                {
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log(ex.Message, w);
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

                //if (Request.QueryString["status"] != null)
                //{
                //    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                //    lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
                //    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                //}

                //added by zaheer
                ContractFlag = "E";

                dtTemp = objSQL.DataTable_return("select distinct ApprovalID from ntcontractupd_approvalmaster where ContractFlag = '" + ContractFlag + "' and ApprovalEmail='" + Session["username"].ToString() + "';");                
                if (dtTemp.Rows.Count > 0)
                    approverCode = dtTemp.Rows[0][0].ToString(); //"10065";
                else
                    approverCode = "1036543";

                //setRequestIds();

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
                                strApproveList = gv_approvallist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                            else
                                strApproveList = strApproveList + "," + gv_approvallist.DataKeys[gr.RowIndex].Value.ToString();
                        }

                        CheckBox checkR = (CheckBox)gr.FindControl("chkRR");
                        if (checkR.Checked)
                        {
                            if (strRejectList == string.Empty)
                                strRejectList = gv_approvallist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                            else
                                strRejectList = strRejectList + "," + gv_approvallist.DataKeys[gr.RowIndex].Value.ToString();
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
            ContractFlag = "E";
            try
            {                
                objSQL.ExecuteNonQuery_IUD("update ntcontractupd_approvaltransaction set Approval_Status=2, ApproverResponseDate=now() where level=1 and Approval_Status in (0,1) and ContractFlag = '" + ContractFlag + "' and Approval_Id=" + approverCode + " and ContractID in (" + strApproveList + "); ");
                //END
                foreach (string strCCI in strApproveList.Split(','))
                {
                    dtTemp = objSQL.DataTable_return("select * from ntcontractupd_approvaltransaction where level=0 and Approval_Status=0 and ContractFlag = '" + ContractFlag + "' and ContractID=" + strCCI + " order by Priority limit 1;");
                    if (dtTemp.Rows.Count > 0)
                    {
                        objSQL.ExecuteNonQuery_IUD("update ntcontractupd_approvaltransaction set  Approval_Status=0, level = 1 where ContractFlag = '" + ContractFlag + "' and TransactionID =" + dtTemp.Rows[0]["TransactionID"] + ";");
                    }
                    else
                    {
                        objSQL.ExecuteNonQuery_IUD("update ntcontractupd_input set Status=5,ApproverResponseDate=now(),remark='Approved',description='Approved by approver' where ContractFlag = '" + ContractFlag + "' and ContractID= " + strCCI + " and Status in (2,4);");
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
            ContractFlag = "E";
            try
            {
                objSQL.ExecuteNonQuery_IUD("update ntcontractupd_approvaltransaction set Approval_Status=3, ApproverResponseDate=now() where level=1 and Approval_Status in (0,1) and ContractFlag = '" + ContractFlag + "' and Approval_Id=" + approverCode + " and ContractID in (" + strRejectList + "); ");
                foreach (string strCCI in strRejectList.Split(','))
                    objSQL.ExecuteNonQuery_IUD("update ntcontractupd_input set Status=7, ApproverResponseDate=now(), CaseProcessedDate=now(), remark='Fail',description='Rejected by approver' where ContractFlag = '" + ContractFlag + "' and ContractID= " + strCCI + " and status in (2,4);");
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
        protected void GetDataForApproval()
        {            
           
            dtTemp = objSQL.DataTable_return("SELECT distinct cc_appro.ApprovalEmail,ncc.ReqID,ncc.ContractID,cc_appro.ApprovalID,ncc.zone,ncc.SalesOrg,ncc.ContractNo,ncc.ContractExtendedDate,ncc.FloorPrice,ncc.ContractRemarks FROM ntcontractupd_input ncc inner join ntcontractupd_approvaltransaction cc_appro on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID = ncc.ReqID and ncc.ContractFlag = '" + ContractFlag + "' where cc_appro.Level = 1 and cc_appro.ApprovalID = '" + approverCode + "' and ncc.Status in (2,4);");
            
            if(dtTemp.Rows.Count < 1)
            {
                dvCExtension.Visible = true;
            }
            gv_approvallist.DataSource = dtTemp;
            gv_approvallist.AutoGenerateColumns = false;
            gv_approvallist.DataBind();

            var dtRqIds = dtTemp.Clone();
            var addedRows = new List<string>();
            foreach (DataRow drtableOld in dtTemp.Rows)
            {
                if (addedRows.FirstOrDefault(x => x == drtableOld["ReqID"].ToString()) == null)
                {
                    addedRows.Add(drtableOld["ReqID"].ToString());
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
        protected void requestids_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ContractFlag = "E";

            gv_approvallist.DataSource = null;
            if (requestids.SelectedValue == "0")
                GetDataForApproval();
            else
            {
                dtTemp = objSQL.DataTable_return("SELECT distinct cc_appro.ApprovalEmail,ncc.ReqID,ncc.ContractID,cc_appro.ApprovalID,ncc.zone,ncc.SalesOrg,ncc.ContractNo,ncc.ContractExtendedDate,ncc.FloorPrice,ncc.ContractRemarks FROM ntcontractupd_input ncc inner join ntcontractupd_approvaltransaction cc_appro on cc_appro.ContractID = ncc.ContractID and cc_appro.ReqID = ncc.ReqID and ncc.ContractFlag = '" + ContractFlag + "' where cc_appro.Level = 1 and ncc.ReqId='" + requestids.SelectedValue + "' and cc_appro.ApprovalID = '" + approverCode + "' and ncc.Status in (2,4);");
              
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