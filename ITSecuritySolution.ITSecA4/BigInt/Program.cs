﻿using System;
using System.IO;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt
{
    class Program
    {
        static void Main(string[] args)
        {
            //BigInteger BA19 = new BigInteger();
            //BigInteger BB19 = new BigInteger();
            //BigInteger BM19 = new BigInteger();
            //BigInteger BC19 = new BigInteger();
            //BigInteger.TryParse("00000000008987b26b8f2992b9b43d650552c64ce58d103833498a574c950c39", NumberStyles.AllowHexSpecifier, null, out BA19);
            //BigInteger.TryParse("0000000000000000000000000000000000000000001369016e83c30a026230b5", NumberStyles.AllowHexSpecifier, null, out BB19);
            //BigInteger.TryParse("000000000000000000000000000000000000000000000000000005e1d9223723", NumberStyles.AllowHexSpecifier, null, out BM19);
            //BigInteger.TryParse("00000000000000000000000000000000000000000000000000000255c61b5eca", NumberStyles.AllowHexSpecifier, null, out BC19);
            //BigInteger BC19PowModPrim = BigInteger.ModPow(BA19, BB19, BM19);
            //BigInteger GCD = BigInteger.GreatestCommonDivisor(BA19, BM19);
            //BigInt A19 = new BigInt(256, "+0x00000000008987b26b8f2992b9b43d650552c64ce58d103833498a574c950c39");
            //BigInt B19 = new BigInt(256, "+0x0000000000000000000000000000000000000000001369016e83c30a026230b5");
            //BigInt M19 = new BigInt(256, "+0x000000000000000000000000000000000000000000000000000005e1d9223723");
            //BigInt A19 = new BigInt(256, "+0x9581368bcd3c01aea64");
            //BigInt B19 = A19 % M19;
            //BigInt RandomBigInt = new BigInt(100, 0);
            //Random Rand = new Random();
            //RandomBigInt.GenRandomBits(100, Rand);

            //BigInt C19 = new BigInt(256, "+0x00000000000000000000000000000000000000000000000000000255c61b5eca");
            //BigInt C19Power = A19.PowModPrim(B19, M19);

            // Run All Test Cases A, B and C
            //BigIntSpecialTestCases.RunABCTestCases();
            //BigInt E = new BigInt(1536, "++0x000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002eecc62ef4d405558ab84d5d0ff8790e4b8eb8b5853e5350716b71f89eb472edc844fda4e6cc937134be3cef6ae0dddc46dae1ebeb82a9b9dbd475e95026d295d0315c7b2988649fa3dd785799aef5faff107226f9019155c44e988ed8ebf4f85c6a4b9b95222ddf910e5ed5000f");
            //byte[] EBytes = E.ToByteArray();
            //BigInt ECopy = new BigInt(1536, EBytes);
            //BigInt F = new BigInt(1536, "+0x000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002eecc62ef4d405558ab84d5d0ff8790e4b8eb8b5853e5350716b71f89eb472edc844fda4e6cc937134be3cef6ae0dddc46dae1ebeb82a9b9dbd475e95026d295d0315c7b2988649fa3dd785799aef5faff107226f9019155c44e988ed8ebf4f85c6a4b9b95222ddf910e5ed5000f");
            //int BitSize = F.BitsCount();
            //for (int i = BitSize; i >= 0; i--)
            //{
            //    Console.Write(F.Bit(i) ? "1" : "0");
            //}
            //BigInt D = E.ModInverse(F);
            //Keys K = Keys.GenerateRSAKeys(E, 1024);
            //int Rounds = 5;
            //BigInt p = new BigInt(E.Size, 0);
            //Random Rand = new Random();
            //p.GenPseudoPrime(512, Rounds, Rand);
            //BigInt S = new BigInt(1024, "+0x0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000094");
            //BigInt P = new BigInt(1024, "+0x000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000b");
            //BigInt Q = new BigInt(1024, "+0x0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000013");

            //BBS Bbs = new BBS(S, P, Q);
            //BigInt[] ZBBSCal = new BigInt[16];
            //Bbs.GetRandomBBS(ref ZBBSCal);

            // Test Binary conversion
            // b=+0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000060b6
            // B = +0b110000010110110

            //BigInt BHex = new BigInt(512, "+0x000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ebdb");
            //BigInt BBin = new BigInt(512, "+0b1110101111011011");

            //string BHexToBin = BHex.ToStringBinary();
            //string BBinToBin = BBin.ToStringBinary();

            //uint[] BBS8Statistics200 = BBS.GetRandomBBSStatistic256(1024, 200);
            //BBS.Log(BBS8Statistics200, "BBS_Byte_Gen_200_Statistics");
            //uint[] BBS16Statistics200 = BBS.GetRandomBBSStatistic64K(1024, 200);
            //BBS.Log(BBS16Statistics200, "BBS_2_Bytes_Gen_200_Statistics");
            //uint[] BBS8Statistics400 = BBS.GetRandomBBSStatistic256(1024, 400);
            //BBS.Log(BBS8Statistics400, "BBS_Byte_Gen_400_Statistics");
            uint[] BBS16Statistics400 = BBS.GetRandomBBSStatistic64K(1024, 400);
            BBS.Log(BBS16Statistics400, "BBS_2_Bytes_Gen_400_Statistics");
            //byte[] BA = { 255, 255 };
            //uint BAVal = BBS.ToUint(BA);
            //Console.ReadKey();
        }
    }
}