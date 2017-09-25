using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TheCircle.Util
{
    //Estas clases se ejecutan antes de cada route que contenga las mismas
    //Si la autenticación falla, se retorna un UnauthorizedResult o Redirect respectivamente
    internal class VIEWauthAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public VIEWauthAttribute(params string[] cargos) {
            this.cargos = cargos;
        }

        public VIEWauthAttribute()
        {
            cargos = new[] { "medico", "asistenteSalud", "sistema", "bodeguero", "coordinador", "contralor", "coordinadorCC", "director" };
        }

        public override void OnActionExecuting(ActionExecutingContext aec)
        {
            try
            {
                aec.ActionArguments["token"] = Token.Check(aec.HttpContext.Request, cargos);
                base.OnActionExecuting(aec);

            } catch (Exception e) {//Si el token es invalido se setea null
                aec.Result = new LocalRedirectResult("/login");
                base.OnActionExecuting(aec);
            }
        }
    }


    internal class APIauthAttribute : ActionFilterAttribute
    {
        private string[] cargos;

        public APIauthAttribute(params string[] cargos) {
            this.cargos = cargos;
        }

        public override void OnActionExecuting(ActionExecutingContext aec)
        {
            var http = aec.HttpContext;

            try
            {
                aec.ActionArguments["token"] = Token.Check(http.Request, cargos);
                base.OnActionExecuting(aec);

            } catch (Exception e) {//Si el token es invalido, termina la conexión.
                aec.Result = new UnauthorizedResult();
                base.OnActionExecuting(aec);
            }
        }
    }
}
