using FinanceCalc.Domain.Abstractions;
using FinanceCalc.Domain.Extensions;
using FinanceCalc.Domain.Models;
using FinanceCalc.Domain.Models.Bonds;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinanceCalc.ViewModels
{
    public class BondEditorViewModel : INotifyPropertyChanged
    {
        private IBondData _data;
        private IBond _computed;

        public BondEditorViewModel () : this(null) {}

        public BondEditorViewModel(IReadOnlyBondData? data = null)
        {
            _data = new BondDataViewModel(OnDataChanged, 
                data?.AsMutable() ?? new BondData
                {
                    Name = string.Empty,
                    Ticker = string.Empty,
                    Nominal = 0,
                    Cost = 0,
                    DateStart = DateTime.Today,
                    DateEnd = DateTime.Today.AddYears(1)
                });
            _computed = new Bond(_data);
        }

        public IBondData Data
        {
            get => _data;
            set
            {
                if (value == _data)
                    return;
                _data = value;
                OnPropertyChanged();
                Recalculate();
            }
        }

        public IBond Computed
        {
            get => _computed;
            private set
            {
                _computed = value;
                OnPropertyChanged(nameof(Computed));
            }
        }

        public void Recalculate()
        {
            Computed = new Bond(_data);
        }

        private void OnDataChanged(object? sender, PropertyChangedEventArgs e) 
        { 
            Recalculate(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
