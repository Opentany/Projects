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

        static void Main(string[] args)
        {
            string Test = "Test";
            using (
                                System.IO.StreamWriter file =
                                    new System.IO.StreamWriter(@"E:\Dokumenty\Uczelnia\Krypto\" + Test + ".txt",
                                        true))
            {
                file.WriteLine(DateTime.Now);
            }
            Bruteforce(0, 4294967296, Test);
            Console.WriteLine("Koniec "+ DateTime.Now);
        }

        static void Bruteforce(Int64 start, Int64 stop, string filename)
        {
            Parallel.For(start,stop,delegate(Int64 j)
            //for (Int64 j = start; j < stop; j++)

            {
                if (j% 1000000/*1073741824*/ == 0)
                {
                    Console.WriteLine("Klucz " + j + " " + DateTime.Now);
                    using (
                        System.IO.StreamWriter file =
                            new System.IO.StreamWriter(@"E:\Dokumenty\Uczelnia\Krypto\" + filename + ".txt",
                                true))
                    {
                        file.WriteLine("Klucz " + j + " " + DateTime.Now);
                    }
                }
                try
                {
                    //string original = "Ala ma kota. Kot ma, ale... to jednak ona go posiada. Jednakże gdy przeczytamy to ponownie to...";

                    // Create a new instance of the Aes
                    // class.  This generates a new key and initialization 
                    // vector (IV).
                    string hex = j.ToString("X8");
                    string SufKey = "1fb7d76820734c94b97233a15022dd4cc2f827e7e2195685fdf848b4";
                    string Iv = "9022544f4cb454ec88880674f06dab4d";
                    string PreKey = hex;
                    string Key = PreKey + SufKey;

                    byte[] byteKey = StringToByteArray(Key);
                    byte[] byteIV = StringToByteArray(Iv);
                    using (Aes myAes = Aes.Create())
                    {
                        // Encrypt the string to an array of bytes.
                        //byte[] encrypted = EncryptStringToBytes_Aes(original, byteKey, byteIV);
                        string input =
                            "00011101001010110010010111111100101101011100110111101110001101111000100000101010111100011100100100101011100111011110001010101011111110100110111101110101110010011001001000101101110010001101011111101001010101000011111100000110001001110100100100100111110111001101101011001110011001100000101111110111010011000100101100011111101100101111011100101001010010100011101000000000001100100111001001000100010111111110100111000111110001000011001110110000011100100100110101101001011001010101110110100100101001100101011010010000";
                        int numOfBytes = input.Length/8;

                        byte[] bytes = new byte[numOfBytes];
                        for (int i = 0; i < numOfBytes; ++i)
                        {
                            bytes[i] = Convert.ToByte(input.Substring(8*i, 8), 2);
                        }
                        byte[] encrypted = bytes;
                        // Decrypt the bytes to a string.

                        var roundtrip = DecryptStringFromBytes_Aes(encrypted, byteKey, byteIV);



                        //Display the original data and the decrypted data.
                        //Console.WriteLine("Original:   {0}", original);
                        //Console.WriteLine("Round Trip: {0}", roundtrip);
                        string sOut = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(roundtrip));
                        if (!sOut.Contains("??"))
                        {
                            using (
                                System.IO.StreamWriter file =
                                    new System.IO.StreamWriter(@"E:\Dokumenty\Uczelnia\Krypto\" + filename + ".txt",
                                        true))
                            {
                                file.WriteLine(sOut);
                            }
                        }
                    }

                }
                catch (Exception e)
                {

                    //
                }
            //}



            });
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
