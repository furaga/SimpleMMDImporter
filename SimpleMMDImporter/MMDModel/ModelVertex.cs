using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    class ModelVertex
    {
        public float[] Position { get; private set; }
        public float[] NormalVector { get; private set; }
        public float[] UV { get; private set; }
        public WORD[] BoneNum { get; private set; }
        public byte BoneWeight { get; private set; }
        public byte NonEdgeFlag { get; private set; }

        public ModelVertex(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }

        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            Position = new float[3];
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            NormalVector = new float[3];
            for (int i = 0; i < NormalVector.Length; i++)
            {
                NormalVector[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            UV = new float[2];
            for (int i = 0; i < UV.Length; i++)
            {
                UV[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            BoneNum = new WORD[2];
            for (int i = 0; i < BoneNum.Length; i++)
            {
                BoneNum[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            }
            BoneWeight = reader.ReadByte();
            NonEdgeFlag = reader.ReadByte();
            Position[2] *= CoordZ;
            NormalVector[2] *= CoordZ;
        }

        public void Write(StreamWriter writer)
        {
            foreach (var v in Position) writer.Write(v + ",");
            foreach (var v in NormalVector) writer.Write(v + ",");
            foreach (var v in UV) writer.Write(v + ",");
            foreach (var v in BoneNum) writer.Write(v + ",");
            writer.WriteLine(BoneWeight + "," + NonEdgeFlag.ToString());
        }
    }
}
