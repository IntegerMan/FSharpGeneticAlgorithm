using System;
using System.Collections.ObjectModel;
using System.Linq;
using MattEland.FSharpGeneticAlgorithm.Genetics;
using MattEland.FSharpGeneticAlgorithm.Logic;
using Microsoft.FSharp.Core;

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
            var initialState = Simulator.buildStartingState(_random);

            Population.Clear();
            for (int i = 0; i <= 10; i++)
            {
                var brain = Genes.getRandomChromosome(_random, _nextId++);
                Population.Add(new SimulationResultViewModel(Simulator.simulate(_random, brain, initialState)));
            }

            SelectedBrain = Population.First();
        }

        private void AdvanceToNextGeneration()
        {
        }

        public ActionCommand BrainCommand { get; }

        private SimulationResultViewModel _brain;
        private int _nextId = 1;
    }
}