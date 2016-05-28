using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Accounting
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        DataService dataService = new DataService();
        List<Item> _items;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            windowInit();
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy/MM/dd";
        }

        private void button_Insert_Click(object sender, RoutedEventArgs e)
        {
            Item item = makeItemByInput();
            if (item != null)
                dataService.insert(item);
            updateDataGrid(dataService.select());
            setTextNull();
        }

        private void button_Select_Click(object sender, RoutedEventArgs e)
        {
            string cusName = textBox_Select_CusName.Text.Replace(" ","");
            DateTime? start = datePicker_Start.SelectedDate;
            DateTime? end = datePicker_End.SelectedDate;
            updateDataGrid(dataService.select(cusName, start, end));
        }

        private void button_Print_Click(object sender, RoutedEventArgs e)
        {
            PdfPrinter printer = new PdfPrinter();
            HashSet<string> cusNames = new HashSet<string>();
            foreach(Item item in _items)
                cusNames.Add(item.cusName);
            printer.setPageCount(cusNames.Count);
            for (int i = 0; i < cusNames.Count; i++)
            {
                string cusName = cusNames.ToList()[i];
                int totalPrice = 0;
                printer.drawTextLine(i + 1, cusName, Size.Large, true);
                printer.drawTextLine(i + 1, string.Format(
                    "{0,8}{1,11}{2,8}{3,8}{4,8}{5,8}{6,8}", 
                    "日期", "品名", "件數", "重量", "單價", "小計", "備註"), Size.Small);
                foreach (Item item in _items.Where(item => item.cusName == cusName))
                {
                    int sumPrice = item.price * item.weight;
                    totalPrice = totalPrice + sumPrice;
                    printer.drawTextLine(i + 1, string.Format(
                        "{0,10}{1,10}{2,10}{3,10}{4,10}{5,10}{6,10}", 
                        item.time.ToShortDateString(), item.itemName, 
                        item.count, item.weight, item.price, sumPrice, item.note), Size.Small);
                }
                printer.drawTextLine(i + 1, "總計: " + totalPrice, Size.Medium);
            }
            printer.print("Accounting.pdf");
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item item = (Item)dataGrid.SelectedItem;
            setTextByItem(item);
        }

        private void windowInit()
        {
            itemNameInit();
            updateDataGrid(dataService.select());
        }

        private void itemNameInit()
        {
            List<string> list = new List<string>();
            list.Add("紅辣椒A");
            list.Add("紅辣椒B");
            list.Add("青辣椒");
            list.Add("朝天椒A");
            list.Add("朝天椒B");
            list.Add("大脫");
            list.Add("小脫");
            list.Add("辣椒醬");
            comboBox_ItemName.ItemsSource = list;
        }

        private void updateDataGrid(List<Item> items)
        {
            _items = items;
            dataGrid.ItemsSource = _items;
            dataGrid.Columns[0].Header = "編號";
            dataGrid.Columns[1].Header = "日期";
            dataGrid.Columns[2].Header = "姓名";
            dataGrid.Columns[3].Header = "品名";
            dataGrid.Columns[4].Header = "件數";
            dataGrid.Columns[5].Header = "重量";
            dataGrid.Columns[6].Header = "單價";
            dataGrid.Columns[7].Header = "備註";
        }

        private Item makeItemByInput()
        {
            Item item = new Item();
            try
            {
                item.count = int.Parse(textBox_Count.Text);
                item.weight = int.Parse(textBox_Weight.Text);
                item.price = int.Parse(textBox_Price.Text);
            }
            catch
            {
                MessageBox.Show("請數入數字");
                return null;
            }
            item.cusName = textBox_CusName.Text;
            item.itemName = comboBox_ItemName.SelectedItem.ToString();
            item.time = DateTime.Now;
            item.note = textBox_Note.Text;
            return item;
        }

        private void setTextByItem(Item item)
        {
            if (item != null)
            {
                textBox_CusName.Text = item.cusName;
                comboBox_ItemName.SelectedValue = item.itemName;
                textBox_Count.Text = item.count.ToString();
                textBox_Price.Text = item.price.ToString();
                textBox_Weight.Text = item.weight.ToString();
                textBox_Note.Text = item.note;
            }
        }

        private void setTextNull()
        {
            textBox_CusName.Text = "";
            comboBox_ItemName.SelectedIndex = -1;
            textBox_Count.Text = "";
            textBox_Price.Text = "";
            textBox_Weight.Text = "";
            textBox_Note.Text = "";
        }
        
    }
}
