using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public static class Methods
    {
        public static string GenerateBarcode()
        {
            try
            {
                string[] charPool = "1-2-3-4-5-6-7-8-9-0".Split('-');
                StringBuilder rs = new StringBuilder();
                int length = 10;
                Random rnd = new Random();
                while (length-- > 0)
                {
                    int index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }
                return rs.ToString();
            }
            catch { }
            return "";
        }

        public static string CleanPhoneNumber(string stringu)
        {
            string str = "";
            if (stringu != null && stringu != "")
            {
                str = Regex.Replace(stringu, "[^0-9+]+", string.Empty);      //fshin te gjitha karakterat perveq numrave dhe shenjen e plusit (+)      
            }

            return str;
        }

        public static string Truncate<T>(this T input, int maxLength)
        {
            if (input.ToString().Length > maxLength && input != null)
            {
                return input.ToString().Substring(0, maxLength) + "...";

            }
            return input.ToString();
        }

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }

        public static bool NotIn<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return !items.Contains(item);
        }

        public static T IsNull<T>(this T v1, T defaultValue)
        {
            if (v1 == null || v1.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                return v1;
            }
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string GetSafeFilename(string filename)
        {
            string _filename;

            if (filename.Length > 94)
            {
                _filename = filename.Substring(0, 94) + ".pdf";
            }
            else
            {
                _filename = filename;
            }

            _filename = string.Join("_", _filename.Split(Path.GetInvalidFileNameChars()));
            return _filename.Replace(",", "");
        }

        public static byte[] FileToByte(IFormFile file)
        {
            Stream dokumentiStream = file.OpenReadStream();
            Int32 length = Convert.ToInt32(dokumentiStream.Length);
            byte[] dokumentiByte = new byte[length];
            dokumentiStream.Read(dokumentiByte, 0, length);
            return dokumentiByte;
        }

        public static string GetSafeImageName(string filename, string extension)
        {
            string _filename;

            if (filename.Length > 94)
            {
                _filename = filename.Substring(0, 94) + extension;
            }
            else
            {
                _filename = filename;
            }

            _filename = string.Join("_", _filename.Split(Path.GetInvalidFileNameChars()));
            _filename = _filename.Replace(",", "");
            _filename = _filename.Replace("+", "");
            return _filename;
        }

        public static String GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
    }
}
