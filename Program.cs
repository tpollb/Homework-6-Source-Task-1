using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;

namespace Homework_6_Source_Task_1
{
    class Program
    {
        /// <summary>
        /// метод, определяющий свойсво делимости
        /// </summary>
        /// <param name="number">числа</param>
        /// <returns></returns>
        public static List<List<int>> GroupsNumbers(int number)
        {
            List<List<int>> groups = new List<List<int>>();
            
            //проверка входных данных
            if (number == 0)
            {
                return groups;
            }
                
            groups.Add(new List<int>() { 1 });

            if (number == 1)
            {
                return groups;
            }

            //создаём список чисел Range(генерирует последовательность целых чисел) диапозоном (от 2х до количества чисел - 1)
            List<int> numbers = Enumerable.Range(2, number - 1).ToList();

            //пока есть хотябы 1 элемент в списке
            while (numbers.Any())
            {
                //создаём список групп
                List<int> group = numbers.ToList();

                //перебираем элементы группы
                for (int i = 0; i < group.Count; i++)
                {
                    group.RemoveAll(x => x != group[i] && x % group[i] == 0);
                }
                    
                groups.Add(group);
                numbers.RemoveAll(x => group.Contains(x));
            }
            return groups;
        }

        /// <summary>
        /// метод считывающий число из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public int GetNumber(string path)
        {
            int result;
            bool flag;

            flag = int.TryParse(File.ReadLines(path).First(), out result);
            if (flag == false)
            {
                return 0;
            }

            return result;
        }
        /// <summary>
        /// Метод записи строки в файл
        /// </summary>
        /// <param name="filepath">путь к файлу</param>
        /// <param name="Source">строка для записи</param>
        static public void WriteFile(string filepath, string Source)
        {
            using (StreamWriter sw = new StreamWriter(filepath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine($"{Source}");
                sw.Close();
            }
        }
        /// <summary>
        /// Метод архивации файла
        /// </summary>
        /// <param name="OutFile">Путь до файла, который заархивировать</param>
        /// <param name="CompressedFileName">Путь до файла вывода</param>
        public static void CompressFile(string OutFile, string CompressedFileName)
        {
            using (FileStream ss = new FileStream(OutFile, FileMode.OpenOrCreate))
            {
                using (FileStream ts = File.Create(CompressedFileName))
                {
                    using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                    {
                        ss.CopyTo(cs);
                        cs.Close();
                    }
                    ts.Close();
                }
                ss.Close();
            }
        }
        static void Main(string[] args)
        {
            bool flag = default;
            int i = 1;
            string input;
            int FirstInput = 3;
            string FilePath = @"db.txt";
            string OutFileName = @"out.txt";
            string OutDirectory = "0";
            char key;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Домашнее задание 6. Задача из исходников.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cвойства делимости целых чисел.");
            Console.ResetColor();

            //выбираем режим работы программы
            while (flag == false || FirstInput < 1 || FirstInput > 2)
            {
                Console.WriteLine("Введите 1, если хотите вывести на экран только количество групп\nВведите 2, если хотите получить заполненные группы и записать их в файл.");
                input = Console.ReadLine();
                flag = int.TryParse(input, out FirstInput);
            }

            //режим 1
            if (FirstInput == 1)
            {
                //проверка существования файла
                /*
                while (!File.Exists(FilePath) || GetNumber(FilePath) == 0)
                {
                    Console.WriteLine(@"Введите путь до файла в формате C:\Dir\Subdir\file.txt Файл должен содержать только число в начале первой строки");
                    FilePath = Console.ReadLine();
                }
                */

                //начало замера время выполнения 
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Console.WriteLine($"Количество групп: {(int)Math.Log(GetNumber(FilePath), 2) + 1}");

                //конец замера время выполнения 
                stopWatch.Stop();
                Console.WriteLine($"Время выполнения: {stopWatch.ElapsedMilliseconds / 1000} секунд {stopWatch.ElapsedMilliseconds} миллисекунд");
            } else //режим 2
            {
                //проверка существования директории
                /*
                while (!Directory.Exists(OutDirectory))
                {
                    Console.WriteLine(@"Введите путь до директории вывода в формате C:\Dir\Subdir\");
                    OutDirectory = Console.ReadLine();
                }

                while (!File.Exists(FilePath) || GetNumber(FilePath) == 0)
                {
                    Console.WriteLine(@"Введите имя файла в формате file.txt Файл должен содержать только число в начале первой строки");
                    FilePath = $"{OutDirectory}{Console.ReadLine()}";
                }
                */

                //начало замера время выполнения 
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string OutFile = $"{OutFileName}";
                WriteFile(OutFile, string.Join("\r\n", GroupsNumbers(GetNumber(FilePath)).Select(gr => string.Join(", ", gr))));

                //конец замера время выполнения 
                stopWatch.Stop();
                Console.WriteLine($"Время выполнения: {stopWatch.ElapsedMilliseconds / 1000} секунд {stopWatch.ElapsedMilliseconds} миллисекунд");

                Console.WriteLine("Хотите заархивировать полученные данные? y/n");
                key = Console.ReadKey(true).KeyChar;
                if (char.ToLower(key) == 'y')
                {
                    //начало замера время выполнения 
                    stopWatch.Start();

                    string compressed = $"out.zip";
                    CompressFile(OutFile, compressed);

                    stopWatch.Stop();
                    Console.WriteLine($"Время выполнения: {stopWatch.ElapsedMilliseconds / 1000} секунд {stopWatch.ElapsedMilliseconds} миллисекунд");
                }
            }
            Console.ReadKey();
        }
    }
}
