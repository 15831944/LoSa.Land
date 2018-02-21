using LoSa.Land.Parcel;
using LoSa.Land.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoSa.Land.Service
{
    public static class ServiceForm6Zem
    {
        public static Form6Zem GetBaseForm()
        {
            Form6Zem form6Zem = new Form6Zem();

            #region Rows

            List<RowForm6Zem> rows = new List<RowForm6Zem>();
            List<int> joinRow;

            // 01
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 2, 9, 16 });

            rows.Add(new RowForm6Zem(1, new NumberRow("1"), joinRow,
                            "Сільськогосподарські підприємства (всього земель у власності і користуванні) (02+09+16)")
                        );

            // 02
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 4, 5, 6, 7, 8 });

            rows.Add(new RowForm6Zem(2, new NumberRow("1.1"), joinRow,
                            "Недержавні сільськогосподарські підприємства - всього (04+05+06+07+08)")
                        );

            // 03
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(3, new NumberRow("1.1.0"), joinRow,
                            "у тому числі резервний фонд")
                        );

            // 04
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(4, new NumberRow("1.1.1"), joinRow,
                            "Колективні сільськогосподарські  підприємства")
                        );

            // 05
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(5, new NumberRow("1.1.2"), joinRow,
                            "Сільськогосподарські кооперативи")
                        );


            // 06
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(6, new NumberRow("1.1.3"), joinRow,
                            "Сільськогосподарські товариства")
                        );

            // 07
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(7, new NumberRow("1.1.4"), joinRow,
                            "Підсобні сільські господарства недержав-них  підприємств, установ  та організацій")
                        );

            // 08
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(8, new NumberRow("1.1.5"), joinRow,
                            "Інші недержавні сільськогосподарські підприємства")
                        );

            // 09
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 10, 12, 13, 15 });

            rows.Add(new RowForm6Zem(9, new NumberRow("1.2"), joinRow,
                            "Державні сільськогосподарські підприємства - всього (10+12+13+15)")
                        );

            // 10
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(10, new NumberRow("1.2.1"), joinRow,
                            "Радгоспи всіх систем")
                        );

            // 11
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(11, new NumberRow("1.2.1.1"), joinRow,
                            " у тому числі радгоспи оборони")
                        );

            // 12
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(12, new NumberRow("1.2.2"), joinRow,
                            "Сільськогосподарські науково-дослідні установи і навчальні заклади")
                        );

            // 13
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(13, new NumberRow("1.2.3"), joinRow,
                            "Підсобні сільські  господарства державних підприємств, установ, організацій")
                        );

            // 14
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(14, new NumberRow("1.2.3.1"), joinRow,
                            "у тому числі частин, підприємств, установ, організацій оборони")
                        );

            // 15
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(15, new NumberRow("1.2.4"), joinRow,
                            "Інші державні сільськогосподарські підприємства")
                        );

            // 16
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(16, new NumberRow("1.3"), joinRow,
                            "Міжгосподарські підприємства")
                        );

            // 17
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 18, 19, 21, 22, 23, 27, 31, 35, 39, 40 });

            rows.Add(new RowForm6Zem(17, new NumberRow("2"), joinRow,
                            "Громадяни, яким надані землі у власність і користування (18+19+21+22+23+27+31+35+39+40)")
                        );

            // 18
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(18, new NumberRow("2.1"), joinRow,
                            "Селянські (фермерські) господарства")
                        );

            // 19
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(19, new NumberRow("2.2"), joinRow,
                            "Ділянки для ведення товарного сільскогосподарського виробництва")
                        );

            // 20
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(20, new NumberRow("2.2.1"), joinRow,
                            "у тому числі на земельних частках (паях)")
                        );

            // 21
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(21, new NumberRow("2.3"), joinRow,
                            "Особисті підсобні господарства")
                        );

            // 22
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(22, new NumberRow("2.4"), joinRow,
                            "Ділянки для будівництва та обслуговування житлового будинку і господар-ських будівель (присадибні ділянки)")
                        );

            // 23
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 24, 26 });

            rows.Add(new RowForm6Zem(23, new NumberRow("2.5"), joinRow,
                            "Ділянки для садівництва (24+26)")
                        );

            // 24
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(24, new NumberRow("2.5.1"), joinRow,
                            "Колективне садівництво")
                        );

            // 25
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(25, new NumberRow("2.5.1.1"), joinRow,
                            "у тому числі землі загального користування")
                        );

            // 26
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(26, new NumberRow("2.5.2"), joinRow,
                            "Індивідуальне садівництво")
                        );


            // 27
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 28, 30 });

            rows.Add(new RowForm6Zem(27, new NumberRow("2.6"), joinRow,
                            "Ділянки для дачного будівництва (28+30)")
                        );


            // 28
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(28, new NumberRow("2.6.1"), joinRow,
                            "Кооперативне дачне будівництво")
                        );


            // 29
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(29, new NumberRow("2.6.1.1"), joinRow,
                            "у тому числі землі загального користування")
                        );

            // 30
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(30, new NumberRow("2.6.2"), joinRow,
                            "Індивідуальне дачне будівництво")
                        );

            // 31
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 32, 34 });

            rows.Add(new RowForm6Zem(31, new NumberRow("2.7"), joinRow,
                            "Ділянки для гаражного будівництва (32+34)")
                        );

            // 32
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(32, new NumberRow("2.7.1"), joinRow,
                            "Кооперативне гаражне будівництво")
                        );

            // 33
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(33, new NumberRow("2.7.1.1"), joinRow,
                            " у тому числі землі загального користування")
                        );

            // 34
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(34, new NumberRow("2.7.2"), joinRow,
                            "Індивідуальне гаражне будівництво")
                        );

            // 35
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 36, 38 });

            rows.Add(new RowForm6Zem(35, new NumberRow("2.8"), joinRow,
                            "Ділянки для городництва (36+38)")
                        );

            // 36
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(36, new NumberRow("2.8.1"), joinRow,
                            "Колективне городництво")
                        );

            // 37
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(37, new NumberRow("2.8.1.1"), joinRow,
                            "у тому числі землі загального користування")
                        );

            // 38
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(38, new NumberRow("2.8.2"), joinRow,
                            "Індивідуальне городництво")
                        );

            // 39
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(39, new NumberRow("2.9"), joinRow,
                            "Ділянки для здійснення несільськогоспо-дарської підприємницької діяльності")
                        );

            // 40
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(40, new NumberRow("2.10"), joinRow,
                            "Ділянки для сінокосіння та випасання худоби")
                        );

            // 41
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58 });

            rows.Add(new RowForm6Zem(41, new NumberRow("3"), joinRow,
                            "Заклади, установи, організації (42+43+44+45+46+47+48+49+50+51+52+53+54+55+56+57+58)")
                        );

            // 42
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(42, new NumberRow("3.1"), joinRow,
                            "Органи державної влади та місцевого самоврядування")
                        );

            // 43
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(43, new NumberRow("3.2"), joinRow,
                            "Громадські організації")
                        );

            // 44
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(44, new NumberRow("3.3"), joinRow,
                            "Заклади науки")
                        );

            // 45
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(45, new NumberRow("3.4"), joinRow,
                            "Заклади освіти")
                        );

            // 46
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(46, new NumberRow("3.5"), joinRow,
                            "Заклади культурно-просвітницького обслуговування")
                        );

            // 47
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(47, new NumberRow("3.6"), joinRow,
                            "Релігійні організації")
                        );

            // 48
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(48, new NumberRow("3.7"), joinRow,
                            "Заклади фізичної культури та спорту")
                        );

            // 49
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(49, new NumberRow("3.8"), joinRow,
                            "Заклади охорони здоров`я")
                        );

            // 50
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(50, new NumberRow("3.9"), joinRow,
                            "Заклади соціального забезпечення")
                        );

            // 51
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(51, new NumberRow("3.10"), joinRow,
                            "Кредитно-фінансові заклади")
                        );

            // 52
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(52, new NumberRow("3.11"), joinRow,
                            "Заклади торгівлі")
                        );

            // 53
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(53, new NumberRow("3.12"), joinRow,
                            "Заклади громадського харчування")
                        );

            // 54
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(54, new NumberRow("3.13"), joinRow,
                            "Заклади побутового обслуговування")
                        );

            // 55
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(55, new NumberRow("3.14"), joinRow,
                            "Заклади комунального обслуговування")
                        );

            // 56
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(56, new NumberRow("3.15"), joinRow,
                            "Екстериторіальні організації та органи")
                        );

            // 57
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(57, new NumberRow("3.16"), joinRow,
                            "Житлово-експлуатаційні організації")
                        );

            // 58
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(58, new NumberRow("3.17"), joinRow,
                            "Інші заклади, установи, організації")
                        );

            // 59
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 60, 61, 62, 63, 64, 65 });

            rows.Add(new RowForm6Zem(59, new NumberRow("4"), joinRow,
                            "Промислові та інші підприємства (60+61+62+63+64+65)")
                        );

            // 60
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(60, new NumberRow("4.1"), joinRow,
                            "Підприємства добувної промисловості")
                        );

            // 61
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(61, new NumberRow("4.2"), joinRow,
                            "Металургійні підприємства та підприємства з обробки металу")
                        );

            // 62
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(62, new NumberRow("4.3"), joinRow,
                            "Підприємства з виробництва та розподілу електроенергії")
                        );

            // 63
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(63, new NumberRow("4.4"), joinRow,
                            "Підприємства з виробництва будівельних матеріалів")
                        );

            // 64
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(64, new NumberRow("4.5"), joinRow,
                            "Підприємства харчової промисловості та з перероблення сільськогосподарських продуктів")
                        );

            // 65
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(65, new NumberRow("4.6"), joinRow,
                            "Підприємства інших галузей промисловості")
                        );

            // 66
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 67, 68, 69, 70, 71, 72, 73, 74 });

            rows.Add(new RowForm6Zem(66, new NumberRow("5"), joinRow,
                            "Підприємства та організації транспорту, зв`язку (67+68+69+70+71+72+73+74)")
                        );

            // 67
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(67, new NumberRow("5.1"), joinRow,
                            "Залізничного транспорту")
                        );

            // 68
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(68, new NumberRow("5.2"), joinRow,
                            "Автомобільного транспорту")
                        );

            // 69
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(69, new NumberRow("5.3"), joinRow,
                            "Трубопровідного транспорту")
                        );

            // 70
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(70, new NumberRow("5.4"), joinRow,
                            "Морського транспорту")
                        );

            // 71
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(71, new NumberRow("5.5"), joinRow,
                            "Внутрішнього водного транспорту")
                        );

            // 72
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(72, new NumberRow("5.6"), joinRow,
                            "Повітряного транспорту")
                        );

            // 73
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(73, new NumberRow("5.7"), joinRow,
                            "Іншого транспорту")
                        );

            // 74
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(74, new NumberRow("5.8"), joinRow,
                            "Зв`язку")
                        );

            // 75
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 76, 77, 78, 79, 80, 81, 82 });

            rows.Add(new RowForm6Zem(75, new NumberRow("6"), joinRow,
                            "Частини, підприємства, організації, установи, навчальні заклади оборони (76+77+78+79+80+81+82)")
                        );

            // 76
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(76, new NumberRow("6.1"), joinRow,
                            "Міністерство оборони")
                        );

            // 77
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(77, new NumberRow("6.2"), joinRow,
                            "Міністерство внутрішніх справ")
                        );

            // 78
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(78, new NumberRow("6.3"), joinRow,
                            "Національна гвардія")
                        );

            // 79
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(79, new NumberRow("6.4"), joinRow,
                            "Державний комітет у справах охорони державного кордону")
                        );

            // 80
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(80, new NumberRow("6.5"), joinRow,
                            "Товариство сприяння обороні України")
                        );

            // 81
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(81, new NumberRow("6.6"), joinRow,
                            "Іноземні військові формування")
                        );

            // 82
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(82, new NumberRow("6.7"), joinRow,
                            "Інші військові формування")
                        );

            // 83
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 84, 85, 86, 87 });

            rows.Add(new RowForm6Zem(83, new NumberRow("7"), joinRow,
                            "Організації, підприємства і установи природоохоронного, оздоровчого, рекреаційного та історико-культурного призначення (84+85+86+87)")
                        );

            // 84
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(84, new NumberRow("7.1"), joinRow,
                            "Природоохоронного призначення")
                        );

            // 85
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(85, new NumberRow("7.2"), joinRow,
                            "Оздоровчого призначення")
                        );

            // 86
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(86, new NumberRow("7.3"), joinRow,
                            "Рекреаційного призначення")
                        );

            // 87
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(87, new NumberRow("7.4"), joinRow,
                            "Історико-культурного призначення")
                        );

            // 88
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(88, new NumberRow("8"), joinRow,
                            "Лісогосподарські підприємства")
                        );

            // 89
            joinRow = new List<int>();


            rows.Add(new RowForm6Zem(89, new NumberRow("8.1"), joinRow,
                            "у тому числі військові лісгоспи")
                        );

            // 90
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(90, new NumberRow("9"), joinRow,
                            "Водогосподарські підприємства")
                        );

            // 91
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(91, new NumberRow("10"), joinRow,
                            "Спільні підприємства, міжнародні об`єднання і організації  з участю українських, іноземних юридичних та фізичних осіб")
                        );

            // 92
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(92, new NumberRow("11"), joinRow,
                            "Підприємства, що повністю належать іноземним інвесторам")
                        );

            // 93
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 94, 95, 96, 97 });

            rows.Add(new RowForm6Zem(93, new NumberRow("12"), joinRow,
                            "Землі запасу та землі, не надані у власність та постійне користування в межах населених пунктів (які не надані у тимчасове користування) (94+95+96+97)")
                        );

            // 94
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(94, new NumberRow("12.1"), joinRow,
                            "Землі запасу")
                        );

            // 95
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(95, new NumberRow("12.2"), joinRow,
                            "Землі резервного фонду, не надані в постійне користування")
                        );

            // 96
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(96, new NumberRow("12.3"), joinRow,
                            "Землі, не надані у власність або постійне користування в межах населених пунктів")
                        );

            // 97
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(97, new NumberRow("12.4"), joinRow,
                            "Землі загального користування")
                        );

            // 98
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(98, new NumberRow("12.0"), joinRow,
                            "Крім того, землі запасу та землі, не надані у власність та постійне користування в межах населених пунктів (які надані у тимчасове користування)")
                        );

            // 99
            joinRow = new List<int>();
            joinRow.AddRange(new int[] { 01, 17, 41, 59, 66, 75, 83, 88, 90, 91, 92, 93 });

            rows.Add(new RowForm6Zem(99, new NumberRow("13"), joinRow,
                            "Всього земель в межах населених пунктів (01+17+41+59+66+75+83+88+90+91+92+93 = 103+104+105)")
                        );

            // 100
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(100, new NumberRow("13.1"), joinRow,
                            "землі за межами адміністративно-територіальних одиниць")
                        );

            // 101
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(101, new NumberRow("13.2"), joinRow,
                            "землі, які входять до інших адміністративно-територіальних одиниць")
                        );

            // 102
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(102, new NumberRow("13.3"), joinRow,
                            "землі, які входять до інших адміністративно-територіальних одиниць")
                        );

            // 103
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(103, new NumberRow("13.1"), joinRow,
                            "Землі міст")
                        );

            // 104
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(104, new NumberRow("13.2"), joinRow,
                            "Землі селищ")
                        );

            // 105
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(105, new NumberRow("13.3"), joinRow,
                            "Землі сільских населених пунктів")
                        );

            // 106
            joinRow = new List<int>();

            rows.Add(new RowForm6Zem(106, new NumberRow("13.0"), joinRow,
                            "Крім того, земель організацій, підприємств  та установ, адміністративно підпорядко-ваних сільській, селищній, міській радам - за межами населених пунктів")
                        );

            #endregion Rows

            #region Sections

            foreach (RowForm6Zem row in rows)
            {
                if (row.Code != 103 &&
                        row.Code != 104 &&
                        row.Code != 105 &&
                        row.Code != 106)
                {
                    form6Zem.FirstSection.Add(row);
                }
            }

            foreach (RowForm6Zem row in rows)
            {
                if (row.Code != 100 &&
                        row.Code != 101 &&
                        row.Code != 102)
                {
                    form6Zem.SecondSection.Add(row);
                }
            }

            #endregion Sections

            #region Cols

            List<ColForm6Zem> cols = new List<ColForm6Zem>();
            List<int> joinCol;

            #region Col_A
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(1001,
                        "Номер рядка",
                        joinCol,
                        ""
                    )
            );
            #endregion Col_A

            #region Col_B
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(1002,
                        "Власники землі,  землекористувачі та землі державної власності, не надані у власність або користування",
                        joinCol,
                        ""
                    )
            );
            #endregion Col_B

            #region Col_V
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(1003,
                        "Шифр рядка",
                        joinCol,
                        ""
                    )
            );
            #endregion Col_V

            #region Col_1
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(1, 
                        "кількість власників землі та землекористувачів",
                        joinCol,
                        ""
                    )
            );
            #endregion Col_1

            #region Col_2
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 3, 21, 34, 63, 66, 67, 72 });

            form6Zem.Cols.Add(new ColForm6Zem(2, 
                        "Загальна площа земель, всього (графи 3+21+34+63+66+67+72)",
                        joinCol,
                        ""
                    )
            );
            #endregion Col_2

            #region Col_3
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 4, 14, 1, 16, 17, 18, 20 });


            form6Zem.Cols.Add(new ColForm6Zem(3,
                    "всього (графи 4+14+15+16+17+18+20)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_3

            #region Col_4
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 5, 6, 7, 11, 12 });

            form6Zem.Cols.Add(new ColForm6Zem(4, 
                    "всього (графи 5+6+7+11+12)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_4

            #region Col_5
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(5,
                    "рілля",
                    joinCol,
                    ""
                )
            );
            #endregion Col_5

            #region Col_6
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(6, 
                    "перелоги",
                    joinCol,
                    ""
                )
            );
            #endregion Col_6

            #region Col_7
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 8, 9, 10 });

            form6Zem.Cols.Add(new ColForm6Zem(7,
                    "всього (графи 8+9+10)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_7

            #region Col_8
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(8,
                    "садів",
                    joinCol,
                    ""
                )
            );
            #endregion Col_8

            #region Col_9
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(9,
                    "виноградників",
                    joinCol,
                    ""
                )
            );
            #endregion Col_9

            #region Col_10
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(10,
                    "інших багаторічних насаджень",
                    joinCol,
                    ""
                )
            );
            #endregion Col_10

            #region Col_11
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(11, 
                    "сіножаті",
                    joinCol,
                    ""
                )
            );
            #endregion Col_11

            #region Col_12
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(12,
                    "всього",
                    joinCol,
                    ""
                )
            );
            #endregion Col_12

            #region Col_13
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(13,
                    "з усіх пасовищ гірські",
                    joinCol,
                    ""
                )
            );
            #endregion Col_13

            #region Col_14
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(14,
                    "під господарськими будівлями і дворами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_14

            #region Col_15
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(15,
                    "під господарськими шляхами і прогонами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_15

            #region Col_16
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(16,
                    "землі, які перебувають у стадії меліоративного будівництва та відновлення родючості",
                    joinCol,
                    ""
                )
            );
            #endregion Col_16

            #region Col_17
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(17,
                    "землі тимчасової консервації",
                    joinCol,
                    ""
                )
            );
            #endregion Col_17

            #region Col_18
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(18,
                    "всього",
                    joinCol,
                    ""
                )
            );
            #endregion Col_18

            #region Col_19
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(19,
                    "у тому числі техногенно забруднені, включаючи радіонуклідне",
                    joinCol,
                    ""
                )
            );
            #endregion Col_19

            #region Col_20
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(20,
                    "інші сільськогосподарські землі",
                    joinCol,
                    ""
                )
            );
            #endregion Col_20

            #region Col_21
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 22, 28 });

            form6Zem.Cols.Add(new ColForm6Zem(21,
                    "всього (графи  22+28)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_21

            #region Col_22
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 23, 26, 27 });

            form6Zem.Cols.Add(new ColForm6Zem(22,
                    "всього (графи  23+26+27)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_22

            #region Col_23
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(23,
                    "всього",
                    joinCol,
                    ""
                )
            );
            #endregion Col_23

            #region Col_24
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(24,
                    "полезахисних лісосмуг",
                    joinCol,
                    ""
                )
            );
            #endregion Col_24

            #region Col_25
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(25,
                    "інших захисних насаджень",
                    joinCol,
                    ""
                )
            );
            #endregion Col_25

            #region Col_26
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(26,
                    "не вкритих лісовою рослинністю",
                    joinCol,
                    ""
                )
            );
            #endregion Col_26

            #region Col_27
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(27,
                    "інші лісові землі",
                    joinCol,
                    ""
                )
            );
            #endregion Col_27

            #region Col_28
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(28,
                    "чагарники",
                    joinCol,
                    ""
                )
            );
            #endregion Col_28

            #region Col_29
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(29,
                    "група I",
                    joinCol,
                    ""
                )
            );
            #endregion Col_29

            #region Col_30
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(30,
                    "група II",
                    joinCol,
                    ""
                )
            );
            #endregion Col_30

            #region Col_31
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(31,
                    "для  виробництва  деревини",
                    joinCol,
                    ""
                )
            );
            #endregion Col_31

            #region Col_32
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(32,
                    "для захисної, природоохоронної та біологічної  мети",
                    joinCol,
                    ""
                )
            );
            #endregion Col_32

            #region Col_33
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(33,
                    "для відпочинку",
                    joinCol,
                    ""
                )
            );
            #endregion Col_33

            #region Col_34
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 35, 36, 37, 38, 42, 43, 44, 45, 50, 55 });

            form6Zem.Cols.Add(new ColForm6Zem(34,
                    "всього (граф  35+36+37+38+42+43+44+45+50+55)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_34

            #region Col_35
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(35,
                    "одно-  та двоповерховою житловою забудовою",
                    joinCol,
                    ""
                )
            );
            #endregion Col_35

            #region Col_36
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(36,
                    "житловою забудовою з трьома і більше поверхами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_36

            #region Col_37
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(37,
                    "землі промисловості",
                    joinCol,
                    ""
                )
            );
            #endregion Col_37

            #region Col_38
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 39, 40, 41 });

            form6Zem.Cols.Add(new ColForm6Zem(38,
                    "всього (графи 39+40+41)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_38

            #region Col_39
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(39,
                    "під  торфорозробками, які експлуатують",
                    joinCol,
                    ""
                )
            );
            #endregion Col_39

            #region Col_40
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(40,
                    "відкриті розробки та кар'єри, шахти, які експлуатують",
                    joinCol,
                    ""
                )
            );
            #endregion Col_40

            #region Col_41
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(41,
                    "інші землі (під відпрацьовані розробки та кар'єри; закриті шахти; відвали; терикони, які не експлуатують)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_41

            #region Col_42
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(42,
                    "землі,  які  використовуються  в комерційних цілях",
                    joinCol,
                    ""
                )
            );
            #endregion Col_42

            #region Col_43
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(43,
                    "землі  громадського призначення",
                    joinCol,
                    ""
                )
            );
            #endregion Col_43

            #region Col_44
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(44,
                    "землі змішаного використання",
                    joinCol,
                    ""
                )
            );
            #endregion Col_44

            #region Col_45
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 46, 47, 48, 49 });


            form6Zem.Cols.Add(new ColForm6Zem(45,
                    "всього (графи 46+47+48+49)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_45

            #region Col_46
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(46,
                    "під  дорогами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_46

            #region Col_47
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(47,
                    "під залізницями",
                    joinCol,
                    ""
                )
            );
            #endregion Col_47

            #region Col_48
            joinCol = new List<int>();



            form6Zem.Cols.Add(new ColForm6Zem(48,
                    "під аеропортами та відповідними спорудами",
                    joinCol,
                    ""
                )

            );
            #endregion Col_48

            #region Col_49
            joinCol = new List<int>();

            form6Zem.Cols.Add(new ColForm6Zem(49,
                    "інші землі",
                    joinCol,
                    ""
                )
            );
            #endregion Col_49

            #region Col_50
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 51, 52, 53, 54 });

            form6Zem.Cols.Add(new ColForm6Zem(50,
                    "всього (графи  51+52+53+54)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_50

            #region Col_51
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(51,
                    "для виделення відходів",
                    joinCol,
                    ""
                )
            );
            #endregion Col_51

            #region Col_52
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(52,
                    "для водозабеспечення та очищення стічних вод",
                    joinCol,
                    ""
                )
            );
            #endregion Col_52

            #region Col_53
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(53,
                    "для виробництва та розподілення електроенергії",
                    joinCol,
                    ""
                )
            );
            #endregion Col_53

            #region Col_54
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(54,
                    "інші землі",
                    joinCol,
                    ""
                )
            );
            #endregion Col_54

            #region Col_55
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 56, 57, 58, 59, 60, 61, 62 });

            form6Zem.Cols.Add(new ColForm6Zem(55,
                    "всього (графи 56+57+58+59+60+61+62)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_55

            #region Col_56
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(56,
                    "зелених  насаджень  загального користування",
                    joinCol,
                    ""
                )
            );
            #endregion Col_56

            #region Col_57
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(57,
                    "кемпінгів, будинків для відпочинку або для проведення відпусток",
                    joinCol,
                    ""
                )
            );
            #endregion Col_57

            #region Col_58
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(58,
                    "зайнятих поточним будівництвом",
                    joinCol,
                    ""
                )
            );
            #endregion Col_58

            #region Col_59
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(59,
                    "відведених під будівництво (будівництво на яких не розпочато)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_59

            #region Col_60
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(60,
                    "під гідротехнічними спорудами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_60

            #region Col_61
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(61,
                    "вулиць, набережних, площ",
                    joinCol,
                    ""
                )
            );
            #endregion Col_61

            #region Col_62
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(61,
                    "кладовищ",
                    joinCol,
                    ""
                )
            );
            #endregion Col_62

            #region Col_63
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 64, 65 });

            form6Zem.Cols.Add(new ColForm6Zem(63,
                    "болота, всього (графи  64+65)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_63

            #region Col_64
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(64,
                    "верхові",
                    joinCol,
                    ""
                )
            );
            #endregion Col_64

            #region Col_65
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(65,
                    "низинні",
                    joinCol,
                    ""
                )
            );
            #endregion Col_65

            #region Col_66
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(66,
                    "Сухі відкриті землі з особливим рослинним покривом",
                    joinCol,
                    ""
                )
            );
            #endregion Col_66

            #region Col_67
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 68, 69, 70, 71 });

            form6Zem.Cols.Add(new ColForm6Zem(67,
                    "всього (графи 68+69+70+71)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_67

            #region Col_68
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(68,
                    "кам'янисті місця",
                    joinCol,
                    ""
                )
            );
            #endregion Col_68

            #region Col_69
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(69,
                    "піски (включаючи  пляжі)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_69

            #region Col_70
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(70,
                    "яри",
                    joinCol,
                    ""
                )
            );
            #endregion Col_

            #region Col_71
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(71,
                    "інші",
                    joinCol,
                    ""
                )
            );
            #endregion Col_71

            #region Col_72
            joinCol = new List<int>();
            joinCol.AddRange(new int[] { 73, 74, 75, 76, 77 });

            form6Zem.Cols.Add(new ColForm6Zem(72,
                    "внутрішні води, всього (графи   73+74+75+76+77)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_72

            #region Col_73
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(73,
                    "природними водотоками (річками та струмками)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_

            #region Col_74
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(74,
                    "штучними водотоками (каналами, колекторами, канавами)",
                    joinCol,
                    ""
                )
            );
            #endregion Col_74

            #region Col_75
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(75,
                    "озерами, прибережними замкнутими водойомами, лиманами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_75

            #region Col_76
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(76,
                    "ставками",
                    joinCol,
                    ""
                )
            );
            #endregion Col_76

            #region Col_77
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(77,
                    "штучними водосховищами",
                    joinCol,
                    ""
                )
            );
            #endregion Col_77

            #region Col_78
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(78,
                    "природоохоронного призначення",
                    joinCol,
                    ""
                )
            );
            #endregion Col_78

            #region Col_79
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(79,
                    "оздоровчого призначення",
                    joinCol,
                    ""
                )
            );
            #endregion Col_79

            #region Col_80
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(80,
                    "рекреаційного призначення",
                    joinCol,
                    ""
                )
            );
            #endregion Col_80

            #region Col_81
            joinCol = new List<int>();
            //joinCol.AddRange(new int[] { });

            form6Zem.Cols.Add(new ColForm6Zem(81,
                    "історико-культурного призначення",
                    joinCol,
                    ""
                )
            );
            #endregion Col_81

            #endregion Cols

            #region BlocksTable

                BlockColForm6Zem[] block;

                for (int i = 0; i < 10; i++ )
                {
                    int allBlock;

                    block = new BlockColForm6Zem[1];
                    block[0] = null;

                    #region  Graga_1
                    if (i == 0)
                    {
                        allBlock = 1;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++) 
                        { 
                            block[iBlock] = new BlockColForm6Zem(); 
                        }
                        block[0].NumberCols.Add(1);
                    }  
                    #endregion Graga_1

                    #region  Graga_2
                    else if (i == 1)
                    {
                        allBlock = 1;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }
                        block[0].NumberCols.Add(2);
                    }  
                    #endregion Graga_2

                    #region  Graga_3-10
                    else if (i == 2)
                    {
                        allBlock = 6;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[5].Name = "у тому числі";
                        block[5].NumberCols.Add(8);
                        block[5].NumberCols.Add(9);
                        block[5].NumberCols.Add(10);

                        block[4].Name = "багаторічні насадження";
                        block[4].NumberCols.Add(7);
                        block[4].Blocks.Add(block[5]);

                        block[3].Name = "з них";
                        block[3].NumberCols.Add(5);
                        block[3].NumberCols.Add(6);
                        block[3].Blocks.Add(block[4]);

                        block[2].Name = "сільськогосподарські угіддя";
                        block[2].NumberCols.Add(4);
                        block[2].Blocks.Add(block[3]);

                        block[1].Name = "у тому числі";
                        block[1].Blocks.Add(block[2]);

                        block[0].Name = "Сільськогосподарські землі";
                        block[0].NumberCols.Add(3);
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_3-10

                    #region  Graga_11-20
                    else if (i == 3)
                    {
                        allBlock = 6;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[5].Name = "забруднені сільськогосподар- ські угіддя,які не використову- ються в сільскогосподарському виробництві";
                        block[5].NumberLinesTitle = 4;
                        block[5].NumberCols.Add(18);
                        block[5].NumberCols.Add(19);

                        block[4].Name = "пасовища";
                        block[4].NumberCols.Add(12);
                        block[4].NumberCols.Add(13);

                        block[3].Name = "з них";
                        block[3].NumberCols.Add(11);
                        block[3].Blocks.Add(block[4]);

                        block[2].Name = "сільськогосподарські угіддя";
                        block[2].Blocks.Add(block[3]);

                        block[1].Name = "у тому числі";
                        block[1].Blocks.Add(block[2]);
                        block[1].NumberCols.Add(14);
                        block[1].NumberCols.Add(15);
                        block[1].NumberCols.Add(16);
                        block[1].NumberCols.Add(17);
                        block[1].Blocks.Add(block[5]);
                        block[1].NumberCols.Add(20);

                        block[0].Name = "Сільськогосподарські землі";
                        block[0].Blocks.Add(block[1]);
                    }
                    #endregion Graga_11-20

                    #region  Graga_21-33
                    else if (i == 4)
                    {
                        allBlock = 9;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[8].Name = "у тому числі";
                        block[8].NumberCols.Add(24);
                        block[8].NumberCols.Add(25);

                        block[7].Name = "вкриті лісовою (деревною та чагарниковою) рослинністю";
                        block[7].NumberLinesTitle = 2;
                        block[7].NumberCols.Add(23);
                        block[7].Blocks.Add(block[8]);

                        block[6].Name = "з них";
                        block[6].Blocks.Add(block[7]);
                        block[6].NumberCols.Add(26);
                        block[6].NumberCols.Add(27);

                        block[5].Name = "лісові землі";
                        block[5].NumberCols.Add(22);
                        block[5].Blocks.Add(block[6]);

                        block[4].Name = " у тому числі";
                        block[4].Blocks.Add(block[5]);
                        block[4].NumberCols.Add(28);

                        block[3].Name = "групи лісів";
                        block[3].NumberLinesTitle = 2;
                        block[3].NumberCols.Add(29);
                        block[3].NumberCols.Add(30);

                        block[2].Name = "з основною визнаною функцією використання";
                        block[2].NumberLinesTitle = 2;
                        block[2].NumberCols.Add(31);
                        block[2].NumberCols.Add(32);
                        block[2].NumberCols.Add(33);

                        block[1].Name = "з усіх лісів та  інших лісовкритих площ";
                        block[1].NumberLinesTitle = 2;
                        block[1].Blocks.Add(block[3]);
                        block[1].Blocks.Add(block[2]);

                        block[0].Name = "Ліси та інші лісовкриті площі";
                        block[0].NumberCols.Add(21);
                        block[0].Blocks.Add(block[1]);
                        block[0].Blocks.Add(block[4]);

                    }
                    #endregion Graga_21-33

                    #region  Graga_34-44
                    else if (i == 5)
                    {
                        allBlock = 5;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[4].Name = "у тому числі";
                        block[4].NumberCols.Add(39);
                        block[4].NumberCols.Add(40);
                        block[4].NumberCols.Add(41);

                        block[3].Name = "землі під відкритими розробками, кар`єрами, шахтами та відповідними спорудами";
                        block[3].NumberLinesTitle = 2;
                        block[3].NumberCols.Add(38);
                        block[3].Blocks.Add(block[4]);
                        
                        block[2].Name = "під житловою забудовою";
                        block[2].NumberLinesTitle = 2;
                        block[2].NumberCols.Add(35);
                        block[2].NumberCols.Add(36);

                        block[1].Name = "у тому числі";
                        block[1].Blocks.Add(block[2]);
                        block[1].NumberCols.Add(37);
                        block[1].Blocks.Add(block[3]);
                        block[1].NumberCols.Add(42);
                        block[1].NumberCols.Add(43);
                        block[1].NumberCols.Add(44);

                        block[0].Name = "Забудовані  землі";
                        block[0].NumberCols.Add(34);
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_34-44

                    #region  Graga_45-54
                    else if (i == 6)
                    {
                        allBlock = 6;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[5].Name = "у тому числі";
                        block[5].NumberCols.Add(51);
                        block[5].NumberCols.Add(52);
                        block[5].NumberCols.Add(53);
                        block[5].NumberCols.Add(54);

                        block[4].Name = "землі, які використовуються для технічної інфраструктури";
                        block[4].NumberCols.Add(50);
                        block[4].Blocks.Add(block[5]);

                        block[3].Name = "у тому числі";
                        block[3].NumberCols.Add(46);
                        block[3].NumberCols.Add(47);
                        block[3].NumberCols.Add(48);
                        block[3].NumberCols.Add(49);

                        block[2].Name = "землі, які використовуються для транспорту та зв`язку";
                        block[2].NumberCols.Add(45);
                        block[2].Blocks.Add(block[3]);

                        block[1].Name = "у тому числі";
                        block[1].Blocks.Add(block[2]);
                        block[1].Blocks.Add(block[4]);

                        block[0].Name = "Забудовані  землі";
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_45-54

                    #region  Graga_55-62
                    else if (i == 7)
                    {
                        allBlock = 4;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[3].Name = "у тому числі";
                        block[3].NumberCols.Add(56);
                        block[3].NumberCols.Add(57);
                        block[3].NumberCols.Add(58);
                        block[3].NumberCols.Add(59);
                        block[3].NumberCols.Add(60);
                        block[3].NumberCols.Add(61);
                        block[3].NumberCols.Add(62);

                        block[2].Name = "землі, які використовуються для відпочинку та інші відкриті землі";
                        block[2].NumberCols.Add(55);
                        block[2].Blocks.Add(block[3]);

                        block[1].Name = "у тому числі";
                        block[1].Blocks.Add(block[2]);

                        block[0].Name = "Забудовані  землі";
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_55-62

                    #region  Graga_63-65
                    else if (i == 8)
                    {
                        allBlock = 2;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[1].Name = "у тому числі";
                        block[1].NumberCols.Add(64);
                        block[1].NumberCols.Add(65);

                        block[0].Name = "Відкриті заболочені землі";
                        block[0].NumberCols.Add(63);
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_63-65

                    #region  Graga_66
                    else if (i == 9)
                    {
                        allBlock = 1;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }
                        block[0].NumberCols.Add(66);
                    }
                    #endregion Graga_66

                    #region  Graga_67-71
                    else if (i == 10)
                    {
                        allBlock = 2;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[1].Name = "у тому числі";
                        block[1].NumberCols.Add(68);
                        block[1].NumberCols.Add(69);
                        block[1].NumberCols.Add(70);
                        block[1].NumberCols.Add(71);

                        block[0].Name = "Відкриті землі без рослинного покриву або з незначним рослинним покривом";
                        block[0].NumberLinesTitle = 2;
                        block[0].NumberCols.Add(67);
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_67-71

                    #region  Graga_72-77
                    else if (i == 11)
                    {
                        allBlock = 2;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[1].Name = "у тому числі під";
                        block[1].NumberCols.Add(73);
                        block[1].NumberCols.Add(74);
                        block[1].NumberCols.Add(75);
                        block[1].NumberCols.Add(76);
                        block[1].NumberCols.Add(77);

                        block[0].Name = "Води";
                        block[0].NumberCols.Add(72);
                        block[0].Blocks.Add(block[1]);

                    }
                    #endregion Graga_72-77

                    #region  Graga_78-81
                    else if (i == 12)
                    {
                        allBlock = 2;
                        block = new BlockColForm6Zem[allBlock];
                        for (int iBlock = 0; iBlock < allBlock; iBlock++)
                        {
                            block[iBlock] = new BlockColForm6Zem();
                        }

                        block[0].Name = "З усіх земель";
                        block[0].NumberCols.Add(78);
                        block[0].NumberCols.Add(79);
                        block[0].NumberCols.Add(80);
                        block[0].NumberCols.Add(81);
                    }
                    #endregion Graga_78-81

                    form6Zem.Blocks.Add(block[0]);
                }
            #endregion BlocksTable

            return form6Zem;
        }

        public static Form6Zem GetFormForParcel(LandParcel parcel)
        {
            Form6Zem form6Zem = GetBaseForm();

            foreach (LandPolygon poligon in parcel.Lands)
            {
                int numberCol = Convert.ToInt32(poligon.FindInfo("CZG").Value);
                int codeRow = Convert.ToInt32(poligon.FindInfo("CZR").Value);

                ColForm6Zem col = FindCol(form6Zem.Cols, numberCol);
                RowForm6Zem row = FindRow(form6Zem.FirstSection, codeRow);
            }

            return form6Zem;
        }

        public static RowForm6Zem FindRow(List<RowForm6Zem> list, int code)
        {
            return list.Find
                (
                    delegate(RowForm6Zem row)
                    {
                        return row.Code == code;
                    }
                );
        }

        public static ColForm6Zem FindCol(List<ColForm6Zem> list, int number)
        {
            return list.Find
                (
                    delegate(ColForm6Zem col)
                    {
                        return col.Number == number;
                    }
                );
        }
    }
}
