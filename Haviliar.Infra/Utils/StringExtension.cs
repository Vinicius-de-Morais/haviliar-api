using System.Text.RegularExpressions;

namespace Haviliar.Infra.Utils;

public static class StringExtension
{

    public static string ReplaceSimbols(this string valor)
    {
        return Regex.Replace(valor, @"[\*|'\,\.\-\\\/\(\)\{\}\[\]\""\+\|]", "");
    }
    public static bool IsCpf(this string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || !cpf.Distinct().Skip(1).Any())
        {
            return false;
        }
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = digito + resto.ToString();
        return cpf.EndsWith(digito);
    }

    public static bool IsPhoneNumber(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return Regex.IsMatch(phone, @"^(1[1-9]|[2-9][0-9])9\d{8}$");
    }

}
