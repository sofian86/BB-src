using System.Web.Mvc.Html;
using System.Web.Mvc;

public static class HtmlHelpers
{
    public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
    {
        var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
        var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

        var builder = new TagBuilder("li")
        {
            InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
        };

        if (controllerName == currentController && actionName == currentAction)
            builder.AddCssClass("active");

        return new MvcHtmlString(builder.ToString());
    }
}