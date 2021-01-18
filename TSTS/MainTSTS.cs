using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;


namespace TSTS
{
    class MainTSTS
    {
        static void Main(string[] args)
        {

            DateTime bWorked = DateTime.Now;
            DateTime eWorked;
            Log log = new Log();

            //Поделючаемся к 1 серверу, для получения данных
            Autorization serverFirst = new Autorization();
            try
            {
                serverFirst.CreatConnectionString("FConnection"); //FConnection - ключ в App.config строки подключения

                log.WLog("Создание строки к серверу 1 - успешно", null);

                serverFirst.ConnectionServer();

                log.WLog("Подключение к серверу 1 - успешно", null);
            }
            catch (Exception ex)
            {
                log.WLogLexema("Ошибка - Подключение к серверу 1 - Проверьте правильность строки подключения - Подробнее см.лог фаил", null);
                log.WLog("Ошибка -  Подключения к серверу 1 - ", ex);
                log.CloseWlog();
                Environment.Exit(1);

            }

            DataTable dt = new DataTable();
            try
            {
                //Получаем объект подключения, для обработки данных
                ProcessingTable getTd = new ProcessingTable(); //Класс для работы с SQL
                getTd.SetConnection(serverFirst.GetObjectConnection());

                log.WLog("Объект подключения к серверу 1 - успешно", null);

                //Получаем с помощью запроса( запрос в конфигурационном файле - ключ query) данные и формируем их DataTable 
                dt = new DataTable();
                dt = getTd.GetDateTable(ConfigurationManager.AppSettings.Get("query"));

                log.WLog("Таблица с сервера 1 получена - успешно", null);

                //Закрываем подключение
                serverFirst.CloseConnectionServer();
            }
            catch (Exception ex)
            {
                log.WLogLexema("Ошибка - Получение таблицы с сервера 1 - Подробнее см.лог фаил", null);
                log.WLog("Ошибка - Получение таблицы с сервера - ", ex);
                log.CloseWlog();
                Environment.Exit(1);
            }

            //Подключаемся ко 2 серверу, для переноса данных
            Autorization serverSecond = new Autorization();
            try
            {
                serverSecond.CreatConnectionString("SConnection"); //SConnection - ключ в App.config строки подключения
                serverSecond.ConnectionServer();

                log.WLog("Подключение к серверу 2 - успешно", null);
            }
            catch (Exception ex)
            {
                log.WLogLexema("Ошибка - Подключение к серверу 2 - Проверьте правильность строки подключения - Подробнее см.лог фаил", null);
                log.WLog("Ошибка -  Подключения к серверу 2 - ", ex);
                log.CloseWlog();
                Environment.Exit(1);
            }

            try
            {
                //Получаем обьект подключения
                ProcessingTable pTable = new ProcessingTable(); //Класс для работы с SQL
                pTable.SetConnection(serverSecond.GetObjectConnection());

                log.WLog("Объект подключения к серверу 2 - успешно", null);

                //Вставляем таблицу в базу данных прописанную в SConnection (См.config) в таблицу NameInInsertTable - ключ к имени таблицы в App.config
                pTable.InsertDataTable(dt);

                log.WLog("Вставка на сервер 2 - успешно", null);

                //Закрываем подключение
                serverSecond.CloseConnectionServer();
            }
            catch (Exception ex)
            {
                log.WLogLexema("Ошибка - Вставка таблицы на сервер 2 - Подробнее см.лог фаил", null);
                log.WLog("Ошибка - Вставка таблицы на сервер 2 - ", ex);
                log.CloseWlog();
                Environment.Exit(1);
            }

            eWorked = DateTime.Now;
            if (Convert.ToInt32(ConfigurationManager.AppSettings.Get("SendSuccessWorked")) == 1)
                log.WLogLexema(log.Success(bWorked, eWorked), null);
            log.WLog(log.Success(bWorked, eWorked), null);
            log.CloseWlog();
            System.Environment.Exit(0);

        }
    }
}
