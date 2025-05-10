using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigrin.Models
{
    public class TransactionValueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var transaction = value as Transaction;
            if (transaction == null)
                return Colors.White;

            return transaction.Type == TransactionType.Income
                ? Color.FromArgb("#FF939E5A")
                : Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
