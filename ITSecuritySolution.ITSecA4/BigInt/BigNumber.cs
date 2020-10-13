using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

/**
*
* @author Boris Foko Kouti
*/
namespace BigInt
{
    public abstract class BigNumber : BigIntConfiguration
    {
        public short Size;    // length of cell array
        public short Spart;   // length of significant part in cells
        public signt Sign;    // sign
        public uint [] Value; // array itself

        #region Constructors
        protected BigNumber()
        {
            Size = INIT_SIZE;
            Value = new uint[Size];
            Spart = 1;
            Sign = signt.positive;
        }

        protected BigNumber(BigNumber bn)
        {
            Value = new uint[bn.Size];
            Size = bn.Size;
            Spart = bn.Spart;
            Sign = bn.Sign;
            for (int i = 0; i < Spart; i++)
                Value[i] = bn.Value[i];
        }

        protected BigNumber(BigNumber bn, signt s)
        {
            Value = new uint[bn.Size];
            Size = bn.Size;
            Spart = bn.Spart;
            Sign = s;
            for (int i = 0; i < Spart; i++)
                Value[i] = bn.Value[i];
        }
        protected BigNumber(ulong Val)
        {
            Size = INIT_SIZE;
            Value = new uint[Size];
            Sign = signt.positive;
            ToBigInt(Val);
        }

        protected BigNumber(long Val)
        {
            Size = INIT_SIZE;
            Value = new uint[Size];
            Sign = Val >= 0 ? signt.positive : signt.negative;
            ulong AbsVal = (ulong)Math.Abs(Val);
            ToBigInt(AbsVal);
        }

        protected BigNumber(short BitSize, uint[] Value)
        {
            Spart = (short)Value.Length;
            if (Spart > BitSize)
                throw (new ArithmeticException($"Cell overflow in constructor. The length of the array should be less than {INIT_SIZE}."));
            
            Size = BitSize;
            this.Value = new uint[Size];
            Sign = signt.positive;

            for (int i = Spart - 1, j = 0; i >= 0; i--, j++)
                this.Value[j] = Value[i];

            Reduce();
        }

        protected BigNumber(short BitSize, long Val)
        {
            Size = BitSize;
            Value = new uint[Size];
            Sign = Val >= 0 ? signt.positive : signt.negative;
            ulong AbsVal = (ulong)Math.Abs(Val);
            ToBigInt(AbsVal);
        }

        protected BigNumber(short BitSize, ulong Val)
        {
            Size = BitSize;
            Value = new uint[Size];
            Sign = Val >= 0 ? signt.positive : signt.negative;
            ulong AbsVal = Val;
            ToBigInt(AbsVal);
        }

        public BigNumber(short BitSize, string Str)
        {
            Size = BitSize;
            Value = new uint[Size];
            if (Str != null && Str.Length >= 1)
            {
                Sign = Str[0] == '-' ? signt.negative : signt.positive;
            }
            ToBigInt(Str);
        }
        #endregion

