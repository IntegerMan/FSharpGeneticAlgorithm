using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
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
            AdvanceCommand = new ActionCommand(AdvanceToNextGeneration);
            Advance10Command = new ActionCommand(AdvanceToNext10Generation);
            Advance100Command = new ActionCommand(AdvanceToNext100Generation);

            RandomizeWorlds();
            RandomizeBrains();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (sender, e) => _brain.AdvanceTimer(); 
            timer.Start();
        }

        private void RandomizeWorlds()
        {
            _worlds = WorldGeneration.makeWorlds(_random, 1);
            SimulateCurrentPopulation();
        }

        private void SimulateCurrentPopulation()
        {
            if (!Population.Any()) return;

            var pop = Population.Select(p => p.Brain.Model);
            var result = Genetics.Population.simulateGeneration(_worlds, pop).ToList();
            UpdatePopulation(result);
        }

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

        public bool ShowHeatMap
        {
            get => _showHeatMap;
            set
            {
                if (value == _showHeatMap) return;
                _showHeatMap = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand ResetCommand { get; }
        public ActionCommand RandomizeCommand { get; }
        public ActionCommand AdvanceCommand { get; }
        public ActionCommand Advance10Command { get; }
        public ActionCommand Advance100Command { get; }

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

        private void AdvanceToNext10Generation()
        {
            AdvanceGenerations(10);
        }

        private void AdvanceToNext100Generation()
        {
            AdvanceGenerations(100);
        }

        private void AdvanceGenerations(int numGenerations)
        {
            var priorResults = Population.Select(p => p.Model).ToArray();

            var generation = Genetics.Population.mutateAndSimulateMultiple(_random, _worlds, numGenerations, priorResults);

            UpdatePopulation(generation);
        }

        private void AdvanceToNextGeneration()
        {
            AdvanceGenerations(1);
        }

        private SimulationResultViewModel _brain;
        private World.World[] _worlds;
        private bool _showHeatMap;
    }
}