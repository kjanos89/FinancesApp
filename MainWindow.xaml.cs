using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Newtonsoft.Json;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        public MainWindow()
        {
            InitializeComponent();
            dataListView.ItemsSource = entries;
            LoadEntriesFromFile("entries.json");
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            string input = amountTextBox.Text;
            int value;
            if (int.TryParse(input, out value))
            {
                entries.Add(new Entry { Type = EntryType.Income, Amount = value });
                itemNameTextBox.Text = "";
                amountTextBox.Text = "";
                ItemNameTextBox_LostFocus(sender, e);
                AmountTextBox_LostFocus(sender, e);
            }
            else
            {
                MessageBox.Show("Please input a proper amount!");
            }
            
        }

        private void AddSpending_Click(object sender, RoutedEventArgs e)
        {
            string name = itemNameTextBox.Text;
            string input = amountTextBox.Text;
            int value;
            if (int.TryParse(input, out value))
            {
                entries.Add(new Entry { Type = EntryType.Spending,Name = name, Amount = value });
                itemNameTextBox.Text = "";
                amountTextBox.Text = "";
                ItemNameTextBox_LostFocus(sender, e);
                AmountTextBox_LostFocus(sender,e);
            }
            else
            {
                MessageBox.Show("Please input a proper amount!");
            }
            
        }
        private void ItemNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (itemNameTextBox.Text == "Name")
            {
                itemNameTextBox.Text = "";
                itemNameTextBox.Foreground = Brushes.Black; // Change text color to normal
            }
        }

        private void ItemNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemNameTextBox.Text))
            {
                itemNameTextBox.Text = "Name";
                itemNameTextBox.Foreground = Brushes.Gray; // Change text color to gray
            }
        }

        private void AmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (amountTextBox.Text == "Amount")
            {
                amountTextBox.Text = "";
                amountTextBox.Foreground = Brushes.Black; // Change text color to normal
            }
        }

        private void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(amountTextBox.Text))
            {
                amountTextBox.Text = "Amount";
                amountTextBox.Foreground = Brushes.Gray; // Change text color to gray
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Entry entry)
            {
                entries.Remove(entry);
                UpdateTotal();
            }
        }
        private void UpdateTotal()
        {
            decimal totalIncome = entries.Where(entry => entry.Type == EntryType.Income).Sum(entry => entry.Amount);
            decimal totalSpending = entries.Where(entry => entry.Type == EntryType.Spending).Sum(entry => entry.Amount);
            decimal netTotal = totalIncome - totalSpending;

            totalLabel.Content = $"Total: Income: {totalIncome:C2}, Spending: {totalSpending:C2}, Net: {netTotal:C2}";
        }
        private void SaveEntriesToFile(string filename)
        {
            string json = JsonConvert.SerializeObject(entries);
            File.WriteAllText(filename, json);
        }

        private void LoadEntriesFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                entries = JsonConvert.DeserializeObject<ObservableCollection<Entry>>(json);
                dataListView.ItemsSource = entries;
                UpdateTotal();
            }
        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveEntriesToFile("entries.json");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveEntriesToFile("entries.json");
            Application.Current.Shutdown();
        }

    }
}
