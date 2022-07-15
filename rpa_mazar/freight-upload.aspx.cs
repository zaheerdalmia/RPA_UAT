using rpa_mazar.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace rpa_mazar
{
    public partial class FreightUpload : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static DataTable dtInputExcel = null;
        static int currentReqId = 0;
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
                    //dtTemp = AppUser.AccessModules(Session["username"].ToString(), spoMasterTable: "freightspocmaster", salesOrderApprovalMaster: "freightapprovalmaster");
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
                                break;
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
                    //ClientScript.RegisterStartupScript(this.GetType(), "label", "HideLabel();");
                }

                try
                {
                    //remove hardcoded email id from queries
                    totalrequests.InnerHtml = objSQL.DataTable_return("select count(*) from freightuserinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0].ToString();
                    totalapproved.InnerHtml = objSQL.DataTable_return("select count(*) from freightuserinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=3;").Rows[0][0].ToString();
                    totalrejected.InnerHtml = objSQL.DataTable_return("select count(*) from freightuserinput where SPOCEmail='" + Session["username"].ToString() + "' and Status=4;").Rows[0][0].ToString();
                }
                catch { }
            }
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
                        if (validateRadioButton())
                        {
                            dtInputExcel = Functions.RemoveEmptyRow(dtInputExcel);
                            dtInputExcel.DefaultView.Sort = "Zone";
                            dtInputExcel = dtInputExcel.DefaultView.ToTable();
                            if (dtInputExcel.Rows.Count > 0)
                            {
                                #region Set Request Id
                                dtTemp = objSQL.DataTable_return("select max(ReqID) as lastReqId from freightuserinput;");

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

                                var freightType = rd_primery.Checked ? "P" : "S";
                                var freightTypeRoute = rd_new.Checked ? "N" : "R";
                                var notUploadedRecords = 0;

                                foreach (DataRow dr in dtInputExcel.Rows)
                                {
                                    try
                                    {

                                        //remove hardcoded email id from query
                                        objSQL.ExecuteNonQuery_IUD("insert into freightuserinput (FreightType, " +
                                            "FreightTypeRoute, FreightInputFormat, Zone, StatePlant, TransportationPlanningPoint," +
                                            " DistributionChannel, ShipmentRoute, ShippingTypeForShipmentStage, PackagingMaterials, " +
                                            "ToDate, FromDate, Rate, RevisionRateDiff, ShipingCondition, MeansOfTransType, Incoterms, " +
                                            "ServicesAgent, SpocEmail," +
                                            " ReqId, " +
                                            "EmailStatus, " +
                                            "Status, " +
                                            "CreatedDate" +
                                            ",Comments ) values ('" +
                                            freightType + "','" +//DropDown
                                            freightTypeRoute + "','" +//Dropdown
                                            dr["FreightInputFormat"] + "','" +
                                            dr["Zone"] + "','" +
                                            string.Empty + "','" +
                                            dr["Transportation planning point"] + "','" +//Manoj
                                            dr["Distribution Channel"] + "','" +
                                            dr["Shipment route"] + "','" +
                                            dr["Shipping type for shipment stage"] + "','" +
                                            dr["Packaging Materials"] + "','" +
                                            Convert.ToString(dr["To Date"]).Split(' ').FirstOrDefault() + "','" +
                                            Convert.ToString(dr["Form Date"]).Split(' ').FirstOrDefault() + "','" +
                                            dr["Rate"] + "'," +
                                            "null,'" +
                                            dr["Shiping Condition"] + "','" +
                                            dr["Means-of-Trans# Type"] + "','" +
                                            dr["Incoterms"] + "','" +
                                            dr["Service agent"] + "','" +
                                            Session["username"].ToString() + "'," +
                                            currentReqId + "," +
                                            "0," +
                                            "0," +
                                            "CURRENT_TIMESTAMP," +
                                           "'" + dr["comments"] + "');");


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
                                        notUploadedRecords++;
                                    }
                                }
                                if (notUploadedRecords > 0)
                                {
                                    lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                                    lbl_Message.InnerText = "Total Number of files are not processed: " + notUploadedRecords;
                                }

                                if (currentrequestscount > 0)
                                    Functions.EmailNotification("RPA - Freight Release Notification [Request ID: " + currentReqId + " is Successful], [Failed Records: " + notUploadedRecords + "]", EmailTemplate.UploadEmail.Replace("{{ReqNum}}", currentReqId.ToString()).Replace("{{Requests}}", currentrequestscount.ToString()), Session["username"].ToString(), "Freight");

                                if (notUploadedRecords == 0)
                                    Response.Redirect("freight-upload.aspx?rid=" + currentReqId + "&cnt=" + currentrequestscount);
                            }
                        }
                    }
                    else
                    {
                        lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                        lbl_Message.InnerText = "Total Number of line items in files is more than 500. Cannot Process";
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
                string sheet = "FreightInput$"; // schemaRow["TABLE_NAME"].ToString();

                string query = "SELECT  * FROM [" + sheet + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                daexcel.Fill(dtInputExcel);

                if(dtInputExcel.Rows.Count > 500)
                {
                    conn.Close();

                    File.Delete(ConfigurationManager.AppSettings["filepath"].ToString() + fileupload.PostedFile.FileName);
                    return false;
                }

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

        protected bool validateRadioButton()
        {

            if (!rd_primery.Checked && !rd_secondry.Checked)
            {
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Please select Freight Type!";
                return false;
            }
            if (!rd_new.Checked && !rd_revision.Checked)
            {
                lbl_Message.Attributes.Add("style", "color:red; font-weight:bold;");
                lbl_Message.InnerText = "Please select Freight Type Route!";
                return false;
            }
            else
                return true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Common.Functions.ADFSSignout();
            Response.Redirect("login.aspx");
        }

        protected void lnk_FormatDownload_ServerClick(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=FreightInput_Template.xlsx");
            Response.TransmitFile(ConfigurationManager.AppSettings["basefilepathfordownloads"] + "FreightInput_Template.xlsx");
            Response.End();
        }

        protected void GetTop20()
        {
            //dtTemp = objSQL.DataTable_return("select ReqID,'SO' as ProcessType, ProcessedDate as Submitted_Data_Time,status from `freightuserinput` where SPOCEmail = '" + Session["username"].ToString() + "'order by `ReqID` desc limit 20;");
            //gv_top20requests.DataSource = dtTemp;
            //gv_top20requests.DataBind();
        }
    }
}