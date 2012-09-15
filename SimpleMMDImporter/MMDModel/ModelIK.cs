using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    class ModelIK
    {
        public WORD IKBoneIndex { get; set; }
        public WORD IKTargetBoneIndex { get; set; }
        public WORD Iterations {get; set;}
        public float AngleLimit { get; set; }
        public WORD[] IKChildBoneIndex { get; set; }

        public ModelIK(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            IKBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            IKTargetBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            byte chainLength = reader.ReadByte();
            Iterations = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            AngleLimit = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            IKChildBoneIndex = new WORD[chainLength];
            for (int i = 0; i < chainLength; i++)
            {
                IKChildBoneIndex[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            }
        }
    }
}
