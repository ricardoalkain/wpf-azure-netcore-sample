using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace TTMS.UI.Helpers
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = value.GetType();
            var enumValueName = value.ToString();

            if (!enumType.IsEnum)
                throw new ArgumentException($"Paramenter {nameof(enumType)} must be an enum");

            var attributes = enumType.GetField(enumValueName).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            return enumValueName;

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
