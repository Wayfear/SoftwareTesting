using FirstFloor.ModernUI.Windows.Controls;
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

namespace SoftwareTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public abstract class BaseModel
    {
        private int index;

        public bool inputCorrect;

        public abstract string Output();

        public abstract bool ResultDecide();
       
        public int AtIndex
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

     
        public string InputCorrect
        {
            get
            {
                return inputCorrect ? "True" : "False";
            }
        }

    }
}
