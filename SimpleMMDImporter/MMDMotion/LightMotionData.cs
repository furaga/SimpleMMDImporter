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
    class LightMotionData
    {
        public DWORD FrameNo { get; set; }
        public float[] Color { get; private set; }
        public float[] Location { get; private set; }
        public LightMotionData(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }
        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            FrameNo = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Color = new float[3];
            for (int i = 0; i < Color.Length; i++)
            {
                Color[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            Location = new float[3];
            for (int i = 0; i < Location.Length; i++)
            {
                Location[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            Location[2] *= CoordZ;
        }
        public void Write(StreamWriter writer)
        {
            writer.Write(FrameNo + ",");
            foreach (var v in Color) writer.Write(v + ",");
            foreach (var v in Location) writer.Write(v + ",");
            writer.WriteLine();
        }
    }
}
