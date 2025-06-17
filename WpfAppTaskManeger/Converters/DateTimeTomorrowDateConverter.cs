using System.Globalization;
using System.Windows.Data;

namespace WpfAppTaskManeger.Converters
{
    /// <summary>
    /// Конвертер, который определяет, соответствует ли заданная дата завтрашней дате.
    /// Возвращает true, если дата завтра, иначе false.
    /// </summary>
    class DateTimeTomorrowDateConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует значение даты в булево значение, указывающее, является ли дата завтрашней.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate)
            {
                return dueDate.Date == DateTime.Today.AddDays(1).Date;
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
