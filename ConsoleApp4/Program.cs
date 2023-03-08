using Npgsql;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int z1, z2, k = 3;
            double quotient = 1;
            double all = 1;
            int count = 0;
            Console.Write("введите U: ");
            string u1 = Console.ReadLine();


            double[] firstArray = new double[29];
            double[] secondArray = new double[266];

           // Console.WriteLine("Соединение с БД открыто");

            //Создание массива firstArray от 7 до 35 и массива secondArray от 35 до 300
            for (int i = 0; i < 29; i++)
            {
                firstArray[i] = i + 7;
            }
            for (int i = 0; i < 266; i++)
            {
                secondArray[i] = i + 35;
            }
            double[] arr = new double[70];
            var str = string.Join(" ", firstArray);
           // Console.WriteLine(str);

            var str1 = string.Join(" ", secondArray);
           // Console.WriteLine(str1);

            z1 = Convert.ToInt32(firstArray.Length);
            z2 = secondArray.Length;


            //подключение к БД postgres
            double clearall;
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=root;Database=postgres;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            /*
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = npgSqlConnection;
                            cmd.CommandText = @"CREATE TABLE General (id serial PRIMARY KEY, z1 integer, z2 integer, k integer, u decimal);"; // создаётся таблица с названием General, т.е "Общее придаточное отношение".
                            cmd.ExecuteNonQuery();

                        }

             */


            //подключение к таблице General
            if (decimal.TryParse(u1, out decimal value))
            {
                var cmd = new NpgsqlCommand("SELECT * FROM General WHERE u = @value", npgSqlConnection);
                cmd.Parameters.AddWithValue("value", value);
                //var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM General WHERE u = @u", npgSqlConnection);
                //int rowCount = Convert.ToInt32(countCommand.ExecuteScalar());
                  

                //поиск по таблице Genereal U и вывод всех z1/z2 который ей равны
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count++;
                        var bebraID = reader.GetInt32(0);
                        var bebra = reader.GetDecimal(4);
                        var bebra1 = reader.GetInt32(1);
                        var bebra2 = reader.GetInt32(2);
                    
                        
                        Console.WriteLine($"id = {bebraID} | u = {bebra} | z1 = {bebra1} | z2 = {bebra2} ");
                    
                    
                }
                
                if (count == 0)
                {
                    Console.WriteLine("Совпадений не найдено.");
                }
                else
                {
                    Console.WriteLine($"Всего {count} совпадений.");
                }


            }
            //алгоритм нахождения U  и заполнение ее из массивов firstArray и secondArray
            for (int i = 0; i < z1; i++)
            {
                for (int j = 0; j < z2; j++)
                {
                    for (int g = 0; g < k; g++)
                    {
                        quotient = secondArray[j] / firstArray[i];
                        all *= quotient;
                        all = Convert.ToDouble(TruncateWithPrecision(all, 3));
                      /*  using (var cmd = new NpgsqlCommand(sql, npgSqlConnection))
                        {
                            
                            cmd.Connection = npgSqlConnection;
                            cmd.CommandText = "INSERT INTO General (z1, z2, k, u) VALUES (@z1, @z2, @k, @u)";
                            cmd.Parameters.AddWithValue("z1", firstArray[i]);
                            cmd.Parameters.AddWithValue("z2", secondArray[j]);
                            cmd.Parameters.AddWithValue("k", g + 1);
                            cmd.Parameters.AddWithValue("u", all);
                            cmd.ExecuteNonQuery();


                        }
                      */
                    }
                  //  Console.WriteLine($"All = {TruncateWithPrecision(all, 3)}: Z1= {secondArray[j]}, Z2 = {firstArray[i]}");
                    all = 1;
                }
            }
            npgSqlConnection.Close();

            Console.ReadKey();
        }

        // Метод точной обрезки после запятой
        static string TruncateWithPrecision(double value, int precision) 
        {
            if (precision < 0) throw new ArgumentOutOfRangeException(nameof(precision));

            var prepared = string.Format($"{{0:f{precision + 1}}}", value);
            return prepared.Substring(0, prepared.Length - (precision == 0 ? 2 : 1));
        }

    }
}

