using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleMMDImporter.MMDModel
{
    class MMDUtils
    {
        /// <summary>
        /// pmdファイルはshift-jis
        /// </summary>
        public static Encoding encoder = Encoding.GetEncoding("shift-jis");
        public static string GetString(byte[] bytes)
        {
            int i = 0;
            foreach (var v in bytes)
            {
                if (v == 0) break;
                i++;
            }
            if (i < bytes.Length)
            {
                // 文字数がbytes.Length未満のとき
                return encoder.GetString(bytes, 0, i);
            }
            return encoder.GetString(bytes);
        }
    }
}
