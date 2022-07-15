using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace rpa_mazar
{
    public partial class usermodulemap : System.Web.UI.Page
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTemp = null;
        static bool IsPageAllowed = false;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    Common.Functions.ADFSSignout();
                }
            }
            else
            {
                //Common.Functions.ADFSSignout();
                Response.Redirect("login.aspx");
            }
            
            lbl_Message.InnerText = string.Empty;
            if (!IsPostBack)
            {
                GetUserList();
                chk_ModuleList.Visible = false;
            }
            }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Common.Functions.ADFSSignout();
        }


        protected void GetUserList()
        {
            chk_ModuleList.Items.Clear();
            if (rd_spoc.Checked == true)
            {
                gv_userlist.DataSource = null;
                dtTemp = objSQL.DataTable_return("select sm1.SPOCID, sm1.email, Case when `Current Assigned Modules` is null then 'No Modules Assigned' else `Current Assigned Modules` end as `Assigned Modules`  from " +
                    "(select sm.SPOCID, sm.email, GROUP_CONCAT(mm.modulename) as `Current Assigned Modules` from " +
                    "spocmaster sm   join tbl_usermodulemap um on sm.SPOCID = um.userid " +
                     "join tbl_modulemaster mm on um.moduleid = mm.moduleid where um.isactive=1 " +
                    "group by um.userid) t  right join  spocmaster sm1 on t.SPOCID = sm1.SPOCID; ");

                gv_userlist.DataSource = dtTemp;
                gv_userlist.DataBind();
            }
            if (rd_approver.Checked == true)
            {
                gv_userlist.DataSource = null;
                dtTemp = objSQL.DataTable_return("select sm1.ApprovalID, sm1.ApprovalEmail, Case when `Current Assigned Modules` is null then 'No Modules Assigned' else `Current Assigned Modules` end as `Assigned Modules`  from " +
                "(select sm.ApprovalID, sm.ApprovalEmail, GROUP_CONCAT(mm.modulename) as `Current Assigned Modules` from " +
                "salesorderapprovalmaster sm   join tbl_usermodulemap um on sm.ApprovalID = um.userid " +
                " join tbl_modulemaster mm on um.moduleid = mm.moduleid where um.isactive=1 " +
                "group by um.userid) t  right join  salesorderapprovalmaster sm1 on t.ApprovalID = sm1.ApprovalID; ");

                gv_userlist.DataSource = dtTemp;
                gv_userlist.DataBind();
            }
        }

        protected void btn_serachuser_ServerClick(object sender, EventArgs e)
        {
            if (txt_usersearch.Text.Trim() == string.Empty)
                GetUserList();
            else
            {
                chk_ModuleList.Visible = true;
                if (rd_spoc.Checked == true)
                {
                    gv_userlist.DataSource = null;
                    dtTemp = objSQL.DataTable_return("select sm1.SPOCID, sm1.email, Case when `Current Assigned Modules` is null then 'No Modules Assigned' else `Current Assigned Modules` end as `Assigned Modules`  from " +
                        "(select sm.SPOCID, sm.email, GROUP_CONCAT(mm.modulename) as `Current Assigned Modules` from " +
                        "spocmaster sm   join tbl_usermodulemap um on sm.SPOCID = um.userid " +
                         "join tbl_modulemaster mm on um.moduleid = mm.moduleid where um.isactive=1 " +
                        "group by um.userid) t  right join  spocmaster sm1 on t.SPOCID = sm1.SPOCID where sm1.email like '%" + txt_usersearch.Text.Trim() + "%' ; ");

                    gv_userlist.DataSource = dtTemp;
                    gv_userlist.DataBind();

                    dtTemp = objSQL.DataTable_return("select * from tbl_modulemaster where modulegroup in ('ALL','SPOC');");
                    chk_ModuleList.DataSource = dtTemp;
                    chk_ModuleList.DataTextField = "modulename";
                    chk_ModuleList.DataValueField = "moduleid";
                    chk_ModuleList.DataBind();
                }
                if (rd_approver.Checked == true)
                {
                    gv_userlist.DataSource = null;
                    dtTemp = objSQL.DataTable_return("select sm1.ApprovalID, sm1.ApprovalEmail, Case when `Current Assigned Modules` is null then 'No Modules Assigned' else `Current Assigned Modules` end as `Assigned Modules`  from " +
                    "(select sm.ApprovalID, sm.ApprovalEmail, GROUP_CONCAT(mm.modulename) as `Current Assigned Modules` from " +
                    "salesorderapprovalmaster sm   join tbl_usermodulemap um on sm.ApprovalID = um.userid " +
                    " join tbl_modulemaster mm on um.moduleid = mm.moduleid  where um.isactive=1 " +
                    "group by um.userid) t  right join  salesorderapprovalmaster sm1 on t.ApprovalID = sm1.ApprovalID where sm1.ApprovalEmail like '%" + txt_usersearch.Text.Trim() + "%'; ");

                    gv_userlist.DataSource = dtTemp;
                    gv_userlist.DataBind();

                    dtTemp = objSQL.DataTable_return("select * from tbl_modulemaster where modulegroup in ('ALL','APPROVER');");
                    chk_ModuleList.DataSource = dtTemp;
                    chk_ModuleList.DataTextField = "modulename";
                    chk_ModuleList.DataValueField = "moduleid";
                    chk_ModuleList.DataBind();
                }
            }
            txt_usersearch.Text = string.Empty;
        }

        protected void btn_SetUserModuleAccess_ServerClick(object sender, EventArgs e)
        {
            if (chk_ModuleList.Visible == true)
            {
                foreach (GridViewRow gr in gv_userlist.Rows)
                {
                    objSQL.ExecuteNonQuery_IUD("update tbl_usermodulemap set isactive=0 where userid=" + gr.Cells[0].Text + ";");
                    foreach (ListItem cb in chk_ModuleList.Items)
                    {
                        if (cb.Selected)
                            objSQL.ExecuteNonQuery_IUD("insert into tbl_usermodulemap (userid, moduleid, byuserid, mapdatetime, isactive) value(" + gr.Cells[0].Text + "," + cb.Value + "," + cb.Value + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1);");
                    }
                }
                chk_ModuleList.Visible = false;
                GetUserList();
                txt_usersearch.Text = string.Empty;
                lbl_Message.Attributes.Add("style", "color:green; font-weight:bold;");
                lbl_Message.InnerText = "Yaay! Transaction Approve / Reject Updated Successfully";
            }
        }
    }
}