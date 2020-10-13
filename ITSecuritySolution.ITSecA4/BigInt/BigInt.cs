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
            Value = new uint[Size];
            if (Str != null && Str.Length >= 1)
            {
                Sign = Str[0] == '-' ? signt.negative : signt.positive;
            }
            ToBigInt(Str);
        }

        public BigInt(short BitSize, string Str) : base(BitSize, Str)
        {
        }

        public BigInt(byte[] ByteArray)
        {
            Size = INIT_SIZE;
            Value = new uint[Size];
            Sign = signt.positive;
            ToBigInt(ByteArray);
        }

        public BigInt(short BitSize, byte[] ByteArray)
        {
            Size = BitSize;
            Value = new uint[Size];
            Sign = signt.positive;
            ToBigInt(ByteArray);
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
        public BigInt Clone()
        {
            this.Reduce();
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

        public static BigInt SignedMod(BigInt B1, BigInt B2)
        {
            B2 = (BigInt)B1.SameSize(B2);
            BigInt B2Copy = B2.Clone();
            BigInt Q = new BigInt(B2Copy.Size, 0);
            BigInt R = new BigInt(B1, signt.positive);
            switch (B1.LookAtSign(B2Copy))
            {
                case signt2.pp:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    R.Sign = signt.positive;
                    break;
                case signt2.np:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    R.Sign = signt.negative;
                    break;
                case signt2.pn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.negative);
                    R.Sign = signt.negative;
                    break;
                case signt2.nn:
                    _Divmod(ref Q, ref R, ref B1, ref B2Copy, signt.positive);
                    R.Sign = signt.positive;
                    break;
                default:
                    throw new Exception("Could not determine the sign of the result value.");
            }

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
            B1.Reduce();
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
            BigInt R = new BigInt(B1, signt.positive);
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
            BigInt R = new BigInt(B1, signt.positive);
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

        #region PrimNumber & Fermat/Euler test (ITSecA2)

        public void GenPseudoPrime(short Size, int Rounds = 50, Random Rand = null)
        {
            if (Rand == null)
                Rand = new Random();

            bool Done = false;
            int Counter = 0;
            while (!Done)
            {
                this.GenRandomOdd(Size, Rand);

                // prime test
                Done = this.IsProbablePrime(Rounds);
                Counter++;
            }
        }

        public void GenRandomOdd(short Size, Random Rand)
        {
            this.GenRandomBits(Size, Rand);

            // Make the generated BigInt Odd
            this.Value[0] |= 0x01;
        }

        public bool IsProbablePrime(int Rounds = 50, bool OnlyMR = true)
        {
            int PBitCount = this.BitsCount();
            // Number smaller than 3,1*10^23 (78 bit)
            // All prime number less than 100 
            int[] PrimeTestList1 = PBitCount <= 78 ? PRIME_NUMBERS_LIST2000.Take(24).ToArray() : PRIME_NUMBERS_LIST2000;
            int[] PrimeTestList2 = PBitCount <= 78 ? PRIME_NUMBERS_LIST2000.Take(12).ToArray() : PRIME_NUMBERS_LIST2000;
            // Test division with all prime numbers less than 100
            if (!this.IsPrimeDiv(PrimeTestList1))
                return false;

            if (!OnlyMR)
            {
                // Fermat-Test with random generated BigInts on 50 Rounds and all prime numbers less or equal to 37
                if (this.IsPrimeFermat(Rounds) >= 0 || !this.IsPrimeFermat(PrimeTestList2))
                    return false;

                // Euler-Test with random generated BigInts on 50 Rounds and all prime numbers less or equal to 37
                if (this.IsPrimeEuler(Rounds) >= 0 || !this.IsPrimeEuler(PrimeTestList2))
                    return false;
            }

            // Miller-Rabin-Test with random generated BigInts on 50 Rounds and all prime numbers less or equal to 37
            this.Reduce();
            if (this.IsPrimeMR(Rounds) >= 0)
                return false;

            //if (!P.IsPrimeMR(PrimeTestList2))
            //    return false;

            return true;
        }

        /// <summary>
        /// Uses the Fermat-Test to check if the BigInt (this) is prime or not
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        public bool IsPrimeFermat(BigInt Base, bool SkipVerification = false)
        {
            this.Reduce();
            BigInt P = this.Clone();
            BigInt A = Base.Clone();
            if (!SkipVerification)
            {
                if (this.Spart == 1)
                {
                    // Process Test for decimal number
                    if (this.Value[0] == 0 || this.Value[0] == 1)
                        return false;
                    else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                        return true;
                }

                // Check of even Numbers
                if ((this.Value[0] & 0x1) == 0)
                    return false;

                // Check if P is prime using the basis A
                // 1. First check wheter a Egc exist for P and A
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;
            }

            BigInt PSub1 = P - 1;

            // 2. Calculate A^(P-1) mod P
            BigInt APowModP = A.PowModPrim(PSub1, P);

            // 3. If APowModP != 1 then P is not prime
            if (APowModP != 1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Uses the Fermat-Test to check if the BigInt (this) is prime or not
        /// using the provided list of BigInt 
        /// </summary>
        /// <param name="Bases"></param>
        /// <returns></returns>
        public bool IsPrimeFermat(BigInt[] Bases)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return false;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return true;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return false;

            for (int i = 0; i < Bases.Length; i++)
            {
                BigInt P = this.Clone();
                BigInt A = Bases[i].Clone();
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;

                if (!this.IsPrimeFermat(Bases[i], true))
                    return false;
            }
            return true;
        }

        public bool IsPrimeFermat(int [] Bases)
        {
            if (Bases != null && Bases.Length > 0)
            {
                BigInt[] BigIntBases = new BigInt[Bases.Length];
                for (int i = 0; i < Bases.Length; i++)
                {
                    BigIntBases[i] = new BigInt(this.Size, Bases[i]);
                }

                return this.IsPrimeFermat(BigIntBases);
            }
            
            return false;
        }

        /// <summary>
        /// Uses the Fermat-Test to check if the BigInt (this) is prime or not
        /// using random generated BigInt Bases
        /// </summary>
        /// <param name="Rounds"></param>
        /// <returns></returns>
        public int IsPrimeFermat(int Rounds = 50)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return 0;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return -1;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return 0;

            BigInt P = this.Clone();
            int PBitCount = P.BitsCount();
            // All prime number less than 41
            int[] PrimeTestList1 = PBitCount <= 78 ? PRIME_NUMBERS_LIST2000.Take(12).ToArray() : PRIME_NUMBERS_LIST2000;
            
            if (!P.IsPrimeFermat(PrimeTestList1))
                return 0;

            BigInt A = new BigInt(this.Size, 0);
            Random Rand = new Random();
            int Bits = P.BitsCount();

            for (int i = 0; i < Rounds; i++)
            {
                // Generate random bits in A
                bool done = false;

                while (!done)       // Generate A < P
                {
                    short TestBits = 0;

                    // Make sure A has at least 2 bits
                    while (TestBits < 2)
                        TestBits = (short)(Rand.NextDouble() * Bits);

                    A.GenRandomBits(TestBits, Rand);

                    int byteLen = A.Spart;

                    // Make sure A is not 0 & A > 1
                    if (byteLen > 1 || (byteLen == 1 && A.Value[0] != 1))
                        done = true;
                }

                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return i + 1;

                if (!this.IsPrimeFermat(A, true))
                    return i + 1;
            }
            return -1;
        }

        /// <summary>
        /// Uses the Euler-Test to check if the BigInt (this) is prime or not
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        public bool IsPrimeEuler(BigInt Base, bool SkipVerification = false)
        {
            this.Reduce();
            BigInt P = this.Clone();
            BigInt A = Base.Clone();
            if (!SkipVerification)
            {
                if (this.Spart == 1)
                {
                    // Process Test for decimal number
                    if (this.Value[0] == 0 || this.Value[0] == 1)
                        return false;
                    else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                        return true;
                }

                // Check of even Numbers
                if ((this.Value[0] & 0x1) == 0)
                    return false;

                // Check if P is prime using the basis A
                // 1. First check wheter a Egc exist for P and A
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;
            }
            BigInt PSub1 = P - 1;
            BigInt PSub1Div2 = PSub1 >> 1;

            // 2. Calculate A^((P-1)/2) mod P
            BigInt APowModP = A.PowModPrim(PSub1Div2, P);

            // 3. If APowModP != 1 Or != -1 Or != n -1 then P is not prime
            if (APowModP != 1 && APowModP != -1 && APowModP != PSub1)
                return false;

            return true;
        }

        /// <summary>
        /// Uses the Euler-Test to check if the BigInt (this) is prime or not
        /// using the the provided list of BigInt 
        /// </summary>
        /// <param name="Bases"></param>
        /// <returns></returns>
        public bool IsPrimeEuler(BigInt[] Bases)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return false;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return true;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return false;

            for (int i = 0; i < Bases.Length; i++)
            {
                BigInt P = this.Clone();
                BigInt A = Bases[i].Clone();
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;
                if (!this.IsPrimeEuler(Bases[i], true))
                    return false;
            }
            return true;
        }

        public bool IsPrimeEuler(int [] Bases)
        {
            if (Bases != null && Bases.Length > 0)
            {
                BigInt[] BigIntBases = new BigInt[Bases.Length];
                for (int i = 0; i < Bases.Length; i++)
                {
                    BigIntBases[i] = new BigInt(this.Size, Bases[i]);
                }

                return this.IsPrimeEuler(BigIntBases);
            }

            return false;
        }

        /// <summary>
        /// Uses the Euler-Test to check if the BigInt (this) is prime or not
        /// using random generated BigInt Bases
        /// </summary>
        /// <param name="Rounds"></param>
        /// <returns></returns>
        public int IsPrimeEuler(int Rounds = 50)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return 0;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return -1;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return 0;

            BigInt P = this.Clone();

            int PBitCount = P.BitsCount();
            // All prime number less than 41
            int[] PrimeTestList1 = PBitCount <= 78 ? PRIME_NUMBERS_LIST2000.Take(12).ToArray() : PRIME_NUMBERS_LIST2000;

            if (!P.IsPrimeEuler(PrimeTestList1))
                return 0;

            BigInt A = new BigInt(this.Size, 0);
            Random Rand = new Random();
            int Bits = P.BitsCount();

            for (int i = 0; i < Rounds; i++)
            {
                // Generate random bits in A
                bool done = false;

                while (!done)       // Generate A < P
                {
                    short TestBits = 0;

                    // Make sure A has at least 2 bits
                    while (TestBits < 2)
                        TestBits = (short)(Rand.NextDouble() * Bits);

                    A.GenRandomBits(TestBits, Rand);

                    int byteLen = A.Spart;

                    // Make sure A is not 0 & A > 1
                    if (byteLen > 1 || (byteLen == 1 && A.Value[0] != 1))
                        done = true;
                }

                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return i + 1;

                if (!this.IsPrimeEuler(A, true))
                    return i + 1;
            }
            return -1;
        }

        /// <summary>
        /// Uses the Miller-Rabin-Test to check if the BigInt (this) is prime or not
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        public bool IsPrimeMR(BigInt Base, bool SkipVerification = false)
        {
            this.Reduce();
            BigInt P = this.Clone();
            BigInt A = Base.Clone();
            if (!SkipVerification)
            {
                if (this.Spart == 1)
                {
                    // Process Test for decimal number
                    if (this.Value[0] == 0 || this.Value[0] == 1)
                        return false;
                    else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                        return true;
                }

                // Check of even Numbers
                if ((this.Value[0] & 0x1) == 0)
                    return false;

                // Check if P is prime using the basis A
                // 1. First check wheter a Egc exist for P and A
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;
            }
            BigInt PSub1 = P - 1;
            PSub1.Reduce();

            int S = 0;
            for (int i = 0; i < PSub1.Spart; i++)
            {
                uint Mask = 0x01;
                for (int j = 0; j < CELL_SIZE; j++)
                {
                    if ((PSub1.Value[i] & Mask) != 0)
                    {
                        i = PSub1.Spart;
                        break;
                    }
                    Mask <<= 1;
                    S++;
                }
            }
            BigInt T = PSub1 >> S;
            BigInt B = A.PowModPrim(T, P);
            bool Result = false;

            // 2. Check whether B == 1, if yes Result take true
            if (B.Spart == 1 && B.Value[0] == 1)
                Result = true;

            // 3. Calculate in a for loop A^((2^j)*T) mod P
            for (int j = 0; Result == false && j < S; j++)
            {
                if (B == PSub1) // B == P - 1
                {
                    Result = true;
                    break;
                }

                B = (B * B) % P;
            }

            return Result;
        }

        /// <summary>
        /// Uses the Miller-Rabin-Test to check if the BigInt (this) is prime or not
        /// using the the provided list of BigInt 
        /// </summary>
        /// <param name="Bases"></param>
        /// <returns></returns>
        public bool IsPrimeMR(BigInt[] Bases)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return false;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return true;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return false;

            BigInt P = this.Clone();
            BigInt PSub1 = P - 1;
            PSub1.Reduce();

            int S = 0;
            for (int i = 0; i < PSub1.Spart; i++)
            {
                uint Mask = 0x01;
                for (int j = 0; j < CELL_SIZE; j++)
                {
                    if ((PSub1.Value[i] & Mask) != 0)
                    {
                        i = PSub1.Spart;
                        break;
                    }
                    Mask <<= 1;
                    S++;
                }
            }
            BigInt T = PSub1 >> S;
            
            for (int i = 0; i < Bases.Length; i++)
            {
                BigInt A = Bases[i].Clone();
                BigInt EgcAP = Egc(P, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return false;

                BigInt B = A.PowModPrim(T, P);

                bool Result = false;

                // 2. Check whether B == 1, if yes Result take true
                if (B.Spart == 1 && B.Value[0] == 1)
                    Result = true;

                // 3. Calculate in a for loop A^((2^j)*T) mod P
                for (int j = 0; Result == false && j < S; j++)
                {
                    if (B == PSub1) // B == P - 1
                    {
                        Result = true;
                        break;
                    }

                    B = (B * B) % P;
                }

                if (!Result)
                    return false;
            }
            return true;
        }

        public bool IsPrimeMR(int[] Bases)
        {
            if (Bases != null && Bases.Length > 0)
            {
                BigInt[] BigIntBases = new BigInt[Bases.Length];
                for (int i = 0; i < Bases.Length; i++)
                {
                    BigIntBases[i] = new BigInt(this.Size, Bases[i]);
                }

                return this.IsPrimeMR(BigIntBases);
            }

            return false;
        }

        /// <summary>
        /// Uses the Miller-Rabin-Test to check if the BigInt (this) is prime or not
        /// using random generated BigInt Bases
        /// </summary>
        /// <param name="Rounds"></param>
        /// <returns></returns>
        public int IsPrimeMR(int Rounds = 50)
        {
            if (this.Spart == 1)
            {
                // Process Test for decimal number
                if (this.Value[0] == 0 || this.Value[0] == 1)
                    return 0;
                else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                    return -1;
            }

            // Check of even Numbers
            if ((this.Value[0] & 0x1) == 0)
                return 0;

            BigInt PSub1 = this - 1;

            int S = 0;
            for (int i = 0; i < PSub1.Spart; i++)
            {
                uint Mask = 0x01;
                for (int j = 0; j < CELL_SIZE; j++)
                {
                    if ((PSub1.Value[i] & Mask) != 0)
                    {
                        i = PSub1.Spart;
                        break;
                    }
                    Mask <<= 1;
                    S++;
                }
            }
            BigInt T = PSub1 >> S;

            BigInt A = new BigInt(this.Size, 0);
            Random Rand = new Random();
            int Bits = this.BitsCount();

            for (int i = 0; i < Rounds; i++)
            {
                // Generate random bits in A
                bool Done = false;

                while (!Done)       // Generate A < P
                {
                    short TestBits = 0;

                    // Make sure A has at least 2 bits
                    while (TestBits < 2)
                        TestBits = (short)(Rand.NextDouble() * Bits);

                    A.GenRandomBits(TestBits, Rand);

                    // Make sure A is not 0 & A > 1
                    if (A.Spart > 1 || (A.Spart == 1 && A.Value[0] != 1))
                        Done = true;
                }

                BigInt EgcAP = Egc(this, A);
                if (EgcAP.Spart == 1 && EgcAP.Value[0] != 1)
                    return i + 1;

                BigInt B = A.PowModPrim(T, this);

                bool Result = false;

                // 2. Check whether B == 1, if yes Result take true
                if (B.Spart == 1 && B.Value[0] == 1)
                    Result = true;

                // 3. Calculate in a for loop A^((2^j)*T) mod P
                for (int j = 0; Result == false && j < S; j++)
                {
                    if (B == PSub1) // B == P - 1
                    {
                        Result = true;
                        break;
                    }

                    B = (B * B) % this;
                }

                if (!Result)
                    return i + 1;
            }
            return -1;
        }

        /// <summary>
        /// Uses the Divibility-Test to check if the BigInt (this) is prime or not
        /// and use the list of values provided in the array Bases 
        /// </summary>
        /// <param name="Bases"></param>
        /// <returns></returns>
        public bool IsPrimeDiv(int[] Bases, bool SkipVerification = false)
        {
            this.Reduce();
            if (!SkipVerification)
            {
                if (this.Spart == 1)
                {
                    // Process Test for decimal number
                    if (this.Value[0] == 0 || this.Value[0] == 1)
                        return false;
                    else if (this.Value[0] == 2 || this.Value[0] == 3 || this.Value[0] == 5 || this.Value[0] == 7)
                        return true;
                }

                // Check of even Numbers
                if ((this.Value[0] & 0x1) == 0)
                    return false;
            }

            for (int i = 0; i < Bases.Length; i++)
            {
                if (this == Bases[i])
                    return true;
                if (this % Bases[i] == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Calculate the Sqrt of this N times
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public BigInt Sqrt(uint N = 1)
        {
            BigInt Result = new BigInt(this.Size, 0);
            uint NumBits = (uint)this.BitsCount();

            if ((NumBits & 0x1) != 0)
                NumBits = (NumBits >> 1) + 1;
            else
                NumBits = NumBits >> 1;

            uint BytePos = NumBits >> BINARY_CELL_SIZE_POW;
            byte BitPos = (byte)(NumBits & 0x1F);

            uint Mask;
            if (BitPos == 0)
                Mask = HIGH_BIT_MASK;
            else
            {
                Mask = (uint)1 << BitPos;
                BytePos++;
            }
            Result.Spart = (short)BytePos;

            for (int i = (int)BytePos - 1; i >= 0; i--)
            {
                while (Mask != 0)
                {
                    // Rate the value at i
                    Result.Value[i] ^= Mask;

                    // Undo the previous guess if the square of the result is larger than expected
                    if (Result.Square() > this)
                    {
                        Result.Value[i] ^= Mask;
                    }

                    Mask >>= 1; // Mask = Mask / 2
                }
                Mask = HIGH_BIT_MASK;
            }
            return Result;
        }

        /// <summary>
        /// A ^ (2 ^ N) with A = this.clone()
        /// </summary>
        /// <param name="N" description="Number of Square repetition"></param>
        /// <returns></returns>
        public BigInt Square(uint N = 1)
        {
            BigInt C = this.Clone();
            if (N == 1)
            {
                C = _Square(C);
            }
            else if (N % 2 == 0)
            {
                int i = 2;
                while (i <= N)
                {
                    C = _Square(C);
                    i = i << 1;
                }
            }

            return C;
        }

        /// <summary>
        /// A ^ B
        /// </summary>
        /// <param name="B"></param>
        /// <returns> A ^ B </returns>
        public static BigInt operator ^ (BigInt A,  ulong B)
        {
            BigInt ACopy = A.Clone();
            BigInt C = new BigInt(A.Size, 1);
            BigInt n = new BigInt(A.Size, B);

            _Pow(ref C, ref ACopy, ref n);
            return C;
        }

        /// <summary>
        /// A ^ B
        /// </summary>
        /// <param name="B"></param>
        /// <returns> A ^ B </returns>
        public static BigInt operator ^ (BigInt A, BigInt B){
            BigInt ACopy = A.Clone();
            BigInt C = new BigInt(A.Size, 1);
            BigInt n = B.Clone();

            _Pow(ref C, ref ACopy, ref n);
            return C;
        }

        /// <summary>
        /// A^N Ξ C (mode M)
        /// </summary>
        /// <param name="N"></param>
        /// <param name="M"></param>
        public BigInt PowMod(BigInt N, BigInt M)
        {
            BigInt n = N.Clone();
            BigInt m = M.Clone();
            BigInt C = new BigInt(this.Size, 1);
            BigInt A = this.Clone();
            n.Reduce();
            if (n == 0 || (n.Spart == 1 && n.Value[0] == 0))
            {
                return C;
            }
            if (A == 0 || (A.Spart == 1 && A.Value[0] == 0))
            {
                return new BigInt(this.Size, 0);
            }
            _Powmod(ref C, ref A, ref n, ref m);
            
            return C;
        }

        /// <summary>
        /// A^N Ξ C (mod M)
        /// </summary>
        /// <param name="N"></param>
        /// <param name="M"></param>
        public BigInt PowModPrim(BigInt N, BigInt M)
        {
            BigInt C = new BigInt(this.Size, 1);
            BigInt A = this.Clone();

            if (N == 0 || (N.Spart == 1 && N.Value[0] == 0))
            {
                return C;
            }
            if (A == 0 || (A.Spart == 1 && A.Value[0] == 0))
            {
                return new BigInt(this.Size, 0);
            }
            if (N < (M - 1))
            {
                // _Powmod(ref C, ref A, ref n, ref m);
                // _PowmodBR using the BarretReduction is in this case more efficiant than the previous one
                _PowmodBR(ref C, ref A, ref N, ref M);
                return C;
            }
            if (A < M) // Fermat Optimierung: ggt(a, m) = 1 gilt
            {
                BigInt ModN = N % (M - 1);
                if (ModN == 0)
                    ModN = N.Clone();
                // _Powmod(ref C, ref A, ref ModN, ref m);
                _PowmodBR(ref C, ref A, ref ModN, ref M);
                return C;
            }

            // A >= p AND n >= m - 1
            if (Egc(A, M) == 1)
            {
                BigInt ModN = N % (M - 1);
                if (ModN != 0)
                    N = ModN;
            }

            // _Powmod(ref C, ref A, ref n, ref m);
            _PowmodBR(ref C, ref A, ref N, ref M);
            return C;
        }

        public static BigInt Egc(BigInt A, BigInt B)
        {
            BigInt B2K = new BigInt(B.Size, 0);
            _EgcBin(ref B2K, A, B);

            return B2K;
        }
        #endregion

        #region RSA (ITSecA3)

        public BigInt GenCoPrime(short Size, Random Rand = null)
        {
            if (Rand == null)
                Rand = new Random();

            BigInt Result = new BigInt(this.Size, 0);

            bool Done = false;

            while (!Done)
            {
                Result.GenRandomOdd(Size, Rand);

                // gcd test
                BigInt G = Egc(Result, this);
                if (G.Spart == 1 && G.Value[0] == 1)
                    Done = true;
            }

            return Result;
        }

        public BigInt ModInverse(BigInt M)
        {
            BigInt[] P = { new BigInt(this.Size, 0), new BigInt(this.Size, 1) };
            BigInt[] Q = new BigInt[2];
            BigInt[] R = { new BigInt(this.Size, 0), new BigInt(this.Size, 0) };

            int Steps = 0;
            BigInt A = M.Clone();
            BigInt B = this.Clone();
            BigInt PVal;
            BigInt Quotient;
            BigInt Remainder;

            while (B.Spart > 1 || (B.Spart == 1 && B.Value[0] != 0))
            {
                if (Steps > 1)
                {
                    PVal = SignedMod(P[0] - (P[1] * Q[0]),  M);
                    P[0] = P[1];
                    P[1] = PVal;
                }

                Quotient = A / B;
                Remainder = A % B;

                Q[0] = Q[1];
                R[0] = R[1];
                Q[1] = Quotient;
                R[1] = Remainder;

                A = B;
                B = Remainder;

                Steps++;
            }

            R[0].Reduce();
            if (R[0].Spart > 1 || (R[0].Spart == 1 && R[0].Value[0] != 1))
                throw new ArithmeticException("No inverse could be found!");

            BigInt Inv = SignedMod(P[0] - (P[1] * Q[0]), M);

            if (Inv.Sign == signt.negative)
                Inv += M;

            return Inv;
        }


        public static BigInt Egcd(BigInt A, BigInt B, ref BigInt U, ref BigInt V)
        {
            BigInt G = new BigInt(B.Size, 0);
            _EgcdMod(ref G, ref U, ref V, A, B);

            // Korrektur durch Addieren des Moduls
            //while (Bu.Sign == signt.negative)
            //{
            //    Bu = Bu + B;
            //}
            G.Reduce();

            return G;
        }
        #endregion

        #region BBS (ITSecA4)

        /// <summary>
        /// Smallest Common Multiple
        /// </summary>
        /// <param name="B1"></param>
        /// <param name="B2"></param>
        /// <returns></returns>
        public static BigInt Scm(BigInt B1, BigInt B2)
        {
            if (B1 == 0 || B2 == 0)
                throw new ArgumentException("An error occur while calculating the SCM: B1 == 0 OR B2 == 0");
            else
            {
                BigInt B1B2 = (B1 * B2) / Egc(B1, B2);
                B1B2.Sign = signt.positive;
                
                return B1B2;
            }
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
            else if (StrUpper.StartsWith("+0B") || StrUpper.StartsWith("-0B") || StrUpper.StartsWith("0B"))
            {
                int xPos = StrUpper.IndexOf("B");
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
                        ResultBigInt = (ResultBigInt << 1) + (uint)n;
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

            for (int i = 0; i < ResultBigInt.Spart; i++)
                this.Value[i] = ResultBigInt.Value[i];

            this.Spart = ResultBigInt.Spart;


            if (this.Spart == 0)
                this.Spart = 1;
        }

        public override void ToBigInt(byte[] ByteArray)
        {
            string HexString = ByteArrayToString(ByteArray);
            HexString = "+0x" + HexString;
            this.ToBigInt(HexString);
        }

        public byte[] ToByteArray()
        {
            string HexString = this.ToString().Substring(3);
            if (HexString.Length % 2 != 0) 
            {
                HexString = "0" + HexString;
            }
            return StringToByteArray(HexString);
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

        public string ToStringBinary(bool Fill = true)
        {
            return ToString(2, Fill);
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

            if (DefaultBase == 2)
                Result = this.Sign == signt.positive ? $"+0b{Result}" : $"-0b{Result}";
            else if (DefaultBase == 8)
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
            C.Reduce();
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
            //Q = new BigInt(B1.Size, 0); // Q: Quotient
            //R = new BigInt(B1, signt.positive); // R: Rest
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
                BigInt K, S, Y;

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

                    K = new BigInt(B1.Size, DividendPart);
                    S = B2 * Q_hat;
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
                    Y = K - S;
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

        /// <summary>
        /// A^n Ξ C (mod m) 
        /// Using BarrettReduction
        /// </summary>
        /// <param name="C"></param>
        /// <param name="A"></param>
        /// <param name="Exp"></param>
        /// <param name="M"></param>
        public static void _PowmodBR(ref BigInt C, ref BigInt A, ref BigInt Exp, ref BigInt M)
        {
            int i = M.Spart << 1;
            BigInt Tmp = A % M;
            BigInt Const = new BigInt(C.Size, 0);
            Const.Value[i] = MIN_CELL_VALUE;
            Const.Spart = (short)(i + 1);

            Const = Const / M;
            int TotalBits = Exp.BitsCount();
            int Count = 0;

            // Square and Exponentiation
            for (int pos = 0; pos < Exp.Spart; pos++)
            {
                uint Mask = 0x01;
                for (int index = 0; index < CELL_SIZE; index++)
                {
                    if ((Exp.Value[pos] & Mask) != 0)
                        C = _BarrettReduction(C * Tmp, M, Const);

                    Mask <<= 1;
                    Tmp = _BarrettReduction(Tmp * Tmp, M, Const);
                    if (Tmp.Spart == 1 && Tmp.Value[0] == 1)
                    {
                        pos = Exp.Spart;
                        break;
                    }
                    Count++;
                    if (Count == TotalBits)
                        break;
                }
            }

            C.Reduce();
        }

        /// <summary>
        /// A^n Ξ C (mod m)
        /// </summary>
        /// <param name="C"></param>
        /// <param name="A"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        private static void _Powmod(ref BigInt C, ref BigInt A, ref BigInt n, ref BigInt m)
        {
            BigInt T = A.Clone();
            while (n > 0)
            {
                if (n.Odd())
                {
                    C = (C * T) % m;
                }
                T = _Square(T) % m;
                n = n >> 1; // n = n / 2
            }
            C.Reduce();
        }

        /// <summary>
        /// A^n = C
        /// </summary>
        /// <param name="C"></param>
        /// <param name="A"></param>
        /// <param name="n"></param>
        private static void _Pow(ref BigInt C, ref BigInt A, ref BigInt n)
        {
            BigInt T = A.Clone();
            while (n > 0)
            {
                if (n.Odd())
                {
                    C = C * T;
                }
                T = _Square(T);
                n = n >> 1; // n = n / 2
            }
            C.Reduce();
        }

        private static void _EgcdMod(ref BigInt G, ref BigInt Tu, ref BigInt Tv, BigInt A, BigInt B)
        {
            BigInt Au = new BigInt(A.Size, 1), Av = new BigInt(A.Size, 0),
                Bu = new BigInt(B.Size, 0), Bv = new BigInt(B.Size, 1);
            BigInt ACopy = A.Clone();
            BigInt BCopy = B.Clone();
            BigInt T = new BigInt(B.Size, 1);
            BigInt Q = new BigInt(A.Size, 1);
            ACopy.Sign = signt.positive;
            BCopy.Sign = signt.positive;
            int k;
            for (k = 0; ACopy.Even() && BCopy.Even(); k++)
            {
                ACopy = ACopy >> 1; // X = X / 2
                BCopy = BCopy >> 1; // Y = Y / 2
            }

            while (BCopy != 0)
            {
                Q = ACopy / BCopy;
                T = ACopy % BCopy;
                Tu = Au - Q * Bu;
                Tv = Av - Q * Bv;
                ACopy = BCopy;
                Au = Bu;
                Av = Bv;
                BCopy = T;
                Bu = Tu;
                Bv = Tv;
            }

            // G = ACopy * 2^k
            G = ACopy << k;
            Tu = Au;
            Tv = Av;
        }

        private static void _EgcBin(ref BigInt B2K, BigInt A, BigInt B)
        {
            BigInt X = A.Clone();
            BigInt Y = B.Clone();
            X.Sign = signt.positive;
            Y.Sign = signt.positive;
            int k;
            for (k = 0; X.Even() && Y.Even(); k++)
            {
                X = X >> 1; // X = X / 2
                Y = Y >> 1; // Y = Y / 2
            }

            while (X != 0)
            {
                while (X.Even())
                {
                    X = X >> 1;
                }

                while (Y.Even())
                {
                    Y = Y >> 1;
                }

                if (Compare(X, Y) == -1)
                {
                    Y = Y - X;
                }
                else
                {
                    X = X - Y;
                }
            }

            // B2K = B * 2^k
            B2K = Y.Clone();
            B2K = B2K << k;
        }

        private static BigInt _Square(BigInt A)
        {
            ulong Tmp;
            BigInt C = new BigInt(A.Size, 0);
            BigInt A2 = A << 1;
            C.Resize((short)(A2.Spart + 1));
            for (int i = 0; i < A.Spart; i++)
            {
                for (int j = i + 1; j < A.Spart; j++)
                {
                    Tmp = (ulong)A.Value[i] * (ulong)A.Value[j];
                    C.AddCell2(i + j, Tmp);
                    C.AddCell2(i + j, Tmp); // Zweimal
                }
                Tmp = (ulong)A.Value[i] * (ulong)A.Value[i];
                C.AddCell2(2 * i, Tmp); // Addition der Quadrate
            }
            C.Reduce();
            return C;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <param name="Constant"></param>
        /// <returns></returns>
        private static BigInt _BarrettReduction(BigInt X, BigInt N, BigInt Constant)
        {
            //N.Reduce();
            //X.Reduce();
            int K = N.Spart, KPlusOne = K + 1, KMinOne = K - 1;
            BigInt Q1 = new BigInt(X.Size, 0);

            // Q1 = X / B^(K-1)
            for (int i = KMinOne, j = 0; i < X.Spart; i++, j++)
                Q1.Value[j] = X.Value[i];

            Q1.Spart = (short)(X.Spart - KMinOne);
            if (Q1.Spart <= 0)
                Q1.Spart = 1;

            BigInt Q2 = Q1 * Constant;
            BigInt Q3 = new BigInt(X.Size, 0);
            //Q2.Reduce();

            // Q3 = Q2 / B^(K+1)
            for (int i = KPlusOne, j = 0; i < Q2.Spart; i++, j++)
                Q3.Value[j] = Q2.Value[i];
            Q3.Spart = (short)(Q2.Spart - KPlusOne);
            if (Q3.Spart <= 0)
                Q3.Spart = 1;

            // R1 = X mod B^(K+1)
            BigInt R1 = new BigInt(X.Size, 0);
            int LengthToCopy = (X.Spart > KPlusOne) ? KPlusOne : X.Spart;
            for (int i = 0; i < LengthToCopy; i++)
                R1.Value[i] = X.Value[i];
            R1.Spart = (short)LengthToCopy;

            // R2 = (Q3 * N) mod B^(K+1)
            // Partial multiplication of Q3 and N
            BigInt R2 = new BigInt(X.Size, 0);
            for (int i = 0; i < Q3.Spart; i++)
            {
                if (Q3.Value[i] == 0)
                    continue;

                ulong MCarry = 0;
                int T = i;
                for (int j = 0; j < N.Spart && T < KPlusOne; j++, T++)
                {
                    ulong Val = ((ulong)Q3.Value[i] * (ulong)N.Value[j]) + (ulong)R2.Value[T] + MCarry;
                    R2.Value[T] = (uint)(Val & CELL_MASK);
                    MCarry = Val >> CELL_SIZE;
                }

                if (T < KPlusOne)
                    R2.Value[T] = (uint)MCarry;
            }
            R2.Spart = (short)KPlusOne;
            //R2.Reduce();
            R1 -= R2;
            if (R1.Sign == signt.negative)
            {
                BigInt Val = new BigInt(R1.Size, 0);
                Val.Value[KPlusOne] = MIN_CELL_VALUE;
                Val.Spart = (short)(KPlusOne + 1);
                R1 += Val;
            }

            //R1.Reduce();
            if (R1 >= N)
            {
                BigInt DiffR1N = (R1 - N) / N;
                R1 = R1 - (DiffR1N * N);
                while (R1 >= N)
                {
                    R1 -= N;
                }
            }
            
            //if (R1 >= N)
            //{
            //    int F = 1;
            //    while (R1 >= N)
            //    {
            //        R1 -= N << F;
            //        if (F < 32)
            //            F++;
            //        else
            //            F = 1;
            //    }

            //    if (R1 >= N)
            //    {
            //        while (R1 >= N)
            //        {
            //            R1 -= N;
            //        }
            //    }
            //    else
            //    {
            //        F--;
            //        R1 += N << F;
            //        while (R1 < N)
            //        {
            //            R1 += N;
            //        }
            //        R1 -= N;
            //    }
            //}

            //R1.Reduce();
            return R1;
        }
        #endregion
    }
}
