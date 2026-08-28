class Account {
    private decimal balance; //decimal eftersom både double och float har avrundningsfel

    // => är en read only funktion, håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf
    public decimal Balance => balance; //"expression-bodied property"


    //I ett riktigt scenario sköter de funktionerna bara requests till API:er och databser eller nått på helt andra servrar
    public void ReportBalance() {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {Balance} kr.\n");
        Console.ForegroundColor = ConsoleColor.White;
    }
    public void Deposit(string userInput) {

        Console.WriteLine("Hur mycket vill du sätta in?"); //måste ha en wrapper eller något för prompten dyker inte upp due to ReadLine i parametenr...

        decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

        if (decimal.TryParse(userInput, out validAmount)){ //TryParse är typ en ternary operator
            balance += validAmount;
            Console.WriteLine($"{validAmount} kr insatt på kontot.");
            //return validAmount;
        }
        else {       
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nOgiltliga tecken eller belopp, försök igen...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);

            //return 0m;
        }
    }

    public void Withdraw(string userInput) {

        Console.WriteLine("Hur mycket vill du ta ut?"); //måste ha en wrapper eller något för prompten dyker inte upp due to ReadLine i parametenr...

        decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

        if (decimal.TryParse(userInput, out validAmount) && (balance > validAmount)){ //TryParse är typ en ternary operator
            balance -= validAmount;
            Console.WriteLine($"{validAmount} kr uttaget.");
            //return validAmount;
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nOgiltligt belopp, försök igen...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);  
        }
        //return 0m;
    }
}