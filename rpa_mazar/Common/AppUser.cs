using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace rpa_mazar.Common
{
    public static class AppUser
    {
        static MySQLHelper objSQL = new MySQLHelper();
        static DataTable dtTempUserAccess = null;
        static string userIDs = string.Empty;
        public static DataTable AccessModules(string UserEmail, string spoMasterTable= "spocmaster",string salesOrderApprovalMaster= "salesorderapprovalmaster")
        //public static DataTable AccessModules(string UserEmail, string spoMasterTable, string salesOrderApprovalMaster)
        {
            try
            {
                userIDs = objSQL.DataTable_return("select SPOCID from "+ spoMasterTable + " where Email='" + UserEmail + "';").Rows[0][0].ToString();
            }
            catch
            {
                userIDs = string.Empty;
            }

            try
            {
                if (userIDs == string.Empty)
                    userIDs = objSQL.DataTable_return("select ApprovalID from "+ salesOrderApprovalMaster + " where ApprovalEmail='" + UserEmail + "';").Rows[0][0].ToString();
                else
                    userIDs = userIDs + "," + objSQL.DataTable_return("select ApprovalID from " + salesOrderApprovalMaster + " where ApprovalEmail='" + UserEmail + "';").Rows[0][0].ToString();
            }
            catch
            {
                //userIDs = string.Empty;
            }

            if (userIDs != string.Empty)
            {
                dtTempUserAccess = objSQL.DataTable_return("select mm.moduleid,mm.modulename,mm.moduleelementid,mm.modulepagename from tbl_modulemaster mm, tbl_usermodulemap um where mm.moduleid=um.moduleid and um.userid in (" + userIDs +") and um.isactive=1;");
            }

            return dtTempUserAccess;
            
        }

    }
}