class Account {
    private decimal balance; //decimal eftersom både double och float har avrundningsfel

    // => är en read only funktion, håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf
    public decimal Balance => balance;


    public void ReportBalance() {
        //Console.WriteLine($"Du har {balance} kr på ditt konto.");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {balance} kr.\n");
        Console.ForegroundColor = ConsoleColor.White;
    }
    public decimal Deposit(string userInput) {
        decimal validValue = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

        if (decimal.TryParse(userInput, out validValue)){ //TryParse är typ en ternary operator
            return validValue;
        }
        else {       
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nOgiltliga tecken, försök igen...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);

            return 0m;
        }
    }

    public decimal Withdraw(string userInput) {
        return 0m;
    }
}