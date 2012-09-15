using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WORD = System.UInt16;
using DWORD = System.UInt32;

namespace SimpleMMDImporter.MMDModel
{
    class ModelMaterial
    {
        public float[] DiffuseColor { get; private set; }
        public float Alpha {get; set;}
        public float Specularity { get; set; }
        public float[] SpecularColor { get; private set; }
        public float[] AmbientColor { get; private set; }
        public byte ToonIndex { get; set; }
        public byte EdgeFlag { get; set; }
        public DWORD FaceVertCount { get; set; }
        public string TextureFileName { get; set; }
        public string SphereTextureFileName { get; set; }

        public ModelMaterial(BinaryReader reader)
        {
            DiffuseColor = new float[3];
            for (int i = 0; i < DiffuseColor.Length; i++)
            {
                DiffuseColor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            Alpha = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Specularity = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            SpecularColor = new float[3];
            for (int i = 0; i < SpecularColor.Length; i++)
            {
                SpecularColor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            AmbientColor = new float[3];
            for (int i = 0; i < AmbientColor.Length; i++)
            {
                AmbientColor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            }
            ToonIndex = reader.ReadByte();
            EdgeFlag = reader.ReadByte();
            FaceVertCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            string FileName = MMDUtils.GetString(reader.ReadBytes(20));
            string[] FileNames = FileName.Split('*');
            TextureFileName = SphereTextureFileName = "";
            foreach (var s in FileNames)
            {
                string ext = Path.GetExtension(s).ToLower();
                if (ext == ".sph" || ext == ".spa")
                {
                    SphereTextureFileName = s.Trim();
                }
                else
                {
                    TextureFileName = s.Trim();
                }
            }
        }

        public void Read(BinaryReader reader)
        {

        }
    }
}
