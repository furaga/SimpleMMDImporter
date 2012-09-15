using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    class ModelSkinVertexData
    {
        public DWORD SkinVertIndex { get; set; }
        public float[] SkinVertPos { get; set; }

        public ModelSkinVertexData(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }

        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            SkinVertIndex = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            SkinVertPos = new float[3];
            for (int i = 0; i < SkinVertPos.Length; i++)
            {
                SkinVertPos[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            SkinVertPos[2] *= CoordZ;

        }
    }

    class ModelSkin
    {
        public string SkinName { get; set; }
        public byte SkinType {get; set;}
        public ModelSkinVertexData[] SkinVertDatas { get; set; }
        public string SkinNameEnglish { get; set; }

        public ModelSkin(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }

        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            SkinName = MMDUtils.GetString(reader.ReadBytes(20));
            DWORD skinVertCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            SkinType = reader.ReadByte();
            SkinVertDatas = new ModelSkinVertexData[skinVertCount];
            for (int i = 0; i < SkinVertDatas.Length; i++)
            {
                SkinVertDatas[i] = new ModelSkinVertexData(reader, CoordZ, scale);
            }
            SkinNameEnglish = null;
        }

        public void ReadEnglishExpantion(BinaryReader reader)
        {
            SkinNameEnglish = MMDUtils.GetString(reader.ReadBytes(20));
        }
    }
}
