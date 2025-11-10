using FinanceCalc.Domain.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinanceCalc.ViewModels
{
    public class BondDataViewModel : IBondData, INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _ticker = string.Empty;
        private decimal _nominal;
        private decimal _cost;
        private decimal? _coupon;
        private int? _couponsPerYear;
        private DateTime? _nextCouponDate;
        private DateTime _dateStart = DateTime.Today;
        private DateTime _dateEnd = DateTime.Today;
        private DateTime? _offerDate;
        private bool _needQualification;    

        public BondDataViewModel() { }

        public BondDataViewModel(PropertyChangedEventHandler? editAction, IReadOnlyBondData source)
        {
            _name = source.Name;
            _ticker = source.Ticker;
            _nominal = source.Nominal;
            _cost = source.Cost;
            _coupon = source.Coupon;
            _couponsPerYear = source.CouponsPerYear;
            _dateStart = source.DateStart;
            _dateEnd = source.DateEnd;
            _offerDate = source.OfferDate;
            _needQualification = source.NeedQualification;
            if (editAction is not null)
            {
                PropertyChanged += editAction;
            }
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string Ticker
        {
            get => _ticker;
            set => SetField(ref _ticker, value);
        }

        public decimal Nominal
        {
            get => _nominal;
            set => SetField(ref _nominal, value);
        }

        public decimal Cost
        {
            get => _cost;
            set => SetField(ref _cost, value);
        }

        public decimal? Coupon
        {
            get => _coupon;
            set => SetField(ref _coupon, value);
        }

        public int? CouponsPerYear
        {
            get => _couponsPerYear;
            set => SetField(ref _couponsPerYear, value);
        }

        public DateTime? NextCouponDate
        {
            get => _nextCouponDate;
            set => SetField(ref _nextCouponDate, value);
        }

        public DateTime DateStart
        {
            get => _dateStart;
            set => SetField(ref _dateStart, value);
        }

        public DateTime DateEnd
        {
            get => _dateEnd;
            set => SetField(ref _dateEnd, value);
        }

        public DateTime? OfferDate
        {
            get => _offerDate;
            set => SetField(ref _offerDate, value);
        }

        public bool NeedQualification
        {
            get => _needQualification;
            set => SetField(ref _needQualification, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}