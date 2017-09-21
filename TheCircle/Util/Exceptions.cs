using Microsoft.AspNetCore.Mvc;
using System;

namespace TheCircle.Util
{
    internal class TokenException : Exception
    {
        public TokenException() { }

        public TokenException(string message) : base(message) { }

        public TokenException(string message, Exception innerException) : base(message, innerException) { }
    }

    internal class LocalidadException : Exception
    {
        public LocalidadException() { }

        public LocalidadException(string message) : base(message) { }

        public LocalidadException(string message, Exception innerException) : base(message, innerException) { }
    }
}




namespace TheCircle.Controllers
{
    [Produces("application/json")]
    public class ErrorController : Controller
    {
        public ErrorController() { }

        [Route("error")]
        public IActionResult Error()
        {
            return StatusCode(500);
        }
    }
}