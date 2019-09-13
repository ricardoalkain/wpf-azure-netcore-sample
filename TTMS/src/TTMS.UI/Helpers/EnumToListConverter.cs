using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace TTMS.UI.Helpers
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
    public class EnumToListConverter : MarkupExtension, IValueConverter
    {
        public struct ValueDescription
        {
            public object Value { get; set; }

            public string Description { get; set; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetAllValuesAndDescriptions(value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        private string GetEnumDescription(Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            return value.ToString();
        }

        private IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException($"Paramenter {nameof(enumType)} must be an enum");

            return Enum.GetValues(enumType).Cast<Enum>()
                .Select((e) => new ValueDescription()
                {
                    Value = e,
                    Description = GetEnumDescription(e)
                })
                .OrderBy(v => v.Description)
                .ToList();
        }
    }
}
