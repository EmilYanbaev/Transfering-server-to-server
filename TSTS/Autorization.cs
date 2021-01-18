using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace TSTS
{
    //Класс для подключения к серверу.
    //CreatConnectionString(создание строки подключения) и ConnectionServer(подключение к серверу)
    //GetObjectConnection возвращает объект подключения
    //CloseConnection закрывает подключение к серверу
    class Autorization
    {
        private string ConnectionString;                //Строка подключения
        public SqlConnection connection;                // объект подключения

        /// <summary>
        /// Создает строку подключения по ключу из конфигурационного файла.
        /// </summary>
        ///<param name="NameServerKeyConfig">Ключ к строке подключения сервера в App.config</param>
        ///<returns>Возвращает true при выполении без Exceptions</returns>
        public bool CreatConnectionString(string NameServerKeyConfig)
        {

            ConnectionString = ConfigurationManager.ConnectionStrings[NameServerKeyConfig].ConnectionString;

            //Проверка на сущесование строки подключения в конфигурационном файле
            if (ConnectionString == null)
                throw new Exception("null ConnectionString");

            return true;
        }

        /// <summary>
        /// Подключение к серверу и вывод сводных данных о нем
        /// </summary>
        ///<returns>Возвращает true при выполении без Exceptions</returns>
        public bool ConnectionServer()
        {

            //Проверка на сущесование строки подключения
            if (ConnectionString == null)
                throw new Exception("null ConnectionString");


            connection = new SqlConnection(ConnectionString);
            connection.Open();

            Console.WriteLine("Свойства подключения:");
            Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
            Console.WriteLine("\tБаза данных: {0}", connection.Database);
            Console.WriteLine("\tСервер: {0}", connection.DataSource);
            Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
            Console.WriteLine("\tСостояние: {0}", connection.State);
            Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);

            return true;

        }

        /// <summary>
        /// Возвращает объект подключения
        /// </summary>
        ///<returns>Возвращает объект подключения, при его НЕ существовании возвращает null</returns>
        public SqlConnection GetObjectConnection()
        {

            return (connection != null) ? connection : null;

        }

        /// <summary>
        /// Закрыть подключение с сервером 
        /// </summary>
        public void CloseConnectionServer()
        {
            connection.Close();
        }


    }
}
