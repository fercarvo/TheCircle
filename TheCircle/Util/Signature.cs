using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace TheCircle.Util
{
    public class Signature
    {
        private byte[] key;
        private Byte[10] salt; //= new Byte[50];
        private HMACSHA256 sign_algorithm;
        private SHA256 hash_algorithm;
        private Encoding encode;

        //ALERTA, cambiar el string del key generado deriva en anulacion de todos los tokens generados
        public Signature(){
            Encoding enconde = Encoding.Unicode;
            byte[] key = encode.GetBytes("ThECircle_signUnik3");

            this.key = key;
            this.salt = new Byte[10];
            this.sign_algorithm = new HMACSHA256(key);
            this.hash_algorithm = SHA256.Create();
            this.encode = encode;
        }

        public Dictionary<string, string> hashing_SHA256(string clave){
            try {
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

            } catch (Exception e) {
                throw new Exception("Error de hashing clave at Signature.hashing_SHA256");
            }
        }

        public string sign_HMAC(string data) {
            try {
                byte[] data_Byte = this.encode.GetBytes(data);
                byte[] data_Byte_HMAC = this.sign_algorithm.ComputeHash(data_Byte);
                string data_Byte_HMAC_Base = Convert.ToBase64String(data_Byte_HMAC);
                return data_Byte_HMAC_Base;

            } catch (Exception e) {
                throw new Exception("Error generando HMAC, Signature.sign_HMAC");
            }
        }

        //Recibe un string y verifica su hash con hashToCheck usando la salt
        public void check_hashing(string clave, string hashToCheck, string salt) {

            string data = $"{salt},{clave}";
            byte[] data_Byte = this.encode.GetBytes(toCheck);
            byte[] data_Byte_Hash = this.hash_algorithm.ComputeHash(data_Byte); //Hash resultante
            string data_Byte_Hash_base = Convert.ToBase64String(data_Byte_Hash);

            if (hashToCheck != data_Byte_Hash_base)
                throw new Exception("Clave erronea at Signature.check_hashing");
        }

        //Recibe un string y verifica su HMAC con hmacToCheck
        public bool checkHMAC(string data, string hmacToCheck) {
            try {
                byte[] data_Byte = this.encode.GetBytes(data);
                byte[] data_Byte_HMAC = this.algorithm.ComputeHash(data_Byte); //Signature resultante
                string data_Byte_HMAC_Base = Convert.ToBase64String(data_Byte_HMAC);

                if (hmacToCheck == data_Byte_HMAC_Base)
                    return true;
                return false;

            } catch (Exception e) {
                throw new Exception("Error al verificar HMAC, Signature.checkHMAC");
            }
        }

        //Genera un clave aleatoria
        public string random() {
            var bytes = new Byte[2];
            var randomNumber = new Random();
            randomNumber.NextBytes(bytes);
            var bytes_int = BitConverter.ToInt32(bytes);

            string resultado = $"{bytes_int}";
            return resultado;
        }

    }
}
