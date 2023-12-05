using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;

namespace Дискретная_математика___Классы_функций
{
    internal class Program
    {
//Обработка меню

        void SpisokMenu(ref string[] mas)
        {
            for (int i = 0; i < mas.Length; i++)
            {
                Console.WriteLine((i + 1) + ") " + mas[i]);
            }
        }

        int SelectMenu(int length)
        {
            bool isChek = false;
            int element = 0;
            string str;
            do
            {
                str = Console.ReadLine();

                Int32.TryParse(str, out element);

                isChek = element > 0 && element < length + 1;
            } while (!isChek);

            return element;
        }
        //Конец обработки меню

//Работа с вводом
        string Assembling(string str)
        {
            string vector = "";
            Regex regex = new Regex(@"\w+");

            MatchCollection matches = regex.Matches(str);

            foreach (Match match in matches)
            {
                vector += match.Value;
            }

            return vector;
        }

        void InputVector(ref string[] function)
        {
            string func;

            Console.WriteLine("Введите количество функций");
            int str;
            Int32.TryParse(Console.ReadLine(), out str);

            function = new string[str];
            for(int i = 0; i < str; i++)
            {
                Console.WriteLine("Введите "+(i+1)+" функцию");

                func = Console.ReadLine();

                func = Assembling(func);

                function[i] = func;
            }
        }

        void InputFileVector(ref string[] function)
        {
            StreamReader file = new StreamReader("D:\\Мои проекты Vethyal studia\\Дискретная математика - Классы функций\\Function.txt");

            string str = "";

            function = new string[System.IO.File.ReadAllLines("D:\\Мои проекты Vethyal studia\\Дискретная математика - Классы функций\\Function.txt").Length];

            for(int i = 0; i<function.Length; i++)
            {
                str = file.ReadLine();

                str = Assembling(str);

                function[i] = str;
            }
        }

        void MenuInpyt(ref string[] function)
        {
            bool isEnd = false;

            string[] mas = new string[3];

            mas[0] = "Ввод функций в ручную";
            mas[1] = "Ввод функций из файла";
            mas[2] = "Отмена работы";

            do
            {
                SpisokMenu(ref mas);
                switch(SelectMenu(mas.Length)) 
                {
                    case 1:
                        {
                            InputVector(ref function);
                            break;
                        }
                    case 2:
                        {
                            InputFileVector(ref function);
                            break;
                        }
                }
                isEnd = true;
            } while (!isEnd);
        }
//Конец работы с вводом

//Работа с классами
        //Манатонность функции
        bool Manatonic(string function, string[][] tree)
        {
            bool isM = true;

            int a = 1, b = 0, c = 0;

            int count = 0;

            while(a < tree.Length && isM)
            {
                while(b < tree[a - 1].Length && isM)
                {
                    while(c < tree[a].Length && isM)
                    {
                        for(int i = 0; i < tree[a][c].Length;i++)
                        {
                            if (tree[a-1][b][i] != tree[a][c][i])
                            {
                                count++;
                            }
                        }

                        if(count == 1)
                        {
                            char One = function[Convert.ToInt32(tree[a - 1][b], 2)];
                            char Two = function[Convert.ToInt32(tree[a][c], 2)];

                            if (One > Two)
                            {
                                isM = false;
                            }
                        }

                        c++;
                        count = 0;
                    }

                    b++;
                    c = 0;
                }

                a++;
                b = 0;
            }

            return isM;
        }

        bool FunctionManatonic(string function, ref string[][] tree)
        {
            bool isM = true;

            int n = 1;
            int N = 2;

            while ( N != function.Length)
            {
                n++;
                N *= 2;
            }

            string[] tableTrue = new string[function.Length];

            for (int i = 0; i < function.Length; i++)
            {
                string str = "";

                string strTable = "";

                str = Convert.ToString(i, 2);
                for (int j = 0; j < n - str.Length; j++)
                {
                    strTable += '0';
                }

                for (int j = 0; j < str.Length; j++)
                {
                    strTable += str[j];
                }

                tableTrue[i] = strTable;
            }

            int[] counter = new int[n+1];

            foreach (string strTable in tableTrue)
            {
                int count = 0;

                for(int i = 0; i < strTable.Length; i++)
                {
                    if (strTable[i] == '1')
                    {
                        count++;
                    }
                }

                counter[count]++;
            }

            tree = new string[n + 1][];

            for(int i = 0; i < counter.Length; i++)
            {
                tree[i] = new string[counter[i]]; 
            }

            foreach(string strTable in tableTrue)
            {
                int count = 0;

                for(int i = 0; i < strTable.Length; i++)
                {
                    if (strTable[i] == '1')
                    {
                        count++;
                    }
                }

                tree[count][counter[count] - 1] = strTable;

                counter[count]--;
            }

            isM = Manatonic(function, tree);

            return isM;
        }

