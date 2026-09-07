class Account {
/*
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
        genUtil.pressToContinue();
    }
*/
    private const string thWith = "uttag"; //(egentligen)server-side var.
    private const string thDepo = "insättning"; //(egentligen)server-side var.

//-------------------------------------------------------------------------------------------------------------------
// => i detta fallet blir en readonly funktion(?), håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf

    //(egentligen)server-side op.
    private readonly List<transaktionsHistorik> _transaktioner = new List<transaktionsHistorik>();
    public List<transaktionsHistorik> _Transaktioner => _transaktioner;
    private decimal balance; //decimal eftersom både double och float har avrundningsfel
    public decimal Balance => balance; //"expression-bodied property"
    private readonly List<string> transaktioner = new List<string>(); //readonly för att stoppa injektion och/eller att du byter ut den för att fibbla med värdena men tillåter fortfarande .Add() och .Remove()...
    public List<string> Transaktioner => transaktioner;

    private void registerTransaktionsHistorik(string typ, decimal belopp) {
        var tran = new transaktionsHistorik{
            tidsstämpel = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            transaktionsTyp = typ,
            belopp = belopp,
        };
        _transaktioner.Add(tran);
    }
    public void displayTransaktionsHistorik() {
        
        Console.Clear();
        Console.WriteLine("Transaktionshistorik för konto:\n");

        foreach (var tran in _Transaktioner) {
            Console.WriteLine($"Transaktionsdatum: {tran.tidsstämpel} - Transaktionstyp {tran.transaktionsTyp} - Transaktionsvärde: {tran.belopp}");
        }

        Console.WriteLine(); //simpel skiljerad (vet ej om är baseline idiomatin eller baseline idiotin)
        DisplayBalance(false);
        genUtil.pressToContinue();
    }
//-------------------------------------------------------------------------------------------------------------------
    public void gurkTransaction() {

        Console.Clear();
        
        string gurka = "10";
        //int gurka2 = 0;

        //Console.WriteLine($"Gurka #2 har värdet {gurka2}");

        if (int.TryParse(gurka, out int gurka2)) {
            Console.WriteLine($"Gurktransaction completed. Gurka #2 har nu värdet {gurka2}");
        }

        genUtil.pressToContinue();
    }
    //I ett riktigt scenario (tror jag) sköter de funktionerna bara requests till API:er och databser eller nått på helt andra servrar
/*
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
        genUtil.pressToContinue();
    }
*/

    public void DisplayBalance(bool clearTerminal) { //if false betyder (oftast) att du återanvänder funktionen's WriteLine del i andra funktioner...
        
        if (clearTerminal) 
            {Console.Clear();}

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {Balance} kr.\n");
        Console.ResetColor();

        if (clearTerminal) //sjukt fult med två av samma if checks, I know...
            {genUtil.pressToContinue();}
    }
    public void Deposit() { //Wrap-battle
        Console.Clear();
        Console.WriteLine("Hur mycket vill du sätta in?");
        
        DepositCheck(Console.ReadLine());
    }
        void DepositCheck(string userInput) {
            decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)
            
            if (!decimal.TryParse(userInput, out validAmount)) { //TryParse är nästan också en ternary operator
                throw new Exception("\nOgiltliga tecken eller belopp, försök igen...\n");
            }

            balance += validAmount;
            //registerTransaktion(thDepo, validAmount);
            registerTransaktionsHistorik(thDepo, validAmount);
            Console.WriteLine($"\n{validAmount} kr insatt på kontot.\n");
            DisplayBalance(false);
            genUtil.pressToContinue();
        }

    public void Withdraw() { //McWrapper
        Console.Clear();
        Console.WriteLine("Hur mycket vill du ta ut?"); //måste ha en wrapper eller något för prompten dyker inte upp due to ReadLine i parametenr...

        WithdrawCheck(Console.ReadLine());
    }
        void WithdrawCheck(string userInput) {
            decimal validAmount = 0m; //notera: decimal-typen har ändelsen 'm' ('d' är för double-typen)

        //Kom ihåg, mest troligaste fel först cuz nanosekundsoptimisering är sjukt viktigt... ;P
            if (!decimal.TryParse(userInput, out validAmount)) {
                throw new Exception("\nNågot blev fel, försök igen...\n");
            }
            if (validAmount > balance) {
                throw new Exception("\nOtillräckligt saldo, försök igen....\n");
            }
            if (validAmount <= 0) {
                throw new Exception("\nOgiltligt belopp, beloppet måste vara större än 0.\n");
            }
            
            balance -= validAmount;
            //registerTransaktion(thWith, validAmount);
            registerTransaktionsHistorik(thWith, validAmount);
            Console.WriteLine($"\n{validAmount} kr uttaget.\n");
            DisplayBalance(false);
            genUtil.pressToContinue();
        }
}