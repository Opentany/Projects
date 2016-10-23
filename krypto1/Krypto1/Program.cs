using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Krypto1
{
    class Program
    {
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        static Random random = new Random();
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }
        static void Main(string[] args)
        {
            /*
            int i = 0;
            while (i>=0)
            {
                string hex = i.ToString("X8");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\Dokumenty\Uczelnia\Krypto\Keys.txt", true))
                {
                    file.WriteLine(hex);
                }
                i++;
            }
            */
            //*
            string line;
            System.IO.StreamReader file2 = new System.IO.StreamReader(@"E:\Dokumenty\Uczelnia\Krypto\Keys.txt");
            while ((line = file2.ReadLine()) != null)
            {
                try
                {

                    //string original = "Ala ma kota. Kot ma, ale... to jednak ona go posiada. Jednakże gdy przeczytamy to ponownie to...";

                    // Create a new instance of the Aes
                    // class.  This generates a new key and initialization 
                    // vector (IV).
                    string SufKey = "1fb7d76820734c94b97233a15022dd4cc2f827e7e2195685fdf848b4";
                    string Iv = "9022544f4cb454ec88880674f06dab4d";
                    string PreKey = line;
                    string Key = PreKey + SufKey;

                    byte[] byteKey = StringToByteArray(Key);
                    byte[] byteIV = StringToByteArray(Iv);
                    using (Aes myAes = Aes.Create())
                    {
                        myAes.KeySize = 256;
                        foreach (var VARIABLE in myAes.LegalKeySizes)
                        {
                            Console.WriteLine(VARIABLE.MaxSize);
                        }
                        // Encrypt the string to an array of bytes.
                        //byte[] encrypted = EncryptStringToBytes_Aes(original, byteKey, byteIV);
                        string input = "00011101001010110010010111111100101101011100110111101110001101111000100000101010111100011100100100101011100111011110001010101011111110100110111101110101110010011001001000101101110010001101011111101001010101000011111100000110001001110100100100100111110111001101101011001110011001100000101111110111010011000100101100011111101100101111011100101001010010100011101000000000001100100111001001000100010111111110100111000111110001000011001110110000011100100100110101101001011001010101110110100100101001100101011010010000";
                        int numOfBytes = input.Length / 8;

                        byte[] bytes = new byte[numOfBytes];
                        for (int i = 0; i < numOfBytes; ++i)
                        {
                            bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
                        }
                        byte[] encrypted = bytes;
                        // Decrypt the bytes to a string.
                        string roundtrip = DecryptStringFromBytes_Aes(encrypted, byteKey, byteIV);

                        //Display the original data and the decrypted data.
                        //Console.WriteLine("Original:   {0}", original);
                        //Console.WriteLine("Round Trip: {0}", roundtrip);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\Dokumenty\Uczelnia\Krypto\Dekrypto.txt", true))
                        {
                            file.WriteLine(roundtrip);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
                //Console.ReadKey();
                

            }
            //*/

        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                                                        // and place them in a string.
                                                        plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;

        }
    
    }
}
