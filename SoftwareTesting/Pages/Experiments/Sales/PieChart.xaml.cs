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

namespace SoftwareTesting.Pages.Experiments.Sales
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : UserControl
    {


        public BasicPage1()
        {
            var pieData = new PieData(FilePage.DataSource);
           
            InitializeComponent();
            pieChart.ItemsSource = pieData;


        }

    }


    public class Pair
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }

    public class PieData : ObservableCollection<Pair>
    {
        public PieData()
        {

        }

        public PieData(IEnumerable<BaseModel> list)
        {
            int passed = 0, failed = 0, incorrect = 0;

            foreach (var v in list)
            {
                if (!v.inputCorrect)
                {
                    incorrect++;
                }
                else if (v.ResultDecide())
                {
                    passed++;
                }
                else
                {
                    failed++;
                }
            }

            Add(new Pair { Label = "Passed", Value = passed });
            Add(new Pair { Label = "Failed", Value = failed });
            Add(new Pair { Label = "Incorrect input", Value = incorrect });

        }

    }
}
