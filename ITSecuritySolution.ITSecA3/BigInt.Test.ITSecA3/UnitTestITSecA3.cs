using BigInt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BigInt.Test.ITSecA2
{
    public class UnitTestITSecA3
    {
        [Theory]
        [InlineData("egcd-Tests.txt")]
        public void RunEgcdTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                   GHexVal = "", GDecVal = "", UHexVal = "", UDecVal = "",
                   VHexVal = "", VDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);
                TestSet.Parameters.TryGetValue("u", out UHexVal);
                TestSet.Parameters.TryGetValue("U", out UDecVal);
                TestSet.Parameters.TryGetValue("v", out VHexVal);
                TestSet.Parameters.TryGetValue("V", out VDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);
                BigInt U = new BigInt(TestSet.Size, UHexVal);
                BigInt UDec = new BigInt(TestSet.Size, UDecVal);
                BigInt V = new BigInt(TestSet.Size, VHexVal);
                BigInt VDec = new BigInt(TestSet.Size, VDecVal);

                BigInt UCal = new BigInt(TestSet.Size, 0);
                BigInt UDecCal = new BigInt(TestSet.Size, 0);
                BigInt VCal = new BigInt(TestSet.Size, 0);
                BigInt VDecCal = new BigInt(TestSet.Size, 0);
                BigInt GGcd = BigInt.Egcd(A, B, ref UCal, ref VCal);       // GGcd = ggT(A, B)
                BigInt cGcd = BigInt.Egcd(ADec, BDec, ref UDecCal, ref VDecCal); // gGcd = ggT(ADec, BDec)

                Assert.True(G == GGcd, $"Expected G to be equal to gcd(A, B), but got wrong value: {TestSet.Title}.");
                Assert.True(GDec == cGcd, $"Expected GDec to be equal to gcd(ADec, BDec), but got wrong value: {TestSet.Title}.");
                Assert.True(U == UCal, $"Expected U to be equal to UCal, but got wrong value: {TestSet.Title}.");
                Assert.True(UDec == UDecCal, $"Expected UDec to be equal to UDecCal, but got wrong value: {TestSet.Title}.");
                Assert.True(V == VCal, $"Expected V to be equal to VCal, but got wrong value: {TestSet.Title}.");
                Assert.True(VDec == VDecCal, $"Expected VDec to be equal to VDecCal, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("RSA-Tests-1.txt")]
        public void RunRSATestGenerateRSAKeys(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string PHexVal = "", PDecVal = "", QHexVal = "", QDecVal = "",
                    EHexVal = "", EDecVal = "", FHexVal = "", FDecVal = "",
                    DHexVal = "", DDecVal = "";

                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("q", out QHexVal);
                TestSet.Parameters.TryGetValue("Q", out QDecVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("E", out EDecVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("F", out FDecVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("D", out DDecVal);


                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                BigInt Q = new BigInt(TestSet.Size, QHexVal);
                BigInt QDec = new BigInt(TestSet.Size, QDecVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt EDec = new BigInt(TestSet.Size, EDecVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt FDec = new BigInt(TestSet.Size, FDecVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt DDec = new BigInt(TestSet.Size, DDecVal);

                Keys K = Keys.GenerateRSAKeys(E, P, Q);
                Keys KDec = Keys.GenerateRSAKeys(EDec, PDec, QDec);
                PrivateKey PriVateK = Keys.GetPrivateRSA(K);
                PrivateKey PriVateKDec = Keys.GetPrivateRSA(KDec);

                Assert.True(PriVateK.D == D, $" {TestSet.Title} - Expected Private Key PriVateK.D to be {D}, but got wrong value: {PriVateK.D}.");
                Assert.True(PriVateKDec.D == DDec, $" {TestSet.Title} - Expected Private Key PriVateKDec.D to be {DDec}, but got wrong value: {PriVateKDec.D}.");
            }
        }

        [Theory]
        [InlineData("RSA-Tests-2.txt")]
        public void RunRSATestEncryptionSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string PHexVal = "", PDecVal = "", QHexVal = "", QDecVal = "",
                                    EHexVal = "", EDecVal = "", NHexVal = "", NDecVal = "",
                                    DHexVal = "", DDecVal = "",
                                    GHexVal = "", GDecVal = "", HHexVal = "", HDecVal = "",
                                    IHexVal = "", IDecVal = "", JHexVal = "", JDecVal = "",
                                    KHexVal = "", KDecVal = "", LHexVal = "", LDecVal = "";

                TestSet.Parameters.TryGetValue("n", out NHexVal);
                TestSet.Parameters.TryGetValue("N", out NDecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("q", out QHexVal);
                TestSet.Parameters.TryGetValue("Q", out QDecVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("E", out EDecVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("D", out DDecVal);

                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);
                TestSet.Parameters.TryGetValue("H", out HDecVal);
                TestSet.Parameters.TryGetValue("i", out IHexVal);
                TestSet.Parameters.TryGetValue("I", out IDecVal);
                TestSet.Parameters.TryGetValue("j", out JHexVal);
                TestSet.Parameters.TryGetValue("J", out JDecVal);
                TestSet.Parameters.TryGetValue("k", out KHexVal);
                TestSet.Parameters.TryGetValue("K", out KDecVal);
                TestSet.Parameters.TryGetValue("l", out LHexVal);
                TestSet.Parameters.TryGetValue("L", out LDecVal);

                BigInt N = new BigInt(TestSet.Size, NHexVal);
                BigInt NDec = new BigInt(TestSet.Size, NDecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                BigInt Q = new BigInt(TestSet.Size, QHexVal);
                BigInt QDec = new BigInt(TestSet.Size, QDecVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt EDec = new BigInt(TestSet.Size, EDecVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt DDec = new BigInt(TestSet.Size, DDecVal);

                Keys Key = Keys.GenerateRSAKeys(E, P, Q);
                Keys KeyDec = Keys.GenerateRSAKeys(EDec, PDec, QDec);
                PrivateKey PriVateK = Keys.GetPrivateRSA(Key);
                PrivateKey PriVateKDec = Keys.GetPrivateRSA(KeyDec);

                Assert.True(PriVateK.D == D, $" {TestSet.Title} - Expected Private Key PriVateK.D to be {D}, but got wrong value: {PriVateK.D}.");
                Assert.True(PriVateKDec.D == DDec, $" {TestSet.Title} - Expected Private Key PriVateKDec.D to be {DDec}, but got wrong value: {PriVateKDec.D}.");

                PublicKey PublicK = Keys.GetPublicRSA(Key);
                PublicKey PublicKDec = Keys.GetPublicRSA(KeyDec);

                // Plain Text G, Cipher Text H
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);
                BigInt HDec = new BigInt(TestSet.Size, HDecVal);

                BigInt HCal = Keys.EncryptRSA(PublicK, G);
                BigInt HDecCal = Keys.EncryptRSA(PublicKDec, GDec);

                Assert.True(HCal == H, $" {TestSet.Title} - Expected Cipher HCal to be {H}, but got wrong value: {HCal}.");
                Assert.True(HDecCal == HDec, $" {TestSet.Title} - Expected Cipher HDecCal to be {HDec}, but got wrong value: {HDecCal}.");

                // Plain Text I, Cipher Text J
                BigInt I = new BigInt(TestSet.Size, IHexVal);
                BigInt IDec = new BigInt(TestSet.Size, IDecVal);
                BigInt J = new BigInt(TestSet.Size, JHexVal);
                BigInt JDec = new BigInt(TestSet.Size, JDecVal);

                BigInt JCal = Keys.EncryptRSA(PublicK, I);
                BigInt JDecCal = Keys.EncryptRSA(PublicKDec, IDec);

                Assert.True(JCal == J, $" {TestSet.Title} - Expected Cipher JCal to be {J}, but got wrong value: {JCal}.");
                Assert.True(JDecCal == JDec, $" {TestSet.Title} - Expected Cipher JDecCal to be {JDec}, but got wrong value: {JDecCal}.");


                // Plain Text K, Cipher Text L
                BigInt K = new BigInt(TestSet.Size, KHexVal);
                BigInt KDec = new BigInt(TestSet.Size, KDecVal);
                BigInt L = new BigInt(TestSet.Size, LHexVal);
                BigInt LDec = new BigInt(TestSet.Size, LDecVal);

                BigInt LCal = Keys.EncryptRSA(PublicK, K);
                BigInt LDecCal = Keys.EncryptRSA(PublicKDec, KDec);

                Assert.True(LCal == L, $" {TestSet.Title} - Expected Cipher LCal to be {L}, but got wrong value: {LCal}.");
                Assert.True(LDecCal == LDec, $" {TestSet.Title} - Expected Cipher LDecCal to be {LDec}, but got wrong value: {LDecCal}.");
            }
        }

        [Theory]
        [InlineData("RSA-Tests-3.txt")]
        public void RunRSATestWrongEncryption(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile, true);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "",
                BHexVal = "", BDecVal = "", PHexVal = "", PDecVal = "", 
                QHexVal = "", QDecVal = "", EHexVal = "", EDecVal = "", 
                FHexVal = "", FDecVal = "", DHexVal = "", DDecVal = "",
                GHexVal = "", GDecVal = "", HHexVal = "", HDecVal = "",
                IHexVal = "", IDecVal = "", JHexVal = "", JDecVal = "",
                KHexVal = "", KDecVal = "", LHexVal = "", LDecVal = "";

                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("q", out QHexVal);
                TestSet.Parameters.TryGetValue("Q", out QDecVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("E", out EDecVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("D", out DDecVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("F", out FDecVal);

                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);
                TestSet.Parameters.TryGetValue("H", out HDecVal);
                TestSet.Parameters.TryGetValue("i", out IHexVal);
                TestSet.Parameters.TryGetValue("I", out IDecVal);
                TestSet.Parameters.TryGetValue("j", out JHexVal);
                TestSet.Parameters.TryGetValue("J", out JDecVal);
                TestSet.Parameters.TryGetValue("k", out KHexVal);
                TestSet.Parameters.TryGetValue("K", out KDecVal);
                TestSet.Parameters.TryGetValue("l", out LHexVal);
                TestSet.Parameters.TryGetValue("L", out LDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);

                // P = A * B, beside the first TestSet P is a combined number and not prime
                BigInt PCal  = A * B;
                BigInt PDecCal  = ADec * BDec;

                Assert.True(PCal == P, $" {TestSet.Title} - Expected PCal to be {P}, but got wrong value: {PCal}.");
                Assert.True(PDecCal == PDec, $" {TestSet.Title} - Expected PDecCal to be {PDec}, but got wrong value: {PDecCal}.");

                BigInt Q = new BigInt(TestSet.Size, QHexVal);
                BigInt QDec = new BigInt(TestSet.Size, QDecVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt EDec = new BigInt(TestSet.Size, EDecVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt DDec = new BigInt(TestSet.Size, DDecVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt FDec = new BigInt(TestSet.Size, FDecVal);

                Keys Key = Keys.GenerateRSAKeys(E, P, Q);
                Keys KeyDec = Keys.GenerateRSAKeys(EDec, PDec, QDec);
                PrivateKey PriVateK = Keys.GetPrivateRSA(Key);
                PrivateKey PriVateKDec = Keys.GetPrivateRSA(KeyDec);

                Assert.True(PriVateK.D == D, $" {TestSet.Title} - Expected Private Key PriVateK.D to be {D}, but got wrong value: {PriVateK.D}.");
                Assert.True(PriVateKDec.D == DDec, $" {TestSet.Title} - Expected Private Key PriVateKDec.D to be {DDec}, but got wrong value: {PriVateKDec.D}.");

                // phi(p, q)
                BigInt FCal = (P - 1) * (Q - 1);
                BigInt FDecCal = (PDec - 1) * (QDec - 1);

                Assert.True(FCal == F, $" {TestSet.Title} - Expected FCal to be {F}, but got wrong value: {FCal}.");
                Assert.True(FDecCal == FDec, $" {TestSet.Title} - Expected FDecCal to be {FDec}, but got wrong value: {FDecCal}.");

                PublicKey PublicK = Keys.GetPublicRSA(Key);
                PublicKey PublicKDec = Keys.GetPublicRSA(KeyDec);

                // Plain Text G, Cipher Text H
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);
                BigInt HDec = new BigInt(TestSet.Size, HDecVal);

                BigInt HCal = Keys.EncryptRSA(PublicK, G);
                BigInt HDecCal = Keys.EncryptRSA(PublicKDec, GDec);

                // The encrypted values got here are wrong and correspond to the expected (in the TestSet)
                Assert.True(HCal == H, $" {TestSet.Title} - Expected Cipher HCal to be {H}, but got wrong value: {HCal}.");
                Assert.True(HDecCal == HDec, $" {TestSet.Title} - Expected Cipher HDecCal to be {HDec}, but got wrong value: {HDecCal}.");

                // Plain Text I, Cipher Text J
                BigInt I = new BigInt(TestSet.Size, IHexVal);
                BigInt IDec = new BigInt(TestSet.Size, IDecVal);
                BigInt J = new BigInt(TestSet.Size, JHexVal);
                BigInt JDec = new BigInt(TestSet.Size, JDecVal);

                BigInt JCal = Keys.EncryptRSA(PublicK, I);
                BigInt JDecCal = Keys.EncryptRSA(PublicKDec, IDec);

                Assert.True(JCal == J, $" {TestSet.Title} - Expected Cipher JCal to be {J}, but got wrong value: {JCal}.");
                Assert.True(JDecCal == JDec, $" {TestSet.Title} - Expected Cipher JDecCal to be {JDec}, but got wrong value: {JDecCal}.");


                // Plain Text K, Cipher Text L
                BigInt K = new BigInt(TestSet.Size, KHexVal);
                BigInt KDec = new BigInt(TestSet.Size, KDecVal);
                BigInt L = new BigInt(TestSet.Size, LHexVal);
                BigInt LDec = new BigInt(TestSet.Size, LDecVal);

                BigInt LCal = Keys.EncryptRSA(PublicK, K);
                BigInt LDecCal = Keys.EncryptRSA(PublicKDec, KDec);

                Assert.True(LCal == L, $" {TestSet.Title} - Expected Cipher LCal to be {L}, but got wrong value: {LCal}.");
                Assert.True(LDecCal == LDec, $" {TestSet.Title} - Expected Cipher LDecCal to be {LDec}, but got wrong value: {LDecCal}.");
            }
        }
    }
}
