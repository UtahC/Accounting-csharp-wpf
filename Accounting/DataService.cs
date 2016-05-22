using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Accounting
{
    public class DataService
    {
        private OleDbConnection conn;
        public DataService()
        {

        }

        public List<Item> select()
        {
            
            //conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString);
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(
                    @"SELECT * FROM Accounting;", conn);
                OleDbDataReader dr = comm.ExecuteReader();
                List<Item> items = new List<Item>();
                while (dr.Read())
                {
                    items.Add(
                        new Item(dr.GetInt32(0), dr.GetDateTime(1).ToShortDateString(), dr.GetString(2), dr.GetString(3),
                        dr.GetInt32(4), dr.GetInt32(5), dr.GetInt32(6), dr.GetString(7)));
                }
                return items;
            }
        }

        public bool insert(Item item)
        {
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(
                    @"INSERT INTO Accounting (日期, 姓名, 品名, 件數, 重量, 單價, 備註)
                      VALUES (@time, @cusName, @itemName, @count, @weight, @price, @note);", conn);
                comm.Parameters.AddWithValue("@time", item.time);
                comm.Parameters.AddWithValue("@cusName", item.cusName);
                comm.Parameters.AddWithValue("@itemName", item.itemName);
                comm.Parameters.AddWithValue("@count", item.count);
                comm.Parameters.AddWithValue("@weight", item.weight);
                comm.Parameters.AddWithValue("@price", item.price);
                comm.Parameters.AddWithValue("@note", item.note);
                int result = comm.ExecuteNonQuery();
                if (result <= 0)
                    return false;
                else
                    return true;
            }
        }

        public bool delete(int id)
        {
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(
                    @"DELETE * FROM Accounting WHERE 編號 = @id;", conn);
                comm.Parameters.AddWithValue("@id", id);
                int result = comm.ExecuteNonQuery();
                if (result <= 0)
                    return false;
                else
                    return true;
            }
        }

        public bool update(int id, Item item)
        {
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(
                    @"UPDATE Accounting
                      SET 日期 = @time, 姓名 = @cusName, 品名 = @itemName, 件數 = @count,
                          重量 = @weight, 單價 = @price, 備註 = @note
                      WHERE 編號 = @id;", conn);
                comm.Parameters.AddWithValue("@time", item.time);
                comm.Parameters.AddWithValue("@cusName", item.cusName);
                comm.Parameters.AddWithValue("@itemName", item.itemName);
                comm.Parameters.AddWithValue("@count", item.count);
                comm.Parameters.AddWithValue("@weight", item.weight);
                comm.Parameters.AddWithValue("@price", item.price);
                comm.Parameters.AddWithValue("@note", item.note);
                comm.Parameters.AddWithValue("@id", id);
                int result = comm.ExecuteNonQuery();
                if (result <= 0)
                    return false;
                else
                    return true;

            }
        }
    }
}
