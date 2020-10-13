using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt
{
    public class BigInt : BigNumber
    {
        #region constructors
        public BigInt() : base()
        {
        }

        public BigInt(BigInt bi) : base(bi)
        {
        }

        public BigInt(BigInt bi, signt s) : base(bi, s)
        {
        }

        public BigInt(string Str)
        {
            Size = INIT_SIZE;
            if (Str != null && Str.Length >= 1)
            {
                Sign = Str[0] == '-' ? signt.negative : signt.positive;
            }
            ToBigInt(Str);
        }

        public BigInt(short BitSize, string Str) : base(BitSize, Str)
        {
        }

        public BigInt(ulong Val) : base(Val)
        {
        }

        public BigInt(long Val) : base(Val)
        {
        }

        public BigInt(short BitSize, long Val) : base(BitSize, Val)
        {
        }

        public BigInt(short BitSize, ulong Val) : base(BitSize, Val)
        {
        }

        public BigInt(short BitSize, uint[] Value) : base(BitSize, Value)
        { }

        #endregion

        #region public basic function & procedure
        /// <summary>
        /// Return true if this is an even number (gerade)
        /// </summary>
        /// <returns></returns>
        public bool Even()
        {
            BigInt A = this.Clone();

            if (A % 2 != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return true if this is not an even number (ungerade) 
        /// </summary>
        /// <returns></returns>
        public bool Odd()
        {
            BigInt A = this.Clone();

            if (A % 2 != 0)
            {
                return true;
            }

            return false;
        }

        public BigInt Clone()
        {
            BigInt tmp = new BigInt(this.Size, 0);
            try
            {
                tmp.Size = this.Size;
                tmp.Spart = this.Spart;
                tmp.Sign = this.Sign;
                for (int i = 0; i < this.Spart; i++)
                {
                    tmp.Value[i] = this.Value[i];
                }
            }
            catch (NotImplementedException e)
            {
                throw new NotImplementedException(e.Message);
            }

            return tmp;
        }

        public void AddCell2(int Index, ulong CellValue)
        {
            this.Resize((short)(this.Spart + 1));
            ulong Tmp, Over = CellValue;
            while (Over != 0)
            {
                ulong Val = this.Value[Index];
                Tmp = Val + Over;
                this.Value[Index++] = (uint)(Tmp & CELL_MASK);
                Over = (Tmp >> CELL_SIZE) & CELL_MASK;
                //Value[Index++] = (uint)(Over & CELL_MASK);
                //Over >>= CELL_SIZE;
            }

            this.Reduce();
        }

        public void SubCell2(short Index, ulong CellValue)
        {
            ulong Tmp, Over = CellValue;

            while (Over != 0)
            {
                Tmp = this.Value[Index] - (Over & CELL_MASK);

                if (Over > this.Value[Index])
                {
                    Over = Over - this.Value[Index];
                }
                else
                {
                    Over = (Tmp >> CELL_SIZE) & CELL_MASK;
                }
                this.Value[Index++] = (uint)Tmp & CELL_MASK;
            }

            this.Reduce();
        }

        public static int Compare(BigInt B1, BigInt B2)
        {
            int C = 0;
            switch (B1.LookAtSign(B2))
            {
                case signt2.pp:
                    if (B1 == B2)
                    {
                        C = 0;
                    }
                    else if (B1 > B2)
                    {
                        C = 1;
                    }
                    else if (B1 < B2)
                    {
                        C = -1;
                    }
                    break;
                case signt2.np:
                    C = -1;
                    break;
                case signt2.pn:
                    C = 1;
                    break;
                case signt2.nn:
                    if (B1 == B2)
                    {
                        C = 0;
                    }
                    else if (B1 > B2)
                    {
                        C = -1;
                    }
                    else if (B1 < B2)
                    {
                        C = 1;
                    }
                    break;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }

            return C;
        }
        #endregion

        #region Arithmetic function & procedure (ITSecA1)
        public BigInt Mul8()
        {
            BigInt C = new BigInt(this.Size, 0);
            BigInt B1 = this.Clone();
            _MulNumber(ref C, ref B1, 8);
            return C;
        }

        public BigInt Mul10()
        {
            BigInt C = new BigInt(this.Size, 0);
            BigInt B1 = this.Clone();
            _MulNumber(ref C, ref B1, 10);
            return C;
        }

        public BigInt Mul16()
        {
            BigInt C = new BigInt(this.Size, 0);
            BigInt B1 = this.Clone();
            _MulNumber(ref C, ref B1, 16);
            return C;
        }

        public BigInt DivMod10(int m = 1)
        {
            BigInt A = this.Clone();
            BigInt Q = new BigInt(this.Size, 0);
            BigInt R = new BigInt(this.Size, 0);
            _DivCellMod(ref Q, ref R, A, (uint)Math.Pow(10, m), A.Sign);// A / 10 (where A = this.clone())
            return Q;
        }

        public BigInt DivMod16(int m = 1)
        {
            BigInt A = this.Clone();
            BigInt Q = new BigInt(this.Size, 0);
            BigInt R = new BigInt(this.Size, 0);
            _DivCellMod(ref Q, ref R, A, (uint)Math.Pow(16, m), A.Sign);// A / 16 (where A = this.clone())
            return R;
        }

        public static BigInt operator +(BigInt B1, BigInt B2)
        {
            BigInt C = new BigInt(B2.Size, 0);
            switch (B1.LookAtSign(B2))
            {
                case signt2.pp:
                    _Add(ref C, B1, B2, signt.positive);
                    return C;
                case signt2.np:
                    if (B1 <= B2)
                    {
                        _Sub(ref C, B2, B1, signt.positive);
                        return C;
                    }
                    else
                    {
                        _Sub(ref C, B1, B2, signt.negative);
                        return C;
                    }

                case signt2.pn:
                    if (B1 <= B2)
                    {
                        _Sub(ref C, B2, B1, signt.negative);
                        return C;
                    }
                    else
                    {
                        _Sub(ref C, B1, B2, signt.positive);
                        return C;
                    }
                case signt2.nn:
                    _Add(ref C, B1, B2, signt.negative);
                    return C;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }
        }

        public static BigInt operator +(BigInt B, ulong Number)
        {
            BigInt B1 = B.Clone();
            B1.AddCell2(0, Number);
            return B1;
        }

        public static BigInt operator ++(BigInt B)
        {
            return (B + 1);
        }

        public static BigInt operator -(BigInt B1, BigInt B2)
        {
            BigInt C = new BigInt(B2.Size, 0);
            switch (B1.LookAtSign(B2))
            {
                case signt2.pp:
                    if (B1 <= B2)
                    {
                        _Sub(ref C, B2, B1, signt.negative);
                        return C;
                    }
                    else
                    {
                        _Sub(ref C, B1, B2, signt.positive);
                        return C;
                    }
                case signt2.np:
                    _Add(ref C, B1, B2, signt.negative);
                    return C;
                case signt2.pn:
                    _Add(ref C, B1, B2, signt.positive);
                    return C;
                case signt2.nn:
                    if (B1 <= B2)
                    {
                        _Sub(ref C, B2, B1, signt.positive);
                        return C;
                    }
                    else
                    {
                        _Sub(ref C, B1, B2, signt.negative);
                        return C;
                    }
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }
        }

        public static BigInt operator -(BigInt B, ulong Number)
        {
            BigInt B1 = B.Clone();
            B1.SubCell2(0, Number);
            return B1;
        }

        public static BigInt operator --(BigInt B)
        {
            return (B - 1);
        }

        public static BigInt operator *(BigInt B1, BigInt B2)
        {
            BigInt C = new BigInt(B2.Size, 0);
            switch (B1.LookAtSign(B2))
            {
                case signt2.pp:
                    _MulKara(ref C, ref B1, ref B2, signt.positive);
                    return C;
                case signt2.np:
                    _MulKara(ref C, ref B1, ref B2, signt.negative);
                    return C;
                case signt2.pn:
                    _MulKara(ref C, ref B1, ref B2, signt.negative);
                    return C;
                case signt2.nn:
                    _MulKara(ref C, ref B1, ref B2, signt.positive);
                    return C;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }
        }

        public static BigInt operator *(BigInt B, ulong number)
        {
            BigInt C = new BigInt(B.Size, 0);
            _MulNumber(ref C, ref B, number);
            return C;
        }

        public static BigInt operator /(BigInt B1, BigInt B2)
        {
            B2 = (BigInt)B1.SameSize(B2);
            BigInt B2Copy = B2.Clone();
            BigInt Q = new BigInt(B2Copy.Size, 0);
            BigInt R = new BigInt(B2Copy.Size, 0);
            switch (B1.LookAtSign(B2Copy))
            {
                case signt2.pp:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    return Q;
                case signt2.np:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    return Q;
                case signt2.pn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    return Q;
                case signt2.nn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    return Q;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }
        }

        public static BigInt operator /(BigInt B1, long B2)
        {
            return B1 / new BigInt(B1.Size, B2);
        }

        public static BigInt operator %(BigInt B1, BigInt B2)
        {
            B2 = (BigInt)B1.SameSize(B2);
            BigInt B2Copy = B2.Clone();
            BigInt Q = new BigInt(B2Copy.Size, 0);
            BigInt R = new BigInt(B2Copy.Size, 0);
            switch (B1.LookAtSign(B2Copy))
            {
                case signt2.pp:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    break;
                case signt2.np:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    break;
                case signt2.pn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    break;
                case signt2.nn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    break;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }

            if (B1.Sign == signt.negative && R != 0)
            {
                BigInt RCopy = R.Clone();
                RCopy.Sign = signt.negative;
                B2Copy.Sign = signt.positive;

                RCopy = RCopy + B2Copy;
                while (!(RCopy >= 0 && RCopy.Sign == signt.positive))
                {
                    RCopy = RCopy + B2Copy;
                }
                R = RCopy.Clone();
                R.Sign = signt.positive;
            }
            return R;
        }

        public static BigInt operator %(BigInt B1, long B2)
        {
            return B1 % new BigInt(B1.Size, B2);
        }

        public static BigInt operator <<(BigInt B, int ShiftValue)
        {
            B.ShiftLeft((short)ShiftValue);
            return B;
        }

        public static BigInt operator >>(BigInt B, int ShiftValue)
        {
            BigInt BCopy = B.Clone();
            for (int i = 0; i < ShiftValue; i++)
            {
                BCopy.ShiftRigth();
            }

            return BCopy;
        }

        public static bool operator <=(BigInt B1, BigInt B2)
        {
            return B1 < B2 || B1.Equals(B2);
        }

        public static bool operator >=(BigInt B1, BigInt B2)
        {
            return B1 > B2 || B1.Equals(B2);
        }

        public static bool operator <(BigInt B1, BigInt B2)
        {
            // The sign of the number will not be considered in this case. 
            // The BigInt will be considered as absolut values
            B2 = (BigInt)B1.SameSize(B2);
            int i = MAX_SIZE;

            for (i = B1.Spart - 1; i >= 0 && B1.Value[i] == B2.Value[i]; i--) ;

            if (i >= 0)
            {
                if (B1.Value[i] < B2.Value[i])
                    return true;
                return false;
            }

            return false;
        }

        public static bool operator <(BigInt B1, uint B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);

            return B1 < B2B;
        }

        public static bool operator <=(BigInt B1, uint B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);

            return B1 <= B2B;
        }

        public static bool operator >(BigInt B1, BigInt B2)
        {
            // The sign of the number will not be considered in this case. 
            // The BigInt will be considered as absolut values
            B2 = (BigInt)B1.SameSize(B2);
            int i = MAX_SIZE;

            for (i = B1.Spart - 1; i >= 0 && B1.Value[i] == B2.Value[i]; i--) ;

            if (i >= 0)
            {
                if (B1.Value[i] > B2.Value[i])
                    return true;
                return false;
            }

            return false;
        }

        public static bool operator >(BigInt B1, uint B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);

            return B1 > B2B;
        }

        public static bool operator >=(BigInt B1, uint B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);

            return B1 >= B2B;
        }

        public static bool operator ==(BigInt B1, BigInt B2)
        {
            return B1.Equals(B2);
        }

        public static bool operator ==(BigInt B1, long B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);
            B2B.Sign = B1.Sign;

            return B1 == B2B;
        }

        public static bool operator !=(BigInt B1, BigInt B2)
        {
            return !B1.Equals(B2);
        }

        public static bool operator !=(BigInt B1, long B2)
        {
            BigInt B2B = new BigInt(B1.Size, B2);
            B2B.Sign = B1.Sign;

            return B1 != B2B;
        }
        #endregion

        #region converts functions (ToString, To...)
        public override void ToBigInt(string Str)
        {
            BigInt ResultBigInt = new BigInt(this.Size, 0);
            string HString = "0123456789ABCDEF";
            string StrUpper = Str.ToUpper();
            if (IsNumeric(StrUpper))
            {
                if (StrUpper.StartsWith("+") || StrUpper.StartsWith("-"))
                    StrUpper = StrUpper.Substring(1, StrUpper.Length - 1);

                for (int i = 0; i < StrUpper.Length; i++)
                {
                    char cellVal = StrUpper[i];
                    int n = HString.IndexOf(cellVal);
                    if (n == -1)
                        throw (new ArgumentException("Invalid string in constructor."));
                    else
                    {
                        ResultBigInt = ResultBigInt.Mul10() + (uint)n;
                    }
                }
                ResultBigInt.Reduce();
            }
            else if (StrUpper.Contains('O'))
            {
                int xPos = StrUpper.IndexOf("O");
                if (xPos != -1)
                {
                    StrUpper = StrUpper.Substring(xPos + 1, (StrUpper.Length - xPos - 1));
                }
                for (int i = 0; i < StrUpper.Length; i++)
                {
                    char cellVal = StrUpper[i];
                    int n = HString.IndexOf(cellVal);
                    if (n == -1)
                        throw (new ArgumentException("Invalid string in constructor."));
                    else
                    {
                        ResultBigInt = ResultBigInt.Mul8() + (uint)n;
                    }
                }
                ResultBigInt.Reduce();
            }
            else
            {
                int xPos = StrUpper.IndexOf("X");
                if (xPos != -1)
                {
                    StrUpper = StrUpper.Substring(xPos + 1, (StrUpper.Length - xPos - 1));
                }
                for (int i = 0; i < StrUpper.Length; i++)
                {
                    char cellVal = StrUpper[i];
                    int n = HString.IndexOf(cellVal);
                    if (n == -1)
                        throw (new ArgumentException("Invalid string in constructor."));
                    else
                    {
                        ResultBigInt = ResultBigInt.Mul16() + (uint)n;
                    }
                }
                ResultBigInt.Reduce();
            }

            

            /*if (Sign == signt.negative) // overflow check for overflow on negative value
            {
                if ((ResultBigInt.Value[Size - 1] & HIGH_BIT_MASK) == 0)
                    throw (new OverflowException("Size overflow, while initialising the value array."));
            }
            else // overflow check for overflow on positive value
            {
                if ((ResultBigInt.Value[Size - 1] & HIGH_BIT_MASK) != 0)
                    throw (new OverflowException("Size overflow, while initialising the value array."));
            }*/

            for (int i = 0; i < ResultBigInt.Spart; i++)
                this.Value[i] = ResultBigInt.Value[i];

            this.Spart = ResultBigInt.Spart;


            if (this.Spart == 0)
                this.Spart = 1;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ToString16(true);
        }

        public string ToString8(bool Fill = true)
        {
            return ToString(8, Fill);
        }

        public string ToString10(bool Fill = true)
        {
            return ToString(10, Fill);
        }

        private string ToString16(bool Fill = true)
        {
            return ToString16(Fill, OutputFormat.hex);
        }

        private string ToString16(bool Fill, OutputFormat OutType)
        {
            return ToString(16, Fill, OutType);
        }

        private string ToString(uint DefaultBase, bool Fill = true, OutputFormat OutType = OutputFormat.hex)
        {
            if (DefaultBase < 2 || DefaultBase > 16)
                throw new ArgumentOutOfRangeException("Provided argument DefaultBase is out of range.");

            string HString = "0123456789ABCDEF";
            string Result = "";

            BigInt B = this.Clone();
            BigInt Quotient = new BigInt(this.Size, 0);
            BigInt Remainder = new BigInt(this.Size, 0);
            if (B.Spart == 1 && B.Value[0] == 0)
                Result = "0";
            else
            {
                while (B.Spart > 1 || (B.Spart == 1 && B.Value[0] != 0))
                {
                    _DivCellMod(ref Quotient, ref Remainder, B, DefaultBase, B.Sign);

                    if (Remainder.Value[0] < 10)
                        Result = Remainder.Value[0] + Result;
                    else
                        Result = HString[(int)(Remainder.Value[0])] + Result;

                    B = Quotient.Clone();
                }
            }

            // Fill the result with 000... based on size
            if (Fill)
            {
                int ResultSize = this.Size / 4 - Result.Length;
                while (ResultSize > 0)
                {
                    Result = "0" + Result;
                    ResultSize--;
                }
            }
            else if (Result.Length == 0)
            {
                Result = "0";
            }

            if (DefaultBase == 8)
                Result = this.Sign == signt.positive ? $"+0o{Result}" : $"-0o{Result}";
            else if (DefaultBase == 10)
                Result = this.Sign == signt.positive ? $"+{Result}" : $"-{Result}";
            else if (DefaultBase == 16)
                Result = this.Sign == signt.positive ? $"+0x{Result}" : $"-0x{Result}";

            return Result;
        }
        #endregion

        #region private static procedure (basic functions)
        private static void _MulNumber(ref BigInt C, ref BigInt B, ulong A)
        {
            if (B == 0)
            {
                C = new BigInt(B.Size, 0);
            }
            else if (A == 0)
            {
                C = new BigInt(B.Size, 0);
            }
            else if (A == 1)
            {
                C = B;
            }
            else if (A == 2)
            {
                C = B.Clone();
                C.ShiftLeft(1);
            }
            else if (A == 8)
            {
                C = B.Clone();          // C = B
                C.ShiftLeft(2);         // C = C * 4
                C.ShiftLeft(1);         // C = C * 2
            }
            else if (A == 10)
            {
                C = B.Clone();          // C = B
                C.ShiftLeft(2);         // C = C * 4
                C = C + B;              // C = C + B
                C.ShiftLeft(1);         // C = C * 2
            }
            else if (A == 16)
            {
                C = B.Clone();          // C = B
                C.ShiftLeft(2);         // C = C * 4
                C.ShiftLeft(2);         // C = C * 4
            }
            else
            {
                BigInt A1 = new BigInt(B.Size, A);
                _Mul(ref C, ref B, ref A1, B.Sign);
            }
        }

        private static void _Add(ref BigInt C, BigInt B1, BigInt B2, signt ResultSign)
        {
            ulong Tmp, Over = 0;
            B2 = (BigInt) B1.SameSize(B2);
            C.Resize((short)(B1.Spart + 1));

            for (int i = 0; i < B1.Spart; i++)
            {
                ulong B1Val = B1.Value[i], B2Val = B2.Value[i];
                Tmp = B1Val + B2Val + Over;
                C.Value[i] = (uint)(Tmp & CELL_MASK);
                Over = (Tmp >> CELL_SIZE) & CELL_MASK;
            }

            C.Value[B1.Spart] = (uint)Over;
            C.Sign = ResultSign;
        }

        private static void _Sub(ref BigInt C, BigInt B1, BigInt B2, signt ResultSign)
        {
            long Tmp;
            short Over = 0;
            B2 = (BigInt) B1.SameSize(B2);
            C.Resize((short)(B1.Spart + 1));

            for (int i = 0; i < B1.Spart; i++)
            {
                long B1Val = B1.Value[i], B2Val = B2.Value[i];
                Tmp = B1Val - B2Val + Over;
                C.Value[i] = (uint)(Tmp & CELL_MASK);
                Over = (short)((Tmp >> CELL_SIZE) & CELL_MASK);
            }

            C.Value[B1.Spart] = (uint)Over;
            C.Sign = ResultSign;
            C.Reduce();
        }

        private static void _MulCell(ref BigInt C, ref BigInt B1, ulong A)
        {
            BigInt B1C = B1.Clone(); // C = B
            while (A > 1)
            {
                if (A % 16 == 0)
                {
                    B1C.ShiftLeft(2);         // C = C * 4
                    B1C.ShiftLeft(2);         // C = C * 4
                    A = A / 16;
                }
                else if (A % 10 == 0)
                {
                    B1C.ShiftLeft(2);         // C = C * 4
                    B1C = B1C + B1;              // C = C + B
                    B1C.ShiftLeft(1);         // C = C * 2
                    A = A / 10;
                }
                else if (A % 2 == 0)
                {
                    B1C = B1.Clone();
                    B1C.ShiftLeft(1);
                    A = A / 2;
                }
                else
                {
                    // A is not a multiple of 2, 10 or 16
                    BigInt C1 = new BigInt(B1C.Size, 0);
                    BigInt A1 = new BigInt(B1C.Size, A);
                    _Mul(ref C1, ref B1C, ref A1, B1C.Sign);
                    B1C = C1.Clone();
                    break;
                }
            }

            C = B1C.Clone();
        }

        private static void _Mul(ref BigInt C, ref BigInt B1, ref BigInt B2, signt ResultSign)
        {
            ulong Tmp;
            B2 = (BigInt) B1.SameSize(B2);
            C.Resize((short)(2 * B1.Spart + 1));
            for (int i = 0; i < B2.Spart; i++)
            {
                for (int j = 0; j < B1.Spart; j++)
                {
                    ulong B1Val = B1.Value[j], B2Val = B2.Value[i];
                    Tmp = B1Val * B2Val;
                    C.AddCell2((short)(i + j), Tmp);
                }
            }
            C.Sign = ResultSign;
        }

        private static void _MulKara(ref BigInt C, ref BigInt B1, ref BigInt B2, signt ResultSign)
        {
            short B1Spart = B1.Spart;
            short B2Spart = B2.Spart;

            B1.Reduce();
            B2.Reduce();
            B2 = (BigInt) B1.SameSize(B2);
            C.Resize((short) (2 * B1.Spart + 1));

            if (B1.Spart <= SPART_LIMIT || ((B1Spart < SPART_LIMIT && (B2Spart - B1Spart) > SPART_LIMIT) || (B2Spart < SPART_LIMIT && (B1Spart - B2Spart) > SPART_LIMIT)))
            {
                _Mul(ref C, ref B1, ref B2, ResultSign);
            }
            else
            {
                B1.Reduce();
                B2.Reduce();
                short Part = B1.Spart < B2.Spart ? (short)(B1.Spart / 2) : (short)(B2.Spart / 2);

                //short PartCopy = Part;
                //while (PartCopy != 0 && PartCopy < (B1.Spart < B2.Spart ? B1.Spart : B2.Spart) && CELL_SIZE % PartCopy != 0)
                //{
                //    PartCopy++;
                //}
                //if (PartCopy != 0 && CELL_SIZE % PartCopy == 0)
                //{
                //    Part = PartCopy;
                //}
                //else
                //{
                //    PartCopy = Part;
                //    while (PartCopy > 0 && CELL_SIZE % PartCopy != 0)
                //    {
                //        PartCopy--;
                //    }
                //    Part = PartCopy;
                //}
                
                B2 = (BigInt)B1.SameSize(B2);

                BigInt D = new BigInt(B1.Size, 0), E = new BigInt(B1.Size, 0);
                BigInt F = new BigInt(B1.Size, 0), G = new BigInt(B1.Size, 0);
                BigInt Z0 = new BigInt(B1.Size, 0), Z1 = new BigInt(B1.Size, 0), Z2 = new BigInt(B1.Size, 0);
                // C  := Z2 * (Base ^ (2 * Part)) + (Z1 - Z2 - Z0) * (Base ^ Part) + Z0
                B1.Split(ref D, ref E, Part);
                B2.Split(ref F, ref G, Part);
                // K = D + E
                // L = F + G
                BigInt K = D + E;
                BigInt L = F + G;
                // Z0 := E*G
                // Z1 := K*L 
                // Z2 := D*F
                _MulKara(ref Z0, ref E, ref G, ResultSign);
                _MulKara(ref Z1, ref K, ref L, ResultSign);
                _MulKara(ref Z2, ref D, ref F, ResultSign);
                Z0.Reduce();
                Z1.Reduce();
                Z2.Reduce();

                // Z2Copy = Z2 * (Base ^ (2 * Part))
                BigInt Z2Copy = Z2.Clone();
                for (int i = 0; i < 2 * Part; i++)
                {
                    // Z2 * Base
                    // Base = 2 ^ CELL_SIZE
                    Z2Copy.ShiftLeft(CELL_SIZE);
                }

                // (Z1 - Z2 - Z0) * (Base ^ Part)
                BigInt ZDiff = Z1 - Z2 - Z0;
                for (int i = 0; i < Part; i++)
                {
                    // ZDiff * Base
                    // Base = 2 ^ CELL_SIZE
                    ZDiff.ShiftLeft(CELL_SIZE);
                }

                C = Z2Copy + ZDiff + Z0;

                C.Reduce();
            }
        }

        private static void _Divmod(ref BigInt Q, ref BigInt R, ref BigInt B1, ref BigInt B2, signt ResultSign)
        {
            B1.Reduce(); B2.Reduce(); // B1: Dividend and B2: Divisor
            short B1Spart = B1.Spart;
            short B2Spart = B2.Spart;
            Q = new BigInt(B1.Size, 0); // Q: Quotient
            R = new BigInt(B1, signt.positive); // R: Rest
            if (B2 == 0)
            {
                throw new ArithmeticException("Divide by zero");
            }

            if (B1 == Q || B1Spart < B2Spart || B1 < B2)
            {
                Q.Sign = ResultSign;
                Q.Reduce();
                R.Reduce();
            }
            else if (B1 == B2)
            {
                Q = new BigInt(B1.Size, 1);
                Q.Sign = ResultSign;
                R = new BigInt(B1.Size, 0);
            }
            else if (B1Spart < 3 && B2Spart < 3)
            {
                // Process cell2-Division
                ulong Cell2Dividend = B1.Spart == 2 ? ((ulong)B1.Value[1] << CELL_SIZE) + B1.Value[0] : B1.Value[0];
                ulong Cell2Divisor = B2.Spart == 2 ? ((ulong)B2.Value[1] << CELL_SIZE) + B2.Value[0] : B2.Value[0];
                Q = new BigInt(B1.Size, Cell2Dividend / Cell2Divisor);
                R = new BigInt(B1.Size, Cell2Dividend % Cell2Divisor);
            }
            else if (B2Spart < 2)
            {
                B2.Reduce();
                _DivCellMod(ref Q, ref R, B1, B2.Value[B2.Spart - 1], ResultSign);
            }
            else
            {
                B1.Reduce();
                B2.Reduce();
                uint[] Result = new uint[B1.Size];
                int RemainderLength = B1.Spart + 1;
                uint[] Remainder = new uint[RemainderLength];
                uint Mask = HIGH_BIT_MASK;
                uint Val = B2.Value[B2.Spart - 1];
                int Shift = 0, ResultPos = 0;

                while (Mask != 0 && (Val & Mask) == 0)
                {
                    Shift++; Mask >>= 1;
                }

                for (int i = 0; i < B1.Spart; i++)
                {
                    Remainder[i] = B1.Value[i];
                }

                ShiftLeft(Remainder, (short)Shift);
                B2.ShiftLeft((short)Shift);
                B2.Reduce();
                int J = RemainderLength - B2.Spart;
                int Pos = RemainderLength - 1;

                ulong FirstDivisorCell = B2.Value[B2.Spart - 1];
                ulong SecondDiviorCell = B2.Value[B2.Spart - 2];

                int DivisorLength = B2.Spart + 1;
                uint[] DividendPart = new uint[DivisorLength];

                while (J > 0)
                {
                    ulong Dividend = ((ulong)Remainder[Pos] << CELL_SIZE) + (ulong)Remainder[Pos - 1];

                    ulong Q_hat = Dividend / FirstDivisorCell;
                    ulong R_hat = Dividend % FirstDivisorCell;

                    bool Done = false;
                    while (!Done)
                    {
                        Done = true;

                        if (Q_hat == MAX_CELL_VALUE ||
                           (Q_hat * SecondDiviorCell) > ((R_hat << CELL_SIZE) + Remainder[Pos - 2]))
                        {
                            Q_hat--;
                            R_hat += FirstDivisorCell;

                            if (R_hat < MAX_CELL_VALUE)
                                Done = false;
                        }
                    }

                    for (int h = 0; h < DivisorLength; h++)
                        DividendPart[h] = Remainder[Pos - h];

                    BigInt K = new BigInt(B1.Size, DividendPart);
                    BigInt S = B2 * Q_hat;
                    //uint f = 1;
                    //BigInt B2Divisor = B2.Clone();
                    while (S > K)
                    {
                        //Q_hat = Q_hat - f;
                        //S = S - B2Divisor;
                        Q_hat--;
                        S -= B2;
                        //f = f << 1;
                        //B2Divisor.ShiftLeft(1);
                    }
                    //f = f >> 1;
                    //B2Divisor.ShiftRigth(1);
                    //Q_hat = Q_hat + f;
                    //S = S + B2Divisor;
                    BigInt Y = K - S;
                    B2.Reduce();

                    for (int h = 0; h < DivisorLength; h++)
                        Remainder[Pos - h] = Y.Value[B2.Spart - h];

                    Result[ResultPos++] = (uint)Q_hat;
                    Pos--;
                    J--;
                }

                Q.Spart = (short)ResultPos;
                int y = 0;
                for (int x = Q.Spart - 1; x >= 0; x--, y++)
                    Q.Value[y] = Result[x];
                for (; y < B1.Size; y++)
                    Q.Value[y] = 0;

                Q.Reduce();

                R.Spart = (short)ShiftRigth(Remainder, (short)Shift);

                for (y = 0; y < R.Spart; y++)
                    R.Value[y] = Remainder[y];
                for (; y < B1.Size; y++)
                    R.Value[y] = 0;
            }

            R.Reduce();
            //BigInt RCopy = R.Clone();
            //int Cmp = Compare(B1, B2);
            //if (Cmp == -1 && B1.Sign == signt.negative && B2.Sign == signt.positive)
            //{
            //    RCopy.Sign = ResultSign;
            //}

            Q.Sign = ResultSign;
            BigInt B1E = Q * B2 + R;
            if (R != 0 && Compare(B1E, B1) != 0)
            {
                if (B2.Sign == signt.negative)
                {
                    if (ResultSign == signt.positive)
                    {
                        Q++;
                    }
                    else
                    {
                        Q--;
                    }
                }
                else if (B1.Sign == signt.negative)
                {
                    if (ResultSign == signt.positive)
                    {
                        Q--;
                    }
                    else
                    {
                        Q++;
                    }
                }
            }
            Q.Reduce();
        }

        private static void _DivCellMod(ref BigInt Q, ref BigInt R, BigInt B, uint DefaultBase, signt ResultSign)
        {
            uint[] Result = new uint[B.Size];
            short ResultPos = 0;

            // First copy the Dividend (B) to the Remainder R
            B.Reduce();
            R = B.Clone();

            short Pos = (short)(R.Spart - 1);
            ulong Dividend = R.Value[Pos];

            if (Dividend >= DefaultBase)
            {
                ulong LocalQuotient = Dividend / DefaultBase;
                Result[ResultPos++] = (uint)LocalQuotient;
                R.Value[Pos] = (uint)(Dividend % DefaultBase);
            }
            Pos--;

            while (Pos >= 0)
            {
                Dividend = ((ulong)R.Value[Pos + 1] << CELL_SIZE) + (ulong)R.Value[Pos];
                ulong LocalQuotient = Dividend / DefaultBase;
                Result[ResultPos++] = (uint)LocalQuotient;

                R.Value[Pos + 1] = 0;
                R.Value[Pos--] = (uint)(Dividend % DefaultBase);
            }

            Q.Spart = ResultPos;
            int j = 0;
            for (int i = Q.Spart - 1; i >= 0; i--, j++)
                Q.Value[j] = Result[i];
            for (; j < B.Size; j++)
                Q.Value[j] = 0;
            Q.Sign = ResultSign;
            Q.Reduce();
            R.Reduce();
        }
        #endregion
    }
}
