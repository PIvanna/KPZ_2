using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using KPZ_2.Models.Enums;

namespace KPZ_2.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AppointmentStatus status)
            {
                switch (status)
                {
                    case AppointmentStatus.Scheduled:
                        return new SolidColorBrush(Colors.Orange);
                    case AppointmentStatus.Completed:
                        return new SolidColorBrush(Colors.Green);
                    case AppointmentStatus.Cancelled:
                        return new SolidColorBrush(Colors.Red);
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}