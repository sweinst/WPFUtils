using System.Collections.ObjectModel;
using System.Windows;
using WPFUtils.Controls;

namespace WPFUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MultiComboBoxListData = new ObservableCollection<MultiSelectComboBox.ItemData>()
            {
                new MultiSelectComboBox.ItemData(){
                    Id=0,
                    ViewName="Tom",
                    IsCheck=false
                },
                new MultiSelectComboBox.ItemData(){
                    Id=1,
                    ViewName="John Doe",
                    IsCheck=false
                },
                new MultiSelectComboBox.ItemData(){
                    Id=2,
                    ViewName="Harry",
                    IsCheck=false
                },
                new MultiSelectComboBox.ItemData(){
                    Id=3,
                    ViewName="Ma Six",
                    IsCheck=false
                },
                new MultiSelectComboBox.ItemData(){
                    Id=4,
                    ViewName="Zhao Qi",
                    IsCheck=true
                },
            };

            TestMSCB.ItemsSource = MultiComboBoxListData;
        }

        private ObservableCollection<MultiSelectComboBox.ItemData> MultiComboBoxListData;
    }
}
