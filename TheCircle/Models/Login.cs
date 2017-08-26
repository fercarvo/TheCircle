using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace TheCircle.Models
{

    public class Token
    {
        public Data payload { get; set; }
        public string sign { get; set; }

        public Token(User user, string localidad) {
            payload = new Data(user, localidad);
            string payloadToString = JsonConvert.SerializeObject(payload); //Se convierte el payload a JsonString
            
        }

        public override string ToString()
        {
            return $"{{\"payload\":{payload},\"sign\":\"{sign}\"}}";
        }

        public string serialize(Token token) {

        }

        public string desSerialize(Token token) {

        }

    }

    public class Data
    {
        public int cedula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public string localidad { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }

        public Data (User u, string localidad) {
            cedula = u.cedula;
            nombres = u.nombre;
            apellidos = u.apellido;
            cargo = u.cargo;
            localidad = localidad;
            issueAt = DateTime.Now;
            expireAt = DateTime.Now.AddHours(10);
        }

        public override string ToString()
        {
            string data = "";
            var properties = typeof(Data).GetProperties();

            foreach(PropertyInfo p in properties) {
                if (properties.Last() == p) {
                    if (typeof(ICollection).IsAssignableFrom(p.PropertyType)) {
                        data = data + $"\"{p.Name}\":[";
                        var collection = (IEnumerable) p.GetValue(this, null);

                        var size = 0;
                        foreach (var item in collection) { size++; };

                        int flag = 1;

                        foreach (var item in collection) {
                            if (flag == size) {
                                data = data + $"\"{item}\"";
                            } else {
                                data = data + $"\"{item}\",";
                            }
                            flag++;
                        }
                        data = data + "]";
                    } else {
                        data = data + $"\"{p.Name}\":\"{p.GetValue(this, null)}\"";
                    }
                } else {
                    if (typeof(ICollection).IsAssignableFrom(p.PropertyType)) {
                        data = data + $"\"{p.Name}\":[";
                        var collection = (IEnumerable) p.GetValue(this, null);

                        var size = 0;
                        foreach (var item in collection) { size++; };
                        int flag = 1;

                        foreach (var item in collection) {
                            if (flag == size) {
                                data = data + $"\"{item}\"";
                            } else {
                                data = data + $"\"{item}\",";
                            }
                            flag++;
                        }
                        data = data + "],";
                    } else {
                        data = data + $"\"{p.Name}\":\"{p.GetValue(this, null)}\",";
                    }
                }
            }
            return "{" + data + "}";
        }
    }

    public enum Localidad { OC, CC0, CC2, CC3, CC5, CC6, HEE }
    public class LoginRequest
    {
        [BindRequired]
        public string cedula { get; set; }
        [BindRequired]
        public string clave { get; set; }
        [BindRequired]
        public Localidad localidad { get; set; }
    }
}
