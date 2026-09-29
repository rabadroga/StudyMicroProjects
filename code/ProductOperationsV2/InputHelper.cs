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

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            if (!T.TryParse(input, CultureInfo.CurrentCulture, out T? result) || result is null)
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            if (validator != null && !validator(result))
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            return result;
        }
        }
}