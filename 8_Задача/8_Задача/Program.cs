using System;
using System.IO;

namespace Graph_Color
{
    class Program
    {
        public struct Graf
        {
            public int stepeni;//Степень
            public int color;//"Цвет" - цифра
            public int number;//Номер
        };

        public static void Fill(Graf[] mass, int n) // инициализация
        {
            for (int s = 0; s < n; s++)
                mass[s].color = -1;
        }
      
        public static void PrintGraph(Graf[] mass)//Выводит результат
        {
            for (int s = 0; s < mass.Length; s++)
                Console.WriteLine(mass[s].number + " -- " + mass[s].color);//Выводит - вершина и её цвет
        }

        public static void PrintMas(int[,] p, int n) // Выводит - массив
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(p[i, j]+" ");
                }
                Console.WriteLine();
            }
        }

        //Возвращает ноль, если все цвета уже присвоены
        public static bool NotAllinColor(Graf[] mass)
        {
            int flag = 0;
            for (int x = 0; x < mass.Length; x++) //Цикл проверяет - все ли цвета посещены
            {
                if ((mass[x].color <= mass.Length) & (mass[x].color != -1))
                    flag++;
            }

            if (flag == mass.Length)
                return false;
            else
                return true;
        }

        static void Main(string[] args)
        {
            int n = 0;// Количество вершин в графе
            int k = 0;
            int versh = 0;

            StreamReader sr = new StreamReader("input.txt");
            string line = sr.ReadLine();
            string[] mas = line.Split(' ');
            n = Convert.ToInt32(mas[0]);
            k = Convert.ToInt32(mas[1]);
            int[,] m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = 0;
                }
            };

            //Для таблицы смежности 4 на 4 = >Максимальное число цветов равно количеству вершин	
            int[] colors = new int[n];// 
            for (int i = 0; i < n; i++)
            {
                colors[i] = i + 1;
            }

            do
            {
                line = sr.ReadLine();
                mas = line.Split(' ');
                int i = Convert.ToInt32(mas[0]) - 1;
                int j = Convert.ToInt32(mas[1]) - 1;
                m[i, j] = 1;
                m[j, i] = 1;
            }
            while (!sr.EndOfStream);
            sr.Close();

            // Создадим графф
            Graf[] graf = new Graf[n];//Граф

            Fill(graf, n);//Забиваем все цвета по умолчанию - -1

            //Находим степени вершин по матрице смежности
            for (int i = 0; i < n; i++, versh++)
            {
                graf[versh].stepeni = 0;
                graf[versh].number = i;

                for (int j = 0; j < n; j++)
                {
                    if (m[i, j] == 1)
                        graf[versh].stepeni++;
                }
            }

            //Сортировка структуры графа по степеням с наибольшей по наименьшую
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (graf[i].stepeni > graf[j].stepeni)
                    {
                        Graf temp = graf[i];
                        graf[i] = graf[j];
                        graf[j] = temp;
                    }
                }

            //Алгоритм по присвоению цветов 
            int t = 0, flag, metka = 0;

            for (int x = 0; NotAllinColor(graf); x++)//Цикл по цветам. Условие выхода - все цвета окрашены! Пока не раскрашено( НеВсёЗакрашено) - работает
            {
                if (graf[x].color == -1)
                    graf[x].color = colors[x];//Присваиваем первый цвет

                // продолжаем дальше;
                for (int i = 0; i < n; i++)//цикл, чтобы не только одни вершины раскрасить, а ВСЕ НЕ смежные
                {
                    t = graf[i].number;//Запоминаем номер исследуемой вершины
                    flag = 0;//Обнуляем Флаг - считает количество вершин, не раскрашенных как исследуемая
                    for (int j = 0; j < n; j++)//Идем по строке матрицы по номеру
                    {
                        //Блок условий проверки на возможность "раскраски" вершины. Под раскраской - подразумевается присваивание номера
                        if ((m[t, j] == 1 && t != j))//Если 1 - можно перейти на какое-то ребро и это ребро - не та же самая вершина(которая рассматривается
                        {
                            for (int s = 0; s < n; s++)
                                if (j == graf[s].number && graf[s].color != colors[x])
                                {
                                    flag++;
                                }
                        };

                        if (graf[j].number == t)
                            metka = j;

                        if (flag == graf[metka].stepeni && graf[metka].color == -1)//Если вершина ещё не посещена - забиваем в неё определенный цвет из массива цветов
                            graf[metka].color = colors[x];
                    }
                }
            }
            PrintMas(m, n);
            Console.WriteLine();
            Console.WriteLine("Результат раскраски - второй столбик. Первый столбик - номер вершины");

            //Показываем результат
            PrintGraph(graf);

            Console.ReadKey();
        }
    }
}
