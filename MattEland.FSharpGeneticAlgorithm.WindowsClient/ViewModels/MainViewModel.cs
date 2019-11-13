using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ResetCommand = new ActionCommand(Reset);
            RandomizeCommand = new ActionCommand(RandomizeWorlds);
            BrainCommand = new ActionCommand(AdvanceToNextGeneration);

            RandomizeWorlds();
            RandomizeBrains();
        }

        private void RandomizeWorlds()
        {
            _worlds = WorldGeneration.makeWorlds(_random, 5);
            SimulateCurrentPopulation();
        }

        private void SimulateCurrentPopulation()
        {
            if (!Population.Any()) return;

            var pop = Population.Select(p => p.Brain.Model);
            var result = Genetics.Population.simulateGeneration(_worlds, GetRandomForSimulation(), pop).ToList();
            UpdatePopulation(result);
        }

        private Random GetRandomForSimulation() => new Random(42);

        public SimulationResultViewModel SelectedBrain
        {
            get => _brain;
            set {
                if (_brain != value)
                {
                    _brain = value;
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public ActionCommand ResetCommand { get; }
        public ActionCommand RandomizeCommand { get; }

        public ObservableCollection<SimulationResultViewModel> Population { get; } =
            new ObservableCollection<SimulationResultViewModel>();

        private void RandomizeBrains()
        {
            var generation = Genetics.Population.simulateFirstGeneration(_worlds, _random);

            UpdatePopulation(generation);
        }

        private void Reset()
        {
            RandomizeWorlds();
            RandomizeBrains();
        }

        private void UpdatePopulation(IEnumerable<Genes.SimulationResult> generation)
        {
            Population.Clear();
            foreach (var result in generation)
            {
                Population.Add(new SimulationResultViewModel(result));
            }

            SelectedBrain = Population.First();
        }

        private void AdvanceToNextGeneration()
        {
            var priorResults = Population.Select(p => p.Model).ToArray();

            var brains = Genetics.Population.mutateBrains(_random, priorResults.Select(r => r.brain).ToArray());
            var generation = Genetics.Population.simulateGeneration(_worlds, GetRandomForSimulation(), brains).ToList();

            UpdatePopulation(generation);
        }

        public ActionCommand BrainCommand { get; }

        private SimulationResultViewModel _brain;
        private World.World[] _worlds;
    }
}