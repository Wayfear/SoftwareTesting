using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FilePage.xaml
    /// </summary>
    public partial class FilePage : UserControl
    {
        public static ObservableCollection<BaseModel> DataSource = new ObservableCollection<BaseModel>();

        private string path;

        public FilePage()
        {
            InitializeComponent();
        }

       
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory(); 
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFileDialog1.FileName;
            }
            File_Parse();
        }

        private void File_Parse()
        {
            string openfilePath = path;
            StreamReader reader = new StreamReader(@openfilePath);
            string line = "";
            line = reader.ReadLine();//读取一行数据
            line = reader.ReadLine();
            while (line != null)
            {
                string[] temps = line.Split(',');//将文件内容分割成数组
                var tempData = new DataModel(int.Parse(temps[0]), int.Parse(temps[1]),
                    int.Parse(temps[2]), int.Parse(temps[3]),
                    float.Parse(temps[4]), float.Parse(temps[5]));
                DataSource.Add(tempData);
                line = reader.ReadLine();
            }
            MessageBox.Show("Finish Import!");

        }

        private void Export_report(object sender, RoutedEventArgs e)
        {
            string filePath = path.Split('.')[0] + "_report.csv";
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(outputHeader());
            foreach (var v in DataSource)
            {
                sw.WriteLine(v.Output());
            }
            sw.Close();
            MessageBox.Show("Finish Export!");
        }


        string outputHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Index").Append(",").Append("PeripheralNumber").Append(", ").
                Append("HostNumber").Append(", ").Append("DisplayNumber").Append(", ").
                Append("ExpectPrice").Append(", ").Append("ExpectProfit").Append(", ").
                Append("OutputPrice").Append(", ").Append("OutputProfit").Append(", ").
                Append("PriceCorrect").Append(", ").Append("ProfitCorrect");
            return sb.ToString();
        }

        public class DataModel: BaseModel
        {

            private int peripheralNumber;

            private int hostNumber;

            private int displayNumber;

            private float expectPrice;

            private float expectProfit;

            private float outputPrice;

            private float outputProfit;

            private bool priceCorrect;

            private bool profitCorrect;

            public DataModel(int index, int peripheralNumber, 
                int hostNumber, int displayNumber, 
                float expectPrice, float expectProfit)
            {
                AtIndex = index;
                PeripheralNumber = peripheralNumber;
                HostNumber = hostNumber;
                DisplayNumber = displayNumber;
                ExpectPrice = expectPrice;
                ExpectProfit = expectProfit;

                if(peripheralNumber > 90 || hostNumber > 70 || displayNumber > 80 ||
                    peripheralNumber < 1 || hostNumber < 1 || displayNumber < 1)
                {
                    inputCorrect = false;
                    return;
                }
                else
                {
                    inputCorrect = true;
                }

                outputPrice = (float)peripheralNumber * 25.0f + (float)hostNumber * 45f + (float)displayNumber * 30f;

                priceCorrect = (System.Math.Abs(outputPrice - this.expectPrice)<0.01);

                if (outputPrice < 1000)
                {
                    outputProfit = (outputPrice * 0.05f);
                }
                else if (outputPrice >= 1800)
                {
                    outputProfit = (outputPrice * 0.15f);
                }
                else
                {
                    outputProfit = (outputPrice * 0.1f);
                }

                profitCorrect = System.Math.Abs(outputProfit - this.expectProfit) < 0.01;
       
            }

            public override bool ResultDecide()
            {
                return priceCorrect && profitCorrect;
            }

            public override string Output()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(AtIndex).Append(",").Append(PeripheralNumber).Append(",").
                    Append(HostNumber).Append(",").Append(DisplayNumber).Append(",").
                    Append(ExpectPrice.ToString("f2")).Append(",").Append(ExpectProfit.ToString("f2")).Append(",");
                if(inputCorrect)
                    sb.Append(OutputPrice.ToString("f2")).Append(",").Append(OutputProfit.ToString("f2")).Append(",").
                    Append(PriceCorrect).Append(",").Append(ProfitCorrect);
                else
                    sb.Append("NULL").Append(", ").Append("NULL").Append(", ").
                    Append("NULL").Append(", ").Append("NULL");
                return sb.ToString();
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

            public float ExpectPrice
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

            public float ExpectProfit
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

            public float OutputPrice
            {
                get
                {
                    return outputPrice;
                }
            }

            public float OutputProfit
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
                    return priceCorrect ? "True" : "False";
                }
            }

            public string ProfitCorrect
            {
                get
                {
                    return profitCorrect ? "True" : "False";
                }
            }


        }

        private void SampleTest(object sender, RoutedEventArgs e)
        {

        }
    }
}
