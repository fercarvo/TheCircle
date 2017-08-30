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

        public User() {}

        public User get(LoginRequest req, MyDbContext _context)
        {
            string query = $"EXEC dbo.User_select @cedula={req.cedula}";
            var _signer = new Signature();

            try {
                var user = _context.User.FromSql(query).First();

                var clave = req.clave;
                var hash = user.clave_hash;
                var salt = user.salt;

                _signer.check_hashing(clave, hash, salt);

                return user;

            } catch (Exception e) {
                throw new Exception("Error cargar User at User.get");
            }
        }

        private void _checkClave(int cedula, string clave, MyDbContext _context) {
            string query = $"EXEC dbo.User_Select @cedula={cedula}";
            var _signer = new Signature();

            try {
                var user = _context.User.FromSql(query).First();

                var hash = user.clave_hash;
                var salt = user.salt;

                _signer.check_hashing(clave, hash, salt);

            } catch (Exception e) {
                throw new Exception("Clave/Usuario incorrecto");
            }
        }

        public void crear(string cedula, string clave, MyDbContext _context)
        {
            try {
                var _signer = new Signature();
                var dic = _signer.hashing_SHA256(clave);
                string hash = dic["hash"];
                string salt = dic["salt"];

                string q = $"EXEC dbo.User_Insert @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                _context.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear usuario at User.create");
            }
        }

        public void activar(int cedula, MyDbContext _context)
        {
            try {
                string q = $"EXEC dbo.User_Update_activar @cedula={cedula}";
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw new Exception("Error al activar usuario at User.activar");
            }
        }

        public void desactivar(int cedula, MyDbContext _context)
        {
            try {
                string q = $"EXEC dbo.User_Update_desactivar @cedula={cedula}";
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw new Exception("Error al desactivar usuario at User.desactivar");
            }
        }

        public void cambiar_clave(int cedula, string nuevaclave, string antiguaClave, MyDbContext _context)
        {
            try {
                var _signer = new Signature();

                _checkClave(cedula, antiguaClave, _context);

                var dic = _signer.hashing_SHA256(nuevaclave);
                string hash = dic["hash"];
                string salt = dic["salt"];
                string q = $"EXEC dbo.User_Update_clave @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                
                _context.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al cambiar clave de usuario at User.cambiar_clave");
            }
        }

        public string nueva_clave(int cedula, MyDbContext _context)
        {
            try {
                var _signer = new Signature();
                //var _mailer = new EmailTC(email);

                string nueva_clave = _signer.random();
                var dic = _signer.hashing_SHA256(nueva_clave);
                string hash = dic["hash"];
                string salt = dic["salt"];

                string q = $"EXEC dbo.User_Update_clave @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                _context.Database.ExecuteSqlCommand(q);

                return nueva_clave;
                //_mailer.send("Reseteo de clave", $"Su nueva clave en TheCircle es {nueva_clave}");

            } catch (Exception e) {
                throw new Exception("Error al resetear clave de usuario at User.nueva_clave");
            }
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

        public UserSafe[] getAll(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_All";

            try {
                var user = _context.UserSafe.FromSql(query).ToArray();
                return user;

            } catch (Exception e) {
                throw new Exception("Error cargar UserSafe at UserSafe.getAll");
            }
        }

        public UserSafe[] getActivos(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_Activos";

            try {
                var user = _context.UserSafe.FromSql(query).ToArray();
                return user;

            } catch (Exception e) {
                throw new Exception("Error cargar UserSafe at UserSafe.getActivos");
            }
        }

        public UserSafe[] getInactivos(MyDbContext _context)
        {
            string query = $"EXEC dbo.UserSafe_Report_Inactivos";

            try {
                var user = _context.UserSafe.FromSql(query).ToArray();
                return user;

            } catch (Exception e) {
                throw new Exception("Error cargar UserSafe at UserSafe.getInactivos");
            }
        }
    }

    public class Clave
    {
        public string actual { get; set; }
        public string nueva { get; set; }
    }

}
