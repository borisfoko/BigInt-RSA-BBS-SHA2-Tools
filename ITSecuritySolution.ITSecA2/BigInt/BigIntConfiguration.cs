using System;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt
{
    public enum signt2
    {
        pp,
        np,
        pn,
        nn
    }

    public enum signt
    {
        positive,
        negative
    }

    public enum OutputFormat
    {
        delimiter,  // cells separated with hex 
        hex,        // even number of hex values
        allHex     // all bits of value
    }

    public enum BigIntOperation
    {
        None = 0,            // Set operation to none 
        ShiftRight = 1,      // Shift right
        ShiftLeft = 2,       // Shift left
        Mul10 = 3,           // Multiplication by 10
        Div10 = 4,           // Division by 10
        DecToHex = 5,        // Convert decimal to hexadecimal
        CompareHexDec = 6,   // Compare hexadecimal & decimal
        ArithHexDec = 7      // Arithmetic operation on hexadecimal & decimal
    }

    public class BigIntConfiguration
    {
        // public final static short INIT_SIZE = 200;  // min number of cells
        public readonly static short INIT_SIZE = 1000;  // min number of cells
        public readonly static short MAX_SIZE = 6000;  // max number of cells

        public readonly static short SPART_LIMIT = 30;        // minimun spart for mulKara
        //public readonly static short CELL_SIZE = 16;           // 32 bit base cells
        public readonly static short CELL_SIZE = 32;          // 64 bit base cells
        public readonly static short BINARY_CELL_SIZE_POW = 5;  // 5 => CELL_SIZE = 2 ^ 5 = 32
        // public readonly static uint CELL_MASK = 0xFFFF;         // 32 bit mask
        public readonly static uint CELL_MASK = 0xFFFFFFFF;    // 64 bit mask
        // public readonly static uint MAX_CELL_VALUE = 0x10000;
        public readonly static ulong MAX_CELL_VALUE = 0x100000000;
        // public readonly static uint HIGH_BIT_MASK = 0x8000;      // 32 bit highBit mask
        public readonly static uint HIGH_BIT_MASK = 0x80000000;  // 64 bit highBit mask
        // public readonly static int HIGH_SHIFT = 15;              // 32 bit shift count
        public readonly static int HIGH_SHIFT = 31;              // 64 bit shift count
        public readonly static short CELL_SIZE_HEX = (short)(CELL_SIZE / 4);   // hex number per cell

        // List of prime number <= 37
        public readonly static int[] PRIME_NUMBERS_LIST2000 = { 
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97,
            101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
            211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293,
            307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397,
            401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499,
            503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599,
            601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691,
            701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797,
            809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887,
            907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997,
            1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097,
            1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193,
            1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279, 1283, 1289, 1291, 1297,
            1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367, 1373, 1381, 1399,
            1409, 1423, 1427, 1429, 1433, 1439, 1447, 1451, 1453, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499,
            1511, 1523, 1531, 1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 1597,
            1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637, 1657, 1663, 1667, 1669, 1693, 1697, 1699,
            1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759, 1777, 1783, 1787, 1789,
            1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877, 1879, 1889,
            1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973, 1979, 1987, 1993, 1997, 1999  };
    }

    public static class BigIntOperationExtensions
    {
        public static string AsString(this BigIntOperation operation)
        {
            switch (operation)
            {
                case BigIntOperation.None: return "None";
                case BigIntOperation.ShiftRight: return "ShiftRight";
                case BigIntOperation.ShiftLeft: return "ShiftLeft";
                case BigIntOperation.Mul10: return "Mul10";
                case BigIntOperation.Div10: return "Div10";
                case BigIntOperation.DecToHex: return "dec-Hex";
                case BigIntOperation.CompareHexDec: return "Compare-HexDec";
                case BigIntOperation.ArithHexDec: return "Arith-HexDec";
                default: throw new ArgumentException("Unknow operation's type provided.");
            }
        }

        public static string GetOperation(string value)
        {
            if (value.Contains(BigIntOperation.ShiftLeft.AsString()))
            {
                return BigIntOperation.ShiftLeft.AsString();
            }
            else if (value.Contains(BigIntOperation.ShiftRight.AsString()))
            {
                return BigIntOperation.ShiftRight.AsString();
            }
            else if (value.Contains(BigIntOperation.Mul10.AsString()))
            {
                return BigIntOperation.Mul10.AsString();
            }
            else if (value.Contains(BigIntOperation.Div10.AsString()))
            {
                return BigIntOperation.Div10.AsString();
            }
            else if (value.Contains(BigIntOperation.DecToHex.AsString()))
            {
                return BigIntOperation.DecToHex.AsString();
            }
            else if (value.Contains(BigIntOperation.CompareHexDec.AsString()))
            {
                return BigIntOperation.CompareHexDec.AsString();
            }
            else if (value.Contains(BigIntOperation.ArithHexDec.AsString()))
            {
                return BigIntOperation.ArithHexDec.AsString();
            }
            else
            {
                return BigIntOperation.None.AsString();
            }
        }
    }

    public static class SigntExtensions
    {
        public static string AsString(this signt sign)
        {
            switch (sign)
            {
                case signt.positive: return "+";
                case signt.negative: return "-";
                default: throw new ArgumentException();
            }
        }
    }
}
