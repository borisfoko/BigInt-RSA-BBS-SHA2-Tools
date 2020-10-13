using BigInt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BigInt.Test.ITSecA2
{
    public class UnitTestITSecA2
    {
        [Theory]
        [InlineData("EulerPseudoPrime-Tests.txt")]
        public void RunEulerPseudoPrimeTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", PHexVal = "", PDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);

                // These instructions always return true (Euler-Test is unable to detect those numbers), but combined with IsPrimeDiv we got false
                bool isPrimeP = P.IsPrimeEuler(A, true);  
                bool isPrimePDec = PDec.IsPrimeEuler(ADec, true);
                Assert.True(isPrimeP == true, $"Expected isPrimeP to be true, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDec == true, $"Expected isPrimePDec to be true, but got wrong value: {TestSet.Title}.");

                if (isPrimeP)
                {
                    int PrimeTestListSize = 0;
                    if (P.BitsCount() <= 78) // Number smaller than 3,1*10^23 (78 bit)
                        PrimeTestListSize = 12; // All prime number less than 41 (It's only necessary to check with primes less or equal to 37)

                    isPrimeP = isPrimePDec = P.IsPrimeMR(PrimeTestListSize > 0 ? BigIntConfiguration.PRIME_NUMBERS_LIST2000.Take(PrimeTestListSize).ToArray() : BigIntConfiguration.PRIME_NUMBERS_LIST2000);
                }

                Assert.True(isPrimeP == false, $"Expected isPrimeP to be false, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDec == false, $"Expected isPrimePDec to be false, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("FermatPseudoPrime-Tests.txt")]
        public void RunFermatPseudoPrimeTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", PHexVal = "", PDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);

                // These instructions always return true (Fermat-Test is unable to detect those numbers), but combined with IsPrimeDiv we got false
                // Beside Fermat-PseudoPrime-16 (with A = 546 and P = 561, A^(P-1) mod P = 177 and Not 1, which is a correct answer) return false
                bool isPrimeP = P.IsPrimeFermat(A, true);         
                bool isPrimePDec = PDec.IsPrimeFermat(ADec, true);
                if (isPrimeP)
                {
                    Assert.True(isPrimeP == true, $"Expected isPrimeP to be true, but got wrong value: {TestSet.Title}.");
                    Assert.True(isPrimePDec == true, $"Expected isPrimePDec to be true, but got wrong value: {TestSet.Title}.");
                    int PrimeTestListSize = 0;
                    if (P.BitsCount() <= 78) // Number smaller than 3,1*10^23 (78 bit)
                        PrimeTestListSize = 12; // All prime number less than 41 (It's only necessary to check with primes less or equal to 37)

                    isPrimeP = isPrimePDec = P.IsPrimeMR(PrimeTestListSize > 0 ? BigIntConfiguration.PRIME_NUMBERS_LIST2000.Take(PrimeTestListSize).ToArray() : BigIntConfiguration.PRIME_NUMBERS_LIST2000);
                }
                

                Assert.True(isPrimeP == false, $"Expected isPrimeP to be false, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDec == false, $"Expected isPrimePDec to be false, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("gcd-Tests.txt")]
        public void RungcdTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                   GHexVal = "", GDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);

                BigInt GGcd = BigInt.Egc(A, B);       // GGcd = ggT(A, B)
                BigInt cGcd = BigInt.Egc(ADec, BDec); // gGcd = ggT(ADec, BDec)

                Assert.True(G == GGcd, $"Expected G to be equal to ggT(A, B), but got wrong value: {TestSet.Title}.");
                Assert.True(GDec == cGcd, $"Expected GDec to be equal to ggT(ADec, BDec), but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("NoPrimeNumber-Tests.txt")]
        public void RunNoPrimeNumberTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", PHexVal = "", PDecVal = "",
                    FVal = "", EVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("f", out FVal);
                TestSet.Parameters.TryGetValue("e", out EVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                bool FermatExpectedVal = FVal == "true" ? true : false;
                bool EulerExpectedVal = EVal == "true" ? true : false;

                // These instructions always return true (Fermat-Test is unable to detect those numbers), but combined with IsPrimeDiv we got false
                bool isPrimePFermat = P.IsPrimeFermat(A, true);
                bool isPrimePDecFermat = PDec.IsPrimeFermat(ADec, true);
                bool isPrimePEuler = P.IsPrimeEuler(A, true);
                bool isPrimePDecEEuler = PDec.IsPrimeFermat(ADec, true);
                Assert.True(isPrimePFermat == FermatExpectedVal, $"Expected isPrimePFermat to be {FermatExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDecFermat == FermatExpectedVal, $"Expected isPrimePDecFermat to be {FermatExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePEuler == EulerExpectedVal, $"Expected isPrimePEuler to be {EulerExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDecEEuler == EulerExpectedVal, $"Expected isPrimePDecEEuler to be {EulerExpectedVal}, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Power-Big-Tests.txt")]
        public void RunPowerBigTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                   CHexVal = "", CDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("C", out CDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt CDec = new BigInt(TestSet.Size, CDecVal);

                BigInt CPower = A ^ B;      // A ^ B
                BigInt cPower = ADec ^ BDec;      // ADec ^ BDec

                Assert.True(C == CPower, $"Expected C to be equal to A ** B, but got wrong value: {TestSet.Title}.");
                Assert.True(CDec == cPower, $"Expected c to be equal to ADec ** BDec, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("PowerMod-Prim-Tests.txt")]
        public void RunPowerModPrimTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                   MHexVal = "", MDecVal = "", CHexVal = "", CDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("m", out MHexVal);
                TestSet.Parameters.TryGetValue("M", out MDecVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("C", out CDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt M = new BigInt(TestSet.Size, MHexVal);
                BigInt MDec = new BigInt(TestSet.Size, MDecVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt CDec = new BigInt(TestSet.Size, CDecVal);

                BigInt CPower = A.PowModPrim(B, M);          // A ^ B Ξ CPower (mode M)
                BigInt cPower = ADec.PowModPrim(BDec, MDec); // ADec ^ BDec Ξ cPower (mode MDec)

                Assert.True(C == CPower, $"Expected C to be equal to A ** B Ξ CPower (mode M), but got wrong value: {TestSet.Title}.");
                Assert.True(CDec == cPower, $"Expected c to be equal to ADec ** BDec Ξ cPower (mode MDec), but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("PowerMod-Tests.txt")]
        public void RunPowerModTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                   MHexVal = "", MDecVal = "", CHexVal = "", CDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("m", out MHexVal);
                TestSet.Parameters.TryGetValue("M", out MDecVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("C", out CDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt M = new BigInt(TestSet.Size, MHexVal);
                BigInt MDec = new BigInt(TestSet.Size, MDecVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt CDec = new BigInt(TestSet.Size, CDecVal);

                BigInt CPower = A.PowMod(B, M);          // A ^ B Ξ CPower (mode M)
                BigInt cPower = ADec.PowMod(BDec, MDec); // ADec ^ BDec Ξ cPower (mode MDec)

                Assert.True(C == CPower, $"Expected C to be equal to A ** B Ξ CPower (mode M), but got wrong value: {TestSet.Title}.");
                Assert.True(CDec == cPower, $"Expected c to be equal to ADec ** BDec Ξ cPower (mode MDec), but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Power-Small-Tests.txt")]
        public void RunPowerSmallTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "",
                    CHexVal = "", CDecVal = "", DHexVal = "", DDecVal = "",
                    EHexVal = "", EDecVal = "", FHexVal = "", FDecVal = "",
                    GHexVal = "", GDecVal = "", HHexVal = "", HDecVal = "", IHexVal = "", IDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("C", out CDecVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("D", out DDecVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("E", out EDecVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("F", out FDecVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);
                TestSet.Parameters.TryGetValue("H", out HDecVal);
                TestSet.Parameters.TryGetValue("i", out IHexVal);
                TestSet.Parameters.TryGetValue("I", out IDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt CDec = new BigInt(TestSet.Size, CDecVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt DDec = new BigInt(TestSet.Size, DDecVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt EDec = new BigInt(TestSet.Size, EDecVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt FDec = new BigInt(TestSet.Size, FDecVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);
                BigInt HDec = new BigInt(TestSet.Size, HDecVal);
                BigInt I = new BigInt(TestSet.Size, IHexVal);
                BigInt IDec = new BigInt(TestSet.Size, IDecVal);

                BigInt BPower = A ^ 0;      // A ^ 0
                BigInt CPower = A ^ 1;      // A ^ 1
                BigInt DPower = A ^ 2;      // A ^ 2
                BigInt EPower = A ^ 3;      // A ^ 3
                BigInt FPower = A ^ 4;      // A ^ 4
                BigInt GPower = A ^ 5;      // A ^ 5
                BigInt HPower = A ^ 6;      // A ^ 6
                BigInt IPower = A ^ 7;      // A ^ 7
                BigInt bPower = ADec ^ 0;      // A ^ 0
                BigInt cPower = ADec ^ 1;      // A ^ 1
                BigInt dPower = ADec ^ 2;      // A ^ 2
                BigInt ePower = ADec ^ 3;      // A ^ 3
                BigInt fPower = ADec ^ 4;      // A ^ 4
                BigInt gPower = ADec ^ 5;      // A ^ 5
                BigInt hPower = ADec ^ 6;      // A ^ 6
                BigInt iPower = ADec ^ 7;      // A ^ 7

                Assert.True(B == BPower, $"Expected B to be equal to A ** 0, but got wrong value: {TestSet.Title}.");
                Assert.True(C == CPower, $"Expected C to be equal to A ** 1, but got wrong value: {TestSet.Title}.");
                Assert.True(D == DPower, $"Expected D to be equal to A ** 2, but got wrong value: {TestSet.Title}.");
                Assert.True(E == EPower, $"Expected E to be equal to A ** 3, but got wrong value: {TestSet.Title}.");
                Assert.True(F == FPower, $"Expected F to be equal to A ** 4, but got wrong value: {TestSet.Title}.");
                Assert.True(G == GPower, $"Expected G to be equal to A ** 5, but got wrong value: {TestSet.Title}.");
                Assert.True(H == HPower, $"Expected H to be equal to A ** 6, but got wrong value: {TestSet.Title}.");
                Assert.True(I == IPower, $"Expected H to be equal to A ** 7, but got wrong value: {TestSet.Title}.");

                Assert.True(BDec == bPower, $"Expected b to be equal to ADec ** 0, but got wrong value: {TestSet.Title}.");
                Assert.True(CDec == cPower, $"Expected c to be equal to ADec ** 1, but got wrong value: {TestSet.Title}.");
                Assert.True(DDec == dPower, $"Expected d to be equal to ADec ** 2, but got wrong value: {TestSet.Title}.");
                Assert.True(EDec == ePower, $"Expected e to be equal to ADec ** 3, but got wrong value: {TestSet.Title}.");
                Assert.True(FDec == fPower, $"Expected f to be equal to ADec ** 4, but got wrong value: {TestSet.Title}.");
                Assert.True(GDec == gPower, $"Expected g to be equal to ADec ** 5, but got wrong value: {TestSet.Title}.");
                Assert.True(HDec == hPower, $"Expected h to be equal to ADec ** 6, but got wrong value: {TestSet.Title}.");
                Assert.True(IDec == iPower, $"Expected H to be equal to ADec ** 7, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("primeNumber-Tests.txt")]
        public void RunPrimNumberTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", AHexVal1 = "", ADecVal1 = "",
                    AHexVal2 = "", ADecVal2 = "", AHexVal3 = "", ADecVal3 = "",
                    AHexVal4 = "", ADecVal4 = "", AHexVal5 = "", ADecVal5 = "",
                    AHexVal6 = "", ADecVal6 = "", AHexVal7 = "", ADecVal7 = "",
                    AHexVal8 = "", ADecVal8 = "", AHexVal9 = "", ADecVal9 = "",
                    AHexVal10 = "", ADecVal10 = "", AHexVal11 = "", ADecVal11 = "",
                    PHexVal = "", PDecVal = "", FVal = "", EVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("a1", out AHexVal1);
                TestSet.Parameters.TryGetValue("A1", out ADecVal1);
                TestSet.Parameters.TryGetValue("a2", out AHexVal2);
                TestSet.Parameters.TryGetValue("A2", out ADecVal2);
                TestSet.Parameters.TryGetValue("a3", out AHexVal3);
                TestSet.Parameters.TryGetValue("A3", out ADecVal3);
                TestSet.Parameters.TryGetValue("a4", out AHexVal4);
                TestSet.Parameters.TryGetValue("A4", out ADecVal4);
                TestSet.Parameters.TryGetValue("a5", out AHexVal5);
                TestSet.Parameters.TryGetValue("A5", out ADecVal5);
                TestSet.Parameters.TryGetValue("a6", out AHexVal6);
                TestSet.Parameters.TryGetValue("A6", out ADecVal6);
                TestSet.Parameters.TryGetValue("a7", out AHexVal7);
                TestSet.Parameters.TryGetValue("A7", out ADecVal7);
                TestSet.Parameters.TryGetValue("a8", out AHexVal8);
                TestSet.Parameters.TryGetValue("A8", out ADecVal8);
                TestSet.Parameters.TryGetValue("a9", out AHexVal9);
                TestSet.Parameters.TryGetValue("A9", out ADecVal9);
                TestSet.Parameters.TryGetValue("a10", out AHexVal10);
                TestSet.Parameters.TryGetValue("A10", out ADecVal10);
                TestSet.Parameters.TryGetValue("a11", out AHexVal11);
                TestSet.Parameters.TryGetValue("A11", out ADecVal11);
                TestSet.Parameters.TryGetValue("p", out PHexVal);
                TestSet.Parameters.TryGetValue("P", out PDecVal);
                TestSet.Parameters.TryGetValue("f", out FVal);
                TestSet.Parameters.TryGetValue("e", out EVal);
                BigInt[] BasesHex = new BigInt[] 
                {
                    new BigInt(TestSet.Size, AHexVal), new BigInt(TestSet.Size, AHexVal1),
                    new BigInt(TestSet.Size, AHexVal2), new BigInt(TestSet.Size, AHexVal3),
                    new BigInt(TestSet.Size, AHexVal4), new BigInt(TestSet.Size, AHexVal5),
                    new BigInt(TestSet.Size, AHexVal6), new BigInt(TestSet.Size, AHexVal7),
                    new BigInt(TestSet.Size, AHexVal8), new BigInt(TestSet.Size, AHexVal9),
                    new BigInt(TestSet.Size, AHexVal10), new BigInt(TestSet.Size, AHexVal11)
                };
                BigInt[] BasesDec = new BigInt[] 
                {
                    new BigInt(TestSet.Size, ADecVal), new BigInt(TestSet.Size, ADecVal1),
                    new BigInt(TestSet.Size, ADecVal2), new BigInt(TestSet.Size, ADecVal3),
                    new BigInt(TestSet.Size, ADecVal4), new BigInt(TestSet.Size, ADecVal5),
                    new BigInt(TestSet.Size, ADecVal6), new BigInt(TestSet.Size, ADecVal7),
                    new BigInt(TestSet.Size, ADecVal8), new BigInt(TestSet.Size, ADecVal9),
                    new BigInt(TestSet.Size, ADecVal10), new BigInt(TestSet.Size, ADecVal11)
                };

                BigInt P = new BigInt(TestSet.Size, PHexVal);
                BigInt PDec = new BigInt(TestSet.Size, PDecVal);
                bool FermatExpectedVal = FVal == "true" ? true : false;
                bool EulerExpectedVal = EVal == "true" ? true : false;

                // These instructions always return true (Fermat-Test is unable to detect those numbers), but combined with IsPrimeDiv we got false
                bool isPrimePFermat = P.IsPrimeFermat(BasesHex);
                bool isPrimePDecFermat = PDec.IsPrimeFermat(BasesDec);
                bool isPrimePEuler = P.IsPrimeEuler(BasesHex);
                bool isPrimePDecEEuler = PDec.IsPrimeFermat(BasesDec);
                Assert.True(isPrimePFermat == FermatExpectedVal, $"Expected isPrimePFermat to be {FermatExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDecFermat == FermatExpectedVal, $"Expected isPrimePDecFermat to be {FermatExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePEuler == EulerExpectedVal, $"Expected isPrimePEuler to be {EulerExpectedVal}, but got wrong value: {TestSet.Title}.");
                Assert.True(isPrimePDecEEuler == EulerExpectedVal, $"Expected isPrimePDecEEuler to be {EulerExpectedVal}, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Square-Tests.txt")]
        public void RunSquareTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "", 
                    CHexVal = "", CDecVal = "", DHexVal = "", DDecVal = "",
                    EHexVal = "", EDecVal = "", FHexVal = "", FDecVal = "",
                    GHexVal = "", GDecVal = "", HHexVal = "", HDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("C", out CDecVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("D", out DDecVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("E", out EDecVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("F", out FDecVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("G", out GDecVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);
                TestSet.Parameters.TryGetValue("H", out HDecVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt ADec = new BigInt(TestSet.Size, ADecVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt BDec = new BigInt(TestSet.Size, BDecVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt CDec = new BigInt(TestSet.Size, CDecVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt DDec = new BigInt(TestSet.Size, DDecVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt EDec = new BigInt(TestSet.Size, EDecVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt FDec = new BigInt(TestSet.Size, FDecVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt GDec = new BigInt(TestSet.Size, GDecVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);
                BigInt HDec = new BigInt(TestSet.Size, HDecVal);

                BigInt BSquare = A.Square();      // A * A
                BigInt CSquare = A.Square(4);     // A ** 4
                BigInt DSquare = A.Square(8);     // A ** 8
                BigInt ESquare = A.Square(16);    // A ** 16
                BigInt FSquare = A.Square(32);    // A ** 32
                BigInt GSquare = A.Square(64);    // A ** 64
                BigInt HSquare = A.Square(128);   // A ** 128
                BigInt bSquare = ADec.Square();      // A * A
                BigInt cSquare = ADec.Square(4);     // A ** 4
                BigInt dSquare = ADec.Square(8);     // A ** 8
                BigInt eSquare = ADec.Square(16);    // A ** 16
                BigInt fSquare = ADec.Square(32);    // A ** 32
                BigInt gSquare = ADec.Square(64);    // A ** 64
                BigInt hSquare = ADec.Square(128);   // A ** 128

                Assert.True(B == BSquare, $"Expected B to be equal to A * A, but got wrong value: {TestSet.Title}.");
                Assert.True(C == CSquare, $"Expected C to be equal to A ** 4, but got wrong value: {TestSet.Title}.");
                Assert.True(D == DSquare, $"Expected D to be equal to A ** 8, but got wrong value: {TestSet.Title}.");
                Assert.True(E == ESquare, $"Expected E to be equal to A ** 16, but got wrong value: {TestSet.Title}.");
                Assert.True(F == FSquare, $"Expected F to be equal to A ** 32, but got wrong value: {TestSet.Title}.");
                Assert.True(G == GSquare, $"Expected G to be equal to A ** 64, but got wrong value: {TestSet.Title}.");
                Assert.True(H == HSquare, $"Expected H to be equal to A ** 128, but got wrong value: {TestSet.Title}.");
                Assert.True(BDec == bSquare, $"Expected b to be equal to ADec * A, but got wrong value: {TestSet.Title}.");
                Assert.True(CDec == cSquare, $"Expected c to be equal to ADec ** 4, but got wrong value: {TestSet.Title}.");
                Assert.True(DDec == dSquare, $"Expected d to be equal to ADec ** 8, but got wrong value: {TestSet.Title}.");
                Assert.True(EDec == eSquare, $"Expected e to be equal to ADec ** 16, but got wrong value: {TestSet.Title}.");
                Assert.True(FDec == fSquare, $"Expected f to be equal to ADec ** 32, but got wrong value: {TestSet.Title}.");
                Assert.True(GDec == gSquare, $"Expected g to be equal to ADec ** 64, but got wrong value: {TestSet.Title}.");
                Assert.True(HDec == hSquare, $"Expected h to be equal to ADec ** 128, but got wrong value: {TestSet.Title}.");
            }
        }
    }
}
