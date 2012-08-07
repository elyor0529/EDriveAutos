using System;
using System.Linq;
using System.Web.Mvc;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
    class CustomControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                return base.CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                try
                {
                    using (eDriveAutoWebEntities _entity = new Models.eDriveAutoWebEntities())
                    {
                        if (_entity.Nop_Topic.Any(m => m.Name == controllerName))
                        {
                            requestContext.RouteData.Values["id"] = controllerName;

                            requestContext.RouteData.Values["controller"] = "Home";
                            requestContext.RouteData.Values["action"] = "Topic";
                            requestContext.RouteData.Values["id"] = controllerName;


                            return base.CreateController(requestContext, "Home");
                        }
                        else
                        {
                            return base.CreateController(requestContext, controllerName);
                        }
                    }
                }
                catch (Exception ex1)
                {
                    return base.CreateController(requestContext, controllerName);
 
                }


               
            }
        }


    }
}