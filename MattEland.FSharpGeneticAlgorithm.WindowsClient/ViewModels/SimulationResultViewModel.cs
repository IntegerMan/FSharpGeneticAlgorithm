using System.Collections.Generic;
using System.Collections.ObjectModel;
using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    public class SimulationResultViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<GameStateViewModel> _states;
        private int _currentIndex;

        public SimulationResultViewModel(IEnumerable<Simulator.GameState> states)
        {
            _states = new ObservableCollection<GameStateViewModel>();
            foreach (var state in states)
            {
                _states.Add(new GameStateViewModel(state));
            }
        }

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
    }
}