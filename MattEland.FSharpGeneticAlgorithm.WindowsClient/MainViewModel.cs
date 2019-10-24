using System.Text;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    internal class MainViewModel : NotifyPropertyChangedBase
    {
        private Simulator.GameState _state;

        public MainViewModel()
        {
            ResetCommand = new ActionCommand(Reset);
            MoveCommand = new ActionCommand(Move);

            Reset();
        }

        public ActionCommand MoveCommand { get; }

        private void Move(object direction)
        {
            // Parameter validation / cleansing
            direction ??= "";
            direction = direction.ToString().ToLowerInvariant();

            // Translate from the command parameter to the GameCommand in F#
            Commands.GameCommand command = direction switch
            {
                "nw" => Commands.GameCommand.MoveUpLeft,
                "n" => Commands.GameCommand.MoveUp,
                "ne" => Commands.GameCommand.MoveUpRight,
                "w" => Commands.GameCommand.MoveLeft,
                "e" => Commands.GameCommand.MoveRight,
                "sw" => Commands.GameCommand.MoveDownLeft,
                "s" => Commands.GameCommand.MoveDown,
                "se" => Commands.GameCommand.MoveDownRight,
                _ => Commands.GameCommand.Wait
            };

            // Process the action and update our new state
            State = Simulator.simulateTurn(_state, command);
        }

        public ActionCommand ResetCommand { get; }

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

        public string TextGrid => BuildAsciiGrid();

        public string GameStatusText => _state.SimState switch
            {
                Simulator.SimulationState.Won => "Won",
                Simulator.SimulationState.Lost => "Lost",
                _ => "Simulating"
            };

        public string TurnsLeftText => _state.TurnsLeft == 1 
                ? "1 Turn Left" 
                : $"{_state.TurnsLeft} Turns Left";


        private string BuildAsciiGrid()
        {
            var world = _state.World;
            var sb = new StringBuilder();

            for (int y = 1; y <= world.MaxY; y++)
            {
                for (int x = 1; x <= world.MaxX; x++)
                {
                    sb.Append(World.getCharacterAtCell(x, y, world));
                }

                // Advance to the next line
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}