using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TheCircle.Util
{
    internal class AllowAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public AllowAttribute(params string[] cargos)
        {
            this.cargos = cargos;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var http = filterContext.HttpContext;

            try
            {
                filterContext.ActionArguments["token"] = new Token().check(http.Request, cargos);
                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                filterContext.ActionArguments["token"] = null;
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
