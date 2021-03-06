﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, "Index");
        public static string BasicNavClass(ViewContext viewContext) => PageNavClass(viewContext, "Basic");

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, "ChangePassword");

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
