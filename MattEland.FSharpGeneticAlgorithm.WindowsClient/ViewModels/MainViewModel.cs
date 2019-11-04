﻿using System;
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
        private Simulator.GameState _state;

        public MainViewModel()
        {
            RandomizeCommand = new ActionCommand(RandomizeBrain);
            BrainCommand = new ActionCommand(GetArtificialIntelligenceMove);
            ResetCommand = new ActionCommand(Reset);

            RandomizeBrain();

            Reset();
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

        private void RandomizeBrain() =>
            Brain = new BrainInfoViewModel(Genes.getRandomChromosome(_random));

        private void GetArtificialIntelligenceMove() => 
            State = Simulator.handleChromosomeMove(_state, _random, Brain.Model);

        public ActionCommand ResetCommand { get; }
        public ActionCommand BrainCommand { get; }

        private void Reset()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            State = new Simulator.GameState(world, Simulator.SimulationState.Simulating, 30);
        }

        private readonly ObservableCollection<ActorViewModel> _actors = new ObservableCollection<ActorViewModel>();
        private BrainInfoViewModel _brain;

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