        #region abstract & override functions
        public override bool Equals(object obj)
        {
            BigNumber B = (BigNumber)obj;
            B = this.SameSize(B); 

            if (this.Size != B.Size || this.Spart != B.Spart)
                return false;
            
            for (int i = 0; i < this.Spart; i++)
            {
                if (this.Value[i] != B.Value[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Size, Spart, Sign, Value);
        }

        public abstract void ToBigInt(string Str);

        public abstract void ToBigInt(byte[] ByteArray);

        #endregion

        #region split, shift & more
        public void SetBit(uint BitNumber)
        {
            uint BytePos = BitNumber >> 5;             // divide by 32
            byte BitByte = (byte)(BitNumber & 0x1F);    // get the lowest 5 bits

            uint Mask = (uint)1 << BitByte;
            this.Value[BytePos] |= Mask;

            if (BytePos >= this.Spart)
                this.Spart = (short)(BytePos + 1);
        }

        public bool Bit(int BitNumber)
        {
            int BytePos = BitNumber >> BINARY_CELL_SIZE_POW;
            byte BitByte = (byte)(BitNumber & 0x1F);

            uint Mask = (uint)1 << BitByte;

            uint BitValue = this.Value[BytePos] & Mask;
            return BitValue > 0;
        }

        public void UnsetBit(uint BitNumber)
        {
            uint BytePos = BitNumber >> BINARY_CELL_SIZE_POW;

            if (BytePos < this.Spart)
            {
                byte BitByte = (byte)(BitNumber & 0x1F);

                uint Mask = (uint)1 << BitByte;
                uint Mask2 = CELL_MASK ^ Mask;

                this.Value[BytePos] &= Mask2;

                if (this.Spart > 1 && this.Value[this.Spart - 1] == 0)
                    this.Spart--;
            }
        }

        /// <summary>
        /// Return true if this is an even number (gerade)
        /// </summary>
        /// <returns></returns>
        public bool Even()
        {
            if ((this.Value[0] & 0x1) != 0)
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
            if (!this.Even())
            {
                return true;
            }

            return false;
        }

        public void Clear()
        {
            this.Reduce();
            for (int i = this.Spart - 1; i >= 0; i--)
                this.Value[i] = 0;

            this.Reduce();
        }

        public void GenRandomBits(short Size, Random Rand)
        {
            if (Size > this.Size)
            {
                throw new ArgumentOutOfRangeException($"The parameter Size can't be bigger than this.Size: {this.Size}.");
            }

            int DWords = Size >> BINARY_CELL_SIZE_POW; // Size / CELL_SIZE using shift right
            int RemSize = Size & 0x1F;

            if (RemSize != 0)
                DWords++;

            for (int i = 0; i < DWords; i++)
                this.Value[i] = (uint)(Rand.NextDouble() * MAX_CELL_VALUE);

            for (int i = DWords; i < this.Size; i++)
                this.Value[i] = 0;

            if (RemSize != 0)
            {
                uint Mask = (uint)(0x01 << (RemSize - 1));
                this.Value[DWords - 1] |= Mask;

                Mask = (uint)(CELL_MASK >> (CELL_SIZE - RemSize));
                this.Value[DWords - 1] &= Mask;
            }
            else
            {
                this.Value[DWords - 1] |= HIGH_BIT_MASK;
            }

            this.Spart = (short)DWords;

            if (this.Spart == 0)
                this.Spart = 1;
        }

        /// <summary>
        /// Instead of ZeroBits we use BitsCount
        /// which return the position of the highest bit with 1
        /// </summary>
        /// <returns></returns>
        public int BitsCount()
        {
            this.Reduce();
            uint HighValue = this.Value[this.Spart - 1];
            int NumberOfBits = CELL_SIZE;
            
            uint HighBitMaskCopy = HIGH_BIT_MASK;
            while (NumberOfBits > 0 && (HighValue & HighBitMaskCopy) == 0)
            {
                NumberOfBits--;
                HighBitMaskCopy >>= 1;
            }
            NumberOfBits += (this.Spart - 1) << BINARY_CELL_SIZE_POW;

            return NumberOfBits;
        }

        public void Split(ref BigInt D, ref BigInt E, short Part)
        {
            if (Part >= this.Size)
                throw new ArgumentOutOfRangeException("The provided Part parameter is out of range");

            short iPart = 0;
            for (short i = 0; i < this.Spart; i++)
            {
                if (i < Part)
                {
                    E.Value[i] = this.Value[i];
                }
                else
                {
                    D.Value[iPart] = this.Value[i];
                    iPart++;
                }
            }
            E.Reduce();
            D.Reduce();
            //for (int i = 0; i < Part; i++)
            //{
            //    D.ShiftLeft(16);
            //}
            
            //BigInt Tmp = D + E;
            //Tmp.Reduce();
        }

        /// <summary>
        /// Shift left << ShiftValue
        /// </summary>
        /// <param name="ShiftValue"></param>
        public void ShiftLeft(short ShiftValue, bool FixedSize = false)
        {
            if (ShiftValue > CELL_SIZE)
                throw new ArgumentOutOfRangeException("The shift function could not be executed, due to a wrong parameter.");

            uint Over = 0;
            this.Resize((short)(this.Spart + 1));

            for (int i = 0; i < this.Spart; i++)
            {
                if (FixedSize)
                {
                    if (i < this.Size / CELL_SIZE)
                    {
                        ulong Tmp = (((ulong)this.Value[i]) << ShiftValue);
                        Tmp |= Over;
                        this.Value[i] = (uint)(Tmp & CELL_MASK);
                        Over = (uint)(Tmp >> CELL_SIZE) & CELL_MASK;
                    }
                    else
                    {
                        Over = 0;
                    }
                }
                else
                {
                    ulong Tmp = (((ulong)this.Value[i]) << ShiftValue);
                    Tmp |= Over;
                    this.Value[i] = (uint)(Tmp & CELL_MASK);
                    Over = (uint)(Tmp >> CELL_SIZE) & CELL_MASK;
                }
            }

            this.Value[this.Spart] = Over;

            if (Over != 0)
                this.Spart++;
        }

        /// <summary>
        /// Shift left << ShiftValue on array
        /// </summary>
        /// <param name="ShiftValue"></param>
        /// <returns></returns>
        public static int ShiftLeft(uint[] Value, short ShiftValue)
        {
            int ShiftAmount = CELL_SIZE;
            int ValLen = Value.Length;

            while (ValLen > 1 && Value[ValLen - 1] == 0)
                ValLen--;

            for (int count = ShiftValue; count > 0;)
            {
                if (count < ShiftAmount)
                    ShiftAmount = count;

                ulong Over = 0;
                for (int i = 0; i < ValLen; i++)
                {
                    ulong Tmp = ((ulong)Value[i]) << ShiftAmount;
                    Tmp |= Over;

                    Value[i] = (uint)(Tmp & CELL_MASK);
                    Over = Tmp >> CELL_SIZE;
                }

                if (Over != 0)
                {
                    if (ValLen + 1 <= Value.Length)
                    {
                        Value[ValLen] = (uint)Over;
                        ValLen++;
                    }
                }
                count -= ShiftAmount;
            }
            return ValLen;
        }

        /// <summary>
        /// Shift rigth >> ShiftValue (diese Funktion kann auf Grund der Maske und der HIGH_SHIFT Constante nur um ein verschieben)
        /// </summary>
        /// <param name="ShiftValue"></param>
        /// <returns></returns>
        public uint ShiftRigth()
        {
            uint Lowest = this.Value[0] & 1;
            for (int i = 0; i <= this.Spart - 2; i++)
            {
                uint low = this.Value[i + 1] & 1;
                this.Value[i] = ((this.Value[i] >> 1) & ~HIGH_BIT_MASK) | (low << HIGH_SHIFT);
            }
            this.Value[this.Spart - 1] = this.Value[this.Spart - 1] >> 1;
            this.Reduce();

            return Lowest;
        }

        /// <summary>
        /// Shift rigth >> ShiftValue on array
        /// </summary>
        /// <param name="ShiftValue"></param>
        /// <returns></returns>
        public static int ShiftRigth(uint[] Value, short ShiftValue)
        {
            int ShiftAmount = CELL_SIZE;
            int InvShift = 0;
            int ValLen = Value.Length;

            while (ValLen > 1 && Value[ValLen - 1] == 0)
                ValLen--;

            for (int count = ShiftValue; count > 0;)
            {
                if (count < ShiftAmount)
                {
                    ShiftAmount = count;
                    InvShift = CELL_SIZE - ShiftAmount;
                }

                ulong Over = 0;
                for (int i = ValLen - 1; i >= 0; i--)
                {
                    ulong Tmp = ((ulong)Value[i]) >> ShiftAmount;
                    Tmp |= Over;

                    Over = ((ulong)Value[i]) << InvShift;
                    Value[i] = (uint)(Tmp);
                }

                count -= ShiftAmount;
            }

            while (ValLen > 1 && Value[ValLen - 1] == 0)
                ValLen--;

            return ValLen;
        }

        public static byte[] StringToByteArray(string Text)
        {
            return Enumerable.Range(0, Text.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(Text.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] ByteArray)
        {
            StringBuilder hex = new StringBuilder(ByteArray.Length * 2);
            foreach (byte b in ByteArray)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion

        #region protected functions
        protected void Reduce()
        {
            // the higher part above spart
            short i = (short)(this.Size - 1);
            for (; i > 0 && this.Value[i] == 0; i--) { }
            this.Spart = (short)(i + 1);
        }

        protected signt2 LookAtSign(BigNumber B)
        {
            if (this.Sign == signt.positive && B.Sign == signt.positive)
            {
                return signt2.pp;
            }
            if (this.Sign == signt.negative && B.Sign == signt.positive)
            {
                return signt2.np;
            }
            if (this.Sign == signt.positive && B.Sign == signt.negative)
            {
                return signt2.pn;
            }
            return signt2.nn;
        }

        protected void Resize(short NewSize)
        {
            if (this.Spart > NewSize)
            {
                this.Reduce();
                if (this.Spart > NewSize)
                    throw new ArgumentException("Unable to resize the BigInt to the provided size.");
            }
            if (this.Spart < NewSize)
            {
                this.Expand(NewSize);
            }
        }
        
        protected void Expand(short NewSize)
        {
            if (this.Size > NewSize )
            {
                this.Spart = NewSize;
            }
            else
            {
                uint[] NewValue = new uint[NewSize];
                for (int i = 0; i < this.Spart; i++)
                {
                    NewValue[i] = this.Value[i];
                }
                this.Value = NewValue;
                this.Size = NewSize;
                this.Reduce();
            }
        }

        protected BigNumber SameSize(BigNumber B)
        {
            if (this.Spart > B.Spart)
            {
                B.Resize(this.Spart);   
            }
            else if (this.Spart < B.Spart)
            {
                this.Resize(B.Spart);
            }

            return B;
        }

        protected void Assign(BigNumber B)
        {

        }

        protected void CorrectNegativeZero()
        {
        }

        protected void ToBigInt(ulong Val)
        {
            Spart = 0;
            while (Val != 0 && Spart < Size)
            {
                Value[Spart] = (uint)(Val & CELL_MASK);
                Val >>= CELL_SIZE;
                Spart++;
            }

            if (Sign == signt.positive) // overflow check for overflow on positive value
            {
                if (Val != 0 || (Value[Size - 1] & HIGH_BIT_MASK) != 0)
                    throw (new OverflowException("Size overflow, while initialising the value array."));
            }

            if (Spart == 0)
                Spart = 1;
        }

        protected bool IsNumeric(string str)
        {
            if (str == null)
                return false;

            // The following verification is only in the context of this execise guilty, then hexadecimal number doesn't necessary contain a 0x
            return !(str.Contains('X') || str.Contains('O') || str.Contains('B'));
        }

        protected uint ToInt()
        {
            if (Spart > 1)
            {
                throw new OutOfMemoryException("The current BigInt can't be converted to integer.");
            }


            return Value[(Spart == 0 ? Spart : Spart - 1)];
        }
        #endregion
    }
}
