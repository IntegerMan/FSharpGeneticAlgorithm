using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    /// <summary>
    /// Represents a single state in a single squirrel's simulation run
    /// </summary>
    public class GameStateViewModel : NotifyPropertyChangedBase
    {
        private readonly Simulator.GameState _state;

        public GameStateViewModel([NotNull] Simulator.GameState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));

            foreach (var actor in _state.World.Actors.Where(a => a.IsActive))
            {
                Actors.Add(new ActorViewModel(actor));
            }
        }

        public ICollection<ActorViewModel> Actors { get; } = new List<ActorViewModel>();

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