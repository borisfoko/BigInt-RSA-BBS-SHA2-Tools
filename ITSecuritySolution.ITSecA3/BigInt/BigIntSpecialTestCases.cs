using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigInt
{
    public static class BigIntSpecialTestCases
    {
        public static BigInt[] PrimeNumbers = {
                new BigInt(500, 900004991749), new BigInt(500, 900004991761), new BigInt(500, 900004991797), new BigInt(500, 900004991959), new BigInt(500, 900004992053),
                new BigInt(500, 900004992119), new BigInt(500, 900004992167), new BigInt(500, 900004992239), new BigInt(500, 900004992301), new BigInt(500, 900004992323),
                new BigInt(500, 900004992433), new BigInt(500, 900004992451), new BigInt(500, 900004992553), new BigInt(500, 900004992611), new BigInt(500, 900004992629),
                new BigInt(500, 1704993683), new BigInt(500, 1704993751), new BigInt(500, 1704993809), new BigInt(500, 1704993821), new BigInt(500, 1704993847),
                new BigInt(500, 1704993889), new BigInt(500, 1704993967), new BigInt(500, 1704994069), new BigInt(500, 1704994097), new BigInt(500, 1704994133),
                new BigInt(500, 1704994211), new BigInt(500, 1704994261), new BigInt(500, 1704994289), new BigInt(500, 1704994363), new BigInt(500, 1704994397),
                new BigInt(500, 9999992869), new BigInt(500, 9999992957), new BigInt(500, 9999992983), new BigInt(500, 9999993049), new BigInt(500, 9999993127),
                new BigInt(500, 99999996713), new BigInt(500, 99999996773), new BigInt(500, 99999996799), new BigInt(500, 99999996839), new BigInt(500, 99999996961),
                new BigInt(500, 99999997303), new BigInt(500, 99999997309), new BigInt(500, 99999997351), new BigInt(500, 99999997381), new BigInt(500, 99999997477),
                new BigInt(500, 99999998401), new BigInt(500, 99999998491), new BigInt(500, 99999998513), new BigInt(500, 99999998591), new BigInt(500, 99999998639),
                new BigInt(500, 100000001201), new BigInt(500, 100000001237), new BigInt(500, 100000001239), new BigInt(500, 100000001267), new BigInt(500, 100000001291),
                new BigInt(500, 100000001567), new BigInt(500, 100000001693), new BigInt(500, 100000001729), new BigInt(500, 100000001807), new BigInt(500, 100000001839),
                new BigInt(500, 100000002313), new BigInt(500, 100000002359), new BigInt(500, 100000002373), new BigInt(500, 100000002497), new BigInt(500, 100000002511),
                new BigInt(500, 608599992677), new BigInt(500, 608599992757), new BigInt(500, 608599992851), new BigInt(500, 608599992859), new BigInt(500, 608599992961),
                new BigInt(500, 608599993373), new BigInt(500, 608599993399), new BigInt(500, 608599993423), new BigInt(500, 608599993447), new BigInt(500, 608599993627),
                new BigInt(500, 608599994003), new BigInt(500, 608599994053), new BigInt(500, 608599994081), new BigInt(500, 608599994089), new BigInt(500, 608599994099),
                new BigInt(500, 608600006563), new BigInt(500, 608600006633), new BigInt(500, 608600006659), new BigInt(500, 608600006713), new BigInt(500, 608600006753),
                new BigInt(500, 100999990093), new BigInt(500, 100999990163), new BigInt(500, 100999990241), new BigInt(500, 100999990253), new BigInt(500, 100999990319),
                new BigInt(500, 100999991233), new BigInt(500, 100999991273), new BigInt(500, 100999991321), new BigInt(500, 100999991327), new BigInt(500, 100999991357),
                new BigInt(500, 999999981887), new BigInt(500, 999999982211), new BigInt(500, 999999982529), new BigInt(500, 999999982843), new BigInt(500, 999999984391)
            };

        public static BigInt[] PseudoPrimeNumbers = {
                new BigInt(500, 10993185054677725597), new BigInt(500, 10993200214802414401), new BigInt(500, 10993212914999515201), new BigInt(500, 10993219690777669621), new BigInt(500, 10993230230236720201),
                new BigInt(500, 13665900215815776721), new BigInt(500, 13665934431781693469), new BigInt(500, 13665947752833424981), new BigInt(500, 13665966796803558601), new BigInt(500, 13665982342965547129),
                new BigInt(500, 16389573418751402959), new BigInt(500, 16389592372782020519), new BigInt(500, 16389601613926674421), new BigInt(500, 16389604839257995781), new BigInt(500, 16389608911648739929),
                new BigInt(500, 16389612715707123901), new BigInt(500, 18446624612099691505), new BigInt(500, 18446635481091507361), new BigInt(500, 18446639547492921493), new BigInt(500, 18446642210724381457),
                new BigInt(500, 18446645392875227881), new BigInt(500, 18446651352006624001), new BigInt(500, 18446654960933245921), new BigInt(500, 18446656269369007723), new BigInt(500, 18446668736549996989),
                new BigInt(500, 18446674239670229281), new BigInt(500, 18446675513908950001), new BigInt(500, 18446677186223359009), new BigInt(500, 18446677902069057901), new BigInt(500, 18446683487144444897),
                new BigInt(500, 18446684791929763201), new BigInt(500, 18446685171082914241), new BigInt(500, 18446686318957015061), new BigInt(500, 18446686952597190001), new BigInt(500, 18446687394396243841),
                new BigInt(500, 18446688291790223401), new BigInt(500, 18446689337983373377), new BigInt(500, 18446689777234897201), new BigInt(500, 18446690000246224753), new BigInt(500, 18446690015682401233),
                new BigInt(500, 18446690259197316001), new BigInt(500, 18446690544360336133), new BigInt(500, 18446690874901097161), new BigInt(500, 18446691311846733601), new BigInt(500, 18446691892041291451),
                new BigInt(500, 18446697183833591905), new BigInt(500, 18446699015649780247), new BigInt(500, 18446699408033728567), new BigInt(500, 18446700105416602351), new BigInt(500, 18446700128167222657),
                new BigInt(500, 18446702884020593281), new BigInt(500, 18446703190816829633), new BigInt(500, 18446704002248331349), new BigInt(500, 18446704742567503447), new BigInt(500, 18446705108449826753),
                new BigInt(500, 18446709571321807417), new BigInt(500, 18446710024174000381), new BigInt(500, 18446710079811121681), new BigInt(500, 18446710243827068281), new BigInt(500, 18446710447435320001),
                new BigInt(500, 18446721202978330081), new BigInt(500, 18446721218536198537), new BigInt(500, 18446721300791617501), new BigInt(500, 18446721615709341967), new BigInt(500, 18446721936576809041),
                new BigInt(500, 18446727097149099841), new BigInt(500, 18446727240125961841), new BigInt(500, 18446727254652387181), new BigInt(500, 18446728542183375841), new BigInt(500, 18446728638365522389),
                new BigInt(500, 18446732048410894801), new BigInt(500, 18446732248657918465), new BigInt(500, 18446732299047587569), new BigInt(500, 18446732893888604471), new BigInt(500, 18446732991028667305),
                new BigInt(500, 18446736502759030381), new BigInt(500, 18446736901932358201), new BigInt(500, 18446737082577197941), new BigInt(500, 18446737307521019281), new BigInt(500, 18446737459516072561),
                new BigInt(500, 18446737742284901281), new BigInt(500, 18446737995530402783), new BigInt(500, 18446738032506318481), new BigInt(500, 18446738264939766601), new BigInt(500, 18446738329966743001),
                new BigInt(500, 18446739198873592321), new BigInt(500, 18446739756676839221), new BigInt(500, 18446740289125757281), new BigInt(500, 18446740492174327009), new BigInt(500, 18446740794165828629),
                new BigInt(500, 18446741349713717041), new BigInt(500, 18446742119396961721), new BigInt(500, 18446742141965751601), new BigInt(500, 18446742194801622841), new BigInt(500, 18446742339906480601),
                new BigInt(500, 18446742593753802109), new BigInt(500, 18446743208455367653), new BigInt(500, 18446743437181319521), new BigInt(500, 18446744010534295051), new BigInt(500, 18446744066047760377)
            };

        public static BigInt[] CombinedNumbers = {
                new BigInt(500, "+0x000000000000000000000000000000000000000000000000000000000000000a"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000000000000000000064"), new BigInt(500, "+0x00000000000000000000000000000000000000000000000000000000000003e8"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000000000000000002710"), new BigInt(500, "+0x00000000000000000000000000000000000000000000000000000000000186a0"),
                new BigInt(500, "+0x00000000000000000000000000000000000000000000000000000000000f4240"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000000000000000989680"), new BigInt(500, "+0x000000000000000000000000000000000000000029d80188db8067259fd9fc5d"), new BigInt(500, "+0x0000000000000000000000000000000000000001a2700f58930407783e83dba2"), new BigInt(500, "+0x000000000000000000000000000000000000001058609975be284ab271269454"),
                new BigInt(500, "+0x00000000000000000000000000000000000000a373c5fe996d92eaf86b81cb48"), new BigInt(500, "+0x000000000000000000000000000000000000066285bbf1fe47bd2db43311f0d0"), new BigInt(500, "+0x0000000000000000000000000000000000003fd9395773eecd63c909feb36820"), new BigInt(500, "+0x0000000000000000000000000000000000027e7c3d6a875405e5da63f3021140"), new BigInt(500, "+0x000000000000000000000000000000000018f0da662949483afa87e77e14ac80"),
                new BigInt(500, "+0x000000000000000000000000000000188db80672a7baccb172978f642ef5bf72"), new BigInt(500, "+0x000000000000000000000000000000f58930407a8d4bfeee79eb99e9d5997a74"), new BigInt(500, "+0x000000000000000000000000000009975be284c984f7f550c334032257fec888"), new BigInt(500, "+0x00000000000000000000000000005fe996d92fdf31af9527a0081f576ff3d550"), new BigInt(500, "+0x0000000000000000000000000003bf1fe47bdeb7f0dbd38c4051396a5f865520"),
                new BigInt(500, "+0x0000000000000000000000000025773eecd6b32f6896437a832c3e27bb3f5340"), new BigInt(500, "+0x0000000000000000000000000176a8754062ffda15dea2c91fba6d8d50794080"), new BigInt(500, "+0x0000000000000000000000000ea2949483ddfe84dab25bdb3d48478524bc8500"), new BigInt(500, "+0x000000000008067259fa562578eadb7230167a56129d80188db80672a604e676"), new BigInt(500, "+0x000000000050407783c75d76b92c9275e0e0c75cba2700f58930407a7c31009c"),
                new BigInt(500, "+0x00000000032284ab25c9a6a33bbdb89ac8c7c99f458609975be284c8d9ea0618"), new BigInt(500, "+0x000000001f592eaf79e082605569360bd7cde038b73c5fe996d92fd883243cf0"), new BigInt(500, "+0x00000001397bd2dac2c517c3561c1c766e0ac237285bbf1fe47bde751f6a6160"), new BigInt(500, "+0x0000000c3ed63c8b9bb2eda15d191ca04c6b96279395773eecd6b0933a27cdc0"), new BigInt(500, "+0x0000007a745e5d7414fd484da2fb1e42fc33dd8bc3d6a8754062e5c0458e0980"),
                new BigInt(500, "+0x000004c88bafa688d1e4d3085dcf2e9dda06a775a662949483dcf982b78c5f00"), new BigInt(500, "+0x000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"), new BigInt(500, "+0x000009fffffffffffffffffffffffffffffffffffffffffffffffffffffffff6"), new BigInt(500, "+0x000063ffffffffffffffffffffffffffffffffffffffffffffffffffffffff9c"), new BigInt(500, "+0x0003e7fffffffffffffffffffffffffffffffffffffffffffffffffffffffc18"),
                new BigInt(500, "+0x00270fffffffffffffffffffffffffffffffffffffffffffffffffffffffd8f0"), new BigInt(500, "+0x01869ffffffffffffffffffffffffffffffffffffffffffffffffffffffe7960"), new BigInt(500, "+0x0f423ffffffffffffffffffffffffffffffffffffffffffffffffffffff0bdc0"), new BigInt(500, "+0x98967fffffffffffffffffffffffffffffffffffffffffffffffffffff676980"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000000000000000000004"),
                new BigInt(500, "+0x0000000000000000000000000000000000000000000000000d6ddf994599fc5d"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000001adbbf328b33f8ba"), new BigInt(500, "+0x00000000000000000000000000000000000000000000000035b77e651667f174"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000006b6efcca2ccfe2e8"), new BigInt(500, "+0x000000000000000000000000000000000000000000000000d6ddf994599fc5d0"),
                new BigInt(500, "+0x000000000000000000000000000000000000000000000001adbbf328b33f8ba0"), new BigInt(500, "+0x0000000000000000000000000000000000000000000000035b77e651667f1740"), new BigInt(500, "+0x000000000000000000000000000000000000000000000006b6efcca2ccfe2e80"), new BigInt(500, "+0x000000000000000000000000000000000000000187baccb172978f642ef5bf72"), new BigInt(500, "+0x00000000000000000000000000000000000000030f759962e52f1ec85deb7ee4"),
                new BigInt(500, "+0x00000000000000000000000000000000000000061eeb32c5ca5e3d90bbd6fdc8"), new BigInt(500, "+0x000000000000000000000000000000000000000c3dd6658b94bc7b2177adfb90"), new BigInt(500, "+0x00000000000000000000000000000000000000187baccb172978f642ef5bf720"), new BigInt(500, "+0x0000000000000000000000000000000000000030f759962e52f1ec85deb7ee40"), new BigInt(500, "+0x0000000000000000000000000000000000000061eeb32c5ca5e3d90bbd6fdc80"),
                new BigInt(500, "+0x00000000000000000000000000000000000000c3dd6658b94bc7b2177adfb900"), new BigInt(500, "+0x00000000000000000000002578eadb7230167a56129d80188db80672a604e676"), new BigInt(500, "+0x00000000000000000000004af1d5b6e4602cf4ac253b00311b700ce54c09ccec"), new BigInt(500, "+0x000000000000000000000095e3ab6dc8c059e9584a76006236e019ca981399d8"), new BigInt(500, "+0x00000000000000000000012bc756db9180b3d2b094ec00c46dc03395302733b0"),
                new BigInt(500, "+0x0000000000000000000002578eadb7230167a56129d80188db80672a604e6760"), new BigInt(500, "+0x0000000000000000000004af1d5b6e4602cf4ac253b00311b700ce54c09ccec0"), new BigInt(500, "+0x00000000000000000000095e3ab6dc8c059e9584a76006236e019ca981399d80"), new BigInt(500, "+0x0000000000000000000012bc756db9180b3d2b094ec00c46dc03395302733b00"), new BigInt(500, "+0x7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"),
                new BigInt(500, "+0x6ddfbaccb172978f642efda8715248dcfe985a9ed627fe77247f98d59fb198aa"), new BigInt(500, "+0xdbbf759962e52f1ec85dfb50e2a491b9fd30b53dac4ffcee48ff31ab3f633154"), new BigInt(500, "+0xb77eeb32c5ca5e3d90bbf6a1c5492373fa616a7b589ff9dc91fe63567ec662a8"), new BigInt(500, "+0x6efdd6658b94bc7b2177ed438a9246e7f4c2d4f6b13ff3b923fcc6acfd8cc550"), new BigInt(500, "+0xddfbaccb172978f642efda8715248dcfe985a9ed627fe77247f98d59fb198aa0"),
                new BigInt(500, "+0xbbf759962e52f1ec85dfb50e2a491b9fd30b53dac4ffcee48ff31ab3f6331540"), new BigInt(500, "+0x77eeb32c5ca5e3d90bbf6a1c5492373fa616a7b589ff9dc91fe63567ec662a80"), new BigInt(500, "+0xefdd6658b94bc7b2177ed438a9246e7f4c2d4f6b13ff3b923fcc6acfd8cc5500"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffd5c504ca"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffab8a0994"),
                new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffffffffffff57141328"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffffffffffeae282650"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffffffffffd5c504ca0"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffffffffffab8a09940"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffffffffff571413280"),
                new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffff2922066ba6603a3"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffffe52440cd74cc0746"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffffca48819ae9980e8c"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffff94910335d3301d18"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffffffffffff2922066ba6603a30"),
                new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffe52440cd74cc07460"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffffca48819ae9980e8c0"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffffffffff94910335d3301d180"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffe7845334e8d68709bd10a408e"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffffcf08a669d1ad0e137a214811c"),
                new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffff9e114cd3a35a1c26f44290238"), new BigInt(500, "+0xfffffffffffffffffffffffffffffffffffffff3c2299a746b4384de88520470"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffe7845334e8d68709bd10a408e0"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffffcf08a669d1ad0e137a214811c0"), new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffff9e114cd3a35a1c26f442902380"),
                new BigInt(500, "+0xffffffffffffffffffffffffffffffffffffff3c2299a746b4384de885204700"), new BigInt(500, "+0xffffffffffffffffffffffda8715248dcfe985a9ed627fe77247f98d59fb198a"), new BigInt(500, "+0xffffffffffffffffffffffb50e2a491b9fd30b53dac4ffcee48ff31ab3f63314"), new BigInt(500, "+0xffffffffffffffffffffff6a1c5492373fa616a7b589ff9dc91fe63567ec6628"), new BigInt(500, "+0xfffffffffffffffffffffed438a9246e7f4c2d4f6b13ff3b923fcc6acfd8cc50")
            };

        /// <summary>
        /// Run the RunPrimeTestCase with 3 differents inputs
        /// A. 100 Prime Numbers
        /// B. 100 Pseudo Prime Numbers
        /// C. Combined Numbers (not prime numbers)
        /// </summary>
        public static void RunABCTestCases()
        {
            Console.WriteLine("####################################(A)#######################################");
            Console.WriteLine("####  Tests on 100 real prime numbers using Fermat, Euler, Miller-Rabin,  ####");
            Console.WriteLine("####  PrimeDivision with Prime Number Bases and random generated numbers  ####");
            Console.WriteLine("####################################(A)#######################################");
            RunPrimeTestCase(PrimeNumbers);
            // B. PseudoPrimeNumbers Test Case
            Console.WriteLine("#####################################(B)#######################################");
            Console.WriteLine("####  Tests on 100 pseudo prime numbers using Fermat, Euler, Miller-Rabin, ####");
            Console.WriteLine("####  PrimeDivision with Prime Number Bases and random generated numbers   ####");
            Console.WriteLine("#####################################(B)#######################################");
            RunPrimeTestCase(PseudoPrimeNumbers);
            // C. Combined Numbers Test Case
            Console.WriteLine("#####################################(C)#######################################");
            Console.WriteLine("####   Tests on 100 combined numbers using Fermat, Euler, Miller-Rabin,    ####");
            Console.WriteLine("####  PrimeDivision with Prime Number Bases and random generated numbers   ####");
            Console.WriteLine("#####################################(C)#######################################");
            RunPrimeTestCase(CombinedNumbers);
        }

        public static void RunPrimeTestCase(BigInt[] Numbers)
        {
            // Test with 50 rounds and 20 Rounds
            int ResultPrimDivCount = 0;
            int ResultPrimFermatCount = 0;
            int ResultPrimEulerCount = 0;
            int ResultPrimMRCount = 0;
            int ResultPrimFermatCount50 = 0;
            int ResultPrimEulerCount50 = 0;
            int ResultPrimMRCount50 = 0;
            int ResultPrimFermatCount20 = 0;
            int ResultPrimEulerCount20 = 0;
            int ResultPrimMRCount20 = 0;
            int ResultNoPrimFermatRound19 = 0;
            int ResultNoPrimEulerRound19 = 0;
            int ResultNoPrimMRRound19 = 0;
            int ResultNoPrimFermatRound29 = 0;
            int ResultNoPrimEulerRound29 = 0;
            int ResultNoPrimMRRound29 = 0;
            int ResultNoPrimFermatRound39 = 0;
            int ResultNoPrimEulerRound39 = 0;
            int ResultNoPrimMRRound39 = 0;
            int ResultNoPrimFermatRound50 = 0;
            int ResultNoPrimEulerRound50 = 0;
            int ResultNoPrimMRRound50 = 0;

            for (int i = 0; i < Numbers.Length; i++)
            {
                int PBitCount = Numbers[i].BitsCount();
                // Number smaller than 3,1*10^23 (78 bit)
                // All prime number less than 100 
                int[] PrimeTestList1 = PBitCount <= 78 ? BigIntConfiguration.PRIME_NUMBERS_LIST2000.Take(24).ToArray() : BigIntConfiguration.PRIME_NUMBERS_LIST2000;
                int[] PrimeTestList2 = PBitCount <= 78 ? BigIntConfiguration.PRIME_NUMBERS_LIST2000.Take(12).ToArray() : BigIntConfiguration.PRIME_NUMBERS_LIST2000;

                // Test division with all prime numbers less than 100
                if (Numbers[i].IsPrimeDiv(PrimeTestList1))
                    ResultPrimDivCount++;

                // Fermat-Test with all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeFermat(PrimeTestList2))
                    ResultPrimFermatCount++;

                // Euler-Test with all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeEuler(PrimeTestList2))
                    ResultPrimEulerCount++;

                // Miller-Rabin-Test with all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeMR(PrimeTestList2))
                    ResultPrimMRCount++;

                // Fermat-Test with random generated BigInts on 50 Rounds
                int FermatTestResult = Numbers[i].IsPrimeFermat();
                if (FermatTestResult == -1)
                    ResultPrimFermatCount50++;
                else if (FermatTestResult < 20)
                    ResultNoPrimFermatRound19++;
                else if (FermatTestResult < 30)
                    ResultNoPrimFermatRound29++;
                else if (FermatTestResult < 40)
                    ResultNoPrimFermatRound39++;
                else if (FermatTestResult <= 50)
                    ResultNoPrimFermatRound50++;

                // Euler-Test with random generated BigInts on 50 Rounds and all prime numbers less or equal to 37
                int EulerTestResult = Numbers[i].IsPrimeEuler();
                if (EulerTestResult == -1)
                    ResultPrimEulerCount50++;
                else if (EulerTestResult < 20)
                    ResultNoPrimEulerRound19++;
                else if (EulerTestResult < 30)
                    ResultNoPrimEulerRound29++;
                else if (EulerTestResult < 40)
                    ResultNoPrimEulerRound39++;
                else if (EulerTestResult <= 50)
                    ResultNoPrimEulerRound50++;

                // Miller-Rabin-Test with random generated BigInts on 50 Rounds and all prime numbers less or equal to 37
                int MRTestResult = Numbers[i].IsPrimeMR();
                if (MRTestResult == -1)
                    ResultPrimMRCount50++;
                else if (MRTestResult < 20)
                    ResultNoPrimMRRound19++;
                else if (MRTestResult < 30)
                    ResultNoPrimMRRound29++;
                else if (MRTestResult < 40)
                    ResultNoPrimMRRound39++;
                else if (MRTestResult <= 50)
                    ResultNoPrimMRRound50++;

                // Fermat-Test with random generated BigInts on 20 Rounds and all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeFermat(20) == -1)
                    ResultPrimFermatCount20++;

                // Euler-Test with random generated BigInts on 20 Rounds and all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeEuler(20) == -1)
                    ResultPrimEulerCount20++;

                // Miller-Rabin-Test with random generated BigInts on 20 Rounds and all prime numbers less or equal to 37
                if (Numbers[i].IsPrimeMR(20) == -1)
                    ResultPrimMRCount20++;
            }

            if (ResultNoPrimFermatRound19 > 0 || ResultNoPrimFermatRound29 > 0 || ResultNoPrimFermatRound39 > 0 || ResultNoPrimFermatRound50 > 0
            || ResultNoPrimEulerRound19 > 0 || ResultNoPrimEulerRound29 > 0 || ResultNoPrimEulerRound39 > 0 || ResultNoPrimEulerRound50 > 0
            || ResultNoPrimMRRound19 > 0 || ResultNoPrimMRRound29 > 0 || ResultNoPrimMRRound39 > 0 || ResultNoPrimMRRound50 > 0)
                Console.WriteLine("#------------------------------------ 1 --------------------------------------#");

            Console.WriteLine($"Percentage PrimeDiv Test on Prime Number less or equal to 37: {ResultPrimDivCount} %");
            Console.WriteLine($"Percentage Prime Fermat Test on Prime Number less or equal to 37: {ResultPrimFermatCount} %");
            Console.WriteLine($"Percentage Prime Euler Test on Prime Number less or equal to 37: {ResultPrimEulerCount} %");
            Console.WriteLine($"Percentage Prime Miller Rabin Test on Prime Number less or equal to 37: {ResultPrimMRCount} %");
            Console.WriteLine($"Percentage Prime Fermat Test by 50 Rounds: {ResultPrimFermatCount50} %");
            Console.WriteLine($"Percentage Prime Euler Test by 50 Rounds: {ResultPrimEulerCount50} %");
            Console.WriteLine($"Percentage Prime Miller Rabin Test by 50 Rounds: {ResultPrimMRCount50} %");
            Console.WriteLine($"Percentage Prime Fermat Test by 20 Rounds: {ResultPrimFermatCount20} %");
            Console.WriteLine($"Percentage Prime Euler Test by 20 Rounds: {ResultPrimEulerCount20} %");
            Console.WriteLine($"Percentage Prime Miller Rabin Test by 20 Rounds: {ResultPrimMRCount20} %");

            if (ResultNoPrimFermatRound19 > 0 || ResultNoPrimFermatRound29 > 0 || ResultNoPrimFermatRound39 > 0 || ResultNoPrimFermatRound50 > 0
            || ResultNoPrimEulerRound19 > 0 || ResultNoPrimEulerRound29 > 0 || ResultNoPrimEulerRound39 > 0 || ResultNoPrimEulerRound50 > 0
            || ResultNoPrimMRRound19 > 0 || ResultNoPrimMRRound29 > 0 || ResultNoPrimMRRound39 > 0 || ResultNoPrimMRRound50 > 0)
            {
                Console.WriteLine("#------------------------------------ 2 --------------------------------------#");
                Console.WriteLine($"Percentage Prime Fermat Test failed between 1 and 19 Rounds: {ResultNoPrimFermatRound19} %");
                Console.WriteLine($"Percentage Prime Euler  Test failed between 1 and 19 Rounds: {ResultNoPrimEulerRound19} %");
                Console.WriteLine($"Percentage Prime Miller-Rabin Test failed between 1 and 19 Rounds: {ResultNoPrimMRRound19} %");
                Console.WriteLine($"Percentage Prime Fermat Test failed between 20 and 29 Rounds: {ResultNoPrimFermatRound29} %");
                Console.WriteLine($"Percentage Prime Euler  Test failed between 20 and 29 Rounds: {ResultNoPrimEulerRound29} %");
                Console.WriteLine($"Percentage Prime Miller-Rabin Test failed between 20 and 29 Rounds: {ResultNoPrimMRRound29} %");
                Console.WriteLine($"Percentage Prime Fermat Test failed between 30 and 39 Rounds: {ResultNoPrimFermatRound39} %");
                Console.WriteLine($"Percentage Prime Euler  Test failed between 30 and 39 Rounds: {ResultNoPrimEulerRound39} %");
                Console.WriteLine($"Percentage Prime Miller-Rabin Test failed between 30 and 39 Rounds: {ResultNoPrimMRRound39} %");
                Console.WriteLine($"Percentage Prime Fermat Test failed between 40 and 50 Rounds: {ResultNoPrimFermatRound50} %");
                Console.WriteLine($"Percentage Prime Euler  Test failed between 40 and 50 Rounds: {ResultNoPrimEulerRound50} %");
                Console.WriteLine($"Percentage Prime Miller-Rabin Test failed between 40 and 50 Rounds: {ResultNoPrimMRRound50} %");
            }

        }

    }
}
