using SQLite;

namespace JxStock.Models
{
    public class Stocks
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 単価
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// 画像のパス
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 区分
        /// </summary>
        public string Category { get; set; }
    }
}
