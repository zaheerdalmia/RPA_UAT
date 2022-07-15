using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace rpa_mazar
{
    public partial class contractcreationapprover : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static DataTable dtTemp1 = null;
        static string strApproveList = string.Empty;
        static string strRejectList = string.Empty;
        static string approverCode = string.Empty;
        static bool IsPageAllowed = false;

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
                        Session.Clear();
                        Common.Functions.ADFSSignout();
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
                catch (Exception ex)
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

                approverCode = dtTemp.Rows[0][0].ToString(); //"10065";
                //else
                //approverCode = "";
                if (approverCode != string.Empty)
                {
                    GetInputDataForApproval();
                    GetOCRDataForApproval();
                    GetAllDataForDisplay();
                }
                else
                {
                    //Common.Functions.ADFSSignout();
                    //Session.Clear();
                    Response.Redirect("dashboard.aspx");
                }
            }
        }


        protected void gv_inputlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GetInputDataForApproval();
            gv_inputlist.PageIndex = e.NewPageIndex;
            gv_inputlist.DataBind();
        }

        protected void GetInputDataForApproval()
        {
            dtTemp = objSQL.DataTable_return("SELECT ContractID,ReqID,Region,Sales_Org,Customer_Name,Customer_Code," +
                                        "Consignee_Code,Qty,`Last_unused_contract/PO_quantity`,Last_PO_price,`Current_status_of_that_contract_Active/Blocked/Expired`," +
                                        "Cement_Type,`Source/Code`,Deatination,`EX/FOR/FPD`,IS_Price,Trade_Price,Diferrence,`Loading_/_Unloading_Charges`,IS_NCR,VC," +
                                        "Contribution_PB_if_Any,IA_Name,IA_Comm,Validity_From,Validity_To,Payment_Terms_No_of_Days,PO_No,PO_Date," +
                                        "Material,Material_type,Plant,Order_reason,Floor_Price_Check,`Above/Below_Floor_Price`,Consider,Status,ApprovedByExcel FROM non_trade_price_contract_creation WHERE Status=2;");
            gv_inputlist.DataSource = dtTemp;
            gv_inputlist.DataBind();

            dtTemp1 = objSQL.DataTable_return("SELECT ContractID,FilePath FROM non_trade_price_contract_creation WHERE Status=2;");
            DataView view = new DataView(dtTemp1);
            gv_pdf.AutoGenerateColumns = false;
            DataTable resultTable = view.ToTable(true,"FilePath");
            gv_pdf.DataSource = dtTemp1;
            gv_pdf.DataBind();

        }

       

        protected void GetOCRDataForApproval()
        {
            dtTemp = objSQL.DataTable_return("SELECT ContractID,ReqID,OCRQty,OCRPrice,OCRPoNumber,OCRPoDate,Status,ApprovedByOCR FROM non_trade_price_contract_creation WHERE Status=2;");
            gv_ocrlist.DataSource = dtTemp;
            gv_ocrlist.DataBind();
        }

        protected void GetAllDataForDisplay()
        {
            dtTemp = objSQL.DataTable_return("SELECT * FROM non_trade_price_contract_creation");
            gv_allrecords.DataSource = dtTemp;
            gv_allrecords.DataBind();
        }

        protected void btnInputDataApproval_Click(object sender, EventArgs e)
        {
            strApproveList = string.Empty;
          

            foreach (GridViewRow gr in gv_inputlist.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox checkA = (CheckBox)gr.FindControl("chkAR");
                    if (checkA.Checked)
                    {
                        if (strApproveList == string.Empty)
                            strApproveList = gv_inputlist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                        else
                            strApproveList = strApproveList + "," + gv_inputlist.DataKeys[gr.RowIndex].Value.ToString();
                    }
                }
            }

            if (strApproveList != string.Empty)
            {
                ApproveRequests(1);
            }

            GetInputDataForApproval();
            GetOCRDataForApproval();
            GetAllDataForDisplay();

        }


        protected void btnOCRDataApproval_Click(object sender, EventArgs e)
        {
            strApproveList = string.Empty;
           

            foreach (GridViewRow gr in gv_ocrlist.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox checkA = (CheckBox)gr.FindControl("chkAR");
                    if (checkA.Checked)
                    {
                        if (strApproveList == string.Empty)
                            strApproveList = gv_ocrlist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                        else
                            strApproveList = strApproveList + "," + gv_ocrlist.DataKeys[gr.RowIndex].Value.ToString();
                    }
                }
            }

            if (strApproveList != string.Empty)
            {
                ApproveRequests(2);
            }

            GetInputDataForApproval();
            GetOCRDataForApproval();
            GetAllDataForDisplay();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
           
            strRejectList = string.Empty;

            foreach (GridViewRow gr in gv_inputlist.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox checkA = (CheckBox)gr.FindControl("chkAR");
                    if (checkA.Checked)
                    {
                        if (strRejectList == string.Empty)
                            strRejectList = gv_inputlist.DataKeys[gr.RowIndex].Value.ToString(); // gr.Cells[1].Text;
                        else
                            strRejectList = strRejectList + "," + gv_inputlist.DataKeys[gr.RowIndex].Value.ToString();
                    }
                }
            }

            if (strRejectList != string.Empty)
            {
                RejectRequests();
            }

            GetInputDataForApproval();
            GetOCRDataForApproval();
            GetAllDataForDisplay();
       }

        protected void ApproveRequests(int flag)
        {
            try
            {
                if (flag == 1) //Input data
                {
                    objSQL.ExecuteNonQuery_IUD("UPDATE non_trade_price_contract_creation SET Status='3', ApprovedByExcel=1, ApprovedByOCR=0, remark='Pass',description='Approved from Requestor.' WHERE  ContractId IN (" + strApproveList + "); ");
                    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                    lbl_Message.InnerText = "Yaay! Input data approved successfully";
                }
                else // flag==2 OCR data
                {
                    objSQL.ExecuteNonQuery_IUD("UPDATE non_trade_price_contract_creation SET Status='3', ApprovedByExcel=0, ApprovedByOCR=1, remark='Pass',description='Approved from Requestor.' WHERE  ContractId IN (" + strApproveList + "); ");
                    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                    lbl_Message.InnerText = "Yaay! OCR data approved successfully";
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
        }

        protected void RejectRequests()
        {
            try
            {
                objSQL.ExecuteNonQuery_IUD("UPDATE non_trade_price_contract_creation SET Status='7', ApprovedByExcel=0, ApprovedByOCR=0, remark='Rejected', description='Rejection from Requestor.' WHERE  ContractId IN (" + strRejectList + "); ");
                lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                lbl_Message.InnerText = "All the records rejected successfully.";
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



        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }
    }
}