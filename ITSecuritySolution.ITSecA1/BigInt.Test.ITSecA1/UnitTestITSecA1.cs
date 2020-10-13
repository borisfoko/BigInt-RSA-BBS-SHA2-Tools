using BigInt;
using System;
using System.Collections.Generic;
using Xunit;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt.Test.ITSecA1
{
    public class UnitTestITSecA1
    {

        [Theory]
        [InlineData("Arithmetic-Tests.txt")]
        public void RunArithmeticTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                Console.WriteLine($"Loading data for test: {TestSet.Title}");
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "", 
                CAddHexVal = "", CAddDecVal = "", DSubHexVal = "", DSubDecVal = "",
                EMulHexVal = "", EMulDecVal = "", FDivHexVal = "", FDivDecVal = "",
                GModHexVal = "", GModDecVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("+", out CAddHexVal);
                TestSet.Parameters.TryGetValue("C", out CAddDecVal);
                TestSet.Parameters.TryGetValue("-", out DSubHexVal);
                TestSet.Parameters.TryGetValue("D", out DSubDecVal);
                TestSet.Parameters.TryGetValue("*", out EMulHexVal);
                TestSet.Parameters.TryGetValue("E", out EMulDecVal);
                TestSet.Parameters.TryGetValue("/", out FDivHexVal);
                TestSet.Parameters.TryGetValue("F", out FDivDecVal);
                TestSet.Parameters.TryGetValue("%", out GModHexVal);
                TestSet.Parameters.TryGetValue("G", out GModDecVal);
                BigInt a = new BigInt(TestSet.Size, AHexVal);
                BigInt A = new BigInt(TestSet.Size, ADecVal);
                BigInt b = new BigInt(TestSet.Size, BHexVal);
                BigInt B = new BigInt(TestSet.Size, BDecVal);
                BigInt CHex = new BigInt(TestSet.Size, CAddHexVal);
                BigInt C = new BigInt(TestSet.Size, CAddDecVal);
                BigInt DHex = new BigInt(TestSet.Size, DSubHexVal);
                BigInt D = new BigInt(TestSet.Size, DSubDecVal);
                BigInt EHex = new BigInt(TestSet.Size, EMulHexVal);
                BigInt E = new BigInt(TestSet.Size, EMulDecVal);
                BigInt FHex = new BigInt(TestSet.Size, FDivHexVal);
                BigInt F = new BigInt(TestSet.Size, FDivDecVal);
                BigInt GHex = new BigInt(TestSet.Size, GModHexVal);
                BigInt G = new BigInt(TestSet.Size, GModDecVal);
                BigInt aAddb = a + b;
                BigInt AAddB = A + B;
                BigInt aSubb = a - b;
                BigInt ASubB = A - B;
                BigInt aMulb = a * b;
                BigInt AMulB = A * B;

                Assert.True(a == A, $"Expected a (Hex) to be equal to A (Dec), but got error while running test: {TestSet.Title}.");
                Assert.True(b == B, $"Expected b (Hex) to be equal to B (Dec), but got error while running test: {TestSet.Title}.");
                Assert.True(aAddb == CHex, $"Expected a + b to be equal to CHex, but got error while running test: {TestSet.Title}.");
                Assert.True(AAddB == C, $"Expected A + B to be equal to C (Dec), but got error while running test: {TestSet.Title}.");
                Assert.True(aSubb == DHex, $"Expected a - b to be equal to DHex, but got error while running test: {TestSet.Title}.");
                Assert.True(ASubB == D, $"Expected A - B to be equal to D (Dec), but got error while running test: {TestSet.Title}.");
                Assert.True(aMulb == EHex, $"Expected a * b to be equal to EHex, but got error while running test: {TestSet.Title}.");
                Assert.True(AMulB == E, $"Expected A * B to be equal to E (Dec), but got error while running test: {TestSet.Title}.");

                try
                {
                    // TestSet.Title == "Arith-HexDec-77"
                    BigInt aDivb = a / b;
                    BigInt ADivB = A / B;
                    BigInt aModb = a % b;
                    //BigInt AModB = A % B;
                    Assert.True(aDivb == FHex, $"Expected a / b to be equal to FHex, but got error while running test: {TestSet.Title}.");
                    Assert.True(ADivB == F, $"Expected A / B to be equal to F (Dec), but got error while running test: {TestSet.Title}.");
                    Assert.True(aModb == GHex, $"Expected a % b to be equal to GHex, but got error while running test: {TestSet.Title}.");
                    //Assert.True(AModB == G, $"Expected A % B to be equal to G (Dec), but got error while running test: {TestSet.Title}.");
                }
                catch (ArithmeticException)
                {

                }
            }
        }

        [Theory]
        [InlineData("Compare-Tests.txt")]
        public void RunCompareTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", ADecVal = "", BHexVal = "", BDecVal = "", CVal = "", DVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("A", out ADecVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("B", out BDecVal);
                TestSet.Parameters.TryGetValue("c", out CVal);
                TestSet.Parameters.TryGetValue("d", out DVal);
                BigInt a = new BigInt(TestSet.Size, AHexVal);
                BigInt A = new BigInt(TestSet.Size, ADecVal);
                BigInt b = new BigInt(TestSet.Size, BHexVal);
                BigInt B = new BigInt(TestSet.Size, BDecVal);
                int C = int.Parse(CVal); // C = Compare(a, b);
                int D = int.Parse(DVal); // D = Compare(b, a);

                Assert.True(a == A, $"Expected a (Hex) to be equal to A (Dec), but got error while running test: {TestSet.Title}.");
                Assert.True(b == B, $"Expected b (Hex) to be equal to B (Dec), but got error while running test: {TestSet.Title}.");

                // Compare a & b
                int CmpAB = BigInt.Compare(a, b);
                int CmpBA = BigInt.Compare(b, a);

                Assert.True(CmpAB == C, $"Expected cmp(a, b) to be equal to C, but got error while running test: {TestSet.Title}.");
                Assert.True(CmpBA == D, $"Expected cmp(b, a) to be equal to D, but got error while running test: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Convert-Hex-Tests.txt")]
        public void RunConvertHexTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string DecVal = "", HexMal = "", OctVal = "";
                TestSet.Parameters.TryGetValue("d", out DecVal);
                TestSet.Parameters.TryGetValue("h", out HexMal);
                TestSet.Parameters.TryGetValue("o", out OctVal);
                BigInt D = new BigInt(TestSet.Size, DecVal);
                BigInt H = new BigInt(TestSet.Size, HexMal);
                BigInt O = new BigInt(TestSet.Size, OctVal);

                // Compare Dec, Hex and Oct
                Assert.True(D == H, $"Expected D (Dec) to be equal to H (Hex), but got error while running test: {TestSet.Title}.");
                Assert.True(D == O, $"Expected D (Dec) to be equal to O (Oct), but got error while running test: {TestSet.Title}.");

                // Compare converted String with ToString8(), ToString10() and ToString16()
                string DOctalUpper = D.ToString8(false).ToUpper(); // Without 0 fill
                string DHexUpper = D.ToString().ToUpper(); // By default ToString return ToString16

                Assert.True(OctVal.ToUpper() == DOctalUpper, $"Expected o (Octal Value) to be equal to D.ToString8() (Convert to Octal), but got error while running test: {TestSet.Title}.");
                Assert.True(HexMal.ToUpper() == DHexUpper, $"Expected h (Hex Value) to be equal to D.ToString16() (Convert to Hex), but got error while running test: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Div10-Tests.txt")]
        public void RunDiv10TestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", BHexVal = "", CHexVal = "", DHexVal = "", EHexVal = "", FHexVal = "", GHexVal = "", HHexVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);

                BigInt ADiv101 = A.DivMod10();      // A / 10
                BigInt ADiv102 = A.DivMod10(2);     // A / 100
                BigInt ADiv103 = A.DivMod10(3);     // A / 1000
                BigInt ADiv104 = A.DivMod10(4);     // A / 10000
                BigInt ADiv105 = A.DivMod10(5);     // A / 100000
                BigInt ADiv106 = A.DivMod10(6);     // A / 1000000
                BigInt ADiv107 = A.DivMod10(7);     // A / 10000000

                Assert.True(B == ADiv101, $"Expected B to be equal to A / 10, but got wrong value: {TestSet.Title}.");
                Assert.True(C == ADiv102, $"Expected C to be equal to A / 100, but got wrong value: {TestSet.Title}.");
                Assert.True(D == ADiv103, $"Expected D to be equal to A / 1000, but got wrong value: {TestSet.Title}.");
                Assert.True(E == ADiv104, $"Expected E to be equal to A / 10000, but got wrong value: {TestSet.Title}.");
                Assert.True(F == ADiv105, $"Expected F to be equal to A / 100000, but got wrong value: {TestSet.Title}.");
                Assert.True(G == ADiv106, $"Expected G to be equal to A / 1000000, but got wrong value: {TestSet.Title}.");
                Assert.True(H == ADiv107, $"Expected H to be equal to A / 10000000, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Mul10-Tests.txt")]
        public void RunMul10TestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", BHexVal = "", CHexVal = "", DHexVal = "", EHexVal = "", FHexVal = "", GHexVal = "", HHexVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);

                BigInt AMul101 = A.Mul10();         // A * 10
                BigInt AMul102 = AMul101.Mul10();   // A * 100
                BigInt AMul103 = AMul102.Mul10();   // A * 1000
                BigInt AMul104 = AMul103.Mul10();   // A * 10000
                BigInt AMul105 = AMul104.Mul10();   // A * 100000
                BigInt AMul106 = AMul105.Mul10();   // A * 1000000
                BigInt AMul107 = AMul106.Mul10();   // A * 10000000

                Assert.True(B == AMul101, $"Expected B to be equal to A * 10, but got wrong value: {TestSet.Title}.");
                Assert.True(C == AMul102, $"Expected C to be equal to A * 100, but got wrong value: {TestSet.Title}.");
                Assert.True(D == AMul103, $"Expected D to be equal to A * 1000, but got wrong value: {TestSet.Title}.");
                Assert.True(E == AMul104, $"Expected E to be equal to A * 10000, but got wrong value: {TestSet.Title}.");
                Assert.True(F == AMul105, $"Expected F to be equal to A * 100000, but got wrong value: {TestSet.Title}.");
                Assert.True(G == AMul106, $"Expected G to be equal to A * 1000000, but got wrong value: {TestSet.Title}.");
                Assert.True(H == AMul107, $"Expected H to be equal to A * 10000000, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Shift-Left-Tests.txt")]
        public void RunShiftLeftTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", BHexVal = "", CHexVal = "", DHexVal = "", EHexVal = "", FHexVal = "", GHexVal = "", HHexVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);

                BigInt AShift1 = A.Clone(); AShift1.ShiftLeft(1, true);         // A << 1
                BigInt AShift2 = A.Clone(); AShift2.ShiftLeft(2, true);         // A << 2
                BigInt AShift3 = A.Clone(); AShift3.ShiftLeft(3, true);         // A << 3
                BigInt AShift4 = A.Clone(); AShift4.ShiftLeft(4, true);         // A << 4
                BigInt AShift5 = A.Clone(); AShift5.ShiftLeft(5, true);         // A << 5
                BigInt AShift6 = A.Clone(); AShift6.ShiftLeft(6, true);         // A << 6
                BigInt AShift7 = A.Clone(); AShift7.ShiftLeft(7, true);         // A << 7

                Assert.True(B == AShift1, $"Expected B to be equal to A << 1, but got wrong value: {TestSet.Title}.");
                Assert.True(C == AShift2, $"Expected C to be equal to A << 2, but got wrong value: {TestSet.Title}.");
                Assert.True(D == AShift3, $"Expected D to be equal to A << 3, but got wrong value: {TestSet.Title}.");
                Assert.True(E == AShift4, $"Expected E to be equal to A << 4, but got wrong value: {TestSet.Title}.");
                Assert.True(F == AShift5, $"Expected F to be equal to A << 5, but got wrong value: {TestSet.Title}.");
                Assert.True(G == AShift6, $"Expected G to be equal to A << 6, but got wrong value: {TestSet.Title}.");
                Assert.True(H == AShift7, $"Expected H to be equal to A << 7, but got wrong value: {TestSet.Title}.");
            }
        }

        [Theory]
        [InlineData("Shift-Right-Tests.txt")]
        public void RunShiftRightTestsSuccess(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile);
            foreach (TestData TestSet in TestSets)
            {
                string AHexVal = "", BHexVal = "", CHexVal = "", DHexVal = "", EHexVal = "", FHexVal = "", GHexVal = "", HHexVal = "";
                TestSet.Parameters.TryGetValue("a", out AHexVal);
                TestSet.Parameters.TryGetValue("b", out BHexVal);
                TestSet.Parameters.TryGetValue("c", out CHexVal);
                TestSet.Parameters.TryGetValue("d", out DHexVal);
                TestSet.Parameters.TryGetValue("e", out EHexVal);
                TestSet.Parameters.TryGetValue("f", out FHexVal);
                TestSet.Parameters.TryGetValue("g", out GHexVal);
                TestSet.Parameters.TryGetValue("h", out HHexVal);

                BigInt A = new BigInt(TestSet.Size, AHexVal);
                BigInt B = new BigInt(TestSet.Size, BHexVal);
                BigInt C = new BigInt(TestSet.Size, CHexVal);
                BigInt D = new BigInt(TestSet.Size, DHexVal);
                BigInt E = new BigInt(TestSet.Size, EHexVal);
                BigInt F = new BigInt(TestSet.Size, FHexVal);
                BigInt G = new BigInt(TestSet.Size, GHexVal);
                BigInt H = new BigInt(TestSet.Size, HHexVal);

                BigInt AShift1 = A >> 1;
                BigInt AShift2 = A >> 2;
                BigInt AShift3 = A >> 3;
                BigInt AShift4 = A >> 4;
                BigInt AShift5 = A >> 5;
                BigInt AShift6 = A >> 6;
                BigInt AShift7 = A >> 7;

                Assert.True(B == AShift1, $"Expected B to be equal to A >> 1, but got wrong value: {TestSet.Title}.");
                Assert.True(C == AShift2, $"Expected C to be equal to A >> 2, but got wrong value: {TestSet.Title}.");
                Assert.True(D == AShift3, $"Expected D to be equal to A >> 3, but got wrong value: {TestSet.Title}.");
                Assert.True(E == AShift4, $"Expected E to be equal to A >> 4, but got wrong value: {TestSet.Title}.");
                Assert.True(F == AShift5, $"Expected F to be equal to A >> 5, but got wrong value: {TestSet.Title}.");
                Assert.True(G == AShift6, $"Expected G to be equal to A >> 6, but got wrong value: {TestSet.Title}.");
                Assert.True(H == AShift7, $"Expected H to be equal to A >> 7, but got wrong value: {TestSet.Title}.");
            }
        }
    }
}
