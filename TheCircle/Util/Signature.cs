using System;
using System.Security.Cryptography;
using System.Text;

namespace TheCircle.Util
{
    public class Signature
    {
        private string salt;
        private SHA256 algorithm;
        private Encoding encode;

        //ATENCIOM, CAMBIAR salt, algorithm y encode causaran una invalidacion de toda la data firmada por esta clase
        public Signature() {
            salt = "TheC1rCl3-Bla12347&%$#blablabla1614_d8g4df64gd"; 
            algorithm = SHA256.Create();
            encode = Encoding.Unicode;
        }

        public string sign(string data)
        {
            try {
                string toSign = $"{data},{this.salt}";

                byte[] toSignByte = this.encode.GetBytes(toSign);
                byte[] hashedByte = this.algorithm.ComputeHash(toSignByte);

                string hashed = Convert.ToBase64String(hashedByte);
                return hashed;

            } catch (Exception e) {
                return null;
            }            
        }

        public bool check(string dataToCheck, string hashedToCheck)
        {
            try
            {
                string toCheck = $"{dataToCheck},{this.salt}";

                byte[] toCheck_Byte = this.encode.GetBytes(toCheck);
                byte[] hashed_toCheck_Byte = this.algorithm.ComputeHash(toCheck_Byte); //Hash resultante
                string resultante = Convert.ToBase64String(hashed_toCheck_Byte);

                if (hashedToCheck == resultante)
                    return true;
                return false;

            } catch (Exception e) {
                return false;
            }
        }
    }
}
