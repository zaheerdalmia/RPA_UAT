using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Configuration;

namespace rpa_mazar
{
    public partial class dashboard : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        DataTable dtTemp = null;
        int sorequest = 0;
        int soprocessed = 0;
        int soapproved = 0;
        int sorejected = 0;
        double soTAT = 0.00;

        int clrequest = 0;
        int clprocessed = 0;
        int clapproved = 0;
        int clrejected = 0;
        double clTAT = 0.00;
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
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log("Logged in user:" + Session["username"].ToString(), w);
                    }
                    #region UserAccess Check
                    dtTemp = Common.AppUser.AccessModules(Session["username"].ToString());
                    if (!(dtTemp == null))
                    {
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
                    Common.Functions.ADFSSignout();
                }
            }
            else
            {
                Common.Functions.ADFSSignout();
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                try
                {
                    if (Session["usertype"].ToString() == "APPROVER")
                    {
                        dtTemp = objSQL.DataTable_return("Select distinct " +
                        "(select  count(distinct CaseCustomerID)  from salesorderapprovaltransaction where ApprovalEmail = '" + Session["username"].ToString() + "' and level = 1) as 'Requests', " +
                        "(select  count(distinct CaseCustomerID)  from salesorderapprovaltransaction where ApprovalEmail = '" + Session["username"].ToString() + "' and Approval_Status = 1 and level = 1) as 'Pending', " +
                        "(select  count(distinct CaseCustomerID)  from salesorderapprovaltransaction where ApprovalEmail = '" + Session["username"].ToString() + "' and Approval_Status = 2 and level = 1) as 'Processed', " +
                        "(select  count(distinct CaseCustomerID)  from salesorderapprovaltransaction where ApprovalEmail = '" + Session["username"].ToString() + "' and Approval_Status = 3 and level = 1) as 'Rejected'  from salesorderapprovaltransaction; ");

                        spn_sorequest.InnerText = dtTemp.Rows[0][0].ToString();
                        spn_soprocessed.InnerText = dtTemp.Rows[0][1].ToString();
                        spn_soapproved.InnerText = dtTemp.Rows[0][2].ToString().ToString();
                        spn_sorejected.InnerText = dtTemp.Rows[0][3].ToString().ToString();
                        //spn_sotat.InnerText = soTAT.ToString();

                        clrequest = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput;").Rows[0][0]);
                        clapproved = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status in (0,1,2);").Rows[0][0]);
                        clprocessed = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=5;").Rows[0][0]);
                        clrejected = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=4;").Rows[0][0]);
                        clTAT = Convert.ToDouble(objSQL.DataTable_return("select case when tat is null then 0 else tat end from (select round(avg(difference), 1) as tat from (select TIMESTAMPDIFF(Minute, CaseCreatedDate, ApproverResponseDate) AS difference from creditlimitinput) t1) t;").Rows[0][0]);
                        spn_clrequest.InnerText = clrequest.ToString();
                        spn_clprocessed.InnerText = clprocessed.ToString();
                        spn_clapproved.InnerText = clapproved.ToString();
                        spn_clrejected.InnerText = clrejected.ToString();
                        spn_cltat.InnerText = clTAT.ToString();
                    }
                    if (Convert.ToBoolean(Session["isadmin"]) == true)
                    {
                        sorequest = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput;").Rows[0][0]);
                        soapproved = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status in (0,1,2);").Rows[0][0]);
                        soprocessed = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status=5;").Rows[0][0]);
                        sorejected = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status=4;").Rows[0][0]);
                        soTAT = Convert.ToDouble(objSQL.DataTable_return("select case when tat is null then 0 else tat end from (select round(avg(difference), 1) as tat from (select TIMESTAMPDIFF(Minute, CreatedDate, ProcessedDate) AS difference from salesorderinput) t1) t;").Rows[0][0]);
                        spn_sorequest.InnerText = sorequest.ToString();
                        spn_soprocessed.InnerText = soprocessed.ToString();
                        spn_soapproved.InnerText = soapproved.ToString();
                        spn_sorejected.InnerText = sorejected.ToString();
                        spn_sotat.InnerText = soTAT.ToString();

                        clrequest = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput;").Rows[0][0]);
                        clapproved = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status in (0,1,2);").Rows[0][0]);
                        clprocessed = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=5;").Rows[0][0]);
                        clrejected = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=4;").Rows[0][0]);
                        clTAT = Convert.ToDouble(objSQL.DataTable_return("select case when tat is null then 0 else tat end from (select round(avg(difference), 1) as tat from (select TIMESTAMPDIFF(Minute, CaseCreatedDate, CaseProcessedDate) AS difference from creditlimitinput) t1) t;").Rows[0][0]);
                        spn_clrequest.InnerText = clrequest.ToString();
                        spn_clprocessed.InnerText = clprocessed.ToString();
                        spn_clapproved.InnerText = clapproved.ToString();
                        spn_clrejected.InnerText = clrejected.ToString();
                        spn_cltat.InnerText = clTAT.ToString();
                    }
                    else
                    {
                        sorequest = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        soapproved = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status in (0,1,2) and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        soprocessed = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status=5 and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        sorejected = Convert.ToInt32(objSQL.DataTable_return("select count(*) from salesorderinput where Status=4 and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        soTAT = Convert.ToDouble(objSQL.DataTable_return("select case when tat is null then 0 else tat end from (select round(avg(difference), 1) as tat from (select TIMESTAMPDIFF(Minute, CreatedDate, ProcessedDate) AS difference from salesorderinput where SPOCEmail='" + Session["username"].ToString() + "') t1) t; ").Rows[0][0]);
                        spn_sorequest.InnerText = sorequest.ToString();
                        spn_soprocessed.InnerText = soprocessed.ToString();
                        spn_soapproved.InnerText = soapproved.ToString();
                        spn_sorejected.InnerText = sorejected.ToString();
                        spn_sotat.InnerText = soTAT.ToString();

                        clrequest = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        clapproved = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status in (0,1,2) and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        clprocessed = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=5 and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        clrejected = Convert.ToInt32(objSQL.DataTable_return("select count(*) from creditlimitinput where Status=4 and SPOCEmail='" + Session["username"].ToString() + "';").Rows[0][0]);
                        clTAT = Convert.ToDouble(objSQL.DataTable_return("select case when tat is null then 0 else tat end from (select round(avg(difference), 1) as tat from (select TIMESTAMPDIFF(Minute, CaseCreatedDate, CaseProcessedDate) AS difference from creditlimitinput where SPOCEmail='" + Session["username"].ToString() + "') t1) t;").Rows[0][0]);
                        spn_clrequest.InnerText = clrequest.ToString();
                        spn_clprocessed.InnerText = clprocessed.ToString();
                        spn_clapproved.InnerText = clapproved.ToString();
                        spn_clrejected.InnerText = clrejected.ToString();
                        spn_cltat.InnerText = clTAT.ToString();
                    }

                    spn_soclrequests.InnerText = (sorequest + clrequest).ToString();
                    spn_soclprocessed.InnerText = (soprocessed + clprocessed).ToString();
                    spn_soclapproved.InnerText = (soapproved + clapproved).ToString();
                    spn_soclrejected.InnerText = (sorejected + clrejected).ToString();
                    spn_socltat.InnerText = ((soTAT + clTAT) / 2).ToString();
                }
                catch (Exception ex)
                {
                    using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                    {
                        Logger.Log(ex.Message + "\n" + ex.StackTrace, w);
                    }
                }
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