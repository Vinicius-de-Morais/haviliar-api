using System.ComponentModel;

namespace Haviliar.Infra.Utils;

[TypeConverter(typeof(EncryptedIntConverter))]
public readonly struct EncryptedInt
{
    public int Value { get; }

    public EncryptedInt(int value) => Value = value;

    public static implicit operator int(EncryptedInt e) => e.Value;

    public static implicit operator EncryptedInt(int value) => new(value);
}
