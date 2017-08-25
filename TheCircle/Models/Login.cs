using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace TheCircle.Models
{

    public class Token
    {
        public Token(Data payload, string sign)
        {
            this.payload = payload;
            this.sign = sign;
        }

        public Data payload { get; set; }
        public string sign { get; set; }

        public override string ToString()
        {
            return $"{{'payload':'{payload}','sign':'{sign}'}}";
        }

    }

    public class Data
    {
        public string cedula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public string localidad { get; set; }
        public string path { get; set; }
        public DateTime issueAt { get; set; }
        public DateTime expireAt { get; set; }

        public override string ToString()
        {
            string data = "";
            var properties = typeof(Data).GetProperties();

            foreach(PropertyInfo p in properties) {
                if (properties.Last() == p) {
                    if (typeof(ICollection<>).IsAssignableFrom(p.PropertyType)) {
                        data = data + $"{{'{p.Name}':[";
                        var collection = (IEnumerable) p.GetValue(this, null);

                        foreach (var item in collection) {
                            if (collection.Last() == item) {
                                data = data + $"'{item}'";
                            } else {
                                data = data + $"'{item}',";
                            }
                        }
                        data = data + "]";
                    } else {
                        data = data + $"{{'{p.Name}':'{p.GetValue(this, null)}'}},";
                    }
                } else {
                    if (typeof(ICollection<>).IsAssignableFrom(p.PropertyType)) {
                        data = data + $"{{'{p.Name}':[";
                        var collection = (IEnumerable) p.GetValue(this, null);

                        foreach (var item in collection) {
                            if (collection.Last() == item) {
                                data = data + $"'{item}'";
                            } else {
                                data = data + $"'{item}',";
                            }
                        }
                        data = data + "]";
                    } else {
                        data = data + $"{{'{p.Name}':'{p.GetValue(this, null)}'}}";
                    }
                }
            }
            return "{" + data + "}";
            //return $"{{'cedula':'{this.cedula}','nombres':'{this.nombres}','apellidos':'{this.apellidos}','issueAt':'{this.issueAt}'}}";
        }
    }

    public class LoginRequest
    {
        [BindRequired]
        public string cedula { get; set; }
        [BindRequired]
        public string clave { get; set; }
        [BindRequired]
        public string localidad { get; set; }
    }


    internal class AuthorizeTheCircle
    {
        public string policy { get; set; }
        public AuthorizeTheCircle() { }

        internal bool validate(IRequestCookieCollection cookies, string v)
        {
            return true;
        }
    }


}
