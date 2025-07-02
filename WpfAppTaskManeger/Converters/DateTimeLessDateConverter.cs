using System.Globalization;
using System.Windows.Data;

namespace WpfAppTaskManeger.Converters
{
    /// <summary>
    /// Конвертер, который определяет, является ли заданная дата прошедшей по отношению к текущей дате.
    /// Возвращает true, если дата в прошлом, иначе false.
    /// </summary>
    public class DateTimeLessDateConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует значение даты в булево значение, указывающее, является ли дата прошедшей.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate)
            {
                return dueDate.Date < DateTime.Today.Date;
            }
            return false;
        }

        /// <summary>
        /// Метод ConvertBack не реализован, так как это односторонний конвертер.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
