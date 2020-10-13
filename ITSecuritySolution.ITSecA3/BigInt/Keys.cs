using System;
using System.Collections.Generic;
using System.Text;

namespace BigInt
{
    public class Keys
    {
        private PublicKey PublicKey;
        private PrivateKey PrivateKey;

        public Keys()
        {
            this.PublicKey = new PublicKey();
            this.PrivateKey = new PrivateKey();
        }

        public Keys(PublicKey PublicKey, PrivateKey PrivateKey)
        {
            this.PublicKey = PublicKey;
            this.PrivateKey = PrivateKey;
        }

        public Keys(Keys K)
        {
            this.PublicKey = K.PublicKey;
            this.PrivateKey = K.PrivateKey;
        }

        public static PublicKey GetPublicRSA(Keys K)
        {
            return K.PublicKey;
        }

        public static PrivateKey GetPrivateRSA(Keys K)
        {
            return K.PrivateKey;
        }

        public static Keys GenerateRSAKeys(BigInt E, BigInt p, BigInt q, bool CheckPreConditions = false)
        {
            Keys Keys = new Keys();
            int DiffPQSize = p.BitsCount() - q.BitsCount();
            if (!CheckPreConditions || (p != q && Math.Abs(DiffPQSize) <= 30))
            {
                BigInt N = new BigInt(E.Size, 0);
                BigInt phiN = new BigInt(E.Size, 0);
                phiN = (p - 1) * (q - 1);

                if (BigInt.Egc(E, phiN) == 1)
                {
                    N = p * q;
                    BigInt D = E.ModInverse(phiN);
                    Keys.PublicKey = new PublicKey(N, E);
                    Keys.PrivateKey = new PrivateKey(N, D, p, q);
                }
                else
                {
                    throw new ArithmeticException("An error occur while generating the RSA Key: Egc(E, phiN) != 1!");
                }
            }
            else
            {
                throw new ArithmeticException("An error occur while generating the RSA Key: p == q or |Size(p) - Size(q)| > 30!");
            }
            
            return Keys;
        }

        public static Keys GenerateRSAKeys(BigInt E, short Size, int Rounds = 20, bool CheckPreConditions = false)
        {
            Keys Keys = new Keys();
            Random Rand = new Random();
            BigInt N = new BigInt(E.Size, 0);
            BigInt phiN = new BigInt(E.Size, 0);
            // Generate p a Prime Number of Size 
            BigInt p = new BigInt(E.Size, 0);
            BigInt q = new BigInt(E.Size, 0);
            bool Done = false;
            int DiffPQSize = 0;
            while (!Done)
            {
                p.GenPseudoPrime(Size, Rounds, Rand);
                do
                {
                    q.GenPseudoPrime(Size, Rounds, Rand);
                    DiffPQSize = p.BitsCount() - q.BitsCount();
                } while (p == q || (Math.Abs(DiffPQSize) > 30 && CheckPreConditions));
                phiN = (p - 1) * (q - 1);

                if (BigInt.Egc(E, phiN) == 1)
                {
                    Done = true;
                    N = p * q;
                }
                else
                {
                    p.Clear();
                    q.Clear();
                }
            }

            BigInt D = E.ModInverse(phiN);
            Keys.PublicKey = new PublicKey(N, E);
            Keys.PrivateKey = new PrivateKey(N, D, p, q);

            return Keys;
        }

        public static byte[] EncryptRSA(Key PublicK, byte[] Plain, short Size = 1000)
        {
            BigInt PPlain = new BigInt(Size, Plain);
            BigInt PCipher = EncryptRSA(PublicK, PPlain);

            return PCipher.ToByteArray();
        }

        public static BigInt EncryptRSA(Key PrivateK, BigInt PPlain)
        {
            PublicKey PK = PrivateK as PublicKey;
            BigInt PCipher = PPlain.PowModPrim(PK.E, PK.N);

            return PCipher;
        }

        public static byte[] DecryptRSA(Key PrivateK, byte[] Cipher, short Size = 1000)
        {
            BigInt PCipher = new BigInt(Size, Cipher);
            BigInt PPlain = DecryptRSA(PrivateK, PCipher);

            return PPlain.ToByteArray();
        }

        public static BigInt DecryptRSA(Key PrivateK, BigInt PCipher)
        {
            PrivateKey PK = PrivateK as PrivateKey;
            BigInt PPlain = PCipher.PowModPrim(PK.D, PK.N);

            return PPlain;
        }
    }
}
