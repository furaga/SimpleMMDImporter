using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    /// <summary>
    /// ボーン枠用表示データ
    /// </summary>
    class ModelBoneDisp
    {
        /// <summary>
        /// 枠用ボーン番号
        /// </summary>
        public WORD BoneIndex { get; set; }
        /// <summary>
        /// 表示枠用番号
        /// </summary>
        public byte BoneDispFrameIndex { get; set; }

        public ModelBoneDisp(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            BoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            BoneDispFrameIndex = reader.ReadByte();
        }
    }
}
