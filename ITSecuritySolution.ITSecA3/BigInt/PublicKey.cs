using System;
using System.Collections.Generic;
using System.Text;

namespace BigInt
{
    public class PublicKey : Key
    {
        public BigInt E { get; set; }

        public PublicKey()
        {
            this.N = new BigInt();
            this.E = new BigInt();
        }

        public PublicKey(short Size)
        {
            this.N = new BigInt(Size, 0);
            this.E = new BigInt(Size, 0);
        }

        public PublicKey(BigInt N, BigInt E)
        {
            this.N = N;
            this.E = E;
        }
    }
}
