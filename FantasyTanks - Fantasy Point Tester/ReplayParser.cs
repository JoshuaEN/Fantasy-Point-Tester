using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTanks___Fantasy_Point_Tester
{
    public static class ReplayParser
    {
        public static ReplayContent Parse(string path)
        {
            string[] chunks = new string[4];

            using (FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BinaryReader fReader = new BinaryReader(fStream))
            {
                byte[] magicBuffer = new byte[4];
                magicBuffer = fReader.ReadBytes(magicBuffer.Length);
                var magicNumber = BitConverter.ToString(magicBuffer);

                fReader.BaseStream.Position = 4;
                int count = fReader.ReadInt32();

                //fReader.ReadInt64();
                var chunk1 = System.Text.Encoding.Default.GetString(fReader.ReadBytes(fReader.ReadInt32()));
                var chunk2 = count > 1 ? System.Text.Encoding.Default.GetString(fReader.ReadBytes(fReader.ReadInt32())) : null;
                var encryptedData = fReader.ReadBytes((int)(fReader.BaseStream.Length - fReader.BaseStream.Position));

                return new ReplayContent(magicNumber, chunk1, chunk2, encryptedData);
            }
        }

        public class ReplayContent
        {
            public string MagicNumber { get; }
            public string Chunk1 { get; }
            public string Chunk2 { get; }
            public byte[] EncryptedData { get; }

            public bool IsMagicNumberValid { get { return this.MagicNumber == "12-32-34-11"; } }

            public ReplayContent(string magicNumber, string chunk1, string chunk2, byte[] encryptedData)
            {
                this.MagicNumber = magicNumber;
                this.Chunk1 = chunk1;
                this.Chunk2 = chunk2;
                this.EncryptedData = encryptedData;
            }
        }
    }
}
