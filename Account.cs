class Account {
    private decimal balance; //decimal eftersom både double och float har avrundningsfel
    private readonly List<string> transaktioner = new List<String>(); //readonly för att stoppa injektion och/eller att du byter ut den för att fibbla med värdena men tillåter fortfarande .Add() och .Remove()...

    // => i detta fallet blir en readonly funktion(?), håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf
    public decimal Balance => balance; //"expression-bodied property"
    public List<string> Transaktioner => transaktioner;

    private void pressToContinue() {
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ReadLine();
    }
    public void gurkTransaction() {
        string gurka = "10";
        //int gurka2 = 0;

        //Console.WriteLine($"Gurka #2 har värdet {gurka2}");

        if (int.TryParse(gurka, out int gurka2)) {
            Console.WriteLine($"Gurktransaction completed. Gurka #2 har nu värdet {gurka2}");
        }

        pressToContinue();
    }
    //I ett riktigt scenario (tror jag) sköter de funktionerna bara requests till API:er och databser eller nått på helt andra servrar
    public void registerTransaktion(string Transaktionstyp, decimal belopp) {
        transaktioner.Add($"Transaktionsdatum: {DateTime.Now} - Transaktionstyp: {Transaktionstyp} - Transaktionsvärde: {belopp}");
    }
    public void DisplayTransactionHistory() {

        Console.Clear();
        Console.WriteLine("Transaktionshistorik för konto:\n");

        foreach (var tran in Transaktioner) {
            Console.WriteLine(tran);
        }

        Console.WriteLine(); //simpel skiljerad (vet ej om är baseline idiomatin eller baseline idiotin)
        DisplayBalance(false);
        pressToContinue();
    }
    public void DisplayBalance(bool clearTerminal) { //if false betyder (oftast) att du återanvänder funktionen's WriteLine del i andra funktioner...
        
        if (clearTerminal) 
            {Console.Clear();}

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {Balance} kr.\n");
        Console.ResetColor();

        if (clearTerminal) //sjukt fult med två av samma if checks, I know...
            {pressToContinue();}
    }
    public void Deposit() { //Wrap-battle
        Console.Clear();
        Console.WriteLine("Hur mycket vill du sätta in?");
        
        DepositCheck(Console.ReadLine());
    }
        void DepositCheck(string userInput) {
            decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

            if (decimal.TryParse(userInput, out validAmount)){ //TryParse är nästan också en ternary operator
                balance += validAmount;
                registerTransaktion("insättning", validAmount);
                Console.WriteLine($"\n{validAmount} kr insatt på kontot.\n");
                DisplayBalance(false);
                pressToContinue();
            }
            else {       
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOgiltliga tecken eller belopp, försök igen...\n");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
            }
        }

    public void Withdraw() { //McWrapper
        Console.Clear();
        Console.WriteLine("Hur mycket vill du ta ut?"); //måste ha en wrapper eller något för prompten dyker inte upp due to ReadLine i parametenr...

        WithdrawCheck(Console.ReadLine());
    }
        void WithdrawCheck(string userInput) {
            decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

            if (decimal.TryParse(userInput, out validAmount) && (balance >= validAmount)){ //TryParse är typ en ternary operator
                balance -= validAmount;
                registerTransaktion("uttag", validAmount);
                Console.WriteLine($"\n{validAmount} kr uttaget.\n");
                DisplayBalance(false);
                pressToContinue();
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOgiltligt belopp, försök igen...\n");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);  
            }
        }
}