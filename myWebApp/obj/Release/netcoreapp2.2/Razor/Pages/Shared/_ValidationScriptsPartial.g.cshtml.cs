#pragma checksum "/Users/danielappelgren/Documents/GitHub/I4PRJ4/myWebApp/Pages/Shared/_ValidationScriptsPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "78307dc8f9e8b1fa80d7f3443df9c0a3dcecc87b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(myWebApp.Pages.Shared.Pages_Shared__ValidationScriptsPartial), @"mvc.1.0.view", @"/Pages/Shared/_ValidationScriptsPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Shared/_ValidationScriptsPartial.cshtml", typeof(myWebApp.Pages.Shared.Pages_Shared__ValidationScriptsPartial))]
namespace myWebApp.Pages.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/Users/danielappelgren/Documents/GitHub/I4PRJ4/myWebApp/Pages/_ViewImports.cshtml"
using myWebApp;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"78307dc8f9e8b1fa80d7f3443df9c0a3dcecc87b", @"/Pages/Shared/_ValidationScriptsPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"76edb56edcba8fdf7fa06cef916423d9602b8097", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared__ValidationScriptsPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 1144, true);
            WriteLiteral(@"<!-- 
<environment include=""Development"">
    <script src=""~/lib/jquery-validation/dist/jquery.validate.js""></script>
    <script src=""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js""></script>
</environment>
<environment exclude=""Development"">

    <script src=""https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js""
            asp-fallback-src=""~/lib/jquery-validation/dist/jquery.validate.min.js""
            asp-fallback-test=""window.jQuery && window.jQuery.validator""
            crossorigin=""anonymous""
            integrity=""sha256-F6h55Qw6sweK+t7SiOJX+2bpSAa3b/fnlrVCJvmEj1A="">
    </script>


    <script src=""https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.11/jquery.validate.unobtrusive.min.js""
            asp-fallback-src=""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js""
            asp-fallback-test=""window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive""
            crossorigin=""ano");
            WriteLiteral("nymous\"\n            integrity=\"sha256-9GycpJnliUjJDVDqP0UEu/bsm9U+3dnQUH8+3W10vkY=\">\n    </script>\n\n\n</environment>\n-->\n");
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
