using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace rpa_mazar
{
    public partial class contractcreationupload : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static DataTable dtInputExcel = null;
        static string bulkInsert = string.Empty;
        static int currentReqId = 0;
        static int currentCustomerCode = 0;
        static int currentrequestscount = 0;
        static bool IsPageAllowed = false;
        static string currentUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["username"] = "rpa-admin@dalmiabharat.com";
            //Session["usertype"] = "SPOC";
            //Session["isadmin"] = false;

            //dtTemp = null;
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
                ///GetTop20();
                if (Request.QueryString["rid"] != null)
                {
                    requestid.InnerText = Request.QueryString["rid"].ToString();
                    requestcount.InnerText = Request.QueryString["cnt"].ToString();

                    lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                    lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";
                    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                }

                try
                {
                    //remove hardcoded email id from queries
                    totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                    totalapproved.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=3;").Rows[0][0].ToString();
                    totalrejected.InnerHtml = objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
                }
                catch { }
            }
        }

        protected void btn_poupload_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lbl_Message.InnerText = string.Empty;
                currentCustomerCode = 0;
                currentrequestscount = 0;
                if (pofileupload.PostedFile.FileName != string.Empty)
                {
                    if (ReadExcelToDataTable())
                        UploadExcel();
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

        protected void lnk_FormatDownload_ServerClick(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=ccformat.xlsx");
            Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "Input_File_Portal.xlsx");
            Response.End();
        }
        private bool UploadExcel()
        {
            currentrequestscount = 0;
            var sendMail = false;

            if (validateInputExcelColumns())
            {
                dtInputExcel.DefaultView.Sort = "Customer Code";
                dtInputExcel = dtInputExcel.DefaultView.ToTable();
                if (dtInputExcel.Rows.Count > 0)
                {
                    #region Set Request Id
                    dtTemp = objSQL.DataTable_return("select max(ReqID) as lastReqId from non_trade_price_contract_creation;");

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

                    currentCustomerCode = Convert.ToInt32(dtInputExcel.Rows[0]["Customer Code"]);
                    foreach (DataRow dr in dtInputExcel.Rows)
                    {
                        try
                        {
                            if (currentCustomerCode != Convert.ToInt32(dr["Customer Code"]))
                            {
                                //  currentCaseCustomerId = currentCaseCustomerId + 1;
                                currentCustomerCode = Convert.ToInt32(dr["Customer Code"]);
                            }

                            if (dr.IsNull("Qty"))
                                dr["Qty"] = 0;
                            if (dr.IsNull("Last unused contract/PO quantity"))
                                dr["Last unused contract/PO quantity"] = 0;
                            if (dr.IsNull("Last PO price"))
                                dr["Last PO price"] = 0.0;
                            if (dr.IsNull("IS Price"))
                                dr["IS Price"] = 0;
                            if (dr.IsNull("Trade Price"))
                                dr["Trade Price"] = 0;
                            if (dr.IsNull("Diferrence"))
                                dr["Diferrence"] = 0;
                            if (dr.IsNull("Loading/Unloading Charges"))
                                dr["Loading/Unloading Charges"] = 0;
                            if (dr.IsNull("VC"))
                                dr["VC"] = 0;
                            if (dr.IsNull("Contribution PB if Any"))
                                dr["Contribution PB if Any"] = 0;
                            if (dr.IsNull("IA Comm"))
                                dr["IA Comm"] = 0;
                            if (dr.IsNull("Floor Price Check"))
                                dr["Floor Price Check"] = 0;
                            if (dr.IsNull("Above/Below Floor Price"))
                                dr["Above/Below Floor Price"] = 0;


                            //remove hardcoded email id from query
                            //objSQL.ExecuteNonQuery_IUD("insert into purchaseorderinput (ReqID,Region,SalesOrg,CustomerName,CustomerCode,ConsigneeCode,Qty," +
                            //    "LastUnusedContractPOQuantity,LastPOPrice,CurrentContarctStatus,CementType,`Source/Code`,Deatination`,`EX/FOR/FPD`,ISPrice," +
                            //    "TradePrice,Diferrence,LoadingUnloadingCharges,ISNCR,VC,ContributionPB,IAName,IAComm,ValidityFrom,ValidityTo," +
                            //    "PaymentTermsNoOfDays,PONo,PODate,Material,OrderReason,MaterialType,Plant,FloorPriceCheck,`Above/BelowFloorPrice`,Consider) " +
                            var filepath = "";
                            foreach (HttpPostedFile uploadedFile in popdffileupload.PostedFiles)
                            {
                                // 20-Jan-22
                                // Changes by Pramod - Check the name with the FIleName columns instead of consignee code column
                                // Start
                                //if (Convert.ToString(dr["Consignee Code"]) == Path.GetFileNameWithoutExtension(uploadedFile.FileName))
                                if (Convert.ToString(dr["FileName"]) == Path.GetFileNameWithoutExtension(uploadedFile.FileName))
                                    // End
                                    
                                {
                                    var newGuid = Guid.NewGuid();
                                    var fileName = newGuid.ToString() + Path.GetExtension(uploadedFile.FileName);
                                    uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/assets/pdf"), fileName));
                                    filepath = ConfigurationManager.AppSettings["virtual_directory"].ToString() + "/assets/pdf/" + fileName;
                                    break;
                                }
                            }

                            objSQL.ExecuteNonQuery_IUD("insert into non_trade_price_contract_creation(ReqID,Region,Sales_Org,Customer_Name,Customer_Code," +
                            "Consignee_Code,Qty,`Last_unused_contract/PO_quantity`,Last_PO_price,`Current_status_of_that_contract_Active/Blocked/Expired`," +
                            "Cement_Type,`Source/Code`,Deatination,`EX/FOR/FPD`,IS_Price,Trade_Price,Diferrence,`Loading_/_Unloading_Charges`,IS_NCR,VC," +
                            "Contribution_PB_if_Any,IA_Name,IA_Comm,Validity_From,Validity_To,Payment_Terms_No_of_Days,PO_No,PO_Date," +
                            "Material,Material_type,Plant,Order_reason,Floor_Price_Check,`Above/Below_Floor_Price`,Consider,requestor_email,Status,CaseCreatedDate,FilePath,Zone) " +
                                                                       "values (" +
                                                                       currentReqId + ",'" +
                                                                       dr["Region"] + "','" +
                                                                       dr["Sales Org"] + "','" +
                                                                       dr["Customer Name"] + "','" +
                                                                       dr["Customer Code"] + "','" +
                                                                       dr["Consignee Code"] + "','" +
                                                                       dr["Qty"] + "','" +
                                                                       dr["Last unused contract/PO quantity"] + "','" + //String.IsNullOrEmpty(str)
                                                                       dr["Last PO price"] + "','" +
                                                                       dr["Current status of that contract Active/Blocked/Expired"] + "','" +
                                                                       dr["Cement Type"] + "','" +
                                                                       dr["Source/Code"] + "','" +
                                                                       dr["Deatination"] + "','" +
                                                                       dr["EX/FOR/FPD"] + "','" +
                                                                       dr["IS Price"] + "','" +
                                                                       dr["Trade Price"] + "','" + // Trade Price As on 20.Nov.21 - dot in column name not allowd and default it converts into #
                                                                       dr["Diferrence"] + "','" +
                                                                       dr["Loading/Unloading Charges"] + "','" +
                                                                       dr["IS NCR"] + "','" +
                                                                       dr["VC"] + "','" +
                                                                       dr["Contribution PB if Any"] + "','" +
                                                                       dr["IA Name"] + "','" +
                                                                       dr["IA Comm"] + "','" +
                                                                       Convert.ToDateTime(dr["Validity From"]).ToString("yyyy-MM-dd") + "','" +
                                                                       Convert.ToDateTime(dr["Validity To"]).ToString("yyyy-MM-dd") + "','" +

                                                                       dr["Payment Terms No of Days"] + "','" +
                                                                       dr["PO No"] + "','" +
                                                                       Convert.ToDateTime(dr["PO Date"]).ToString("yyyy-MM-dd") + "','" +
                                                                       dr["Material"] + "','" +
                                                                       dr["Material type"] + "','" +
                                                                       dr["Plant"] + "','" +
                                                                       dr["Order reason"] + "','" +
                                                                       dr["Floor Price Check"] + "','" +
                                                                       dr["Above/Below Floor Price"] + "','" +
                                                                       dr["Consider"] + //"','" +
                                                                                        //// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + //Hemant - This will be manage from DB (column default value - GETDATE())
                                                                                        //Session["username"].ToString() +
                                                                       "','" + username.InnerText + "'," +
                                                                       "3," +
                                                                       "now(),'" + filepath + "','" + dr["Zone"] + "');");

                            currentrequestscount++;

                            

                            requestid.InnerText = currentReqId.ToString();
                            requestcount.InnerText = currentrequestscount.ToString();
                            sendMail = true;
                        }
                        catch (Exception ex)
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log(ex.StackTrace, w);
                            }
                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                            lbl_Message.InnerText = "Oops! Something Went Wrong... Please check file and retry";
                            ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                            return false;
                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                    if (sendMail)
                    {
                        lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                        lbl_Message.InnerText = "Yaay! File Uploaded Successfully...";
                        Common.Functions.EmailNotification("RPA - PO Release Notification [Request ID: " + currentReqId + " is Successful]", Common.EmailTemplate.UploadEmail.Replace("{{ReqNum}}", currentReqId.ToString()).Replace("{{Requests}}", currentrequestscount.ToString()), Session["username"].ToString(), "CC");
                    }
                    //Response.Redirect("ccupload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);
                }

            }
            return sendMail;
        }

        protected bool ReadExcelToDataTable()
        {
            try
            {
                if (pofileupload.PostedFile.FileName == string.Empty)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! You Forgot To Select File...";
                    return false;
                }

                //fileupload.PostedFile.SaveAs(Server.MapPath(fileupload.PostedFile.FileName));
                pofileupload.PostedFile.SaveAs(ConfigurationManager.AppSettings["filepath"].ToString() + pofileupload.PostedFile.FileName);

                dtInputExcel = new DataTable();
                bool hasHeaders = false;
                string HDR = hasHeaders ? "Yes" : "No";
                string strConn;
                if (pofileupload.PostedFile.FileName.Substring(pofileupload.PostedFile.FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ConfigurationManager.AppSettings["filepath"].ToString() + pofileupload.PostedFile.FileName + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"";
                else
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ConfigurationManager.AppSettings["filepath"].ToString() + pofileupload.PostedFile.FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
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

                File.Delete(ConfigurationManager.AppSettings["filepath"].ToString() + pofileupload.PostedFile.FileName);

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
            if (!dtInputExcel.Columns.Contains("Customer Code"))
            {
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Oops! CustomerCode Column Missing... Please check file and retry";
                return false;
            }
            //else if (!dtInputExcel.Columns.Contains("SalesOrderNo"))
            //{
            //    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
            //    lbl_Message.InnerText = "Oops! SalesOrderNo Column Missing... Please check file and retry";
            //    return false;
            //}
            else
                return true;
        }

        protected void btn_popdfupload_ServerClick(object sender, EventArgs e)
        {
            if (!popdffileupload.HasFiles || !pofileupload.HasFiles)
            {
                if (pofileupload.PostedFile.FileName == string.Empty)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! You Forgot To Select Excel File...";
                }
                else if (popdffileupload.PostedFile.FileName == string.Empty)
                {
                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                    lbl_Message.InnerText = "Oops! You Forgot To Select PO File...";
                }
            }
            else
            {
                try
                {
                    if (ReadExcelToDataTable())
                    {
                        dtInputExcel.DefaultView.Sort = "Customer Code";
                        dtInputExcel = dtInputExcel.DefaultView.ToTable();

                        var count = 0;


                        // 20-Jan-22
                        // CHanges by Pramod
                        // Start

                        var newfilename = "";
                        var oldfilename = "";
                        int exrow = 0;
                        int exfilecount = 0;
                        int filenamematched = 0;
                        var filename = "";

                        foreach (DataRow dr in dtInputExcel.Rows)
                        {
                            filenamematched = 0;
                            if (exrow == 0)
                            {
                                oldfilename = Convert.ToString(dr["FileName"]);
                                newfilename = oldfilename;
                                exfilecount++;
                                exrow = 1;
                            }
                            else
                            {
                                newfilename = Convert.ToString(dr["FileName"]);
                                if (oldfilename != newfilename)
                                {
                                    exfilecount++;
                                }
                            }
                            foreach (var uploadedFile in popdffileupload.PostedFiles)
                            {
                                filename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                if(newfilename == filename)
                                {
                                    filenamematched = 1;
                                    break;
                                }
                            }
                            if(filenamematched == 0)
                            {
                                break;
                            }
                        }

                        /*foreach (DataRow dr in dtInputExcel.Rows)
                        {
                            var consigneeCode = Convert.ToString(dr["Consignee Code"]);
                            foreach (var uploadedFile in popdffileupload.PostedFiles)
                            {
                                var filename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                if (consigneeCode == filename)
                                {
                                    count++;
                                }
                            }
                        }*/

                        if (filenamematched == 0)
                        {
                            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                            lbl_Message.InnerText = "Oops! FileName in ExcelSheet (" + newfilename + ") is not matching with the Attached file (" + filename + ")!";
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log(lbl_Message.InnerText, w);
                            }
                        }
                        else
                        {
                            if (exfilecount != popdffileupload.PostedFiles.Count)
                            {
                                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                lbl_Message.InnerText = "Oops! File count is not matching with the excel file data...";
                                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                {
                                    Logger.Log(lbl_Message.InnerText, w);
                                }
                            }
                            /*if (dtInputExcel.Rows.Count != popdffileupload.PostedFiles.Count || count != dtInputExcel.Rows.Count)
                            {
                                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                lbl_Message.InnerText = "Oops! File count is not matching with the excel file data...";
                            }*/
                            // End

                            else
                            {
                                if (UploadExcel())
                                    Response.Redirect("ccupload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);
                            }
                        }
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

        }
        //protected void btn_popdfupload_ServerClick(object sender, EventArgs e)
        //{
        //    if (!popdffileupload.HasFiles || !pofileupload.HasFiles)
        //    {
        //        if (pofileupload.PostedFile.FileName == string.Empty)
        //        {
        //            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
        //            lbl_Message.InnerText = "Oops! You Forgot To Select Excel File...";
        //        }
        //        else if (popdffileupload.PostedFile.FileName == string.Empty)
        //        {
        //            lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
        //            lbl_Message.InnerText = "Oops! You Forgot To Select PO File...";
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            if (ReadExcelToDataTable())
        //            {
        //                dtInputExcel.DefaultView.Sort = "Customer Code";
        //                dtInputExcel = dtInputExcel.DefaultView.ToTable();

        //                var count = 0;
        //                foreach (DataRow dr in dtInputExcel.Rows)
        //                {
        //                    var consigneeCode = Convert.ToString(dr["Consignee Code"]);
        //                    foreach (var uploadedFile in popdffileupload.PostedFiles)
        //                    {
        //                        var filename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
        //                        if (consigneeCode == filename)
        //                        {
        //                            count++;
        //                        }
        //                    }
        //                }
        //                if (dtInputExcel.Rows.Count != popdffileupload.PostedFiles.Count || count != dtInputExcel.Rows.Count)
        //                {
        //                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
        //                    lbl_Message.InnerText = "Oops! File count is not matching with the excel file data...";
        //                }
        //                else
        //                {
        //                    if (UploadExcel())
        //                    {
        //                        string filepath = string.Empty;
        //                        string filename = string.Empty;

        //                        foreach (HttpPostedFile uploadedFile in popdffileupload.PostedFiles)
        //                        {
        //                            filepath = "../assets/pdf/" + uploadedFile.FileName;
        //                            filename = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
        //                            uploadedFile.SaveAs(Path.Combine(Server.MapPath("~/assets/pdf"), uploadedFile.FileName));
        //                            //listofuploadedfiles.Visible = true;
        //                            //listofuploadedfiles.Text += String.Format("{0}<br />", uploadedFile.FileName);

        //                            int result = objSQL.ExecuteNonQuery_IUD("SET SQL_SAFE_UPDATES = 0;" +
        //                             " UPDATE non_trade_price_contract_creation SET FilePath = '" + filepath + "'" +
        //                             " WHERE  Consignee_Code = '" + filename + "' and ContractID");
        //                            if (result > 0)
        //                            {
        //                                lbl_Message1.Attributes.Add("style", "color:green; font-weight:bold;");
        //                                lbl_Message1.InnerText = "Yaay! File Uploaded Successfully...";
        //                                Response.Redirect("ccupload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);
        //                            }
        //                            else
        //                            {
        //                                lbl_Message1.Attributes.Add("style", "color:red; font-weight:bold;");
        //                                lbl_Message1.InnerText = "Oops! Something Went Wrong... Please check file and retry";
        //                                ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
        //                            }
        //                        }
        //                        Response.Redirect("ccupload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);

        //                    }
        //                }
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
        //            {
        //                Logger.Log(ex.StackTrace, w);
        //            }
        //            lbl_Message.InnerText = ex.Message;
        //            Response.Write(ex.Message + "\n\n\n" + ex.StackTrace);
        //        }
        //    }

        //}


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }


    }
}