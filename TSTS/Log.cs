using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace TSTS
{
    class Log
    {
        string currentPath; //Текущая директория исполняемого файла
        public string pathFile;
        StreamWriter fstream;
        public Log()
        {
            try
            {
                this.currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); //Возвращает текущую директорию исполняемого файла

                if (!Directory.Exists(currentPath + "\\History"))
                {
                    Directory.CreateDirectory(currentPath + "\\History");
                }

                DateTime dt = DateTime.Now;

                string nameFileDate = dt.ToString();
                nameFileDate.Replace(":", ".");
                nameFileDate = nameFileDate.Replace(":", ".");
                this.pathFile = currentPath + "\\History" + "\\" + nameFileDate + ".txt";

                this.fstream = new StreamWriter(pathFile, true, System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                File.WriteAllText("new_file.txt", "Ошибка доступа к файловой директории");
            }
        }

        /// <summary>
        /// Создание строки об успешной работе
        /// </summary>
        ///<param name="bTime">Время начала работы программы</param>
        ///<param name="eTime">Время конца работы программы</param>
        ///<returns>Возвращает сформированную строку об успешной работе</returns>
        public string Success(DateTime bTime, DateTime eTime)
        {
            TimeSpan temp = bTime.Subtract(eTime);
            string intervalTime = temp.ToString();
            string com = "TranslationServerToServer отработал успешно, время работы:" + intervalTime;
            return com;
        }

        /// <summary>
        /// Отправка в сообщения в лексему
        /// </summary>
        ///<param name="str">Сообщение</param>
        ///<param name="ex">Ошибка для отправки</param>
        ///<returns></returns>
        public void WLogLexema(string str, Exception ex)
        {
            try
            {
                Autorization auto = new Autorization();
                auto.CreatConnectionString("MessConnection");
                auto.ConnectionServer();

                str = str + ex;

                string query = " INSERT INTO dbo.Messages( ToUser, ToHost, FromUser, FromHost, ReadFlag, bdate, edate, Text) " +
                               " select ac.USERNAME, '', USER, HOST_NAME(), 0, getdate(), DATEADD(dd, 2, getdate()), '" + str + "' " +
                               " from Access_ACT a " +
                               " inner join Access_ACTSPECS ac ON a.VCODE = ac.PCODE " +
                               " where a.TASK = 'Склад' " +
                               " and a.TDOCDOC = 'adm' " +
                               " and a.ANALITIKA = '02' ";

                ProcessingTable messLex = new ProcessingTable();
                messLex.SetConnection(auto.GetObjectConnection());
                messLex.ExecutCommand(query);

                auto.CloseConnectionServer();
            }
            catch (Exception exLex)
            {
                WLog("Не удалось отправить в лексему" + exLex + "\n Получена:", ex);
            }
        }

        /// <summary>
        /// Отправка в сообщения в лог
        /// </summary>
        ///<param name="str">Сообщение</param>
        ///<param name="ex">Ошибка для отправки</param>
        ///<returns></returns>
        public void WLog(string str, Exception ex)
        {
            str += ex;
            fstream.WriteLine(str);
        }

        /// <summary>
        /// Закрыть поток ввода в лог
        /// </summary>
        public void CloseWlog()
        {
            fstream.Close();
        }


    }
}