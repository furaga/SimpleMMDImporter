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
    class FaceMotionData
    {
        public string FaceName { get; set; }
        public DWORD FrameNo { get; set; }
        public float Rate { get; set; }

        public FaceMotionData(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }
        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            FaceName = MMDUtils.GetString(reader.ReadBytes(15));
            FrameNo = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Rate = BitConverter.ToSingle(reader.ReadBytes(4), 0);
        }
        public void Write(StreamWriter writer)
        {
            writer.Write(FaceName + ",");
            writer.Write(FrameNo + ",");
            writer.Write(Rate + ",");
            writer.WriteLine();
        }
    }
}
