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
    /// 剛体
    /// </summary>
    class ModelRigidBody
    {
        public string RigidBodyName { get; set; }
        public WORD RelateBoneIndex { get; set; }
        public byte GroupIndex { get; set; }
        public WORD GroupTarget { get; set; }
        public byte ShapeType { get; set; }
        public float ShapeWidth { get; set; }
        public float ShapeHeight { get; set; }
        public float ShapeDepth { get; set; }
        public float[] Position { get; private set; }
        public float[] Rotation { get; private set; }
        public float Weight { get; set; }
        public float LinerDamping { get; set; }
        public float AngularDamping { get; set; }
        /// <summary>
        /// 反発係数
        /// </summary>
        public float Restitution { get; set; }
        public float Friction { get; set; }
        /// <summary>
        /// 剛体タイプ
        /// </summary>
        /// <remarks>0:Bone追従 1:物理演算 2:物理演算(Bone位置合わせ)</remarks>
        public byte Type { get; set; }

        public ModelRigidBody(BinaryReader reader, float CoordZ, float scale)
        {
            Read(reader, CoordZ, scale);
        }

        public void Read(BinaryReader reader, float CoordZ, float scale)
        {
            RigidBodyName = MMDUtils.GetString(reader.ReadBytes(20));
            RelateBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            GroupIndex = reader.ReadByte();
            GroupTarget = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            ShapeType = reader.ReadByte();
            ShapeWidth = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            ShapeHeight = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            ShapeDepth = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            Position = new float[3];
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0) * scale;
            }
            Rotation = new float[3];
            for (int i = 0; i < Rotation.Length; i++)
            {
                Rotation[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            Weight = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            LinerDamping = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            AngularDamping = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Restitution = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Friction = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Type = reader.ReadByte();
            Position[2] *= CoordZ;
            // 必要らしい
            Rotation[0] *= CoordZ;
            Rotation[1] *= CoordZ;
        }
    }
}
