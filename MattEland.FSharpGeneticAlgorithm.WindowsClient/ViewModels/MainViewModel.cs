using System;
using MattEland.FSharpGeneticAlgorithm.Genetics;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    internal class MainViewModel : NotifyPropertyChangedBase
    {
        private readonly Random _random = new Random();

        public MainViewModel()
        {
            RandomizeCommand = new ActionCommand(RandomizeBrain);
            BrainCommand = new ActionCommand(SimulateBrain);

            RandomizeBrain();
        }

        public BrainInfoViewModel Brain
        {
            get => _brain;
            set {
                if (_brain != value)
                {
                    _brain = value;
                    OnPropertyChanged();
                }
            }
        }

        public ActionCommand RandomizeCommand { get; }

        private void RandomizeBrain()
        {
            Brain = new BrainInfoViewModel(Genes.getRandomChromosome(_random));
            SimulateBrain();
        }

        private void SimulateBrain()
        {
            GameResult = new SimulationResultViewModel(Simulator.simulate(_random, _brain.Model));
        }

        public SimulationResultViewModel GameResult
        {
            get => _gameResult;
            set
            {
                if (Equals(value, _gameResult)) return;
                _gameResult = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand BrainCommand { get; }

        private BrainInfoViewModel _brain;
        private SimulationResultViewModel _gameResult;
    }
}