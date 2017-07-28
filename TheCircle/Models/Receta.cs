using System;
using System.ComponentModel.DataAnnotations;


namespace TheCircle.Models
{
    public class Receta
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fCaducidad { get; set; }
        public int despachada { get; set; }
        public int eliminada { get; set; }
        public int idDoctor { get; set; }
        public int idApadrinado { get; set; }


        public Receta() { }
    }

    public class ItemReceta
    {
        [Key]
        public int id { get; set; }
        public int idItemFarmacia { get; set; }
        public string diagnostico { get; set; }
        public int cantidad { get; set; }
        public int idReceta { get; set; }
        public DateTime fecha { get; set; }
        public string posologia { get; set; }
        public int funciono { get; set; }

        public ItemReceta() { }
    }

    public class ItemRecetaNuevo
    {
        public int idItemFarmacia { get; set; }
        public int idDiagnostico { get; set; }
        public int cantidad { get; set; }
        public int posologia { get; set; }

        public ItemRecetaNuevo() { }
    }

    public class RecetaNueva
    {
        public int idDoctor { get; set; }
        public int idApadrinado { get; set; }

        public RecetaNueva() { }
    }

    public class RecetaNuevaItems
    {
        public int idReceta { get; set; }
        public ItemRecetaNuevo[] items { get; set; }

        public RecetaNuevaItems() { }
    }
}
