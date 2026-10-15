namespace BeginCsh.Base;
using System.Globalization;

public static class InputHelper{
public static T Prompt<T> (string prompt, Func<T, bool>? validator = null, string errorMessage = "Вы ввели недопустимое значение")
        where T: IParsable<T>
        {
            while (true)
        {
            Console.WriteLine(prompt);
            string? input = Console.ReadLine();

            if (input is null)
        {
            throw new InvalidOperationException("Входной поток был неожиданно закрыт.");
        }

            if 
        (
            !string.IsNullOrWhiteSpace(input)
            && T.TryParse(input, CultureInfo.CurrentCulture, out T? result)
            && result is not null
            && (validator == null || validator(result))
        )
            {
                return result;
            }
            
            Console.WriteLine(errorMessage);
        }
        }

public static TEnum PromptEnum<TEnum> (string prompt, Func<TEnum, bool>? validator = null, string errorMessage = "Недопустимое значение")
        where TEnum : struct, Enum
        {
            while (true)
        {
            Console.WriteLine(prompt);
            string? input = Console.ReadLine();

            if (input is null)
        {
            throw new InvalidOperationException("Входной поток был неожиданно закрыт.");
        }

        if 
        (
            !string.IsNullOrWhiteSpace(input)
            && Enum.TryParse(input, ignoreCase: true, out TEnum result) 
            && Enum.IsDefined(result)
            && (validator == null || validator(result))
        )
            {
                return result;
            }
        
            Console.WriteLine(errorMessage);
        }   
        }
}