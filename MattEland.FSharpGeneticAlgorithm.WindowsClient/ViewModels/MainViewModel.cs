using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
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

        public IEnumerable<ActorViewModel> Actors => _actors;

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

                State = value.Last();

                _actors.Clear();
                foreach (var actor in State.World.Actors.Where(a => a.IsActive))
                {
                    _actors.Add(new ActorViewModel(actor));
                }

                // Notify all properties changed
                OnPropertyChanged(string.Empty);
            }
        }

        public ActionCommand BrainCommand { get; }

        private readonly ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        private BrainInfoViewModel _brain;
        private Simulator.GameState[] _gameResult;

        public Simulator.GameState State { get; private set; }

        public string GameStatusText => State.SimState switch
            {
                Simulator.SimulationState.Won => "Won",
                Simulator.SimulationState.Lost => "Lost",
                _ => "Simulating"
            };

        public Brush GameStatusBrush => State.SimState switch
            {
                Simulator.SimulationState.Won => Brushes.MediumSeaGreen,
                Simulator.SimulationState.Lost => Brushes.LightCoral,
                _ => Brushes.LightGray
            };

        public string TurnsLeftText => State.TurnsLeft == 1 
                ? "1 Turn Left" 
                : $"{State.TurnsLeft} Turns Left";
    }
}