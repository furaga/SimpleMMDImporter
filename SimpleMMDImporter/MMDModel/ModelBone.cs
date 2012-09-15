using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;


namespace SimpleMMDImporter.MMDModel
{
    class ModelBone
    {
        /// <summary>
        /// ボーン名
        /// </summary>
        public string BoneName { get; set; }
        /// <summary>
        /// 親ボーン番号
        /// </summary>
        public WORD ParentBoneIndex { get; set; }
        public WORD TailPosBoneIndex { get; set; }
        /// <summary>
        /// ボーンのタイプ
        /// </summary>
        /// <remarks>0:回転 1:回転・移動 2:IK 3:不明 4:IK影響下 5:IK接続先 6:非表示 7:唸り 9:回転運動 </remarks>
        public byte BoneType { get; set; }
        /// <summary>
        /// IKボーン番号
        /// </summary>
        public WORD IKParentBoneIndex { get; set; }
        /// <summary>
        /// ボーンのヘッドの位置(x, y, z)
        /// </summary>
        public float[] BoneHeadPos { get; private set; }
        /// <summary>
        /// ボーン名
        /// </summary>
        public string BoneNameEnglish { get; set; }

        public ModelBone(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }

        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            BoneHeadPos = new float[3];
            BoneName = MMDUtils.GetString(reader.ReadBytes(20));
            ParentBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            TailPosBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            BoneType = reader.ReadByte();
            IKParentBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            for (int i = 0; i < BoneHeadPos.Length; i++)
            {
                BoneHeadPos[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            BoneNameEnglish = null; // MMDModel.EnglishExpantionがtrueなら後で設定される
            // 座標系の調整
            BoneHeadPos[2] *= CoordZ;
        }

        public void ReadEnglishExpantion(BinaryReader reader)
        {
            BoneNameEnglish = MMDUtils.GetString(reader.ReadBytes(20));
        }
    }
}
