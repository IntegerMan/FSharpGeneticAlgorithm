using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MattEland.FSharpGeneticAlgorithm.Genetics;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    public class SimulationResultViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<GameStateViewModel> _states;
        private int _currentIndex;

        public SimulationResultViewModel(Genes.SimulationResult result)
        {
            Model = result;
            _states = new ObservableCollection<GameStateViewModel>();
            foreach (var state in Model.results.SelectMany(r => r.states))
            {
                _states.Add(new GameStateViewModel(state, result.brain));
            }
            Brain = new BrainInfoViewModel(result.brain);
        }

        public double Score => Model.totalScore;
        public string ScoreText => $"Score: {Score:F1}";
        public int Age => Model.brain.age;

        public string DisplayText => $"Gen {Age} - {ScoreText}";

        public IEnumerable<GameStateViewModel> States => _states; 

        public GameStateViewModel SelectedState => _states[CurrentIndex];

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (value == _currentIndex) return;
                _currentIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedState));
            }
        }

        public int MaxStateIndex => _states.Count - 1;
        public BrainInfoViewModel Brain { get; }
        public Genes.SimulationResult Model { get; }

        public void AdvanceTimer()
        {
            if (CurrentIndex == MaxStateIndex)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;
            }
        }
    }
}