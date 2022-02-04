using System;
using System.Collections.Generic;
using SQLite;

namespace JxStock.Models
{
    public static class DataBaseManager
    {
        private static string DatabasePath= System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"db\Stock.db");

        /// <summary>
        /// 在庫を登録する
        /// </summary>
        /// <param name="stock"></param>
        public static void Insert(Stocks stock)
        {
            using (var connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<Stocks>();
                connection.Insert(stock);
            }
        }

        /// <summary>
        /// 在庫を更新する
        /// </summary>
        /// <param name="stock"></param>
        public static void Update(Stocks stock)
        {
            using (var connection = new SQLiteConnection(DatabasePath))
            {
                connection.CreateTable<Stocks>();
                connection.Update(stock);
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

        /// <summary>
        /// 在庫を登録・更新する
        /// </summary>
        /// <param id="削除する在庫のキー"></param>
        public static void Delete(int id)
        {
            using (var connection = new SQLiteConnection(DatabasePath))
            {
                connection.Delete<Stocks>(id);
            }
        }
    }
}
