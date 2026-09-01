class Account {
    private decimal balance; //decimal eftersom både double och float har avrundningsfel

    // => är en read only funktion, håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf
    public decimal Balance => balance; //"expression-bodied property"


    //I ett riktigt scenario sköter de funktionerna bara requests till API:er och databser eller nått på helt andra servrar
    public void ReportBalance() {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {Balance} kr.\n");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ReadLine();
    }
    public void Deposit() { //Wrap-battle
        Console.Clear();
        Console.WriteLine("Hur mycket vill du sätta in?");
        
        DepositCheck(Console.ReadLine());
    }
    void DepositCheck(string userInput) {
        decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

        if (decimal.TryParse(userInput, out validAmount)){ //TryParse är typ en ternary operator
            balance += validAmount;
            Console.WriteLine($"\n{validAmount} kr insatt på kontot.");
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadLine();
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
            Console.WriteLine($"\n{validAmount} kr uttaget.");
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadLine();
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nOgiltligt belopp, försök igen...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);  
        }
    }
}