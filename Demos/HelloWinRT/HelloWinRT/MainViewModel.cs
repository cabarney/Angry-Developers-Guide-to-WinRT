using System.ComponentModel.DataAnnotations;

namespace HelloWinRT
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Number = 44;
        }
        private double? _number;
        [Range(0, 100, ErrorMessage = "Number must be between 0-100")]
        public double? Number
        {
            get { return _number; }
            set
            {
                SetPropertyAndValidate(() => _number, x => _number = x, value, "Number");
            }
        }
    }
}