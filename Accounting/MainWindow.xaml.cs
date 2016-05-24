using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        DataService dataService;

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
        }

        private void button_Select_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void windowInit()
        {
            itemNameInit();
            updateDataGrid();
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

        private void updateDataGrid()
        {
            dataService = new DataService();
            List<Item> items = dataService.select();
            dataGrid.ItemsSource = items;
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
            finally
            {
                item.cusName = textBox_CusName.Text;
                item.itemName = comboBox_ItemName.SelectedItem.ToString();
                item.time = DateTime.Now;
                item.note = textBox_Note.Text;
            }
            return item;
        }
    }
}
