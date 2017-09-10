using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace TheCircle.Util
{
    public class Signature
    {
        static byte[] key = encode.GetBytes("ThECircle_signUnik3afsasdaqwdasd");
        static Byte[] salt = new Byte[10]; //= new Byte[50];
        static HMACSHA512 sign_algorithm = new HMACSHA512(key);
        static SHA256 hash_algorithm = SHA256.Create() //Cambiar el algoritmo invalida TODAS LAS CLAVES EN LA BDD, INCLUSO SISTEMA
        static Encoding encode = Encoding.Unicode;

        //ALERTA, cambiar el string del key generado deriva en anulacion de todos los tokens generados
        //public Signature(){
            //key = encode.GetBytes("ThECircle_signUnik3afsasdaqwdasd");
            //this.salt = new Byte[10];
            //sign_algorithm = new HMACSHA512(key);
            //this.hash_algorithm = SHA256.Create();            
        //}

        public static Dictionary<string, string> HashingSHA256(string clave)
        {
            var dic = new Dictionary<string, string>(); //Contendra salt y el hash de la clave
            Random randomNumber = new Random();
            randomNumber.NextBytes(this.salt);
            string salt_Base = Convert.ToBase64String(this.salt); //Ramdon salt string

            string data = $"{salt_Base},{clave}";
            byte[] data_Byte = this.encode.GetBytes(data);
            byte[] data_Byte_Hash = this.hash_algorithm.ComputeHash(data_Byte);
            string data_Byte_Hash_base = Convert.ToBase64String(data_Byte_Hash);

            dic.Add("salt", salt_Base);
            dic.Add("hash", data_Byte_Hash_base);

            return dic;
        }

        //Recibe un string, retorna la firma de ese string
        public static string Sign(string data) 
        {
            byte[] data_Byte = this.encode.GetBytes(data);
            byte[] data_Byte_HMAC = this.sign_algorithm.ComputeHash(data_Byte);
            string data_Byte_HMAC_Base = Convert.ToBase64String(data_Byte_HMAC);
            return data_Byte_HMAC_Base;
        }

        //Recibe un string y verifica su hash con hashToCheck usando la salt
        public static void CheckHashing(string clave, string hashToCheck, string salt) 
        {
            string data = $"{salt},{clave}";
            byte[] data_Byte = this.encode.GetBytes(data);
            byte[] data_Byte_Hash = this.hash_algorithm.ComputeHash(data_Byte); //Hash resultante
            string data_Byte_Hash_base = Convert.ToBase64String(data_Byte_Hash);

            if (hashToCheck != data_Byte_Hash_base)
                throw new Exception("Clave erronea at Signature.check_hashing");
        }

        //Recibe un string y verifica su HMAC con hmacToCheck
        public static void CheckHMAC(string data, string hmacToCheck) 
        {
            byte[] data_Byte = this.encode.GetBytes(data);
            byte[] data_Byte_HMAC = this.sign_algorithm.ComputeHash(data_Byte); //Signature resultante
            string data_Byte_HMAC_Base = Convert.ToBase64String(data_Byte_HMAC);

            if (hmacToCheck != data_Byte_HMAC_Base)
                throw new TokenException("Signature no concuerda, data alterada");
        }

        //Genera un clave aleatoria
        public static string Random() {
            var bytes = new Byte[2];
            var randomNumber = new Random();
            randomNumber.NextBytes(bytes);
            var bytes_int = BitConverter.ToUInt16(bytes, 0);

            string resultado = $"{bytes_int}";
            return resultado;
        }

        
        public static string ToBase(string text) {
            byte[] text_Byte = this.encode.GetBytes(text);
            string text_Byte_Base = Convert.ToBase64String(text_Byte);
            return text_Byte_Base;
        }

        public static string FromBase(string base64) {
            byte[] base64_Byte = Convert.FromBase64String(base64);
            string base64_Byte_String = this.encode.GetString(base64_Byte);
            return base64_Byte_String;
        }
    }
}
