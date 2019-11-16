using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.FSharpGeneticAlgorithm.Genetics;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    /// <summary>
    /// Represents a single state in a single squirrel's simulation run
    /// </summary>
    public class GameStateViewModel : NotifyPropertyChangedBase
    {
        private readonly States.GameState _state;

        public GameStateViewModel([NotNull] States.GameState state, Genes.ActorChromosome brain)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));

            foreach (var actor in _state.World.Actors.Where(a => a.IsActive))
            {
                Actors.Add(new ActorViewModel(actor));
            }

            var values = new Dictionary<WorldPos.WorldPos, double>();

            for (int y = 1; y <= state.World.MaxY; y++)
            {
                for (int x = 1; x <= state.World.MaxX; x++)
                {
                    var pos = new WorldPos.WorldPos(x, y);
                    values[pos] = Genes.evaluateTile(brain, state.World, pos);
                }
            }

            var min = values.Values.Min();
            var max = values.Values.Max();

            foreach (var (pos, value) in values)
            {
                HeatMap.Add(new HeatMapViewModel(pos, value, min, max));
            }
        }

        public ICollection<ActorViewModel> Actors { get; } = new List<ActorViewModel>();
        public ICollection<HeatMapViewModel> HeatMap { get; } = new List<HeatMapViewModel>();

        public string GameStatusText => _state.SimState switch
        {
            States.SimulationState.Won => "Won",
            States.SimulationState.Lost => "Lost",
            _ => "Simulating"
        };

        public Brush GameStatusBrush => _state.SimState switch
        {
            States.SimulationState.Won => Brushes.MediumSeaGreen,
            States.SimulationState.Lost => Brushes.LightCoral,
            _ => Brushes.LightGray
        };

        public string TurnsLeftText => _state.TurnsLeft == 1
            ? "1 Turn Left"
            : $"{_state.TurnsLeft} Turns Left";
    }

    public class HeatMapViewModel : NotifyPropertyChangedBase
    {
        private readonly double _min;
        private readonly double _max;

        public HeatMapViewModel(WorldPos.WorldPos pos, double value, double min, double max)
        {
            Pos = pos;
            Value = value;
            _min = min;
            _max = max;
        }

        public WorldPos.WorldPos Pos { get; }
        public double Value { get; }
        public string Text => $"{Pos.X}, {Pos.Y} = {Value:F2}";
        // Subtract 1 since my data's indexes start at 1 instead of 0
        public int PosX => (Pos.X - 1) * 10;
        public int PosY => (Pos.Y - 1) * 10;
        public Brush Fill
        {
            get
            {
                var val = Value;
                var min = _min;
                var max = _max;

                if (min < 0)
                {
                    max -= min;
                    val -= min;
                }
                
                byte rgb = (byte) Math.Round((val / max) * 255);
                var color = Color.FromRgb(rgb, rgb, rgb);
                return new SolidColorBrush(color);
            }
        }
    }
}