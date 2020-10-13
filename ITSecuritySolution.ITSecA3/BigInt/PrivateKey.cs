using System;
using System.Collections.Generic;
using System.Text;

namespace BigInt
{
    public class PrivateKey : Key
    {
        public BigInt D { get; set; }
        public BigInt P { get; set; }
        public BigInt Q { get; set; }

        public PrivateKey()
        {
            this.N = new BigInt();
            this.D = new BigInt();
            this.P = new BigInt();
            this.Q = new BigInt();
        }

        public PrivateKey(short Size)
        {
            this.N = new BigInt(Size, 0);
            this.D = new BigInt(Size, 0);
            this.P = new BigInt(Size, 0);
            this.Q = new BigInt(Size, 0);
        }

        public PrivateKey(BigInt N, BigInt D, BigInt P, BigInt Q)
        {
            this.N = N;
            this.D = D;
            this.P = P;
            this.Q = Q;
        }
    }
}
