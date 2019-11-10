using System;
using System.Linq;
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
            GameResult = Simulator.simulate(_random, _brain.Model);
        }

        public Simulator.GameState[] GameResult
        {
            get => _gameResult;
            set
            {
                if (_gameResult == value) return;

                _gameResult = value;

                State = new GameStateViewModel(value.Last());
            }
        }

        public ActionCommand BrainCommand { get; }

        private BrainInfoViewModel _brain;
        private Simulator.GameState[] _gameResult;
        private GameStateViewModel _state;

        public GameStateViewModel State
        {
            get => _state;
            private set
            {
                if (Equals(value, _state)) return;
                _state = value;
                OnPropertyChanged();
            }
        }
    }
}