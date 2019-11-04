using JetBrains.Annotations;
using MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [UsedImplicitly]
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
