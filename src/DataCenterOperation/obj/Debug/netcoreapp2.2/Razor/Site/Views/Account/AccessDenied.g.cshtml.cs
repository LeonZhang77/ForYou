#pragma checksum "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/Account/AccessDenied.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aae48ade9a25677fe7c434be6bbf704a8ec4097b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Site_Views_Account_AccessDenied), @"mvc.1.0.view", @"/Site/Views/Account/AccessDenied.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Site/Views/Account/AccessDenied.cshtml", typeof(AspNetCore.Site_Views_Account_AccessDenied))]
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
#line 1 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/_ViewImports.cshtml"
using DataCenterOperation;

#line default
#line hidden
#line 2 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/_ViewImports.cshtml"
using DataCenterOperation.Data.Entities;

#line default
#line hidden
#line 3 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/_ViewImports.cshtml"
using DataCenterOperation.Site.ViewModels;

#line default
#line hidden
#line 4 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/_ViewImports.cshtml"
using DataCenterOperation.Site.Extensions;

#line default
#line hidden
#line 5 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/_ViewImports.cshtml"
using DataCenterOperation.Services;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aae48ade9a25677fe7c434be6bbf704a8ec4097b", @"/Site/Views/Account/AccessDenied.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b4baad81b80067f0813e7ab64ffa6f7d904464a7", @"/Site/Views/_ViewImports.cshtml")]
    public class Site_Views_Account_AccessDenied : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "/Users/leonzhang/git/ForYou/src/DataCenterOperation/Site/Views/Account/AccessDenied.cshtml"
  
    ViewData["Title"] = "Access Denied";

#line default
#line hidden
            BeginContext(46, 140, true);
            WriteLiteral("\n<header>\n    <h2 class=\"text-danger\">Access Denied.</h2>\n    <p class=\"text-danger\">You do not have access to this resource.</p>\n</header>\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
