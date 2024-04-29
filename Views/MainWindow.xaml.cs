using MelodiousHaven.ViewsModels;
using System.Windows;
namespace MelodiousHaven
{
    // <summary>
    // Interaction logic for MainWindow.xaml
    // </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindow_VM();
        }
    }
}