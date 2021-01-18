using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace TSTS
{
    //Данный класс создан для работы с SQL: выполнение команд, получение и вставка таблиц
    //Все методы(кроме GetDataTable) в случае неправильной работы возвращают exception, для записи в лог
    class ProcessingTable
    {


        private SqlConnection connection; //объект подключения
        //private string SqlExpression; //Запрос к базе
        private SqlDataAdapter adapter;
        private DataSet ds;
        private DataTable dt;
        public SqlCommand command = new SqlCommand();

        /// <summary>
        /// Сохраняем объект подключения для работы с ним
        /// </summary>
        ///<param name="connection">Объект подключения</param>
        ///<returns>Возвращает true при выполении без Exceptions</returns>
        public bool SetConnection(SqlConnection connection)
        {
            if (connection == null)
                throw new Exception("null connection object");
            this.connection = connection;
            return true;
        }

        /// <summary>
        /// Выполение команд не ссвязанных с получением данных
        /// </summary>
        ///<param name="SqlExpression">Команда для отправки на сервер</param>
        ///<returns>Возвращает true при выполении без Exceptions</returns>
        public bool ExecutCommand(string SqlExpression)
        {

            if (connection == null)
                throw new Exception("null connection object");

            command.CommandText = SqlExpression;
            command.Connection = connection;
            command.ExecuteNonQuery(); //Выполнение команды

            return true;

        }

        /// <summary>
        /// Получение таблицы с сервера
        /// </summary>
        ///<param name="SqlExpression">Запрос на сервер</param>
        ///<returns>DataTable</returns>
        public DataTable GetDateTable(string SqlExpression)
        {
            SqlCommand cm = new SqlCommand(SqlExpression, connection);
            cm.CommandTimeout = 20 * 60 * 1000;
            if (connection == null)
                throw new Exception("null connection object");

            adapter = new SqlDataAdapter(cm);
            ds = new DataSet();

            adapter.Fill(ds);
            dt = ds.Tables[0];
            return dt;

        }

        /// <summary>
        /// Вставка таблицы dt на сервер
        /// </summary>
        ///<param name="dt">Таблица для вставки</param>
        ///<returns>Возвращает true при выполении без Exceptions</returns>
        public bool InsertDataTable(DataTable dt)
        {
            string InsertInTable = ConfigurationManager.AppSettings.Get("NameInInsertTable"); //Имя таблицы в базе данных
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = InsertInTable;

                bulkCopy.WriteToServer(dt);

                return true;
            }
        }
    }
}
