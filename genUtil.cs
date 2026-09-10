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
}