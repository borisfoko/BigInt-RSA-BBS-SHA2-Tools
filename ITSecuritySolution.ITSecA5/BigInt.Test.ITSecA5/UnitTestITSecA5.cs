using SHA2.Utils;
using System;
using System.Collections.Generic;
using Xunit;

namespace BigInt.Test.ITSecA5
{
    public class UnitTestITSecA5
    {
        [Theory]
        [InlineData("sha256-tests.txt")]
        public void RunSHA256Test(string TestFile)
        {
            List<TestData> TestSets = DataLoader.LoadData(TestFile, true);
            string LogText = "";
            foreach (TestData TestSet in TestSets)
            {
                string PVal = "", WVal = "", hVal = "", dVal = "";
                TestSet.Parameters.TryGetValue("p", out PVal);
                TestSet.Parameters.TryGetValue("h", out hVal);
                TestSet.Parameters.TryGetValue("d", out dVal);
                // Check W values including padding
                string[] SHA256Blocks = SHA256Tool.GetBlocks(PVal);
                for(int i = 0; i < SHA256Blocks.Length; i++)
                {
                    if (i == 0)
                    {
                        TestSet.Parameters.TryGetValue("W", out WVal);
                    }
                    else
                    {
                        TestSet.Parameters.TryGetValue($"W{i}", out WVal);
                    }

                    Assert.True($"+0x{SHA256Blocks[i]}" == WVal, $"{TestSet.Title} - Expected SHA256Blocks[i] to be equal to WVal: {WVal}, but got wrong value: +0x{SHA256Blocks[i]}.");
                }

                // # sha256(...)
                LogText = LogText + $"t={TestSet.Title}\n";
                LogText = LogText + "# sha256(...)\n";
                string SHA256HVal = SHA256Tool.SHA256(PVal, ref LogText, true);
                LogText = LogText + $"h={SHA256HVal}\n";

                // # sha256(sha256(...))
                LogText = LogText + "# sha256(sha256(...))\n";
                string SHA256DVal = SHA256Tool.SHA256(SHA256HVal, ref LogText, true);
                SHA256DVal = "+0x" + SHA256DVal;
                LogText = LogText + $"d={SHA256DVal}\n\n";

                Assert.True(SHA256HVal == hVal, $"{TestSet.Title} - Expected SHA256HVal to be equal to hVal: {hVal}, but got wrong value: {SHA256HVal}.");
                Assert.True(SHA256DVal == dVal, $"{TestSet.Title} - Expected SHA256DVal to be equal to dVal: {dVal}, but got wrong value: {SHA256DVal}.");
            }
            SHA256Tool.Log(LogText, "SHA256_Test");
        }
    }
}