        //Линейная
        bool FunctionLinearity(string function, string[][] tree)
        {
            string str;

            bool isL = true;

            char[][] polinomJigal = new char[tree.Length][];

            for(int i = 0; i < tree.Length; i++)
            {
                polinomJigal[i] = new char[tree[i].Length];
            }

            polinomJigal[0][0] = function[0];

            int a = 1, b = 0, c = 0, d = 0;

            int count = 0;

            while(a < polinomJigal.Length)
            {
                str = "" + polinomJigal[0][0];

                while(b < polinomJigal[a].Length)
                {
                    string element = tree[a][b];

                    c = a - 1;

                    while(c > -1)
                    {
                        while(d < polinomJigal[c].Length)
                        {
                            string chek = tree[c][d];

                            for(int i = 0; i < chek.Length; i++)
                            {
                                if (chek[i] == '1' && element[i] == '0')
                                {
                                    count++;
                                }
                            }

                            if(count == 0)
                            {
                                str += polinomJigal[c][d];
                            }

                            count = 0;
                            d++;
                        }

                        d = 0;
                        c--;
                    }

                    for(int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '1') 
                        {
                            count++;
                        }
                    }

                    if (count % 2 == 0)
                    {
                        str = "" + '0';
                    }
                    else
                    {
                        str = "" + '1';
                    }

                    if('0' == function[Convert.ToInt32(tree[a][b], 2)])
                    {
                        polinomJigal[a][b] = str[0];
                    }
                    else
                    {
                        if (str[0] == '1')
                        {
                            polinomJigal[a][b] = '0';
                        }
                        else
                        {
                            polinomJigal[a][b] = '1';
                        }
                    }

                    b++;
                }

                a++;
                b = 0;
            }

            a = 1;

            while (a < polinomJigal.Length && isL)
            {
                while(b < polinomJigal[a].Length && isL)
                {
                    if(a > 1)
                    {
                        if (polinomJigal[a][b] == '1')
                        {
                            isL = false;
                        }
                    }
                    b++;
                }
                a++;
                b = 0;
            }

            return isL;
        }

        //Запуск общёта классов
        void CalculationClasses(string function, ref bool[] functionClass)
        {
            if(function.Length != 1)
            {
                string[][] tree = new string[0][];

                bool isS = true;
                bool isM = true;
                bool isL = true;

                if (function[0] == '0')
                {
                    functionClass[0] = true;
                }

                if (function[function.Length - 1] == '1')
                {
                    functionClass[1] = true;
                }

                int n = 0;

                while((n < function.Length/2+1) && isS) 
                {
                    if (function[n] == function[function.Length-1-n])
                    {
                        isS = false;
                    }
                    n++;
                }

                if (isS)
                {
                    functionClass[2] = true;
                }

                isM = FunctionManatonic(function, ref tree);
                functionClass[3] = isM;

                isL = FunctionLinearity(function, tree);
                functionClass[4] = isL;
            }
            else
            {
                if (function == "0")
                {
                    functionClass[0] = true;
                }
                else
                {
                    functionClass[1] = true;
                }

                functionClass[3] = true;
                functionClass[4] = true;
            }
        }
//Конец работы с классами

        //Полнота функции
        void CompletenessFunction(string[] function, bool[,] functionClass)
        {
            bool isCompleteness = true;


            Console.Write('\t'+"|");
            Console.Write("T0" + "\t|");
            Console.Write("T1" + "\t|");
            Console.Write("S" + "\t|");
            Console.Write("M" + "\t|");
            Console.Write("L" + "\t|");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            for (int i = 0; i < function.Length; i++)
            {
                Console.Write(function[i]+"|");
                for(int j = 0; j < functionClass.GetLength(1); j++)
                {
                    Console.Write(functionClass[i, j] + "\t|");
                }
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------");
            }

            int[] completeness = {-1, -1, -1, -1, -1};

            for(int i = 0; i < functionClass.GetLength(0); i++)
            {
                for(int j = 0;j < functionClass.GetLength(1);j++)
                {
                    if(functionClass[i, j])
                    {
                        if (completeness[j] == -1)
                        {
                            completeness[j] = 1;
                        }
                    }
                    else
                    {
                        completeness[j] = 0;
                    }
                }
            }

            for(int i = 0; i < completeness.Length; i++)
            {
                if (completeness[i] == 1)
                {
                    isCompleteness = false;
                }
            }

            if(isCompleteness ) 
            {
                Console.WriteLine("Система функций полная");
            }
            else
            {
                Console.WriteLine("Система функций неполная");
            }
        }

        void ClassFunction(string[] function, bool[,] functionClass)
        {
            bool[] temp = new bool[5];

            for(int i = 0; i < function.Length;i++) 
            {
                CalculationClasses(function[i], ref temp);

                for(int j = 0; j < 5; j++)
                {
                    functionClass[i, j] = temp[j];
                    temp[j] = false;
                }
            }

            CompletenessFunction(function, functionClass);
        }

        //Меню
        void Menu()
        {
            int lengthClass = 0;

            string[] function = new string[0];
            bool[,] functionClass = new bool[0, 0];

            bool isEnd = false;

            string[] mas = new string[3];

            mas[0] = "Ввод функций";
            mas[1] = "Расчёт полноты системы";
            mas[2] = "Завершить работу";

            do
            {
                SpisokMenu(ref mas);
                switch(SelectMenu(mas.Length)) 
                {
                    case 1:
                        {
                            MenuInpyt(ref function);
                            functionClass = new bool[function.Length, 5];
                            break;
                        }
                    case 2:
                        {
                            ClassFunction(function, functionClass);
                            break;
                        }
                    case 3:
                        {
                            isEnd = true;
                            break;
                        }
                }
            }while (!isEnd);
        }

        static void Main(string[] args)
        {
            Program main = new Program();

            main.Menu();
        }
    }
}