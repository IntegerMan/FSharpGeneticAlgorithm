using System.Text;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient
{
    internal class MainViewModel
    {
        private readonly World.World _world;

        public MainViewModel()
        {
            _world = WorldGeneration.makeTestWorld(false);
        }

        public string TextGrid => BuildAsciiGrid();

        private string BuildAsciiGrid()
        {
            var sb = new StringBuilder();

            for (int y = 1; y < _world.MaxY; y++)
            {
                for (int x = 1; x < _world.MaxX; x++)
                {
                    sb.Append(World.getCharacterAtCell(x, y, _world));
                }

                // Advance to the next line
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}