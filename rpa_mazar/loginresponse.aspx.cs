using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using System.Data;
using System.IO;
using System.Configuration;

namespace rpa_mazar
{
    public partial class loginresponse : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        string userid = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
            {
                Logger.Log("*** Response from AD FS ***\n", w);
            }
            if (!IsPostBack)
            {
                var wid = Convert.ToString(Request.QueryString["wid"]);
                string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
                if (!string.IsNullOrEmpty(wid))
                    ReturnUrl = String.Concat(ReturnUrl, "&wid=", wid);

                foreach (Claim claim in HttpContext.Current.GetOwinContext().Authentication.User.Claims)
                {
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log("*** Claims Start ***\n", w);
                        Logger.Log(claim.Value, w);
                        Logger.Log(Request.UrlReferrer + "URL referrer***\n", w);
                        Logger.Log("*** Claims End ***\n", w);
                    }


                    if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    {
                        using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                        {
                            Logger.Log("1 \n", w);
                        }
                        Session["username"] = claim.Value;
                        //Session["username"] = "rpa-admin@dalmiabharat.com";
                        dtTemp = objSQL.DataTable_return("select * from spocmaster where Email='" + claim.Value + "';");
                        if (dtTemp == null || dtTemp.Rows.Count == 0)
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("2 \n", w);
                            }
                            dtTemp = objSQL.DataTable_return("select * from salesorderapprovalmaster where ApprovalEmail='" + claim.Value + "';");
                            if (dtTemp == null || dtTemp.Rows.Count == 0)
                            {
                                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                {
                                    Logger.Log("3 \n", w);
                                }
                                Common.Functions.ADFSSignout();
                                return;
                            }
                            else
                            {
                                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                                {
                                    Logger.Log("4 \n", w);
                                }
                                Session["usertype"] = "APPROVER";
                            }
                        }
                        else
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("5 \n", w);
                            }
                            Session["usertype"] = "SPOC";
                        }

                        dtTemp = objSQL.DataTable_return("select * from tbl_usermodulemap um, tbl_modulemaster mm where userid in (select SPOCID from spocmaster where Email='" + claim.Value + "') and um.isactive=1 and mm.moduleid = um.moduleid and mm.modulegroup = 'ADMIN';");
                        if (dtTemp == null || dtTemp.Rows.Count == 0)
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("6 \n", w);
                            }
                            Session["isadmin"] = false;
                        }
                        else
                        {
                            using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                            {
                                Logger.Log("7 \n", w);
                            }
                            Session["isadmin"] = true;
                        }

                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            Response.Redirect(ReturnUrl);
                        }
                        else
                        {
                            Response.Redirect("dashboard.aspx");

                        }

                    }
                }

            }
        }
    }
}