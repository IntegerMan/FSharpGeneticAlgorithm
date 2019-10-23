using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Simulator.GameState _state;

        public MainViewModel()
        {
            ResetCommand = new RelayCommand(Reset);

            Reset();
        }

        public string TextGrid => BuildAsciiGrid();

        public string GameStatusText
        {
            get
            {
                if (_state.SimState == Simulator.SimulationState.Won)
                {
                    return "Won";
                }
                if (_state.SimState == Simulator.SimulationState.Lost)
                {
                    return "Lost";
                }

                return "Simulating";
            }
        }

        public string TurnsLeftText => 
            _state.TurnsLeft == 1 
                ? "1 Turn Left" 
                : $"{_state.TurnsLeft} Turns Left";

        public RelayCommand ResetCommand { get; }

        private void Reset()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            State = new Simulator.GameState(world, Simulator.SimulationState.Simulating, 30);
        }

        public Simulator.GameState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(TextGrid));
                OnPropertyChanged(nameof(GameStatusText));
                OnPropertyChanged(nameof(TurnsLeftText));
            }
        }

        private string BuildAsciiGrid()
        {
            var world = _state.World;
            var sb = new StringBuilder();

            for (int y = 1; y < world.MaxY; y++)
            {
                for (int x = 1; x < world.MaxX; x++)
                {
                    sb.Append(World.getCharacterAtCell(x, y, world));
                }

                // Advance to the next line
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}