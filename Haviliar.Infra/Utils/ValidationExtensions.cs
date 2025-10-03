using FluentValidation;

namespace Haviliar.Infra.Utils;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(value => value.IsCpf());
    }

    public static IRuleBuilderOptions<T, string> MustBePhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(value => value.IsPhoneNumber());
    }
}
