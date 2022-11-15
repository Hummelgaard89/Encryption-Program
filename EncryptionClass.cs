using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Runtime.Loader;

namespace Encryption_Program
{
    internal class EncryptionClass
    {
        private SymmetricAlgorithm symetrickAlgorithm;
        public byte[] KEY;
        public byte[] IV;
        public int KeySize;
        public int BlockSize;
        public CipherMode Mode;
        public void SetEncryptionTypeAndMode(string encryptionType, string encryptionMode)
        {
            switch (encryptionType)
            {
                case "AES-128":
                    symetrickAlgorithm = Aes.Create();
                    symetrickAlgorithm.KeySize = 128;
                    break;
                case "AES-192":
                    symetrickAlgorithm = Aes.Create();
                    symetrickAlgorithm.KeySize = 192;
                    break;
                case "AES-256":
                    symetrickAlgorithm = Aes.Create();
                    symetrickAlgorithm.KeySize = 256;
                    break;
                case "3DES-128":
                    symetrickAlgorithm = TripleDES.Create();
                    symetrickAlgorithm.KeySize = 128;
                    break;
                case "3DES-192":
                    symetrickAlgorithm = TripleDES.Create();
                    symetrickAlgorithm.KeySize = 192;
                    break;
            }
            //Determines the ciphermode to be used.
            switch (encryptionMode)
            {
                case "CBC":
                    symetrickAlgorithm.Mode = CipherMode.CBC;
                    break;
                case "ECB":
                    symetrickAlgorithm.Mode = CipherMode.ECB; ;
                    break;
            }
            symetrickAlgorithm.GenerateKey();
            symetrickAlgorithm.GenerateIV();
            KEY = symetrickAlgorithm.Key;
            IV = symetrickAlgorithm.IV;
            KeySize = symetrickAlgorithm.KeySize;
            BlockSize = symetrickAlgorithm.BlockSize;
            Mode = symetrickAlgorithm.Mode;
        }
        public string EncryptData(string inputFile, string outputPath, string encryptionKey, string encryptionIV)
        {
            string defaultFilename = @"\Encrypted Data";
            string defaultFileType = ".txt";
            string fileName = outputPath + defaultFilename + defaultFileType;

            try
            {
                // Check if file already exists. If yes, gives it a number and increments it until unique.     
                if (File.Exists(fileName))
                {
                    int i = 1;
                    while (File.Exists(fileName))
                    {
                        fileName = outputPath + defaultFilename + i + defaultFileType;
                        i++;
                    } 
                }
                //Determines the type of encryption to be used.
                
                Stopwatch timeToEncrypt = new Stopwatch();
                timeToEncrypt.Start();
                string textToEncrypt = File.ReadAllText(inputFile);
                byte[] encryptedBytes;
                symetrickAlgorithm.Key = Convert.FromBase64String(encryptionKey);
                symetrickAlgorithm.IV = Convert.FromBase64String(encryptionIV);
                symetrickAlgorithm.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encrypting = symetrickAlgorithm.CreateEncryptor(symetrickAlgorithm.Key, symetrickAlgorithm.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encrypting, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                //Write all data to the stream.
                                sw.Write(textToEncrypt);
                            }

                            encryptedBytes = ms.ToArray();
                        }
                    }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.Write(Convert.ToBase64String(encryptedBytes));
                }
                timeToEncrypt.Stop();
                return timeToEncrypt.ElapsedMilliseconds.ToString();

            }
            catch (Exception Ex)
            {
                return "";
            }
        }
        public string Decryptdata(string inputFile, string outputPath, string encryptionKey, string encryptionIV)
        {
            string defaultFilename = @"\Decrypted Data";
            string defaultFileType = ".txt";
            string fileName = outputPath + defaultFilename + defaultFileType;
            // Check if file already exists. If yes, gives it a number and increments it until unique.     
            if (File.Exists(fileName))
            {
                int i = 1;
                while (File.Exists(fileName))
                {
                    fileName = outputPath + defaultFilename + i + defaultFileType;
                    i++;
                }
            }
            symetrickAlgorithm.Key = KEY;
            symetrickAlgorithm.IV = IV;
            symetrickAlgorithm.Padding = PaddingMode.PKCS7;
            Stopwatch timeToDecrypt = new Stopwatch();
            timeToDecrypt.Start();
            string textToDecrypt = File.ReadAllText(inputFile);
            byte[] encryptedBytes = Convert.FromBase64String(textToDecrypt);
            string plaintext;

                ICryptoTransform decrypting = symetrickAlgorithm.CreateDecryptor(symetrickAlgorithm.Key, symetrickAlgorithm.IV);

                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decrypting, CryptoStreamMode.Read))
                    {
                        using (StreamReader sw = new StreamReader(cs))
                        {
                            //Write all data to the stream.
                            plaintext = sw.ReadToEnd();
                        }
                    }
                }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(plaintext);
            }
            timeToDecrypt.Stop();
            return timeToDecrypt.ElapsedMilliseconds.ToString();
        }
    }
}
