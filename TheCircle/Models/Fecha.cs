using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

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

    public class Date
    {
        public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
    }
}
