using System;
using System.Collections.Generic;
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
using System.IO;


namespace SoftwareTesting.Pages.Experiments.Sales
{
    /// <summary>
    /// Interaction logic for SplitPage1.xaml
    /// </summary>
    public partial class SplitPage1 : UserControl
    {
        public SplitPage1()
        {
            InitializeComponent();
            listStrArr = new List<List<int>>();
        }

        List<List<int>> listStrArr;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox.Text = openFileDialog1.FileName;
            }
        }

        private void File_Parse(object sender, RoutedEventArgs e)
        {
            String openfilePath = textBox.Text;
            StreamReader reader = new StreamReader(@openfilePath);
            string line = "";
            line = reader.ReadLine();//读取一行数据
            line = reader.ReadLine();
            string[] temps;
            while (line != null)
            {
                List<int> tempt = new List<int>();
                temps = line.Split(',');//将文件内容分割成数组
                for (int i = 0; i < temps.Length; i++)
                {
                    tempt[i] = int.Parse((temps[i]));
                }
                listStrArr.Add(tempt);
                line = reader.ReadLine();
            }
        }

        public class DataModel
        {
            private int index;

            private int peripheralNumber;

            private int hostNumber;

            private int displayNumber;

            private int expectPrice;

            private int expectProfit;

            private int outputPrice;

            private int outputProfit;

            private bool priceCorrect;

            private bool profitCorrect;

            public DataModel(int index, int peripheralNumber, 
                int hostNumber, int displayNumber, 
                int expectPrice, int expectProfit)
            {
                Index = index;
                PeripheralNumber = peripheralNumber;
                HostNumber = hostNumber;
                DisplayNumber = displayNumber;
                ExpectPrice = expectPrice;
                ExpectProfit = expectProfit;


            }


            public int Index
            {
                set
                {
                    index = value;
                }
                get
                {
                    return index;
                }
            }

            public int PeripheralNumber
            {
                set
                {
                    peripheralNumber = value;
                }
                get
                {
                    return peripheralNumber;
                }
            }

            public int HostNumber
            {
                set
                {
                    hostNumber = value;
                }
                get
                {
                    return hostNumber;
                }
            }

            public int DisplayNumber
            {
                set
                {
                    displayNumber = value;
                }
                get
                {
                    return displayNumber;
                }
            }

            public int ExpectPrice
            {
                set
                {
                    expectPrice = value;
                }
                get
                {
                    return expectPrice;
                }
            }

            public int ExpectProfit
            {
                set
                {
                    expectProfit = value;
                }
                get
                {
                    return expectProfit;
                }
            }

            public int OutputPrice
            {
                get
                {
                    return outputPrice;
                }
            }

            public int OutputProfit
            {
                get
                {
                    return outputProfit;
                }
            }
            public string PriceCorrect
            {
                get
                {
                    if (priceCorrect)
                    {
                        return "True";
                    }
                    else
                    {
                        return "False";
                    }
                }
            }

            public string ProfitCorrect
            {
                get
                {
                    if (profitCorrect)
                    {
                        return "True";
                    }
                    else
                    {
                        return "False";
                    }
                }
            }


        }
    }
}
