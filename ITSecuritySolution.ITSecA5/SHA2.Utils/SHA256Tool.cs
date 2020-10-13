using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SHA2.Utils
{
    public struct SHA256Context
    {
        public uint[] W;
        public uint[] H;
    }

    public class SHA256Tool
    {
        public readonly static short CELL_SIZE = 32;          // 64 bit base cells
        public readonly static uint CELL_MASK = 0xFFFFFFFF;    // 64 bit mask
        public readonly static uint CELL_MASK_LOW = 0x000000FF; 
        public readonly static string DefaultHValue = "0x6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19";
        private static uint[] K = {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        public static string SHA256(string Input, ref string LogText, bool DEBUG = false)
        {
            SHA256Context CTX = new SHA256Context();
            CTX.W = new uint[64];
            CTX.H = new uint[8];

            if (DEBUG)
            {
                LogText =LogText +  $"p={Input}\n";
            }
            
            string[] Blocks = GetBlocks(Input);
            SHA256Init(ref CTX, DefaultHValue);
            foreach (string Block in Blocks)
            {
                byte[] InputBytes = StringToByteArray(Block);
                uint[] InputBlock = ByteArrayToUintArray(InputBytes);
                if (DEBUG)
                {
                    LogText = LogText + "# begin of next block\n";
                    LogText = LogText + $"H \t=+0x{SHA256ToString(CTX)}\n";
                    LogText = LogText + $"W \t=+0x{Block}\n";
                }
                SHA256ProcessBlock(ref CTX, InputBlock, ref LogText, DEBUG);
            }

            return SHA256ToString(CTX);
        }

        private static void SHA256Init(ref SHA256Context CTX, string H)
        {
            if (H != DefaultHValue)
            {
                int xPos = H.IndexOf('x');
                if (xPos != -1)
                {
                    string HCopy = H.Substring(xPos + 1, (H.Length - xPos - 1));
                    if (HCopy.Length == 64)
                    {
                        CTX.H[0] = Convert.ToUInt32(HCopy.Substring(0, 8), 16);
                        CTX.H[1] = Convert.ToUInt32(HCopy.Substring(8, 8), 16);
                        CTX.H[2] = Convert.ToUInt32(HCopy.Substring(16, 8), 16);
                        CTX.H[3] = Convert.ToUInt32(HCopy.Substring(24, 8), 16);
                        CTX.H[4] = Convert.ToUInt32(HCopy.Substring(32, 8), 16);
                        CTX.H[5] = Convert.ToUInt32(HCopy.Substring(40, 8), 16);
                        CTX.H[6] = Convert.ToUInt32(HCopy.Substring(48, 8), 16);
                        CTX.H[7] = Convert.ToUInt32(HCopy.Substring(56, 8), 16);
                    }
                }
            }
            else
            {
                CTX.H[0] = 0x6a09e667;
                CTX.H[1] = 0xbb67ae85;
                CTX.H[2] = 0x3c6ef372;
                CTX.H[3] = 0xa54ff53a;
                CTX.H[4] = 0x510e527f;
                CTX.H[5] = 0x9b05688c;
                CTX.H[6] = 0x1f83d9ab;
                CTX.H[7] = 0x5be0cd19;
            }
        }

        private static void SHA256ProcessBlock(ref SHA256Context CTX, uint[] Value, ref string LogText, bool DEBUG = false)
        {
            uint A, B, C, D, E, F, G, H, t1, t2;
            for (int i = 0; i < 16; i++)
            {
                CTX.W[i] = Value[i];
            }

            for (int i = 16; i < 64; i++)
            {
                CTX.W[i] = SSIG1(CTX.W[i - 2]) + CTX.W[i - 7] + SSIG0(CTX.W[i - 15]) + CTX.W[i - 16];
            }

            A = CTX.H[0];
            B = CTX.H[1];
            C = CTX.H[2];
            D = CTX.H[3];
            E = CTX.H[4];
            F = CTX.H[5];
            G = CTX.H[6];
            H = CTX.H[7];
            for (int i = 0; i < 64; i++)
            {
                t1 = H + BSIG1(E) + CH(E, F, G) + K[i] + CTX.W[i];
                t2 = BSIG0(A) + MAJ(A, B, C);
                H = G;
                G = F;
                F = E;
                E = D + t1;
                D = C;
                C = B;
                B = A;
                A = t1 + t2;
                if (DEBUG)
                {
                    LogText = LogText + $"R[{i.ToString("D2")}]=+0x{A.ToString("x8")}{B.ToString("x8")}{C.ToString("x8")}{D.ToString("x8")}{E.ToString("x8")}{F.ToString("x8")}{G.ToString("x8")}{H.ToString("x8")}\n";
                }
            }

            CTX.H[0] += A;
            CTX.H[1] += B;
            CTX.H[2] += C;
            CTX.H[3] += D;
            CTX.H[4] += E;
            CTX.H[5] += F;
            CTX.H[6] += G;
            CTX.H[7] += H;
        }

        private static string SHA256ToString(SHA256Context CTX)
        {
            string H0 = CTX.H[0].ToString("x8");
            string H1 = CTX.H[1].ToString("x8");
            string H2 = CTX.H[2].ToString("x8");
            string H3 = CTX.H[3].ToString("x8");
            string H4 = CTX.H[4].ToString("x8");
            string H5 = CTX.H[5].ToString("x8");
            string H6 = CTX.H[6].ToString("x8");
            string H7 = CTX.H[7].ToString("x8");

            return $"{H0}{H1}{H2}{H3}{H4}{H5}{H6}{H7}";
        }
        
        public static byte[] StringToByteArray(string Input)
        {
            return Enumerable.Range(0, Input.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(Input.Substring(x, 2), 16))
                             .ToArray();
        }

        public static uint[] ByteArrayToUintArray(byte[] Input)
        {
            uint[] Output = new uint[Input.Length / 4];
            for (uint i = 0, j = 0; i < Output.Length; ++i, j += 4)
            {
                Output[i] = ((uint)Input[j + 0] << 24) | ((uint)Input[j + 1] << 16) | ((uint)Input[j + 2] << 8) | ((uint)Input[j + 3]);
            }
            return Output;
        }

        public static string[] GetBlocks(string Input)
        {
            string InputBinary = HexStringToBinary(Input);
            int InputSize = InputBinary.Length;
            int LastPartSize = InputSize % 512;
            bool OneKStarted = false;

            string[] OutputTemp = Split(InputBinary, 512);
            string[] OutputBinary = new string[LastPartSize >= 448 || (LastPartSize == 0 && InputSize >= 512) ? OutputTemp.Length + 1 : OutputTemp.Length];
            for (int i = 0; i < OutputTemp.Length; i++)
            {
                OutputBinary[i] = OutputTemp[i];
                if (i == (OutputTemp.Length - 1) && LastPartSize >= 448)
                {
                    OutputBinary[i] = OutputBinary[i] + "1";
                    while (OutputBinary[i].Length < 512)
                    {
                        OutputBinary[i] = OutputBinary[i] + "0";
                    }
                    OneKStarted = true;
                    LastPartSize = 0;
                }
            }

            int ZeroPartSize = 512 - LastPartSize - (OneKStarted ? 64 : 65);
            ZeroPartSize = ZeroPartSize >= 0 ? ZeroPartSize : 512 - Math.Abs(ZeroPartSize) - (OneKStarted ? 64 : 65);
            if (!OneKStarted)
            {
                OutputBinary[OutputBinary.Length - 1] = OutputBinary[OutputBinary.Length - 1] + "1";
            }
            
            string InputSizeBinaryString = Convert.ToString(InputSize, 2);
            while (InputSizeBinaryString.Length < 64)
            {
                InputSizeBinaryString = "0" + InputSizeBinaryString;
            }

            for (int i = 0; i < ZeroPartSize; i++)
                OutputBinary[OutputBinary.Length - 1] = OutputBinary[OutputBinary.Length - 1] + "0";
            OutputBinary[OutputBinary.Length - 1] = OutputBinary[OutputBinary.Length - 1] + InputSizeBinaryString;

            //Console.WriteLine("OutpuBinary Last Size: " + OutputBinary[OutputBinary.Length - 1].Length);

            string[] Output = new string[OutputBinary.Length];
            for(int i = 0; i < OutputBinary.Length; i++)
            {
                Output[i] = BinaryStringToHexString(OutputBinary[i]);
            }
            return Output;
        }

        public static string[] Split(string Input, int length)
        {
            System.Globalization.StringInfo str = new System.Globalization.StringInfo(Input);

            int lengthAbs = Math.Abs(length);

            if (str == null || str.LengthInTextElements == 0 || lengthAbs == 0 || str.LengthInTextElements <= lengthAbs)
                return new string[] { Input };

            string[] array = new string[(str.LengthInTextElements % lengthAbs == 0 ? str.LengthInTextElements / lengthAbs : (str.LengthInTextElements / lengthAbs) + 1)];

            if (length > 0)
                for (int iStr = 0, iArray = 0; iStr < str.LengthInTextElements && iArray < array.Length; iStr += lengthAbs, iArray++)
                    array[iArray] = str.SubstringByTextElements(iStr, (str.LengthInTextElements - iStr < lengthAbs ? str.LengthInTextElements - iStr : lengthAbs));
            else // if (length < 0)
                for (int iStr = str.LengthInTextElements - 1, iArray = array.Length - 1; iStr >= 0 && iArray >= 0; iStr -= lengthAbs, iArray--)
                    array[iArray] = str.SubstringByTextElements((iStr - lengthAbs < 0 ? 0 : iStr - lengthAbs + 1), (iStr - lengthAbs < 0 ? iStr + 1 : lengthAbs));

            return array;
        }

        public static string HexStringToBinary(string Input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in Input.ToCharArray())
            {
                sb.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryStringToHexString(string binary)
        {
            if (string.IsNullOrEmpty(binary))
                return binary;

            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:x2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static string ByteArrayToString(byte[] ByteArray)
        {
            StringBuilder hex = new StringBuilder(ByteArray.Length * 2);
            foreach (byte b in ByteArray)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static uint SSIG0(uint X)
        {
            return RotRight(X, 7) ^ RotRight(X, 18) ^ (X >> 3);
        }

        private static uint SSIG1(uint X)
        {
            return RotRight(X, 17) ^ RotRight(X, 19) ^ (X >> 10);
        }

        private static uint BSIG0(uint X)
        {
            return RotRight(X, 2) ^ RotRight(X, 13) ^ RotRight(X, 22);
        }

        private static uint BSIG1(uint X)
        {
            return RotRight(X, 6) ^ RotRight(X, 11) ^ RotRight(X, 25);
        }

        private static uint MAJ(uint X, uint Y, uint Z)
        {
            return (X & Y) ^ (X & Z) ^ (Y & Z);
        }

        private static uint CH(uint X, uint Y, uint Z)
        {
            return (X & Y) ^ ((~X) & Z);
        }

        private static uint RotRight(uint A, byte B)
        {
            return (A >> B) | (A << (32 - B));
        }

        private static uint RotLeft(uint A, byte B)
        {
            return (A << B) | (A >> (32 - B));
        }

        public static string Log(string Text, string TestName)
        {
            string LocalFilename = @$"{Directory.GetCurrentDirectory()}\{TestName}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt";
            File.WriteAllText(LocalFilename, Text);
            return LocalFilename;
        }
    }
}
