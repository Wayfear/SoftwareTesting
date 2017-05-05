using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SoftwareTesting.Pages.Experiments.Data
{
    /// <summary>
    /// Interaction logic for SplitPage1.xaml
    /// </summary>
    public partial class SplitPage1 : UserControl
    {
        private string path;

        public static ObservableCollection<BaseModel> DataSource = new ObservableCollection<BaseModel>();

        public SplitPage1() 
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
                var tempData = new Test2DataModel(int.Parse(temps[0]), temps[1], temps[2]);
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
            sb.Append("Index").Append(",").Append("Input Date").Append(", ").
                Append("Expext Date").Append(", ").Append("Output Date").Append(", ").
                Append("Is Correct").Append(", ").Append("Input Correct");
            return sb.ToString();
        }

        public class Test2DataModel: BaseModel
        {
            private int index;

            private DateTime inputDate;

            private DateTime outputDate;

            private DateTime expectDate;

            private bool isCorrect;

       

            public Test2DataModel(int index, string inDate, string exDate)
            {
                AtIndex = index;

                string[] temp = inDate.Split('/');

                try
                {
                    inputDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));

                    temp = exDate.Split('/');

                    expectDate = new DateTime(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));

                    outputDate = inputDate.AddDays(1);

                    isCorrect = (expectDate == outputDate);

                    inputCorrect = true;

                }
                catch (Exception e)
                {
                    inputCorrect = false;
                    outputDate = new DateTime(1970,1,1);
                }

            }


            public override string Output()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(AtIndex).Append(",").Append(InputDate.ToString("yyyy/MM/dd")).Append(",").
                    Append(ExpectDate.ToString("yyyy/MM/dd")).Append(",");
                if (inputCorrect)
                    sb.Append(OutputDate.ToString("yyyy/MM/dd")).Append(",")
                        .Append(IsCorrect).Append(",").Append(InputCorrect);
                else
                    sb.Append("NULL").Append(", ").Append("NULL")
                        .Append(", ").Append(InputCorrect);
                return sb.ToString();
            }

            public override bool ResultDecide()
            {
                return isCorrect;
            }


            public DateTime InputDate
            {
                set
                {
                    inputDate = value;
                }
                get
                {
                    return inputDate;
                }
            }

            public DateTime OutputDate
            {
                set
                {
                    outputDate = value;
                }
                get
                {
                    return outputDate;
                }
            }

            public DateTime ExpectDate
            {
                set
                {
                    expectDate = value;
                }
                get
                {
                    return expectDate;
                }
            }

          
            public string IsCorrect
            {
                get
                {
                    return isCorrect ? "True" : "False";
                }
            }




        }


    }

}
