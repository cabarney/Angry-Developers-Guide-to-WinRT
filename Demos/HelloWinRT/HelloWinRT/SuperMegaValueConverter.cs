using System;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace HelloWinRT
{
    internal class SuperMegaValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var megaParameter = parameter as SuperMegaConverterParameter;
            if (megaParameter == null)
                return value;

            if (value == null)
                return megaParameter.TargetNullValue ?? "";

            if (!string.IsNullOrWhiteSpace(megaParameter.Format))
            {
                var formatted = string.Format("{0:"+megaParameter.Format+"}", value);
                return formatted;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return null;
            
            var megaParameter = parameter as SuperMegaConverterParameter;
            if (megaParameter == null)
                return value;

            switch (megaParameter.Type.ToLower())
            {
                case "int":
                    int testInt;
                    return int.TryParse(value as string, out testInt) ? testInt : value;
                case "double":
                    double testDouble;
                    return double.TryParse(value as string, out testDouble) ? testDouble : value;
                case "datetime":
                    DateTime testDateTime;
                    return DateTime.TryParse(value as string, out testDateTime) ? testDateTime : value;
            }
            return value;
        }
    }

    public class SuperMegaConverterParameter
    {
        public string Format { get; set; }
        public string Type { get; set; }
        public string TargetNullValue { get; set; }
    }
}