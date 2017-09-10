using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheCircle.Util;

namespace TheCircle.Models
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
        public int cedula { get; set; }
        public string clave_hash { get; set; }
        public string salt { get; set; }
        public Boolean? activo { get; set; }

        public User () {}

        public User (string cedula, string clave, MyDbContext _context) 
        {
            var dic = Signature.HashingSHA256(clave);
            string hash = dic["hash"];
            string salt = dic["salt"];

            string q = $"EXEC dbo.User_Insert @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
            _context.Database.ExecuteSqlCommand(q);
        }

        public static User Get(LoginRequest req, MyDbContext _context)
        {
            var user = _context.User.FromSql($"EXEC dbo.User_select @cedula={req.cedula}").First();
            Signature.CheckHashing(req.clave, user.clave_hash, user.salt);

            return user;
        }


        private void _checkClave(string cedula, string clave, MyDbContext _context) 
        {
            string query = $"EXEC dbo.User_Select @cedula={cedula}";

            var user = _context.User.FromSql(query).First();

            var hash = user.clave_hash;
            var salt = user.salt;

            Signature.CheckHashing(clave, hash, salt);
        }


        /*public static void New(string cedula, string clave, MyDbContext _context)
        {
            var dic = Signature.HashingSHA256(clave);
            string hash = dic["hash"];
            string salt = dic["salt"];

            string q = $"EXEC dbo.User_Insert @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
            _context.Database.ExecuteSqlCommand(q);
        }*/


        public static void Activar(int cedula, MyDbContext _context)
        {
            string q = $"EXEC dbo.User_Update_activar @cedula={cedula}";
            _context.Database.ExecuteSqlCommand(q);
        }


        public void desactivar(int cedula, MyDbContext _context)
        {
            string q = $"EXEC dbo.User_Update_desactivar @cedula={cedula}";
            _context.Database.ExecuteSqlCommand(q);
        }


        public void cambiar_clave(string cedula, string antiguaClave, string nuevaclave, MyDbContext _context)
        {
            _checkClave(cedula, antiguaClave, _context);

            var dic = Signature.HashingSHA256(nuevaclave);
            string hash = dic["hash"];
            string salt = dic["salt"];
            string q = $"EXEC dbo.User_Update_clave @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                
            _context.Database.ExecuteSqlCommand(q);
        }
        

        public static string NuevaClave(int cedula, MyDbContext _context)
        {
            //var _mailer = new EmailTC(email);
            string nueva_clave = Signature.Random();
            var dic = Signature.HashingSHA256(nueva_clave);
            string hash = dic["hash"];
            string salt = dic["salt"];

            string q = $"EXEC dbo.User_Update_clave @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
            _context.Database.ExecuteSqlCommand(q);

            return nueva_clave;
            //_mailer.send("Reseteo de clave", $"Su nueva clave en TheCircle es {nueva_clave}");
        }
    }


    public class UserSafe
    {
        [Key]
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
        public int cedula { get; set; }

        public static UserSafe[] GetAll(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_All";

            var user = _context.UserSafe.FromSql(query).ToArray();
            return user;
        }

        public static UserSafe[] GetActivos(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_Activos";

            var user = _context.UserSafe.FromSql(query).ToArray();
            return user;
        }

        public static UserSafe[] GetInactivos(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_Inactivos";

            var user = _context.UserSafe.FromSql(query).ToArray();
            return user;
        }
    }

    public class Clave
    {
        public string cedula { get; set; }
        public string actual { get; set; }
        public string nueva { get; set; }
    }
}
