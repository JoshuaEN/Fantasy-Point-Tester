using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FantasyTanks___Fantasy_Point_Tester
{
    /// <summary>
    /// Modified from http://stackoverflow.com/a/10258083
    /// </summary>
    [ValueConversion(typeof(ReplayPlayer), typeof(Brush))]
    public class ColorGradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var property_name = parameter as string;
            var replayPlayer = value as ReplayPlayer;

            if (replayPlayer == null)
                return Brushes.Transparent;

            double number = (double)replayPlayer.GetType().GetProperty(property_name).GetValue(replayPlayer);


            double min = 0;
            double max = 0;

            if(property_name.StartsWith("OFP_"))
            {
                min = replayPlayer.OFP_Min;
                max = replayPlayer.OFP_Max;
            }
            else if(property_name.StartsWith("NFP_"))
            {
                min = replayPlayer.NFP_Min;
                max = replayPlayer.NFP_Max;
            }
            else
            {
                throw new ArgumentException("Invalid Parameter: Should start with either OFP_ or NFP_");
            }

            if (max == min)
                return Brushes.Transparent;

            if (max <= min)
            {
                throw new ArgumentException("Parameter not valid. MaxDouble has to be greater then MinDouble.");
            }

            if (number >= min && number <= max && number != 0)
            {
                Color color = Colors.Transparent;
                byte r = 0;
                byte g = 0;
  

                if (number < 0) {
                    r = 255;
                    min = Math.Min(-Math.Abs(min), -Math.Abs(max));
                    max = 0;
                }
                else if(number > 0)
                {
                    g = 255;
                    max = Math.Max(Math.Abs(min), Math.Abs(max));
                    min = 0;
                }

                double range = (max - min);
                double factor = 255 / Math.Abs(range);
                double alpha = Math.Abs(number * factor);
                color = Color.FromArgb((byte)alpha, r, g, 0);

                SolidColorBrush brush = new SolidColorBrush(color);
                return brush;
            }

            // Fallback brush
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
