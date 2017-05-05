using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
using SoftwareTesting.Pages.Experiments.Sales;

namespace SoftwareTesting.Pages.Experiments.Phone
{
    /// <summary>
    /// Interaction logic for File.xaml
    /// </summary>
    public partial class File : UserControl
    {
        public static ObservableCollection<BaseModel> DataSource = new ObservableCollection<BaseModel>();

        private string path;

        public File()
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
            reader.ReadLine();//读取一行数据
            reader.ReadLine();
            line = reader.ReadLine();
            while (line != null)
            {
                string[] temps = line.Split(',');//将文件内容分割成数组
                int temp1, temp2;
                float temp3, temp4;
                if (!int.TryParse(temps[1], out temp1))
                    temp1 = -1;
                if (!int.TryParse(temps[2], out temp2))
                    temp2 = -1;
                temps[3] = temps[3].Substring(0, temps[3].Length - 1);
                if (!float.TryParse(temps[3], out temp3))
                    temp3 = -1;
                if (!float.TryParse(temps[4], out temp4))
                    temp4 = -1;
                var tempData = new DataModel(temps[0], temp1,
                    temp2, temp3 / 100, temp4, temps[5]);
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
            sb.Append("ID").Append(",").Append("Holding time").Append(", ").
                Append("Break times").Append(", ").Append("Discount").Append(", ").
                Append("Expect Fare").Append(", ").Append("Middle Value").Append(", ").
                Append("Output Discount").Append(", ").Append("Output Fare").Append(", ").
                Append("Correct");
            return sb.ToString();
        }

        public class DataModel : BaseModel
        {

            private string ucID;

            private int spendTimes;

            private int breakNumber;

            private float expectPrice;

            private float expectDiscount;

            private float outputPrice;

            private float outputDiscount;

            private bool priceCorrect;

            private bool discountCorrect;

            private string middleValue;

            public DataModel(String id, int st,
                int bn, float expectdiscount, 
                float expectprice, string middle)
            {
                ucID = id;
                AtIndex = int.Parse(ucID.Substring(2));
                spendTimes = st;
                breakNumber = bn;
                expectPrice = expectprice;
                expectDiscount = expectdiscount;
                middleValue = middle;

                if (spendTimes < 0 || spendTimes > 44640 || breakNumber<0|| breakNumber>11)
                {
                    inputCorrect = false;
                    return;
                }

                int a;
              
                switch ((spendTimes-1)/60)
                {
                    case 0:
                        outputDiscount = (breakNumber > 1) ? 0F : 0.01F;
                        break;
                    case 1:
                        outputDiscount = (breakNumber > 2) ? 0F : 0.015F;
                        break;
                    case 2:
                        outputDiscount = (breakNumber > 3) ? 0F : 0.02F;
                        break;
                    case 3:
                        outputDiscount = (breakNumber > 3) ? 0F : 0.025F;
                        break;
                    case 4:
                        outputDiscount = (breakNumber > 3) ? 0F : 0.025F;
                        break;
                    default:
                        outputDiscount = (breakNumber > 6) ? 0F : 0.03F;
                        break;
                }

                if (spendTimes == 0) outputDiscount = 0;

                discountCorrect = (outputDiscount == expectDiscount) ? true : false;
                double ttt = (double) outputDiscount;
                float tt = 1.0f - outputDiscount;

                double t =  tt * spendTimes * 0.15;

                outputPrice = (float)Math.Round(t+25, 2);
               
                priceCorrect = (outputPrice == expectPrice) ? true : false;
                inputCorrect = true;
            }

            public override bool ResultDecide()
            {
                return priceCorrect && discountCorrect;
            }

            public override string Output()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(AtIndex).Append(",").Append(ucID).Append(",").
                    Append(spendTimes).Append(",").Append(breakNumber).Append(",").
                    Append(ExpectDiscount).Append(",").Append(ExpectPrice).Append(",").Append(MiddleValue).Append(",");
                if (inputCorrect)
                    sb.Append(OutputDiscount).Append(",").Append(OutputPrice).Append(",").
                        Append(PriceCorrect);
                else
                    sb.Append("NULL").Append(", ").Append("NULL").Append(", ").
                        Append("NULL");
                return sb.ToString();
            }

            public String SpendTimes
            {
                get { return spendTimes.ToString(); }
            }

            public String BreakNumber
            {
                get { return breakNumber.ToString(); }
            }

            public String ExpectPrice
            {
                get
                {
                    return expectPrice == -1.0f ? "Illegal input" : expectPrice.ToString("f2");
                }
            }

            public String ExpectDiscount
            {
                get
                {
                    return expectDiscount == -1.0f ? "\\" : expectDiscount.ToString("P2");
                }
            }

            public String OutputPrice
            {
                get
                {
                    return outputPrice.ToString("f2");
                }
            }

            public String OutputDiscount
            {
                get
                {
                    return outputDiscount.ToString("P2");
                }
            }

            public String MiddleValue
            {
                get { return middleValue; }
            }

         
            public string PriceCorrect
            {
                get
                {
                    return priceCorrect ? "True" : "False";
                }
            }

            public string DiscountCorrect
            {
                get
                {
                    return discountCorrect ? "True" : "False";
                }
            }


        }


    }

}
