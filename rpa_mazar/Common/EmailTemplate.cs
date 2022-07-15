using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpa_mazar.Common
{
    public static class EmailTemplate
    {
        public static string UploadEmail="<div class='canvas' >" +
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout:fixed;background-color:#F9F9F9;' id='bodyTable'>" +
            "<tbody><tr>" +
            "<td align='center' valign='top' style='padding-right:10px;padding-left:10px;' id='bodyCell'>" +
            "<table border='0' cellpadding='0' cellspacing='0' style='max-width:600px;' width='100%' class='wrapperWebview'>" +
            "<tbody></tbody></table>" +
            "<table border='0' cellpadding='0' cellspacing='0' style='max-width:600px;' width='100%' class='wrapperWebview'>" +
            "<tbody><tr><td align='center' valign='top'>" +
            "<table border='0' cellpadding='0' cellspacing='0' width='100%'><tbody><tr>" +
            "<td align='center' valign='middle' style='padding-top: 40px; padding-bottom: 40px;' class='emailLogo'>" +
            "<a href='#' target='_blank' style='text-decoration:none;'>" +
            "<img src='https://adfs.dalmiabharat.com/adfs/portal/logo/logo.jpg' alt='' width='150' border='0' style='width:100%; max-width:150px;'>" +
            "</a></td></tr></tbody></table></td></tr></tbody></table>" +
            "<table border='0' cellpadding='0' cellspacing='0' style='max-width:600px;' width='100%' class='wrapperBody'>" +
            "<tbody><tr><td align='center' valign='top'>" +
            "<table border='0' cellpadding='0' cellspacing='0' style='background-color:#FFFFFF;border-color:#E5E5E5; border-style:solid; border-width:0 1px 1px 1px;' width='100%' class='tableCard'>" +
            "<tbody><tr><td height='3' style='background-color:#003CE5;font-size:1px;line-height:3px;' class='topBorder'>&nbsp;</td>" +
            "</tr><tr><td align='center' valign='top' style='padding-bottom: 20px;' class='imgHero'>" +
            "<a href='#' target='_blank' style='text-decoration:none;'>" +
            "<img src='https://centralmdm.dalmiabharat.com/emailimages/user-welcome.png' width='600' alt='' border='0' style='width:100%; max-width:600px; height:auto; display:block;'>" +
            "</a></td></tr><tr><td align='center' valign='top' style='padding-bottom: 5px; padding-left: 20px; padding-right: 20px;' class='mainTitle'>" +
            "<h2 class='text' style='color:#000000; font-family:'Poppins', Helvetica, Arial, sans-serif; font-size:28px; font-weight:500; font-style:normal; letter-spacing:normal; line-height:36px; text-transform:none; text-align:center; padding:0; margin:0'>" +
            "Upload Successful!</h2></td></tr><tr>" +
            "<td align='center' valign='top' style='padding-bottom: 30px; padding-left: 20px; padding-right: 20px;' class='subTitle'>" +
            "<h4 class='text' style='color:#999999; font-family:'Poppins', Helvetica, Arial, sans-serif; font-size:16px; font-weight:500; font-style:normal; letter-spacing:normal; line-height:24px; text-transform:none; text-align:center; padding:0; margin:0'>" +
            "Request Number is: {{ReqNum}} and total requests is: {{Requests}}</h4></td></tr><tr>" +
            "<td align='center' valign='top' style='padding-left:20px;padding-right:20px;' class='containtTable ui-sortable'>" +
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' class='tableTitleDescription' style=''>" +
            "<tbody><tr><td align='center' valign='top' style='padding-bottom: 10px;' class='mediumTitle'>" +
            "<p class='text' style='color:#000000; font-family:'Poppins', Helvetica, Arial, sans-serif; font-size:18px; font-weight:500; font-style:normal; letter-spacing:normal; line-height:26px; text-transform:none; text-align:center; padding:0; margin:0'>" +
            "<a href='#' target='_blank' style='text-decoration:none;'>Click here to visit RPA Dashboard</a>" +
            "</p></td></tr></tbody></table></td></tr><tr>" +
            "<td height='20' style='font-size:1px;line-height:1px;'>&nbsp;</td></tr>" +
            "<tr><td align='center' valign='middle' style='padding-bottom: 40px;' class='emailRegards'>" +
            "<a href='#' target='_blank' style='text-decoration:none;'>" +
            "<img src='https://centralmdm.dalmiabharat.com/emailimages/signature.png' alt='' width='150' border='0' style='width:100%;max-width:150px; height:auto; display:block;'>" +
            "</a></td></tr></tbody></table>" +
            "<table border='0' cellpadding='0' cellspacing='0' width='100%' class='space'><tbody><tr>" +
            "<td height='30' style='font-size:1px;line-height:1px;'>&nbsp;</td></tr>" +
            "</tbody></table></td></tr></tbody></table></td></tr></tbody></table></div>";
    }
}