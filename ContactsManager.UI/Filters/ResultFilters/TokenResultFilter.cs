﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ResultFilters
{
    public class TokenResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Token", "001A");
        }
    }
}
