using BigInt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BigInt.Test.ITSecA2
{
    public class UnitTestITSecA4
    {
        [Theory]
        [InlineData("BBS-Tests-1.txt")]
        public void RunFindPBBSTest(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", PHexVal = "", PDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);

                BBS Bbs = new BBS(A, (short)A.BitsCount(), true);
                BBS BbsDec = new BBS(ADec, (short)ADec.BitsCount(), true);
                BigInt P = new BigInt(TestSet.Size, 0), PDec = new BigInt(TestSet.Size, 0), PCal = Bbs.P, PDecCal = BbsDec.P;
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                    {
                        TestSet.Parameters.TryGetValue("p", out PHexVal);
                        TestSet.Parameters.TryGetValue("P", out PDecVal);
                        P = new BigInt(TestSet.Size, PHexVal);
                        PDec = new BigInt(TestSet.Size, PDecVal);
                        Assert.True(PCal == P, $"Expected P to be equal to PCal, but got wrong value: {TestSet.Title}.");
                        Assert.True(PDecCal == PDec, $"Expected PDec to be equal to PDecCal, but got wrong value: {TestSet.Title}.");
                    }
                    else
                    {
                        TestSet.Parameters.TryGetValue($"p{i}", out PHexVal);
                        TestSet.Parameters.TryGetValue($"P{i}", out PDecVal);
                        PCal = BBS.GetNextBlumPrime(P);
                        PDecCal = BBS.GetNextBlumPrime(PDec);
                        P = new BigInt(TestSet.Size, PHexVal);
                        PDec = new BigInt(TestSet.Size, PDecVal);

                        Assert.True(PCal == P, $"Expected P to be equal to PCal, but got wrong value: {TestSet.Title}.");
                        Assert.True(PDecCal == PDec, $"Expected PDec to be equal to PDecCal, but got wrong value: {TestSet.Title}.");
                    }
                } 
            }
        }

        [Theory]
        [InlineData("BBS-Tests-2.txt")]
        public void RunFindPAndQBBSTest(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", PHexVal = "", PDecVal = "", 
                    QHexVal = "", QDecVal = "", NHexVal = "", NDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("q", out QHexVal);
                TestSet.Parameters.TryGetValue("Q", out QDecVal);
                TestSet.Parameters.TryGetValue("n", out NHexVal);
                TestSet.Parameters.TryGetValue("N", out NDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                BigInt Q = new BigInt(TestSet.Size, QHexVal);
                BigInt QDec = new BigInt(TestSet.Size, QDecVal);
                BigInt N = new BigInt(TestSet.Size, NHexVal);
                BigInt NDec = new BigInt(TestSet.Size, NDecVal);

                BBS Bbs = new BBS(A, (short)A.BitsCount(), true);
                BBS BbsDec = new BBS(ADec, (short)ADec.BitsCount(), true);
                
                BigInt PCal = Bbs.P, PDecCal = BbsDec.P;
                BigInt QCal = Bbs.Q, QDecCal = BbsDec.Q;
                BigInt NCal = Bbs.N, NDecCal = BbsDec.N;

                Assert.True(PCal == P, $"Expected P to be equal to PCal, but got wrong value: {TestSet.Title}.");
                Assert.True(PDecCal == PDec, $"Expected PDec to be equal to PDecCal, but got wrong value: {TestSet.Title}.");
                Assert.True(QCal == Q, $"Expected Q to be equal to QCal, but got wrong value: {TestSet.Title}.");
                Assert.True(QDecCal == QDec, $"Expected QDec to be equal to QDecCal, but got wrong value: {TestSet.Title}.");
                Assert.True(NCal == N, $"Expected N to be equal to NCal, but got wrong value: {TestSet.Title}.");
                Assert.True(NDecCal == NDec, $"Expected NDec to be equal to NDecCal, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("BBS-Tests-3.txt")]
        public void RunBBSWithPAndQTest(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string RHexVal = "", RDecVal = "", PHexVal = "", PDecVal = "", QHexVal = "", QDecVal = "",
                     ZHexVal = "", ZDecVal = "", BHexVal = "", BDecVal = "";


                TestSet.Parameters.TryGetValue("r", out RHexVal);
                TestSet.Parameters.TryGetValue("R", out RDecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("q", out QHexVal);
                TestSet.Parameters.TryGetValue("Q", out QDecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                BigInt R = new BigInt(TestSet.Size, RHexVal);
                BigInt RDec = new BigInt(TestSet.Size, RDecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                BigInt Q = new BigInt(TestSet.Size, QHexVal);
                BigInt QDec = new BigInt(TestSet.Size, QDecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);

                BBS Bbs = new BBS(R, P, Q);
                BBS BbsDec = new BBS(RDec, PDec, QDec);
                BigInt [] Z = new BigInt[16], ZDec = new BigInt[16], ZBBSCal = new BigInt[16], ZDecBBSCal = new BigInt[16];
                Bbs.GetRandomBBS(ref ZBBSCal);
                BbsDec.GetRandomBBS(ref ZDecBBSCal);

                for (int i = 0; i < 16; i++)
                {
                    if (i == 0)
                    {
                        TestSet.Parameters.TryGetValue("z", out ZHexVal);
                        TestSet.Parameters.TryGetValue("Z", out ZDecVal);
                        Z[i] = new BigInt(TestSet.Size, ZHexVal);
                        ZDec[i] = new BigInt(TestSet.Size, ZDecVal);
                    }
                    else
                    {
                        TestSet.Parameters.TryGetValue($"z{i}", out ZHexVal);
                        TestSet.Parameters.TryGetValue($"Z{i}", out ZDecVal);
                        Z[i] = new BigInt(TestSet.Size, ZHexVal);
                        ZDec[i] = new BigInt(TestSet.Size, ZDecVal);
                    }

                    Assert.True(ZBBSCal[i] == Z[i], $"Expected ZBBSCal[i] to be equal to Z[i]: {Z[i]}, but got wrong value: {TestSet.Title}.");
                    Assert.True(ZDecBBSCal[i] == ZDec[i], $"Expected ZDecBBSCal[i] to be equal to ZDec[i]: {ZDec[i]}, but got wrong value: {TestSet.Title}.");
                }

                BigInt BCal = BBS.GetRandomBBSAsBigInt(ZBBSCal);
                BigInt BDecCal = BBS.GetRandomBBSAsBigInt(ZDecBBSCal);

                Assert.True(BCal == B, $"Expected B to be equal to BCal, but got wrong value: {TestSet.Title}.");
                Assert.True(BDecCal == BDec, $"Expected BDec to be equal to BDecCal, but got wrong value: {TestSet.Title}.");
            }
        }
    }
}
