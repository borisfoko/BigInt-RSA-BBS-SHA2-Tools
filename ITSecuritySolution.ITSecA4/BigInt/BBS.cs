using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BigInt
{
    public class BBS
    {
        private Random Rand;
        public BigInt P { get; private set; }
        public BigInt Q { get; private set; }
        public BigInt S { get; private set; }
        public BigInt N { get; private set; }

        public BBS()
        {
            P = new BigInt();
            Q = new BigInt();
            S = new BigInt();
            N = new BigInt();
            Rand = new Random();
        }

        public BBS(BigInt S, BigInt P, BigInt Q)
        {
            this.S = S;
            this.P = P;
            this.Q = Q;
            this.N = P * Q;
            Rand = new Random();
        }

        public BBS(short Size, short BitSize, int Rounds = 20)
        {
            Rand = new Random();
            this.InitRandomBBS(Size, BitSize, Rounds);
        }

        public BBS(BigInt S, short Size, bool StartValue = false, int Rounds = 20)
        {
            Rand = new Random();
            this.InitRandomBBS(S, Size, StartValue, Rounds);
        }

        public void InitRandomBBS(short Size, short BitSize, int Rounds = 20)
        {
            this.S = new BigInt(Size, 0);
            this.P = new BigInt(Size, 0);
            this.Q = new BigInt(Size, 0);
            this.InitPQN(BitSize, false, Rounds, true);
        }

        public void InitRandomBBS(BigInt S, short Size, bool StartValue = false, int Rounds = 20)
        {
            this.S = S;
            this.P = new BigInt(S.Size, 0);
            this.Q = new BigInt(S.Size, 0);
            short BitSize = Size <= 0 ? (short)this.S.BitsCount() : Size;
            this.InitPQN(BitSize, StartValue, Rounds);
        }

        public BigInt GetNextSeed(int i = 0)
        {
            BigInt B2 = new BigInt(this.S.Size, 2);
            if (i <= 0)
            {
                BigInt Seed = this.S.PowModPrim(B2, this.N);
                return Seed;
            }
            else
            {
                BigInt Index = new BigInt(this.S.Size, i);
                BigInt ScmPQ = BigInt.Scm(P - 1, Q - 1);
                BigInt Exp = B2.PowModPrim(Index, ScmPQ);
                BigInt Seed = this.S.PowModPrim(Exp, this.N);

                return Seed;
            }
        }

        public void GetRandomBBS(ref BigInt [] RBBS)
        {
            int BBSSize = RBBS.Length;
            for (int i = 0; i < BBSSize; i++)
            {
                RBBS[i] = this.GetNextSeed(i + 1);
            }
        }

        public byte GetRandomBBS(bool RefreshConfig = false, short Size = 1024, short BitSize = 300, int Rounds = 20)
        {
            if (RefreshConfig)
                this.InitRandomBBS(Size, BitSize, Rounds);

            BigInt[] RBBS = new BigInt[8];
            this.GetRandomBBS(ref RBBS);

            return GetLastBits(RBBS);
        }

        public byte[] GetRandomBBS16(bool RefreshConfig = false, short Size = 1024, short BitSize = 300, int Rounds = 20)
        {
            if (RefreshConfig)
                this.InitRandomBBS(Size, BitSize, Rounds);

            BigInt[] RBBS = new BigInt[16];
            this.GetRandomBBS(ref RBBS);

            return GetLastBitsArray(RBBS);
        }

        public BigInt GetRandomBBSAsBigInt()
        {
            BigInt[] RBBS = new BigInt[8];
            this.GetRandomBBS(ref RBBS);

            return GetLastBits(RBBS, RBBS[0].Size);
        }

        public static byte GetRandomBBS(BigInt[] RBBS)
        {
            return GetLastBits(RBBS);
        }

        public static BigInt GetRandomBBSAsBigInt(BigInt[] RBBS)
        {
            return GetLastBits(RBBS, RBBS[0].Size);
        }

        public static byte GetLastBits(BigInt[] BArray)
        {
            string LastBits = "";
            foreach(BigInt B in BArray)
            {
                LastBits = $"{LastBits}{(B.Bit(0) ? "1" : "0")}";
            }

            byte[] ByteArray = GetBytes(LastBits);

            return ByteArray[0];
        }

        public static byte[] GetLastBitsArray(BigInt[] BArray)
        {
            string LastBits = "";
            foreach (BigInt B in BArray)
            {
                LastBits = $"{LastBits}{(B.Bit(0) ? "1" : "0")}";
            }

            return GetBytes(LastBits);
        }

        public static BigInt GetLastBits(BigInt[] BArray, short Size)
        {
            string LastBits = "";
            foreach (BigInt B in BArray)
            {
                LastBits = $"{LastBits}{(B.Bit(0) ? "1" : "0")}";
            }

            LastBits = $"+0b{LastBits}";

            return new BigInt(Size, LastBits);
        }

        public static BigInt GetNextBlumPrime(BigInt Start, bool IsP = true, int Rounds = 20)
        {
            BigInt PNext = Start.Clone();
            if (!IsP)
            {
                PNext = PNext + (PNext / 4) + 2;
            }

            //int PBitsCount = Start.BitsCount();
            if (PNext.Even())
            {
                PNext.Value[0] |= 0x01;
            }
            if (!PNext.IsProbablePrime(Rounds) || PNext % 4 != 3 || PNext == Start)
            {
                do
                {
                    PNext += 2;
                }
                while (!PNext.IsProbablePrime(Rounds) || PNext % 4 != 3);
            }

            return PNext;
        }

        public static uint[] GetRandomBBSStatistic256(short Size, short BitSize)
        {
            uint[] Statistics = new uint[256];
            BBS Bbs = new BBS(Size, BitSize);
            byte RandomByte;

            // Run 10.000 BBS Generation and Record the statistics
            for (int i = 0; i < 10000; i++)
            {
                if (i == 0)
                {
                    RandomByte = Bbs.GetRandomBBS();
                }
                else
                {
                    RandomByte = Bbs.GetRandomBBS(true, Size, BitSize);
                }

                Statistics[RandomByte]++;
            }
            
            return Statistics;
        }

        public static uint[] GetRandomBBSStatistic64K(short Size, short BitSize)
        {
            uint[] Statistics = new uint[65536];
            BBS Bbs = new BBS(Size, BitSize);
            byte []RandomBytes;
            //GetRandomBBS16
            // Run 1.000.000 BBS Generation and Record the statistics
            for (uint i = 0; i < 1000000; i++)
            {
                if (i == 0)
                {
                    RandomBytes = Bbs.GetRandomBBS16();
                }
                else
                {
                    RandomBytes = Bbs.GetRandomBBS16(true, Size, BitSize);
                }

                Statistics[ToUint(RandomBytes)]++;
            }
            return Statistics;
        }

        public static string Log(uint [] Statistics, string TestName)
        {
            string Text = string.Join("\n", Statistics);
            string LocalFilename = @$"{Directory.GetCurrentDirectory()}\{TestName}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt";
            File.WriteAllText(LocalFilename, Text);
            return LocalFilename;
        }

        public static uint ToUint(byte[] ByteArray)
        {
            return (uint)((ByteArray[1] << 8) + ByteArray[0]);
        }

        private static byte[] GetBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }

        private void InitPQN(short Size, bool StartValue = false, int Rounds = 20, bool InitS = false)
        {
            bool Done = false;
            while (!Done)
            {
                if (StartValue)
                {
                    this.P = GetNextBlumPrime(this.S);
                }
                else
                {
                    this.P.GenPseudoPrime(Size, Rounds, Rand);
                }
                
                if (P % 4 == 3)
                {
                    do
                    {
                        if (StartValue)
                        {
                            this.Q = GetNextBlumPrime(this.P, false);
                        }
                        else
                        {
                            this.Q.GenPseudoPrime((short)(Size - 1), Rounds, Rand);
                        }
                        
                    } while (this.P == this.Q || this.Q % 4 != 3);

                    this.N = this.P * this.Q;
                    if (InitS)
                    {
                        do
                        {
                            this.S.GenPseudoPrime((short)(this.N.BitsCount() - (Size / 4)), Rounds, Rand);
                        } while (this.S == P || this.S == Q || BigInt.Egc(this.S, this.N) != 1);
                        Done = true;
                    }
                    else
                    {
                        if (BigInt.Egc(this.S, this.N) == 1)
                            Done = true;
                    }
                }
            }
        }
    }
}
