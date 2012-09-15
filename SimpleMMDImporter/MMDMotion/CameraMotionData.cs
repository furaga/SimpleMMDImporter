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
    class CameraMotionData
    {
        public DWORD FrameNo { get; set; }
        public float Length { get; set; }
        public float[] Location { get; private set; }
        public float[] Rotation { get; private set; }
        public byte[][] Interpolation { get; private set; }
        public WORD viewingAngle { get; private set; }
        public byte[] Unknown { get; protected set; }

        public CameraMotionData(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }
        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            FrameNo = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Length = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Location = new float[3];
            for (int i = 0; i < Location.Length; i++)
            {
                Location[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            Rotation = new float[3];
            for (int i = 0; i < Rotation.Length; i++)
            {
                Rotation[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            Interpolation = new byte[6][];
            for (int i = 0; i < Interpolation.Length; i++)
            {
                Interpolation[i] = new byte[4];
                for (int j = 0; j < Interpolation[i].Length; j++)
                {
                    Interpolation[i][j] =  reader.ReadByte();
                }
            }
            Unknown = new byte[3];
            for (int i = 0; i < Unknown.Length; i++)
            {
                Unknown[i] = reader.ReadByte();
            }
            Location[2] *= CoordZ;
            Rotation[2] *= CoordZ;
       }
        public void Write(StreamWriter writer)
        {
            writer.Write(FrameNo + ",");
            writer.Write(Length + ",");
            foreach (var v in Location) writer.Write(v + ",");
            foreach (var v in Rotation) writer.Write(v + ",");
            //foreach (var v1 in Interpolation)
            //    foreach (var v in v1)
            //        writer.Write(v + ",");
            writer.Write(viewingAngle + ",");
            foreach (var v in Unknown) writer.Write(v + ",");
            writer.WriteLine();
        }
    }
}
