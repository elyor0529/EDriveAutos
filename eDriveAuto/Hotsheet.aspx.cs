using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive
{
    public partial class Hotsheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Dealer"))
                {
                    Response.Redirect("~/Dealer/ManageVehicle/Hotsheet");
                }
                if (User.IsInRole("Admin"))
                {
                    Response.Redirect("~/Admin/ManageVehicle/Hotsheet");
                }
            }
            Response.Redirect("Account/Logon");
        }
    }
}