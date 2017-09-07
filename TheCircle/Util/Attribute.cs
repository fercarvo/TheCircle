using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TheCircle.Util
{
    internal class AllowAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public AllowAttribute(params string[] cargos) {
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


    internal class VIEWauthAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public AllowAttribute(params string[] cargos) {
            this.cargos = cargos;
        }

        public override void OnActionExecuting(ActionExecutingContext aec)
        {
            try
            {
                new Token().check(aec.HttpContext.Request, cargos);
                base.OnActionExecuting(aec);

            } catch (Exception e) {//Si el token es invalido se setea null
                //aec.Result = new Redirect("/");
                base.OnActionExecuting(aec);
            }
        }
    }


    internal class APIauthAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public AllowAttribute(params string[] cargos) {
            this.cargos = cargos;
        }

        public override void OnActionExecuting(ActionExecutingContext aec)
        {
            var http = aec.HttpContext;

            try
            {
                aec.ActionArguments["token"] = new Token().check(http.Request, cargos);
                base.OnActionExecuting(aec);

            } catch (Exception e) {//Si el token es invalido, termina la conexión.
                //aec.Result = new Unauthorize();
                base.OnActionExecuting(aec);
            }
        }
    }
}
