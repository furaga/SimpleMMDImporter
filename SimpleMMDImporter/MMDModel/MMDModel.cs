using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    /// <summary>
    /// 座標系
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// 左手座標系（MMDの標準座標系）
        /// </summary>
        LeftHandedCoordinate = 1,
        /// <summary>
        /// 右手座標系（XNAの標準座標系）
        /// </summary>
        RightHandedCoordinate = -1
    }

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

        public MMDModel(string filepath, float scale)
        {
            Vertexes = null;
            EnglishExpantion = ToonExpantion = PhysicsExpantion = false;
            ToonFileNames = new List<string>();
            Coordinate = CoordinateType.LeftHandedCoordinate;

            using (var fs =  new FileStream(filepath, FileMode.Open))
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
                    Console.WriteLine("警告: ファイル末尾以降に不明データ?");
                }
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
            for (DWORD i = 0; i < numVertex; i++)
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
                Joints = new ModelJoint[rigidbodyCount];
                for (int i = 0; i < Joints.Length; i++)
                {
                    Joints[i] = new ModelJoint(reader, CoordZ, scale);
                }
            }
        }
    }
}