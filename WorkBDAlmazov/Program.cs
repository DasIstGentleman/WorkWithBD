using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using CsvHelper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;


namespace SoloLearnAlmazov
{
    
    class Program
    {
        
        public class Parameters 
        {
            // Параметры БД Деператаменты
            public string[] IdOfDepartment { get; set; }
            public string[] NameofDepartment { get; set; }

            // Параметры БД Сотрудники
            public string[] id { get; set; }
          
            public string[] department_id { get; set; }
           
            public string[] chief_id { get; set; }
           
            public string[] name { get; set; }
        
            public string[] salary { get; set; }
            public int COUNTDEPARTMENT { get; set; }

        }
        public class Metods : Parameters
        {
            // Метод, определяющий максимальную зарплату сотрудника и индекс департамента (для дальнейшего определения имени департамента из БД Департаментов)
            public void DepMaxSalary(Parameters p)
            {
               // Console.WriteLine(p.salary.Length);

                int[] x = new int[p.salary.Length];
                for (int i = 0; i < p.salary.Length; i++)
                { 
                x[i] = Convert.ToInt32(p.salary[i]);
                }
                Console.WriteLine("Максимальная зарплата среди всех сотрудников: " + x.Max());
                var indexSalary = Array.IndexOf(x, x.Max());
                var CountDepartament = p.department_id[indexSalary];
                Console.WriteLine("Департамент Сотрудника с наибольшей зарплатой по БД сотрудников : " + CountDepartament);

                //Подключаем парсер для работы с БД Департаменты
                using (TextFieldParser parser2 = new TextFieldParser(@"C:\БД Департаменты.csv"))
                {

                    parser2.TextFieldType = FieldType.Delimited;
                    parser2.SetDelimiters(";");
                    Parameters p2 = new Parameters();

                    int i = 0;
                    string[] IDOFDEPARTMENT = new string[4];
                    string[] NAMEOFDEPARTMENT = new string[4];
                    while (!parser2.EndOfData)
                    {
                        Parameters param = new Parameters();

                        string[] fields = parser2.ReadFields();
                        if (i != 0)
                        {
                            IDOFDEPARTMENT[i] = fields[0];
                            NAMEOFDEPARTMENT[i] = fields[1];

                        }
                        i = i + 1;

                    }
                    p2.IdOfDepartment = IDOFDEPARTMENT;
                    p2.NameofDepartment = NAMEOFDEPARTMENT;

                    // перадаем в метод DepMaxSalary2 параметры индекса департамента и массивы IdOfDepartment, NameofDepartment
                    DepMaxSalary2(CountDepartament, p2);
                    
                }

            }
            // Метод, определяющий название департамента на основе индекса департамента
            public void DepMaxSalary2(string CountDepartment,Parameters p2)
            {
                
                int[] y = new int[p2.IdOfDepartment.Length];

                for (int i = 0; i < p2.IdOfDepartment.Length; i++)
                {
                    y[i] = Convert.ToInt32(p2.IdOfDepartment[i]);
                }
                var BdDepartmentID = Array.IndexOf(y,Convert.ToInt32(CountDepartment));
                var BdDepartmentName = p2.NameofDepartment[BdDepartmentID];
                Console.WriteLine("Департамент сотрудника с наибольшей зарплатой по БД Департаментов:  " + BdDepartmentName);
                
            }
            // Метод, реализующий определение суммы зарплат по департаментам
            public void SumSalary(Parameters p )
            {
                //Заранее создаём массив ID руководителей департамента, чтобы в дальнейшем вычислить их зарплату
                int CountOfDep = Convert.ToInt32(p.department_id.Max());
                int[] AllIDChifsOfDep = new int[CountOfDep];
                int queue = 0;

                int[] y = new int[p.department_id.Length];
                for (int i = 0; i < p.department_id.Length; i++)
                {
                    y[i] = Convert.ToInt32(p.department_id[i]);
                }

                
                int indexvalue = 0;
                // Вычисление длины массива индексов, принадлежащих департаменту 1 
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 1 & p.chief_id[i] != "null")
                    {
                        indexvalue++;
                    }
                }
                int[] AllindexIdofOneDepartment = new int [indexvalue];  //= Array.FindAll(y,id =>.Equals("1"));
                indexvalue = 0;
                // Запись индексов в массив
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 1 & p.chief_id[i] != "null")
                    {
                        AllindexIdofOneDepartment[indexvalue] = i;
                        indexvalue++;
                    }
                
                }
                
                // Вычисляем индекс руководителя департамента 1
                string IdChief;
                IdChief = p.chief_id[AllindexIdofOneDepartment[0]];
                Console.WriteLine("Ид рук деп 1:" + IdChief);

                AllIDChifsOfDep[queue] =Convert.ToInt32(IdChief);
                queue++;
                

                //Вычисляем сумму зарплат без руководителей в департаменте 1
                int[] x = new int[p.salary.Length];
                for (int i = 0; i < p.salary.Length; i++)
                {
                    x[i] = Convert.ToInt32(p.salary[i]);
                }
                int SumSalaryOfDep = 0;
                foreach (int salary in AllindexIdofOneDepartment)
                {
                    SumSalaryOfDep = SumSalaryOfDep + x[salary];
                }
                Console.WriteLine("Сумма зарплат департамента 1 без руководителей = " + SumSalaryOfDep );

                //Вычисляем сумму зарплат с руководителем в департаменте 1
                SumSalaryOfDep = SumSalaryOfDep + x[Convert.ToInt32(IdChief)];
                    Console.WriteLine("Сумма зарплат департамента 1 c руководителем = " + SumSalaryOfDep);

                // Вычисление длины массива индексов, принадлежащих департаменту 2
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 2 & p.chief_id[i] != "null")
                    {
                        indexvalue++;
                    }
                }
                AllindexIdofOneDepartment = new int[indexvalue];  //= Array.FindAll(y,id =>.Equals("1"));
                indexvalue = 0;
                // Запись индексов в массив
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 2 & p.chief_id[i] != "null")
                    {
                        AllindexIdofOneDepartment[indexvalue] = i;
                        indexvalue++;
                    }

                }
                // Вычисляем индекс руководителя департамента 2
                IdChief = p.chief_id[AllindexIdofOneDepartment[0]];
                Console.WriteLine("Ид рук деп 2:" + IdChief);
                AllIDChifsOfDep[queue] = Convert.ToInt32(IdChief);
                queue++;
                //Вычисляем сумму зарплат без руководителей в департаменте 2

                for (int i = 0; i < p.salary.Length; i++)
                {
                    x[i] = Convert.ToInt32(p.salary[i]);
                }
                 SumSalaryOfDep = 0;
                foreach (int salary in AllindexIdofOneDepartment)
                {
                    SumSalaryOfDep = SumSalaryOfDep + x[salary];
                }
                Console.WriteLine("Сумма зарплат департамента 2 без руководителей = " + SumSalaryOfDep);
                //Вычисляем сумму зарплат с руководителем в департаменте 2
                SumSalaryOfDep = SumSalaryOfDep + x[Convert.ToInt32(IdChief)];
                Console.WriteLine("Сумма зарплат департамента 2 c руководителем = " + SumSalaryOfDep);
                // Вычисление длины массива индексов, принадлежащих департаменту 3 
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 3 & p.chief_id[i] != "null")
                    {
                        indexvalue++;
                    }
                }
                 AllindexIdofOneDepartment = new int[indexvalue];  //= Array.FindAll(y,id =>.Equals("1"));
                indexvalue = 0;
                // Запись индексов в массив
                for (int i = 0; i < y.Length; i++)
                {
                    if (y[i] == 3 & p.chief_id[i] != "null")
                    {
                        AllindexIdofOneDepartment[indexvalue] = i;
                        indexvalue++;
                    }

                }

                // Вычисляем индекс руководителя департамента 3
                IdChief = p.chief_id[AllindexIdofOneDepartment[0]];
                Console.WriteLine("Ид рук деп 3:" + IdChief);
                AllIDChifsOfDep[queue] = Convert.ToInt32(IdChief);
                queue++;

                //Вычисляем сумму зарплат без руководителей в департаменте 3
                for (int i = 0; i < p.salary.Length; i++)
                {
                    x[i] = Convert.ToInt32(p.salary[i]);
                }
                SumSalaryOfDep = 0;
                foreach (int salary in AllindexIdofOneDepartment)
                {
                    SumSalaryOfDep = SumSalaryOfDep + x[salary];
                }
                Console.WriteLine("Сумма зарплат департамента 3 без руководителей = " + SumSalaryOfDep);

                //Вычисляем сумму зарплат с руководителем в департаменте 3
                SumSalaryOfDep = SumSalaryOfDep + x[Convert.ToInt32(IdChief)];
                Console.WriteLine("Сумма зарплат департамента 3 c руководителем = " + SumSalaryOfDep);

                //Вычисляем зарплаты всех руководителей по убыванию
                int[] ListOfChiefsSalary = new int[AllIDChifsOfDep.Length];
                for (int i = 0; i < AllIDChifsOfDep.Length; i++)
                {
                    ListOfChiefsSalary[i] = Convert.ToInt32(p.salary[AllIDChifsOfDep[i]]);
                }
                Array.Sort(ListOfChiefsSalary);
                Array.Reverse(ListOfChiefsSalary);
                
                Console.WriteLine("Список всех зарплат руководителей департамента по убыванию:");
                for (int i = 0; i < AllIDChifsOfDep.Length; i++)
                {
                    Console.WriteLine(ListOfChiefsSalary[i]);
                }

            }
        }
        
        static void Main(string[] args)
        {
           // Прописываем пути к файлам CSV 
           string EmpCsvFilePath = @"C:\БД Сотрудники.csv";
            string DepCsvFilePath = @"C:\БД Департаменты.csv";


            Console.WriteLine("Данная программма парсит 2 базы данных " + EmpCsvFilePath + ", " + DepCsvFilePath + " и выводит нам информацию по трём пунктам задачи:");
            Console.WriteLine("1. Покажет департамент, в котором у сотрудника зарплата максимальна" + "\n" 
                + "2. Покажет суммарную зарплату в разрезе департаментов (без руководителей и с руководителями)"
                + "\n" + "3. Зарплаты руководителей департаментов (по убыванию)");

            Console.WriteLine("Для начала работы программы напишите OK : ");
            string ok = Console.ReadLine();
            if (!(ok == "OK" || ok == "Ok" || ok =="ОК" || ok == "Ок"))
            {
                Console.WriteLine("Упс, похоже вы ввели не то слово! Попробуйте ещё раз)");
                return ;
            }
            //Подключаем парсер для БД Сотрудники
            using (TextFieldParser parser = new TextFieldParser(EmpCsvFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                Parameters p = new Parameters();
                int i = 0;
                // Создаём массивы внитри main, чтобы в дальнейшем записать данные в класс Parameters
                string[] ID = new string[8];
                string[] DEPARTMENT_ID = new string[8];
                string[] CHIEF_ID = new string[8];
                string[] NAME = new string[8];
                string[] SALARY = new string[8];
                // Сканируем БД и вынимаем информацию по столбцам
                while (!parser.EndOfData)
                { 
                    Parameters param = new Parameters();
                    string [] fields = parser.ReadFields();
                    if (i != 0)
                    {
                        ID[i] = fields[0];
                        DEPARTMENT_ID[i] = fields[1];
                        CHIEF_ID[i] = fields[2];
                        NAME[i] = fields[3];
                        SALARY[i] = fields[4];
                    }
                    
                    i = i+1;
                }
                // Заносим информацию в массивы класса Parameters
                p.id = ID;
                p.department_id = DEPARTMENT_ID;
                p.chief_id = CHIEF_ID;
                p.name = NAME;
                p.salary = SALARY;


                // Вызов методов
                Metods m = new Metods();
         
                m.DepMaxSalary(p);
                m.SumSalary(p);
            }
          
        }
    }
}
    

