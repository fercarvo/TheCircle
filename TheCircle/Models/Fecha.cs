using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

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
