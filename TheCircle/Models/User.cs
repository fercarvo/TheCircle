using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheCircle.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
        public int cedula { get; set; }

        public User get(LoginRequest req, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_User @cedula={req.cedula}, @clave='{req.clave}'";
            try
            {
                var data = _context.User.FromSql(query).First();
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("Error cargar User, User.get");
            }

        }

    }
}
