using System.ComponentModel.DataAnnotations;

namespace HelloSilverlight
{
    public class MainViewModel : ViewModelBase
    {
        private double? _number;
        [Range(0,100,ErrorMessage="Number must be between 0-100")]
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