using System.ComponentModel;

namespace Haviliar.Infra.Utils;

public class EncryptedIntConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(int) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        System.Globalization.CultureInfo? culture,
        object value
    )
    {
        if (value is int intValue)
        {
            return new EncryptedInt(intValue);
        }
        return base.ConvertFrom(context, culture, value);
    }
}
