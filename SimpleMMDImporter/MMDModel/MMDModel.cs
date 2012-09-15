using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    /// <summary>
    /// MMDモデル(ver.1)
    /// </summary>
    class MMDModel
    {
        public string Magic { get; private set; }
        public float Version {get; private set;}
        public ModelHeader Header { get; private set; }
        public ModelVertex[] Vertexes { get; set; }
        public WORD[] FaceVertexes { get; set; }
        public ModelMaterial[] Materials { get; set; }
        public ModelBone[] Bones { get; set; }
        public ModelIK[] IKs { get; set; }
        public ModelSkin[] Skins { get; set; }
        public WORD[] SkinIndex { get; set; }
        public ModelBoneDispName[] BoneDispName { get; set; }
        public ModelBoneDisp[] BoneDisp { get; set; }
        public bool EnglishExpantion { get; set; }
        public bool ToonExpantion { get; set; }
        public List<string> ToonFileNames { get; private set; }
        const int NumToonFileName = 10;

        public bool PhysicsExpantion { get; set; }
        public ModelRigidBody[] RigidBodies { get; set; }
        public ModelJoint[] Joints { get; set; }
        public CoordinateType Coordinate { get; private set; }
        float CoordZ { get { return (float)Coordinate; } }

        public MMDModel(string inputPath, string outputPath, float scale)
        {
            Vertexes = null;
            EnglishExpantion = ToonExpantion = PhysicsExpantion = false;
            ToonFileNames = new List<string>();
            Coordinate = CoordinateType.LeftHandedCoordinate;

            using (var fs = new FileStream(inputPath, FileMode.Open))
            {
                var reader = new BinaryReader(fs);
                Magic = MMDUtils.GetString(reader.ReadBytes(3));
                if (Magic != "Pmd")
                {
                    throw new FileLoadException("MMDモデルファイルではありません");
                }
                Version = BitConverter.ToSingle(reader.ReadBytes(4), 0);
                if (Version != 1.0)
                {
                    throw new FileLoadException("version=" + Version + "モデルは対応していません");
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
                writer.WriteLine(Magic + "," + Version);
                Write(writer);
                writer.Close();
            }

        }
        public void Read(BinaryReader reader, CoordinateType coordinate, float scale)
        {
            Coordinate = coordinate;
            // ヘッダ
            Header = new ModelHeader(reader);
            // 頂点リスト
            DWORD numVertex = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Vertexes = new ModelVertex[numVertex];
            for (DWORD i = 0; i < Vertexes.Length; i++)
            {
                Vertexes[i] = new ModelVertex(reader, CoordZ, scale);
            }
            DWORD faceVertCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            FaceVertexes = new WORD[faceVertCount];
            for (DWORD i = 0; i < faceVertCount; i++)
            {
                FaceVertexes[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            }
            DWORD materialCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Materials = new ModelMaterial[materialCount];
            for (DWORD i = 0; i < materialCount; i++)
            {
                Materials[i] = new ModelMaterial(reader);
            }
            WORD boneCount = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            Bones = new ModelBone[boneCount];
            for (WORD i = 0; i < boneCount; i++)
            {
                Bones[i] = new ModelBone(reader, CoordZ, scale);
            }
            WORD IKCount = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            IKs = new ModelIK[IKCount];
            for (WORD i = 0; i < IKs.Length; i++)
            {
                IKs[i] = new ModelIK(reader);
            }
            WORD skinCount = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            Skins = new ModelSkin[skinCount];
            for (WORD i = 0; i < skinCount; i++)
            {
                Skins[i] = new ModelSkin(reader, CoordZ, scale);
            }
            byte skinDispCount = reader.ReadByte();
            SkinIndex = new WORD[skinDispCount];
            for (byte i = 0; i < skinDispCount; i++)
            {
                SkinIndex[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            }
            byte boneDispNameCount = reader.ReadByte();
            BoneDispName = new ModelBoneDispName[boneDispNameCount];
            for (byte i = 0; i < boneDispNameCount; i++)
            {
                BoneDispName[i] = new ModelBoneDispName(reader);
            }
            DWORD boneDispCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            BoneDisp = new ModelBoneDisp[boneDispCount];
            for (DWORD i = 0; i < boneDispCount; i++)
            {
                BoneDisp[i] = new ModelBoneDisp(reader);
            }
            EnglishExpantion = (reader.ReadByte() != 0);
            if (EnglishExpantion)
            {
                Header.ReadEnglishExpantion(reader);
                for (int i = 0; i < Bones.Length; i++)
                {
                    Bones[i].ReadEnglishExpantion(reader);
                }
                for (int i = 0; i < Skins.Length; i++)
                {
                    // base のスキンには英名はない
                    if (Skins[i].SkinType != 0)
                    {
                        Skins[i].ReadEnglishExpantion(reader);
                    }
                }
                for (int i = 0; i < BoneDispName.Length; i++)
                {
                    BoneDispName[i].ReadEnglishExpantion(reader);
                }
            }
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                ToonExpantion = false;
            }
            else
            {
                ToonExpantion = true;
                ToonFileNames.Clear();
                for (int i = 0; i < NumToonFileName; i++)
                {
                    ToonFileNames.Add(MMDUtils.GetString(reader.ReadBytes(100)));
                }
            }
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                PhysicsExpantion = false;
            }
            else
            {
                PhysicsExpantion = true;
                // 剛体リスト
                DWORD rigidbodyCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                RigidBodies = new ModelRigidBody[rigidbodyCount];
                for (int i = 0; i < RigidBodies.Length; i++)
                {
                    RigidBodies[i] = new ModelRigidBody(reader, CoordZ, scale);
                }
                DWORD jointCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                Joints = new ModelJoint[jointCount];
                for (int i = 0; i < Joints.Length; i++)
                {
                    Joints[i] = new ModelJoint(reader, CoordZ, scale);
                }
            }
        }

        public void Write(StreamWriter writer)
        {
            writer.WriteLine("\nヘッダ情報");
            Header.Write(writer);
            writer.WriteLine("\n\n頂点リスト," + Vertexes.Length);
            writer.WriteLine("位置,,,法線,,,UV,,ボーンインデックス頂点リスト,,影響度,エッジフラグ");
            foreach (var e in Vertexes) e.Write(writer);
            writer.WriteLine("\n\n面リスト," + FaceVertexes.Length);
            for (int i = 0; i < FaceVertexes.Length / 3; i++)
            {
                writer.Write(FaceVertexes[3 * i] + ",");
                writer.Write(FaceVertexes[3 * i + 1] + ",");
                writer.WriteLine(FaceVertexes[3 * i + 2]);
            }
            writer.WriteLine("\n\n材質リスト," + Materials.Length);
            foreach (var e in Materials) e.Write(writer);
            writer.WriteLine("\n\nボーンリスト," + Bones.Length);
            foreach (var e in Bones) e.Write(writer);
            writer.WriteLine("\n\nIKボーンリスト," + IKs.Length);
            foreach (var e in IKs) e.Write(writer);
            writer.WriteLine("\n\n表情リスト," + Skins.Length);
            foreach (var e in Skins) e.Write(writer);
            writer.WriteLine("\n\n表情枠用の表情番号リスト," + SkinIndex.Length);
            foreach (var e in SkinIndex) writer.WriteLine(e + "");
            writer.WriteLine("\n\nボーン枠用の枠名リスト," + BoneDispName.Length);
            foreach (var e in BoneDispName) e.Write(writer);
            writer.WriteLine("\n\nボーン枠用表示リスト," + BoneDisp.Length);
            foreach (var e in BoneDisp) e.Write(writer);
            writer.WriteLine("\n\n英語拡張, " + EnglishExpantion.ToString());
            writer.WriteLine("\n\nトゥーン指定, " + ToonExpantion.ToString());
            for (int i = 0; i < NumToonFileName; i++)
            {
                writer.WriteLine((i + 1) + "," + ToonFileNames[i]);
            }
            writer.WriteLine("\n\n物理演算拡張, " + PhysicsExpantion.ToString());
            writer.WriteLine("\n剛体リスト, " + RigidBodies.Length);
            foreach (var e in RigidBodies) e.Write(writer);
            writer.WriteLine("\n\nジョイントリスト, " + Joints.Length);
            foreach (var e in Joints) e.Write(writer);
        }
    }
}