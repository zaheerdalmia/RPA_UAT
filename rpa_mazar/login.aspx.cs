using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;

namespace rpa_mazar
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
                string newURL = string.Concat(ConfigurationManager.AppSettings["ida:Rprealm"], "?url=", ReturnUrl);
                HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = newURL },
                  WsFederationAuthenticationDefaults.AuthenticationType);


                //HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = ConfigurationManager.AppSettings["ida:Rprealm"] },
                // CookieAuthenticationDefaults.AuthenticationType);

            }
        }
    }
}