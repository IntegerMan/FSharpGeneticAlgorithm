using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MattEland.FSharpGeneticAlgorithm.Logic;
using MattEland.FSharpGeneticAlgorithm.Genetics;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    internal class MainViewModel : NotifyPropertyChangedBase
    {
        private readonly Random _random = new Random();
        private Simulator.GameState _state;

        public MainViewModel()
        {
            RandomizeCommand = new ActionCommand(RandomizeBrain);
            BrainCommand = new ActionCommand(GetArtificialIntelligenceMove);
            ResetCommand = new ActionCommand(Reset);

            RandomizeBrain();

            Reset();
        }

        private void RandomizeBrain()
        {
            _brain = Genes.getRandomPriorities(_random);
        }

        public ActionCommand RandomizeCommand { get; }

        public IEnumerable<ActorViewModel> Actors => _actors;

        private void GetArtificialIntelligenceMove()
        {
            State = Simulator.handleBrainMove(_brain, _state, _random);
        }

        public ActionCommand ResetCommand { get; }
        public ActionCommand BrainCommand { get; }

        private void Reset()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            State = new Simulator.GameState(world, Simulator.SimulationState.Simulating, 30);
        }

        private readonly ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        private Genes.SquirrelPriorities _brain;

        public Simulator.GameState State
        {
            get => _state;
            set
            {
                _state = value;

                _actors.Clear();
                foreach (var actor in _state.World.Actors.Where(a => a.IsActive))
                {
                    _actors.Add(new ActorViewModel(actor));
                }

                OnPropertyChanged(nameof(GameStatusBrush));
                OnPropertyChanged(nameof(GameStatusText));
                OnPropertyChanged(nameof(TurnsLeftText));
            }
        }
        
        public string GameStatusText => _state.SimState switch
            {
                Simulator.SimulationState.Won => "Won",
                Simulator.SimulationState.Lost => "Lost",
                _ => "Simulating"
            };

        public Brush GameStatusBrush => _state.SimState switch
            {
                Simulator.SimulationState.Won => Brushes.MediumSeaGreen,
                Simulator.SimulationState.Lost => Brushes.LightCoral,
                _ => Brushes.LightGray
            };

        public string TurnsLeftText => _state.TurnsLeft == 1 
                ? "1 Turn Left" 
                : $"{_state.TurnsLeft} Turns Left";
    }
}