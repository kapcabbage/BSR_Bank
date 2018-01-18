using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSRBankingDataAccess.Context
{
    public static class DbContext
    {
        private static SQLiteConnection _dbConneciton;
        private static string _dbFile = @"D:\Dokumenty\Pojects\BSRBanking\db.db";

        public static void CreateAndOpen()
        {
            var dbFilePath = @"../../Db.sqlite";
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            _dbConneciton = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFilePath));

            _dbConneciton.Open();
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + _dbFile+ ";Version=3;");
        }

       
    }
}
