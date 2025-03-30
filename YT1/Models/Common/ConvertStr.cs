using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YT1.Models.Common
{
    public class ConvertStr
    {
        private static readonly string[] VietNamChar = new string[]
        {
            "AaEeOoUuIiDdYy",
            "aàáạảãâầấậẩẫăằắặẳẵ",
            "AÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴ",
            "eèéẹẻẽêềếệểễ",
            "EÈÉẸẺẼÊỀẾỆỂỄ",
            "oòóọỏõôồốộổỗơờớợởỡ",
            "OÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠ",
            "uùúụủũưừứựửữ",
            "UÙÚỤỦŨƯỪỨỰỬỮ",
            "iìíịỉĩ",
            "IÌÍỊỈĨ",
            "d",
            "D",
            "yỳýỵỷỹ",
            "YỲÝỴỶỸ"
        };

        public static string ChuyenCoDauThanhKhongDau(string str)
        {
            str = str.Trim();
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            return str.ToLower();
        }

        public static string FilterChar(string str)
        {
            // Sử dụng hàm ChuyenCoDauThanhKhongDau để chuyển có dấu thành không dấu
            str = ChuyenCoDauThanhKhongDau(str);

            // Xử lý các ký tự đặc biệt
            str = str.Replace(" ", "-");
            str = str.Replace("---", "-");
            str = str.Replace("--", "-");
            str = str.Replace("?", "");
            str = str.Replace("&", "");
            str = str.Replace("!", "");
            str = str.Replace("@", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("%", "");
            str = str.Replace("^", "");
            str = str.Replace("*", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("\\", "");
            str = str.Replace(";", "");
            str = str.Replace("'", "");
            str = str.Replace(".", "");
            str = str.Replace(",", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace(":", "");
            str = str.Replace("/", "");

            return str;
        }

        public static string Money(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (string.IsNullOrEmpty(str))
            {
                sb.Append("0");
            }
            else
            {
                int count = str.Length / 3;
                int index = 0;
                sb.Append(str);

                for (int i = 0; i < count; i++)
                {
                    index = index + 3;
                    sb.Insert(str.Length - index, ".");
                }

                // Thay thế ".." thành "." ở đầu chuỗi nếu có
                if (sb.Length > 1 && sb[0] == '.' && sb[1] == '.')
                {
                    sb.Remove(0, 1);
                }
            }

            return sb.ToString();
        }
    }
}