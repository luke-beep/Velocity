using Microsoft.UI.Xaml.Data;
using static Velocity.Services.BackdropSelectorService;

namespace Velocity.Helpers;

public class BackdropTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not BackdropType backdrop)
        {
            return string.Empty;
        }

        return backdrop switch
        {
            BackdropType.Mica => "Mica",
            BackdropType.MicaAlt => "Mica Alt",
            BackdropType.DesktopAcrylic => "Acrylic",
            _ => "Mica"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}