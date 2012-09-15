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
    class MMDMotion
    {
        public string Magic { get; set; }
        public string ModelName { get; private set; }
        public int Version {get; private set;}
        public MotionData[] Motions { get; set; }
        public FaceMotionData[] FaceMotions { get; set; }
        public CameraMotionData[] CameraMotions { get; set; }
        public LightMotionData[] LightMotions { get; set; }
        public CoordinateType Coordinate { get; set; }
        float CoordZ { get { return (float)Coordinate; } }

        public MMDMotion(string inputPath, string outputPath, float scale)
        {
            Coordinate = CoordinateType.LeftHandedCoordinate;

            //ファイルチェック
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("MMDモーションファイル:" + inputPath + "が見つかりません");
            }
            //ファイルリーダー
            using (FileStream fs = new FileStream(inputPath, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fs);
                //マジック文字列
                Magic = MMDUtils.GetString(reader.ReadBytes(30));
                if (Magic.Substring(0, 20) != "Vocaloid Motion Data")
                    throw new FileLoadException("MMDモーションファイルではありません");
                //バージョン
                Version = Convert.ToInt32(Magic.Substring(21));
                if (Version != 2)
                {
                    throw new FileLoadException("version=" + Version.ToString() + "モデルは対応していません");
                }
                Read(reader, Coordinate, scale);
                if (fs.Length != fs.Position)
                {
                    Console.WriteLine("警告：ファイル末尾以降に不明データ?");
                }
                fs.Close();
            }
            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                var writer = new StreamWriter(fs);
                writer.WriteLine(Magic);
                writer.WriteLine("バージョン," + Version);
                Write(writer);
                fs.Close();
            }
        }
        public void Read(BinaryReader reader, CoordinateType coordinate, float scale)
        {
            Coordinate = coordinate;
            ModelName = MMDUtils.GetString(reader.ReadBytes(20));
            DWORD motionCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Motions = new MotionData[motionCount];
            for (long i = 0; i < Motions.Length; i++)
            {
                Motions[i] = new MotionData(reader, CoordZ, scale);
            }
            DWORD faceCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            FaceMotions = new FaceMotionData[faceCount];
            for (long i = 0; i < FaceMotions.Length; i++)
            {
                FaceMotions[i] = new FaceMotionData(reader, CoordZ, scale);
            }
            DWORD cameraCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            CameraMotions = new CameraMotionData[cameraCount];
            for (long i = 0; i < CameraMotions.Length; i++)
            {
                CameraMotions[i] = new CameraMotionData(reader, CoordZ, scale);
            }
            DWORD lightCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            LightMotions = new LightMotionData[lightCount];
            for (long i = 0; i < LightMotions.Length; i++)
            {
                LightMotions[i] = new LightMotionData(reader, CoordZ, scale);
            }
        }

        public void Write(StreamWriter writer)
        {
            writer.WriteLine("モデル名," + ModelName);
            writer.WriteLine("モデルモーション," + Motions.Length);
            foreach (var e in Motions) e.Write(writer);
            writer.WriteLine("表情モーション," + FaceMotions.Length);
            foreach (var e in FaceMotions) e.Write(writer);
            writer.WriteLine("カメラモーション," + CameraMotions.Length);
            foreach (var e in CameraMotions) e.Write(writer);
            writer.WriteLine("ライトモーション," + LightMotions.Length);
            foreach (var e in LightMotions) e.Write(writer);
            writer.WriteLine();
        }
    }
}
