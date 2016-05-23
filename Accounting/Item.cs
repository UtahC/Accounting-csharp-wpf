using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    public class Item
    {
        public Item()
        {

        }
        public Item(DateTime time, string cusName, string itemName, int count, int weight, int price, string note)
        {
            this.time = time;
            this.cusName = cusName;
            this.itemName = itemName;
            this.count = count;
            this.weight = weight;
            this.price = price;
            this.note = note;
        }
        public Item(int id, DateTime time, string cusName, string itemName, int count, int weight, int price, string note) {
            this.id = id;
            this.time = time;
            this.cusName = cusName;
            this.itemName = itemName;
            this.count = count;
            this.weight = weight;
            this.price = price;
            this.note = note;
        }
        public int id { get; set; }
        public DateTime time { get; set; }
        public string cusName { get; set; }
        public string itemName { get; set; }
        public int count { get; set; }
        public int weight { get; set; }
        public int price { get; set; }
        public string note { get; set; }
    }
}
