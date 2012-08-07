using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Edrive.Edrivie_Service_Ref;
using Edrive.Models;

namespace Edrive.CommonHelpers
{
    public static class HelperExtensions
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, SelectList lstSelect, String propertyName, object htmlAttributes = null)
        {


            List<string> selectedValues = new List<string>();
            //Create div
            TagBuilder divTag = new TagBuilder("div id='" + propertyName + "'");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (SelectListItem item in lstSelect)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                  "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                  propertyName,
                                                  item.Value,

                                                  (lstSelect.SelectedValue != null ? lstSelect.SelectedValue.ToString() == item.Value.ToString() : false) ? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, List<SelectListItem> lstSelect, String propertyName, object htmlAttributes = null)
        {


            List<string> selectedValues = new List<string>();
            //Create div
            TagBuilder divTag = new TagBuilder("div id='" + propertyName + "'");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (SelectListItem item in lstSelect)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                  "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                  propertyName,
                                                  item.Value,

                                                  (item.Selected) ? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, SelectList lstSelect, 
                                                    String propertyName, List<string> selectedValues=null,object htmlAttributes = null  )
        {

            if(selectedValues==null)
                selectedValues = new List<string>();
            selectedValues.Add("-1");
            //Create div
            TagBuilder divTag = new TagBuilder("div id='" + propertyName + "'");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (SelectListItem item in lstSelect)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"radio\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                  "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                  propertyName,
                                                  item.Value,
                                                  selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }
         
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,List<SelectListItem> lstSelect, 
                                                    String propertyName,  object htmlAttributes = null  )
        {

        
            //Create div
            TagBuilder divTag = new TagBuilder("div id='" + propertyName + "'");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (SelectListItem item in lstSelect)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"radio\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                  "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                  propertyName,
                                                  item.Value,
                                                  item.Selected? "checked=\"checked\"" : "",
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }
    

        public static string ActionValidationSummary(this HtmlHelper html, string action)
        {
            string currentAction = html.ViewContext.RouteData.Values["action"].ToString();

            if (currentAction.ToLower() == action.ToLower())
                return html.ValidationSummary().ToHtmlString();

            return string.Empty;
        }

		public static T ConvertTo<T>(this String input) where T : struct
		{
			return ConvertTo<T>(input, default(T));
		}

		public static T ConvertTo<T>(this String input, T defaultValue) where T : struct
		{
			try
			{
				return (T)Convert.ChangeType(input.Trim(), typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		} 
    }
    public class MyRemoteAttribute : RemoteAttribute
    {
        public MyRemoteAttribute(string action, string controller, string areaName)
            : base(action, controller, areaName)
        {
            this.RouteData["area"] = areaName;
        }
    }
}