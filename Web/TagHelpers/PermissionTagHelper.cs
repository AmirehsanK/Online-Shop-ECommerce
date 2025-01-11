using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web.TagHelpers;

[HtmlTargetElement("*", Attributes = "invoke-permission")]
public class PermissionTagHelper(IPermissionService permissionService) : TagHelper
{
    [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContextData { get; set; }

    [HtmlAttributeName("invoke-permission")]
    public string PermissionName { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!ViewContextData.HttpContext.User.Identity!.IsAuthenticated)
        {
            output.SuppressOutput();
            return;
        }

        var userId = ViewContextData.HttpContext.User.GetCurrentUserId();
        if (userId == null)
        {
            output.SuppressOutput();
            return;
        }

        var permissions = PermissionName.Split("|").Select(n => n.Trim()).ToList();
        if (permissions == null || !permissions.Any())
            return;
        foreach (var permission in permissions)
        {
            if (await permissionService.CheckUserPermissionAsync(userId, permission)) break;
            output.SuppressOutput();
            return;
        }
    }
}