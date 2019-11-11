using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MattEland.FSharpGeneticAlgorithm.Genetics;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    internal class MainViewModel : NotifyPropertyChangedBase
    {
        private readonly Random _random = new Random();

        public MainViewModel()
        {
            RandomizeCommand = new ActionCommand(RandomizeBrains);
            BrainCommand = new ActionCommand(AdvanceToNextGeneration);

            RandomizeBrains();
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

        public ActionCommand RandomizeCommand { get; }

        public ObservableCollection<SimulationResultViewModel> Population { get; } =
            new ObservableCollection<SimulationResultViewModel>();

        private void RandomizeBrains()
        {
            var generation = Genetics.Population.simulateFirstGeneration(_random);

            UpdatePopulation(generation);
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
            var generation = Genetics.Population.mutateAndSimulateGeneration(_random, priorResults);

            UpdatePopulation(generation);
        }

        public ActionCommand BrainCommand { get; }

        private SimulationResultViewModel _brain;
    }
}