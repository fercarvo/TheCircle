﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class Apadrinado
    {
        [Key]
        public Int32 id { get; set; }
        public string status { get; set; }
        public string sector { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public decimal edad { get; set; }
        public double peso { get; set; }
        public Int32 talla { get; set; }
        public Int16 income { get; set; }
        public int numBeds { get; set; }
        public int numPer { get; set; }
        public string posesionHogar { get; set; }
        public string responsable { get; set; }

        public Apadrinado() { }

        public static Apadrinado Get (int codigo, MyDbContext _context) 
        {
            string query = $"EXEC dbo.select_Apadrinado @cod={codigo}";
            var data = _context.Apadrinados.FromSql(query).First();
            return data;
        }
    }

    public class Foto
    {
        [Key]
        public string name { get; set; }
        public string path { get; set; }

        public Foto() { }
    }
}
