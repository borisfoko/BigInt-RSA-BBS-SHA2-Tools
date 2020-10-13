using System;
using System.Text;

namespace SHA2.Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            //string SHA256Val1 = SHA256Tool.SHA256("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"); 

            //string SHA256Val2 = SHA256Tool.SHA256(SHA256Val1);
            ////string SHA256Val3 = SHA256Tool.SHA256("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //string SHA256Val4 = SHA256Tool.SHA256("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //string SHA256Val5 = SHA256Tool.SHA256("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            //byte[] HBytes = Encoding.Default.GetBytes("68325720aabd7c82f30f554b313d0570c95accbb7dc4b5aae11204c08ffe732b");
            //string[] Test1Expected = { "bd800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008" };
            //string[] Test1Result = SHA256Tool.GetBlocks("bd");

            //string[] Test2Expected = { "61626380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000018" };
            //string[] Test2Result = SHA256Tool.GetBlocks("616263");

            //string[] Test3Expected = { "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000001b8" };
            //string[] Test3Result = SHA256Tool.GetBlocks("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            //string[] Test4Expected = {
            //    "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000",
            //    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001c0"};
            //string[] Test4Result = SHA256Tool.GetBlocks("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            //string[] Test5Expected = { 
            //    "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000080000000000000",
            //    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001c8"};
            //string[] Test5Result = SHA256Tool.GetBlocks("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            //string[] Test6Expected = { 
            //    "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f70718000000000000000",
            //    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001c0"};
            //string[] Test6Result = SHA256Tool.GetBlocks("6162636462636465636465666465666765666768666768696768696A68696A6B696A6B6C6A6B6C6D6B6C6D6E6C6D6E6F6D6E6F706E6F7071");

            //if (Test6Expected != Test6Result)
            //    Console.WriteLine("Error on Test 6");

            //string[] Test7Expected = { 
            //    "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
            //    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000003b8"};
            //string[] Test7Result = SHA256Tool.GetBlocks("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            //string[] Test8Expected = { 
            //    "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
            //    "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000",
            //    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003c0"
            //};
            //string[] Test8Result = SHA256Tool.GetBlocks("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            //string[] Test9Expected = {
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
            //    "80000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000"
            //};
            //string[] Test9Result = SHA256Tool.GetBlocks("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
        }
    }
}
