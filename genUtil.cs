using System.Dynamic;
using System.Text.RegularExpressions;

public static class genUtil {
    public static void pressToContinue() {
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ReadLine();
    }

    public static bool testaValideraNamn(string input, out string validInput) {
        validInput = input?.Trim();

        //lite valideringscheckar dådå...
        if (string.IsNullOrWhiteSpace(validInput)) {
            return false;
        }

        if (Regex.IsMatch(validInput, "[^\\p{L}-']")) {
            return false;
        }

        //vid succee
        return true;
    }

    public static bool testaValideraMejl(string input, out string validInput) {
        validInput = input?.Trim();

        if (string.IsNullOrWhiteSpace(validInput)) {
            return false;
        }

        if (!Regex.IsMatch(validInput, "[@]")) {
            return false;
        }

        return true;
    }
    public static bool testaValideraPIN(out int validInput) {
        Console.WriteLine();
        //char[] input = []; //nah klydd, C# har massa färdiga funktioner för strings så...
        string input = "";

        while (true) {
            var key = Console.ReadKey(intercept: true);

            if (char.IsDigit(key.KeyChar)) {
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) {
                    input = input[..^1];
                    Console.Write("\b \b"); //tur att man höll på med text-adventure i C asså...
                    continue; //skip the other if-checks cuz behövs ej om du tryckt backspace
                }
                if (key.Key == ConsoleKey.Enter) {
                    if (input.Length != 4) { //Remember: order on if checks matter...
                        validInput = 0;
                        return false;
                    }
                    if (!int.TryParse(input, out validInput)) {
                        return false;
                    } else {
                        return true;
                    }
                }
                if (input.Length < 4 && key.Key != ConsoleKey.Backspace) {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
        }
    }
}