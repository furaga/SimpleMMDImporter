using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDMotion
{
    class MotionData
    {
        public string BoneName { get; set; }
        public DWORD FrameNo { get; set; }
        public float[] Location { get; private set; }
        public float[] Quatanion { get; private set; }
        public byte[][][] Interpolation { get; private set; }

        public MotionData(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }
        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            BoneName = MMDUtils.GetString(reader.ReadBytes(15));
            FrameNo = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Location = new float[3];
            for (int i = 0; i < Location.Length; i++)
            {
                Location[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            Quatanion = new float[4];
            for (int i = 0; i < Quatanion.Length; i++)
            {
                Quatanion[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            Interpolation = new byte[4][][];
            for (int i = 0; i < Interpolation.Length; i++)
            {
                Interpolation[i] = new byte[4][];
                for (int j = 0; j < Interpolation[i].Length; j++)
                {
                    Interpolation[i][j] = new byte[4];
                    for (int k = 0; k < Interpolation[i][j].Length; k++)
                    {
                        Interpolation[i][j][k] = reader.ReadByte();
                    }
                }
            }
            Location[2] *= CoordZ;
            Quatanion[0] *= CoordZ;
            Quatanion[1] *= CoordZ;
        }
        public void Write(StreamWriter writer)
        {
            writer.Write(BoneName + ",");
            foreach (var v in Location) writer.Write(v + ",");
            foreach (var v in Quatanion) writer.Write(v + ",");
            //foreach (var v1 in Interpolation)
            //    foreach (var v2 in v1)
            //        foreach (var v in v2)
            //            writer.Write(v + ",");
            writer.WriteLine();
        }
    }
}
