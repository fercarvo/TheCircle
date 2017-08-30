using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheCircle.Models
{

    public class Fecha
    {
        [BindRequired]
        public string desde { get; set; }
        [BindRequired]
        public string hasta { get; set; }

        public Fecha() { }
    }
}
