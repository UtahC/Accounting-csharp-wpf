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

        public List<Item> select(string cusName, DateTime? start, DateTime? end)
        {
            bool isCusNameValid = (cusName != "");
            bool isDateTimeBothValid = (start.HasValue && end.HasValue);
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                OleDbCommand comm = new OleDbCommand("", conn);
                comm.CommandText = @"SELECT * FROM Accounting";
                if (isCusNameValid || isDateTimeBothValid)
                {
                    comm.CommandText += " WHERE ";
                    if (isCusNameValid)
                    {
                        comm.CommandText += "姓名 = @cusName";
                        comm.Parameters.AddWithValue("@cusName", cusName);
                    }
                    if (isCusNameValid && isDateTimeBothValid)
                        comm.CommandText += " AND ";
                    if (isDateTimeBothValid)
                    {
                        comm.CommandText += "日期 BETWEEN @start AND @end";
                        comm.Parameters.AddWithValue("@start", getDateTimeWithoutHMS(start.Value));
                        comm.Parameters.AddWithValue("@end", getDateTimeWithoutHMS(end.Value));
                    } 
                }
                conn.Open();
                OleDbDataReader dr = comm.ExecuteReader();
                List<Item> items = new List<Item>();
                while (dr.Read())
                {
                    items.Add(
                        new Item(dr.GetInt32(0), dr.GetDateTime(1), dr.GetString(2), dr.GetString(3),
                        dr.GetInt32(4), dr.GetInt32(5), dr.GetInt32(6), dr.GetString(7)));
                }
                return items;
            }
        }

        public List<Item> select()
        {
            return select("", null, null);
        }

        public bool insert(Item item)
        {
            using (conn = new OleDbConnection(Properties.Settings.Default.AccountingDBConnectionString))
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand(
                    @"INSERT INTO Accounting (日期, 姓名, 品名, 件數, 重量, 單價, 備註)
                      VALUES (@time, @cusName, @itemName, @count, @weight, @price, @note);", conn);
                comm.Parameters.AddWithValue("@time", getDateTimeWithoutHMS(item.time));
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
                comm.Parameters.AddWithValue("@time", getDateTimeWithoutHMS(item.time));
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

        private DateTime getDateTimeWithoutHMS(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
