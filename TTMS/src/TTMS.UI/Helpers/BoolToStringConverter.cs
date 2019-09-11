using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TTMS.UI.Helpers
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var values = (parameter as string)?.Split(',');
            var trueStr = bool.TrueString;
            var falseStr = bool.FalseString;

            if (values != null)
            {
                trueStr = values.Length > 0 ? values[0] : trueStr;
                falseStr = values.Length > 1 ? values[1] : falseStr;
            }

            return boolValue ? trueStr : falseStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
