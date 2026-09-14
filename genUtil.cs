using System.Dynamic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public static class genUtil {
    //HashSet<T> är egentligen snabbare när man kommer upp i hundratals array:er
    private static readonly char[] tillåtnaLokal = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789._-+".ToCharArray();
    private static readonly char[] tillåtnaDomän = "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ0123456789-.".ToCharArray();
    public static void pressToContinue() {
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ReadLine();
    }
    public static void SkrivFärgPaus(string str, ConsoleColor färg) {
        Console.ForegroundColor = färg;
        Console.WriteLine(str);
        Console.ResetColor();
        Thread.Sleep(1500);
    }
    //catch-all grejen som alla tester åker igenom, fungerar för allt om den används med rätt logik...
    public record ValideringsResultat<T>(bool succee, T value, string msg); //sjukt tips från LLM efter jag frågade om tips för förbättringar, exdee
    public static ValideringsResultat<string> testaValideraMejl(string? input) {
        string validInput = input?.Trim() ?? ""; // = if input is empty then validInput gets the empty string of ""

        //skriver över min gamla funktions if-checks till en test-bool med msg lista och sedan loopar...
        //styrkan ligger i felmeddelanden för usern som inte är lika goto-brutala som catch
        //enda jobbiga är att bool logiken måste skifta för att foreach loopen bara ska fånga 'false' resultat...
        var regelCheck = new List<(Func<bool> regel, string mdl)> { //testar en "strongly typed List, eller vad det kallas"
            (() => !string.IsNullOrWhiteSpace(validInput), "Fyll i mejladressfältet."),
            (() => !Regex.IsMatch(validInput, @"[()<>[\]:;,/\\""'`|{}^%$!?*=~£€¥₽₹₿¢§©®±×÷•¶°]"), "Ogiltliga tecken"),
            (() => (validInput.Count(c => c == '@') == 1), "Endast ett(1) '@' tillåtet."),
            (() => !(validInput.Contains("..") || validInput.Contains("-.") || validInput.Contains(".-")), "Ogiltliga tecken."),
        };

        foreach (var (rgl, mdl) in regelCheck) { //hade nog funkat med tuple array också men for-loop och tuplearray-namn.Length limiter
            if (!rgl()) {
                return new(false, validInput, mdl); //return bool, string, (string)meddelande -- 
                                            //just i string inputs känns det överflödigt men är mer 
                                            //flex när man sedan använder <int> och andra typer...
            }
        }

        /*
        
        TO DO:
            splitta validInput också, precis som i gamla funktionen och kör några extra checks
            där också genom att skapa en strongly typed lista igen och loopa igenom...

        */

        //Console.WriteLine("\nMEJL BLEV RÄTT"); //MLG PRINTF.DEBUG
        return new(true, validInput, "Giltlig mejladress!"); //return bool, str, (string)message;
    }
    public static ValideringsResultat<string> testaValideraFulltNamn(string? input) {
        
        if (string.IsNullOrWhiteSpace(input)) {
            return new(false, "", ""); //alt. log msg: null string
        }

        if (input.Length < 3 || input.Length > 64) {
            return new(false, input, ""); //alt. log msg: orimlig längd på hela namnet
        }

        string[] delar = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries); //Trim() och removeemptyentries tar bort splits som råkar framkomma av...
                                                                                         //dubbelmellanrum, tror jag...
        if (delar.Length < 2) {
            return new(false, input, ""); //alt. log msg: saknar två namn
        }
        foreach (string p in delar) {
            if (!p.All(char.IsLetter)) {
                return new(false, input, ""); //alt. log msg: namnet innehåller mer än bara bokstäver
            }
        }
/*
        FUNKAR INTE FÖR MIN REFACTORING, MEN DET VAR KUL ATT TESTA REGEX (REGEX ÄR VISST BÄTTRE (optimization/klydd -mässigt) FÖR ATT CHECKA FASTA FORMAT-REGLER PÅ TEXT
                                                                          TEX VAD SOM MÅSTE INKLUDERAS ELLER EXKLDERAS)
        
        if (Regex.IsMatch(input, "[^\\p{L}-']")) {
            return new(false, "", ""); //alt. log msg: fult namn
        }
*/

        //Console.WriteLine("\nNAMN BLEV RÄTT"); //MLG PRINTF.DEBUG
        return new(true, input, ""); //alt. log msg: Aaaw hell yeah.
    }
    public static ValideringsResultat<string> testaValideraPIN(string? input) {

        if (string.IsNullOrWhiteSpace(input)) {
            return new(false, "", "");
        }
        if (input.Length != 4) {
            return new(false, input, "");
        }
        if (!input.All(c => c >= '0' && c <= '9')) { //Tillåter endast UNICODE 1 till 9 siffror, filtrerar bort arabiska och devanagari numeraler
            return new(false, input, "");
        }
        
        //Console.WriteLine("\nPIN BLEV RÄTT\n"); //MLG PRINTF.DEBUG
        return new(true, input, "");
    }
    public static ValideringsResultat<AutentiseringData> testaValideraHash(string? pinput, AutentiseringData data) {
        
        if (string.IsNullOrWhiteSpace(pinput) || data.saltLakrits == null || data.hashKaka == null) {
            return new(false, data, "");
        }

        //hasha:a (data.salt + pinput) = tempHash
        using var pbkdf2 = new Rfc2898DeriveBytes(pinput, data.saltLakrits, data.iterationer, HashAlgorithmName.SHA256);
        byte[] tempHash = pbkdf2.GetBytes(32);
        

        bool jämntidsNormaliserarMatchCheck = CryptographicOperations.FixedTimeEquals(tempHash, data.hashKaka);
        
        if (jämntidsNormaliserarMatchCheck) {
            return new(true, data, ""); //great Success, very najs!
        } else {
            return new(false, data, ""); //Icke matchande PIN
        }
    }
}  

