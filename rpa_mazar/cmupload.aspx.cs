using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace rpa_mazar
{
    public partial class cmupload : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static DataTable dtInputExcel = null;
        static string bulkInsert = string.Empty;
        static int currentReqId = 0;
        static string RequestType = string.Empty;
        static int currentrequestscount = 0;
        static bool IsPageAllowed = false;
        static string currentUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com"; //zaheer
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;


            if (!(Session["username"] == null))
            {
                try
                {
                    username.InnerText = Session["username"].ToString();
                    #region UserAccess Check
                    try
                    {
                        dtTemp = Common.AppUser.AccessModules(Session["username"].ToString());
                        if (dtTemp.Rows.Count > 0)
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("Entered line No 42 Cm Upload", w);
                            }

                            IsPageAllowed = false;
                            foreach (DataRow dr in dtTemp.Rows)
                            {
                                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                {
                                    Logger.Log("Inside Foreach line No 55 Cm Upload", w);
                                }
                                if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("?"))
                                {
                                    //IsPageAllowed = true;
                                    currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0];
                                }
                                else
                                    currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;


                                if (currentUrl.Substring(currentUrl.LastIndexOf("/") + 1, currentUrl.Length - currentUrl.LastIndexOf("/") - 1) == dr["modulepagename"].ToString())
                                {
                                    IsPageAllowed = true;
                                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                    {
                                        Logger.Log("IsPageAllowed= true line No 71 Cm Upload", w);
                                    }
                                }
                            }
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("Ouside Foreach line No 74 Cm Upload", w);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {                            
                            Logger.Log(ex.Message, w);
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
                    //Common.Functions.ADFSSignout();
                    Response.Redirect("login.aspx");
                }
            }
            else
            {
                //Common.Functions.ADFSSignout();
                Session.Clear();
                Response.Redirect("login.aspx");
            }
            
            if (!IsPostBack)
            {
                if (Request.QueryString["rid"] != null)
                {
                    requestid.InnerText = Request.QueryString["rid"].ToString();
                    requestcount.InnerText = Request.QueryString["cnt"].ToString();

                    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                    lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";
                    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                }
            }
            //GetTop20();
            try
            {
                //if (rd_ce.Checked == true)
                //    RequestType = "E";
                //if (rd_csc.Checked == true)
                //    RequestType = "SC";
                //if (rd_plant.Checked == true)
                //    RequestType = "P";
                //if (rd_material.Checked == true)
                //    RequestType = "M";
                //if (rd_master.Checked == true)
                //    RequestType = "LT";

                //totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where ContractFlag = '" + RequestType + "' and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                //totalapproved.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where  ContractFlag = '" + RequestType + "' and SPOCEmail='" + Session["username"].ToString() + "' and Status=5;").Rows[0][0].ToString();
                //totalrejected.InnerHtml = objSQL.DataTable_return("select count(*) from ntcontractupd_input where  ContractFlag = '" + RequestType + "' and SPOCEmail='" + Session["username"].ToString() + "' and Status=7;").Rows[0][0].ToString();
            }
            catch { }

            
        }

        protected void btn_upload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lbl_Message.InnerText = string.Empty;
                currentrequestscount = 0;
                if (fileupload.PostedFile.FileName != string.Empty)
                {
                    if (ReadExcelToDataTable())
                    {
                        //IsCalidContractNumber()
                        if (validateInputExcelColumns())
                        {
                            if (dtInputExcel.Rows.Count > 0)
                            {
                                #region Set Request Id
                                dtTemp = objSQL.DataTable_return("select max(ReqID) as lastReqId from ntcontractupd_input;");

                                if (dtTemp.Rows[0]["lastReqId"] != System.DBNull.Value)
                                {
                                    if (dtTemp.Rows.Count > 0)
                                        currentReqId = Convert.ToInt32(dtTemp.Rows[0]["lastReqId"]) + 1;
                                    else
                                        currentReqId = 1;
                                }
                                else
                                {
                                    currentReqId = 1;
                                }
                                #endregion

                                if (rd_ce.Checked == true)
                                    RequestType = "E";
                                if (rd_csc.Checked == true)
                                    RequestType = "SC";
                                if (rd_plant.Checked == true)
                                    RequestType = "P";
                                if (rd_material.Checked == true)
                                    RequestType = "M";
                                if (rd_master.Checked == true)
                                    RequestType = "LT";

                                foreach (DataRow dr in dtInputExcel.Rows)
                                {
                                    //Contract Extension
                                    if (RequestType == "E")
                                    {
                                        try
                                        {
                                            //Zaheer Start Code
                                            string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log("NTContractUpd_UserInputFiles", w);
                                            }

                                            using (MySqlConnection con = new MySqlConnection(connect))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand("NTContractUpd_UserInputFiles", con))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;                                                   
                                                    cmd.Parameters.AddWithValue("@vReqID", currentReqId);
                                                    cmd.Parameters.AddWithValue("@vUserInputFile", RequestType);                                                 
                                                    cmd.Parameters.AddWithValue("@vZone", dr["Zone"]);
                                                    cmd.Parameters.AddWithValue("@vSalesOrg", dr["Sales_Org"]);
                                                    cmd.Parameters.AddWithValue("@vContractNo", dr["Contract_No"]);
                                                    cmd.Parameters.AddWithValue("@vContractExtendedDate", dr["Contract_Extended_Date"]);
                                                    cmd.Parameters.AddWithValue("@vFloorPrice", dr["Current_Floor_Price"]);
                                                    cmd.Parameters.AddWithValue("@vContractType", dr["Contract_Type"]);                                                   
                                                    cmd.Parameters.AddWithValue("@vContractRemarks", dr["Remark"]);

                                                    cmd.Parameters.AddWithValue("@vLT_ISPrice", ""); 
                                                    cmd.Parameters.AddWithValue("@vCustCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vCustName", "");  
                                                    cmd.Parameters.AddWithValue("@vBalContractQty", "");  
                                                    cmd.Parameters.AddWithValue("@vContractPrice", "");  
                                                    cmd.Parameters.AddWithValue("@vReasonOfRejection", "");  
                                                    cmd.Parameters.AddWithValue("@vExistingPlantCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vModifiedPlantCode", "");  
                                                    cmd.Parameters.AddWithValue("@vExistingMaterialCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vModifiedMaterialCode", "");  
                                                    cmd.Parameters.AddWithValue("@vSLNo", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vCustGrpDescrip", ""); 
                                                    cmd.Parameters.AddWithValue("@vValidityFrom", ""); 
                                                    cmd.Parameters.AddWithValue("@vValidityTo", "");  
                                                    cmd.Parameters.AddWithValue("@vSpocEmail", Convert.ToString(Session["username"]));  
                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            //Zaheer End Code

                                            currentrequestscount++;

                                            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                                            lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";

                                            requestid.InnerText = currentReqId.ToString();
                                            requestcount.InnerText = currentrequestscount.ToString();

                                        }
                                        catch (Exception ex)
                                        {
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log(ex.StackTrace, w);
                                            }
                                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                                        }
                                    }
                                    
                                    //Contract Short Closer
                                    if (RequestType == "SC")
                                    {
                                        try
                                        {
                                            //Zaheer Start Code
                                            string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log("NTContractUpd_UserInputFiles", w);
                                            }

                                            using (MySqlConnection con = new MySqlConnection(connect))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand("NTContractUpd_UserInputFiles", con))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@vReqID", currentReqId);
                                                    cmd.Parameters.AddWithValue("@vUserInputFile", RequestType);
                                                    cmd.Parameters.AddWithValue("@vZone", dr["Zone"]);                                                   
                                                    cmd.Parameters.AddWithValue("@vContractNo", dr["Contract_No"]); 
                                                    cmd.Parameters.AddWithValue("@vCustCode", dr["Customer_Code"]);
                                                    cmd.Parameters.AddWithValue("@vCustName", dr["Customer_Name"]);
                                                    cmd.Parameters.AddWithValue("@vBalContractQty", dr["Balance_Contract_Quantity"]);
                                                    cmd.Parameters.AddWithValue("@vContractPrice", dr["Price"]);
                                                    cmd.Parameters.AddWithValue("@vReasonOfRejection", dr["Reason_of_Rejection"]);
                                                    cmd.Parameters.AddWithValue("@vContractRemarks", dr["Remark"]);
                                                                                                    
                                                    cmd.Parameters.AddWithValue("@vSalesOrg", "");
                                                    cmd.Parameters.AddWithValue("@vContractExtendedDate", "");
                                                    cmd.Parameters.AddWithValue("@vFloorPrice", "");
                                                    cmd.Parameters.AddWithValue("@vLT_ISPrice", ""); 
                                                    cmd.Parameters.AddWithValue("@vExistingPlantCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vModifiedPlantCode", "");  
                                                    cmd.Parameters.AddWithValue("@vExistingMaterialCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vModifiedMaterialCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vSLNo", ""); 
                                                    cmd.Parameters.AddWithValue("@vCustGrpCode", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpDescrip", "");  
                                                    cmd.Parameters.AddWithValue("@vValidityFrom", "");  
                                                    cmd.Parameters.AddWithValue("@vValidityTo", "");  
                                                    cmd.Parameters.AddWithValue("@vSpocEmail", Convert.ToString(Session["username"]));  
                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            //Zaheer End Code

                                            currentrequestscount++;

                                            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                                            lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";

                                            requestid.InnerText = currentReqId.ToString();
                                            requestcount.InnerText = currentrequestscount.ToString();

                                        }
                                        catch (Exception ex)
                                        {
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log(ex.StackTrace, w);
                                            }
                                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                                        }
                                    }

                                    //Contract Modification Plant 
                                    if (RequestType == "P")
                                    {
                                        try
                                        {
                                            //Zaheer Start Code
                                            string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log("NTContractUpd_UserInputFiles", w);
                                            }

                                            using (MySqlConnection con = new MySqlConnection(connect))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand("NTContractUpd_UserInputFiles", con))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@vReqID", currentReqId);
                                                    cmd.Parameters.AddWithValue("@vUserInputFile", RequestType);
                                                    cmd.Parameters.AddWithValue("@vZone", dr["Zone"]);
                                                    cmd.Parameters.AddWithValue("@vContractNo", dr["Contract_No"]); 
                                                    cmd.Parameters.AddWithValue("@vContractPrice", dr["Contract_Price"]);
                                                    cmd.Parameters.AddWithValue("@vModifiedPlantCode", dr["Modified_Plant"]);
                                                    cmd.Parameters.AddWithValue("@vExistingPlantCode", dr["Existing_plant"]);                                           
                                                    cmd.Parameters.AddWithValue("@vContractRemarks", dr["Remark"]);                                                   
                                                   
                                                    cmd.Parameters.AddWithValue("@vCustCode", "");
                                                    cmd.Parameters.AddWithValue("@vCustName","");
                                                    cmd.Parameters.AddWithValue("@vBalContractQty","");                                                    
                                                    cmd.Parameters.AddWithValue("@vReasonOfRejection", "");
                                                    cmd.Parameters.AddWithValue("@vSalesOrg", "");
                                                    cmd.Parameters.AddWithValue("@vContractExtendedDate", "");
                                                    cmd.Parameters.AddWithValue("@vFloorPrice", "");
                                                    cmd.Parameters.AddWithValue("@vLT_ISPrice", "");                                                  
                                                    cmd.Parameters.AddWithValue("@vExistingMaterialCode", "");  
                                                    cmd.Parameters.AddWithValue("@vModifiedMaterialCode", "");  
                                                    cmd.Parameters.AddWithValue("@vSLNo", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpCode", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpDescrip", "");  
                                                    cmd.Parameters.AddWithValue("@vValidityFrom", ""); 
                                                    cmd.Parameters.AddWithValue("@vValidityTo", "");  
                                                    cmd.Parameters.AddWithValue("@vSpocEmail", Convert.ToString(Session["username"]));  
                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            //Zaheer End Code

                                            currentrequestscount++;

                                            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                                            lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";

                                            requestid.InnerText = currentReqId.ToString();
                                            requestcount.InnerText = currentrequestscount.ToString();

                                        }
                                        catch (Exception ex)
                                        {
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log(ex.StackTrace, w);
                                            }
                                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                                        }
                                    }

                                    //Contract Modification Material 
                                    if (RequestType == "M")
                                    {
                                        try
                                        {
                                            //Zaheer Start Code
                                            string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log("NTContractUpd_UserInputFiles", w);
                                            }

                                            using (MySqlConnection con = new MySqlConnection(connect))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand("NTContractUpd_UserInputFiles", con))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@vReqID", currentReqId);
                                                    cmd.Parameters.AddWithValue("@vUserInputFile", RequestType);
                                                    cmd.Parameters.AddWithValue("@vZone", dr["Zone"]);
                                                    cmd.Parameters.AddWithValue("@vContractNo", dr["Contract_No"]); 
                                                    cmd.Parameters.AddWithValue("@vContractPrice", dr["Contract_Price"]);
                                                    cmd.Parameters.AddWithValue("@vModifiedMaterialCode", dr["Modified_Material"]);
                                                    cmd.Parameters.AddWithValue("@vExistingMaterialCode", dr["Existing_Material"]);                                                   
                                                    cmd.Parameters.AddWithValue("@vContractRemarks", dr["Remark"]);

                                                    cmd.Parameters.AddWithValue("@vCustCode", "");
                                                    cmd.Parameters.AddWithValue("@vCustName", "");
                                                    cmd.Parameters.AddWithValue("@vBalContractQty", "");
                                                    cmd.Parameters.AddWithValue("@vReasonOfRejection", "");
                                                    cmd.Parameters.AddWithValue("@vSalesOrg", "");
                                                    cmd.Parameters.AddWithValue("@vContractExtendedDate", "");
                                                    cmd.Parameters.AddWithValue("@vFloorPrice", "");
                                                    cmd.Parameters.AddWithValue("@vExistingPlantCode","");
                                                    cmd.Parameters.AddWithValue("@vModifiedPlantCode", "");                                                    
                                                    cmd.Parameters.AddWithValue("@vLT_ISPrice", "");       
                                                    cmd.Parameters.AddWithValue("@vSLNo", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpCode", "");  
                                                    cmd.Parameters.AddWithValue("@vCustGrpDescrip", "");   
                                                    cmd.Parameters.AddWithValue("@vValidityFrom", "");  
                                                    cmd.Parameters.AddWithValue("@vValidityTo", "");  
                                                    cmd.Parameters.AddWithValue("@vSpocEmail", Convert.ToString(Session["username"]));  

                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            //Zaheer End Code                                          

                                            currentrequestscount++;

                                            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                                            lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";

                                            requestid.InnerText = currentReqId.ToString();
                                            requestcount.InnerText = currentrequestscount.ToString();

                                        }
                                        catch (Exception ex)
                                        {
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log(ex.StackTrace, w);
                                            }
                                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                                        }
                                    }

                                    //Contract Long term 
                                    if (RequestType == "LT")
                                    {
                                        try
                                        {
                                            //Zaheer Start Code
                                            string connect = ConfigurationManager.AppSettings["MySQLKey"].ToString();
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log("NTContractUpd_UserInputFiles", w);
                                            }

                                            using (MySqlConnection con = new MySqlConnection(connect))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand("NTContractUpd_UserInputFiles", con))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@vReqID", currentReqId);
                                                    cmd.Parameters.AddWithValue("@vUserInputFile", RequestType);
                                                    cmd.Parameters.AddWithValue("@vSLNo", dr["Sl No#"]);                                                    
                                                    cmd.Parameters.AddWithValue("@vZone", dr["Zone"]);
                                                    cmd.Parameters.AddWithValue("@vCustGrpCode", dr["Customer_Group_Code"]); 
                                                    cmd.Parameters.AddWithValue("@vCustGrpDescrip", dr["Customer_Group_Description"]);                                                    
                                                    cmd.Parameters.AddWithValue("@vValidityFrom", dr["Valid_From"]);
                                                    cmd.Parameters.AddWithValue("@vValidityTo", dr["Valid_To"]);
                                                    cmd.Parameters.AddWithValue("@vContractRemarks", dr["Remark"]);

                                                    cmd.Parameters.AddWithValue("@vContractNo", "");
                                                    cmd.Parameters.AddWithValue("@vContractPrice", "");
                                                    cmd.Parameters.AddWithValue("@vExistingMaterialCode", "");
                                                    cmd.Parameters.AddWithValue("@vModifiedMaterialCode", "");
                                                    //cmd.Parameters.AddWithValue("@vContractRemarks", "");
                                                    cmd.Parameters.AddWithValue("@vCustCode", "");
                                                    cmd.Parameters.AddWithValue("@vCustName", "");
                                                    cmd.Parameters.AddWithValue("@vBalContractQty", "");
                                                    cmd.Parameters.AddWithValue("@vReasonOfRejection", "");
                                                    cmd.Parameters.AddWithValue("@vSalesOrg", "");
                                                    cmd.Parameters.AddWithValue("@vContractExtendedDate", "");
                                                    cmd.Parameters.AddWithValue("@vFloorPrice", "");
                                                    cmd.Parameters.AddWithValue("@vLT_ISPrice", "");                                              
                                                    cmd.Parameters.AddWithValue("@vExistingPlantCode", ""); 
                                                    cmd.Parameters.AddWithValue("@vModifiedPlantCode", "");                                                                                                                                                                                                      
                                                    cmd.Parameters.AddWithValue("@vSpocEmail", Convert.ToString(Session["username"]));  
                                                    con.Open();
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            //Zaheer End Code                                          

                                            currentrequestscount++;

                                            lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                                            lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";

                                            requestid.InnerText = currentReqId.ToString();
                                            requestcount.InnerText = currentrequestscount.ToString();

                                        }
                                        catch (Exception ex)
                                        {
                                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                            {
                                                Logger.Log(ex.StackTrace, w);
                                            }
                                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                                        }
                                    }
                                }

                                Common.Functions.EmailNotification("RPA - CL " + RequestType + " Upload Notification [Request ID: " + currentReqId + " is Successful]", Common.EmailTemplate.UploadEmail.Replace("{{ReqNum}}", currentReqId.ToString()).Replace("{{Requests}}", currentrequestscount.ToString()), Session["username"].ToString(), "CL");
                                Response.Redirect("cmupload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);
                            }
                        }
                    }
                }
                else
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! You Forgot To Select File...";
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.StackTrace, w);
                }
                lbl_Message.InnerText = ex.Message;
                Response.Write(ex.Message + "\n\n\n" + ex.StackTrace);
            }
        }

        protected bool ReadExcelToDataTable()
        {
            try
            {
                if (fileupload.PostedFile.FileName == string.Empty)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! You Forgot To Select File...";
                    return false;
                }

                //fileupload.PostedFile.SaveAs(Server.MapPath(fileupload.PostedFile.FileName));
                fileupload.PostedFile.SaveAs(ConfigurationManager.AppSettings["filepath"].ToString() + fileupload.PostedFile.FileName);

                dtInputExcel = new DataTable();
                bool hasHeaders = false;
                string HDR = hasHeaders ? "Yes" : "No";
                string strConn;
                if (fileupload.PostedFile.FileName.Substring(fileupload.PostedFile.FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ConfigurationManager.AppSettings["filepath"].ToString() + fileupload.PostedFile.FileName + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"";
                else
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ConfigurationManager.AppSettings["filepath"].ToString() + fileupload.PostedFile.FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
                // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(fileupload.PostedFile.FileName) + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                DataRow schemaRow = schemaTable.Rows[0];
                string sheet = schemaRow["TABLE_NAME"].ToString();

                string query = "SELECT  * FROM [" + sheet + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                daexcel.Fill(dtInputExcel);

                conn.Close();

                File.Delete(ConfigurationManager.AppSettings["filepath"].ToString() + fileupload.PostedFile.FileName);

                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + "\n\n" + ex.StackTrace, w);
                }
                Console.WriteLine("Method Name: ReadExcelToDataTable \n" + ex.Message);
                return false;
            }
        }

        protected bool validateInputExcelColumns()
        {
            if (rd_ce.Checked == true)
            {
                if (!dtInputExcel.Columns.Contains("Zone"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Zone Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Sales_Org"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Sales_Org Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_No"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_No Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_Extended_Date"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_Extended_Date Column Missing... Please check file and retry";
                    return false;
                }
                else if (dtInputExcel.Columns.Contains("Current_Floor_Price").ToString().Length < 1)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Current_Floor_Price Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_Type"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_Type Column Missing... Please check file and retry";
                    return false;
                }
                else
                    return true;
            }
            else if (rd_csc.Checked == true)
            {
                if (!dtInputExcel.Columns.Contains("Zone"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Zone Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_No"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_No Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Customer_Code"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Customer_Code Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Customer_Name"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Customer_Name Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Balance_Contract_Quantity"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Balance_Contract_Quantity Column Missing... Please check file and retry";
                    return false;
                }
                else if (dtInputExcel.Columns.Contains("Price").ToString().Length < 1)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Price Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Reason_of_Rejection"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Reason_of_Rejection Column Missing... Please check file and retry";
                    return false;
                }                
                else
                    return true;
            }
            else if (rd_plant.Checked == true)
            {
                if (!dtInputExcel.Columns.Contains("Zone"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Zone Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_No"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_No Column Missing... Please check file and retry";
                    return false;
                }
                else if (dtInputExcel.Columns.Contains("Contract_Price").ToString().Length < 1)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_Price Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Modified_Plant"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Modified_Plant Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Existing_plant"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Existing_plant Column Missing... Please check file and retry";
                    return false;
                }                
                else
                    return true;
            }
            else if (rd_material.Checked == true)
            {
                if (!dtInputExcel.Columns.Contains("Zone"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Zone Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Contract_No"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_No Column Missing... Please check file and retry";
                    return false;
                }
                else if (dtInputExcel.Columns.Contains("Contract_Price").ToString().Length < 1)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Contract_Price Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Modified_Material"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Modified_Material Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Existing_Material"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Existing_Material Column Missing... Please check file and retry";
                    return false;
                }                
                else
                    return true;
            }
            else if (rd_master.Checked == true)
            {
                if (!dtInputExcel.Columns.Contains("Zone"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Zone Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Customer_Group_Code"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Customer_Group_Code Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Customer_Group_Description"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Customer_Group_Description Column Missing... Please check file and retry";
                    return false;
                }
                else if (dtInputExcel.Columns.Contains("IS_Price").ToString().Length < 1)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! IS_Price Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Valid_From"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Valid_From Column Missing... Please check file and retry";
                    return false;
                }
                else if (!dtInputExcel.Columns.Contains("Valid_To"))
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! Valid_To Column Missing... Please check file and retry";
                    return false;
                }
                else
                    return true;
            }
            else
            {
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Oops! You forgot to select file type: TCL or PCL ";
                return false;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }

        protected void lnk_FormatDownload_ServerClick(object sender, EventArgs e)
        {
            if (rd_ce.Checked == true)
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=contractextensionformat.xlsx");
                Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Contract_Extension_UserInput.xlsx");
                Response.End();
            }
            else if (rd_csc.Checked == true)
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=contractshortcloserformat.xlsx");
                Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Contract_ShortCloser_UserInput.xlsx");
                Response.End();
            }
            else if (rd_plant.Checked == true)
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=contractplantmodificationformat.xlsx");
                Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Contract_Plant_Modification_UserInput.xlsx");
                Response.End();
            }
            else if (rd_material.Checked == true)
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=contractmaterialmodificationformat.xlsx");
                Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Contract_Material_Modification_UserInput.xlsx");
                Response.End();
            }
            else if (rd_master.Checked == true)
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=contractlongtermformat.xlsx");
                Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Contract_LongTerm_UserInput.xlsx");
                Response.End();
            }
            else
            {
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                //lbl_Message.InnerText = "Oops! You forgot to select file type: TCL or PCL ";
                lbl_Message.InnerText = "Oops! You forgot to select file type";
            }
        }

        protected void lnk_MacroDownload_ServerClick(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CLSanityCheck.xlsm");
            Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "CLSanityCheck.xlsm");
            Response.End();
        }

        protected void GetTop20()
        {
            //dtTemp = objSQL.DataTable_return("select ReqID,'CL' as ProcessType, ApproverResponseDate as Submitted_Data_Time,status from `creditlimitinput` where SPOCEmail = '" + Session["username"].ToString() + "'order by `ReqID` desc limit 20;");
            //gv_top20requests.DataSource = dtTemp;
            //gv_top20requests.DataBind();
        }
        
        [WebMethod]
        public static string CheckContractType(string RequestType)
        {
            //string strStatus;
            string totalrequests = "0";
            string totalapproved = "0";
            string totalrejected = "0";
            string totalrequested = "0";
            string requestcount = "0";


            totalrequests = objSQL.DataTable_return("select count(1) from ntcontractupd_input where ContractFlag = '" + RequestType + "' and SPOCEmail='" + HttpContext.Current.Session["username"].ToString() + "';").Rows[0][0].ToString();
            totalapproved = objSQL.DataTable_return("select count(1) from ntcontractupd_input where  ContractFlag = '" + RequestType + "' and SPOCEmail='" + HttpContext.Current.Session["username"].ToString() + "' and Status=5;").Rows[0][0].ToString();
            totalrejected = objSQL.DataTable_return("select count(1) from ntcontractupd_input where  ContractFlag = '" + RequestType + "' and SPOCEmail='" + HttpContext.Current.Session["username"].ToString() + "' and Status=7;").Rows[0][0].ToString();

            totalrequested = objSQL.DataTable_return("select max(ReqID) from ntcontractupd_input where ContractFlag = '" + RequestType + "' and SPOCEmail='" + HttpContext.Current.Session["username"].ToString() + "';").Rows[0][0].ToString();
            requestcount = objSQL.DataTable_return("select count(1) from ntcontractupd_input where ContractFlag = '" + RequestType + "' and ReqID = '" + totalrequested + "' and SPOCEmail='" + HttpContext.Current.Session["username"].ToString() + "';").Rows[0][0].ToString();
            if(totalrequested=="")
            {
                totalrequested = "0";
            }
            return totalrequests +","+totalapproved + "," + totalrejected +"," +totalrequested+ ","+ requestcount;
        }        
    }
}