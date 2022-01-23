using System;
using System.Collections.Generic;
using SQLite;

namespace JxStock.Models
{
    public static class DataBaseManager
    {
        private static string DatabasePath= System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"db\Stock.db");
        //private readonly string ConnectionString = null;
        //private readonly string DataBaseFilePath = AppDomain.CurrentDomain.BaseDirectory + DatabasePath;

        /// <summary>
        /// 在庫を登録・更新する
        /// </summary>
        /// <param name="stock"></param>
        public static void Regist(Stocks stock)
        {
            using (var connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<Stocks>();
                connection.InsertOrReplace(stock);
            }
        }

        /// <summary>
        /// 在庫を検索する
        /// </summary>
        /// <returns>検索結果</returns>
        public static List<Stocks> Select()
        {
            using (var connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<Stocks>();

                string query = "SELECT * FROM Stocks";
                List<Stocks> list = connection.Query<Stocks>(query);

                return list;
            }
        }
    }
}