/*
    public static bool testaValideraMejl(string input, out string validInput) { //notera: ej skriven att tillåta lokala mejlformat
        validInput = input?.Trim();

    //Speedrun-if (low hanging fruit filtrering för snabbast resultat)
        //gör man en fullständig "RFC 5322" sweep är det nog onödigt med en speedrun check överhuvudtaget
        if (!string.IsNullOrWhiteSpace(validInput) ||
            Regex.IsMatch(validInput, @"[()<>[\]:;,/\\""'`|{}^%$!?*=~£€¥₽₹₿¢§©®±×÷•¶°]") || //finns bättre sätt men vill testa Regex usage
            validInput.Count(c => c == '@') != 1 ||
            validInput.Contains("..") ||
            validInput.Contains("-.") || validInput.Contains(".-"))
        {                                           //skiter i K&N vingar här, blir otydligare då...
            return false;
        }
        
    //Finkammnings-if...
        string[] split = validInput.Split('@');
        string lokal = split[0];
        string domän = split[1];
        
    //LOGIK ATT SKRIVA:
        //försöka fånga ogiltliga tecken
        //försök ÄVEN att fånga struktur-fel (som tex, börjar med en punkt eller ".-" etc)

    //LOKAL CHECK
        if (string.IsNullOrWhiteSpace(lokal) ||
            lokal.StartsWith(".") || lokal.EndsWith(".") ||
            !lokal.All(c => tillåtnaLokal.Contains(c)))     //om en symbol i strängen inte matchar whitelist-filtret
        {
            return false;
        }
    //DOMÄN CHECK
        if (!domän.Contains(".") || 
            (domän.StartsWith("-") || domän.EndsWith("-")) ||
            !domän.All(c => tillåtnaDomän.Contains(c)))    //om en symbol i strängen inte matchar whitelist-filtret
        
        {
            return false;
        }
        
            return true;
        }

    public static bool testaValideraPIN(out int validInput) {
        //char[] input = []; //nah klydd, C# har massa färdiga funktioner för strings så...
        string input = "";

        while (true) {
            var key = Console.ReadKey(intercept: true);

            if (char.IsDigit(key.KeyChar)) {
                //fel att de enter och backspace ligger innanför "om det är en siffra"-checken också...
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
*/