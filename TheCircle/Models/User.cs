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
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
        public int cedula { get; set; }
        public string clave_hash { get; set; }
        public string salt { get; set; }

        public User() {}

        public User get(LoginRequest req, MyDbContext _context)
        {
            string query = $"EXEC dbo.select_User @cedula={req.cedula}";
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
            string query = $"EXEC dbo.select_User @cedula={cedula}";
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

        public void crear(int cedula, string clave, MyDbContext _context)
        {
            try {
                var _signer = new Signature();
                var dic = _signer.hashing_SHA256(clave);
                string hash = dic["hash"];
                string salt = dic["salt"];

                string q = $"EXEC dbo.insert_User @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                _context.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al crear usuario at User.create");
            }
        }

        public void activar(string cedula, MyDbContext _context)
        {
            try {
                string q = $"EXEC dbo.update_User_activar @cedula={cedula}";
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw new Exception("Error al activar usuario at User.activar");
            }
        }

        public void desactivar(string cedula, MyDbContext _context)
        {
            try {
                string q = $"EXEC dbo.update_User_desactivar @cedula={cedula}";
                _context.Database.ExecuteSqlCommand(q);
            } catch (Exception e) {
                throw new Exception("Error al desactivar usuario at User.desactivar");
            }
        }

        public void cambiar_clave(int cedula, ,string nuevaclave, string antiguaClave, MyDbContext _context)
        {
            try {
                var _signer = new Signature();

                _checkClave(cedula, antiguaClave, _context);

                var dic = _signer.hashing_SHA256(nuevaclave);
                string hash = dic["hash"];
                string salt = dic["salt"];
                string q = $"EXEC dbo.update_User_clave @cedula={cedula}, @clave_hash='{hash}', @salt='{salt}'";
                
                _context.Database.ExecuteSqlCommand(q);

            } catch (Exception e) {
                throw new Exception("Error al cambiar clave de usuario at User.cambiar_clave");
            }
        }

        public void reset_clave(int cedula, ,string email, MyDbContext _context)
        {
            try {
                var _signer = new Signature();
                //var _mailer = new EmailTC(email);

                string nueva_clave = _signer.random();
                var dic = _signer.hashing_SHA256(nueva_clave);
                string hash = dic["hash"];
                string salt = dic["salt"];

                string q = $"EXEC dbo.update_User_reset @cedula={cedula}, @email='{email}', @clave_hash='{hash}', @salt='{salt}'";
                _context.Database.ExecuteSqlCommand(q);
                //_mailer.send("Reseteo de clave", $"Su nueva clave en TheCircle es {nueva_clave}");

            } catch (Exception e) {
                throw new Exception("Error al resetear clave de usuario at User.reset_clave");
            }
        }
    }
}
