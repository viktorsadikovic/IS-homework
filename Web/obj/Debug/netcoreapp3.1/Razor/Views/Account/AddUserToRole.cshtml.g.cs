#pragma checksum "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9da68a43974becb80656ad04fbee227d238c95b0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_AddUserToRole), @"mvc.1.0.view", @"/Views/Account/AddUserToRole.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\_ViewImports.cshtml"
using Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\_ViewImports.cshtml"
using Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9da68a43974becb80656ad04fbee227d238c95b0", @"/Views/Account/AddUserToRole.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"74b0619e1a302f0598271da1847e697c39d57b88", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_AddUserToRole : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Domain.Identity.AddToRoleModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml"
  
    ViewBag.Title = "AddUserToRole";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>Manage User Roles</h2>\r\n\r\n");
#nullable restore
#line 8 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml"
 using (Html.BeginForm("AddUserToRole", "Account"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"form-group\">\r\n    ");
#nullable restore
#line 11 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml"
Write(Html.DropDownListFor(m => m.selectedUser, new SelectList(Model.users,"Email","Email"), new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n</div>\r\n    <div class=\"form-group\">\r\n        ");
#nullable restore
#line 15 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml"
   Write(Html.DropDownListFor(m => m.selectedRole, new SelectList(Model.roles), new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n    <button type=\"submit\">Add</button>\r\n");
#nullable restore
#line 18 "C:\Users\Sadikovic\source\repos\181042Homework\Web\Views\Account\AddUserToRole.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Domain.Identity.AddToRoleModel> Html { get; private set; }
    }
}
#pragma warning restore 1591