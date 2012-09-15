using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleMMDImporter
{
    /// <summary>
    /// 座標系
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// 左手座標系（MMDの標準座標系）
        /// </summary>
        LeftHandedCoordinate = 1,
        /// <summary>
        /// 右手座標系（XNAの標準座標系）
        /// </summary>
        RightHandedCoordinate = -1
    }

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
