﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using System.Runtime.InteropServices.WindowsRuntime;

namespace PasswordManager.UILogic.Helpers
{
    /// <summary>
    /// Simple symmetric key encryption implementation from Can Bilgin @
    /// http://canbilgin.wordpress.com/2012/10/03/simple-aes-symmetric-key-encryption-in-winrt/
    /// </summary>
    public class CryptographyHelper
    {
        public static String Key { get; set; }

        static CryptographyHelper()
        {
            Key = GetHardwareId();
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Returns an encrypted text.
        /// </summary>
        /// <param name="toEncrypt">String to be encrypted</param>
        /// <param name="key">Unique key for encryption/decryption</param>m>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be encrypted.
                var toDecryptBuffer = CryptographicBuffer.ConvertStringToBinary(toEncrypt, BinaryStringEncoding.Utf8);

                // Open a symmetric algorithm provider for the specified algorithm.
                var aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                // The input key must be securely shared between the sender of the cryptic message
                // and the recipient. The initialization vector must also be shared but does not
                // need to be shared in a secure manner. If the sender encodes a message string
                // to a buffer, the binary encoding method must also be shared with the recipient.
                var buffEncrypted = CryptographicEngine.Encrypt(symetricKey, toDecryptBuffer, null);

                // Convert the encrypted buffer to a string (for display).
                // We are using Base64 to convert bytes to string since you might get unmatched characters
                // in the encrypted buffer that we cannot convert to string with UTF8.
                var strEncrypted = CryptographicBuffer.EncodeToBase64String(buffEncrypted);

                return strEncrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// Decrypt a string using dual encryption method. Return a Decrypted clear string
        /// </summary>
        /// <param name="cipherString">Encrypted string</param>
        /// <param name="key">Unique key for encryption/decryption</param>
        /// <returns>Returns decrypted text.</returns>
        public static string Decrypt(string cipherString, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be decrypted.
                IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(cipherString);

                // Open a symmetric algorithm provider for the specified algorithm.
                SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);

                string strDecrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);

                return strDecrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                //throw;
                return "";
            }
        }


        private static string GetHardwareId()
        {
            // Generate application token
            var token = HardwareIdentification.GetPackageSpecificToken(null);

            //Get app specific hardward id (ASHWID)
            var hardwareId = token.Id;

            //return hardware id buffer as string
            return BitConverter.ToString(hardwareId.ToArray());
        }

        private static IBuffer GetMD5Hash(string key)
        {
            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

            // Hash the message.
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }

            return buffHash;
        }
    }
}
