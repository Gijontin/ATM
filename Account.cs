/*
    EGENTLIGEN BÖR DE FLESTA FNUKTIONER BARA SKICKA SERVER REQUESTS 
    OCH HANTERA UI UPDATES BASERAT PÅ SERVERNS SVAR...
*/

public class Account {
    private readonly int _kontonummer;
    private AccountType _kontotyp; //sikta på databasdriven/value objects eller polymorfiska typer i framtiden
    private string? _fullname;
    private readonly int _pin;
    private readonly DateTime _skapat;
    private decimal _balance; //decimal eftersom både double och float har avrundningsfel

    //private readonly List<bankomat.transaktionsHistorik> _transaktioner = new List<bankomat.transaktionsHistorik>();
    private readonly List<bankomat.transaktionsHistorik> _transaktioner = [];
    private void RegisterTransaktionsHistorik(string typ, decimal belopp) {
        var tran = new bankomat.transaktionsHistorik{
            tidsstämpel = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            transaktionsTyp = typ,
            belopp = belopp,
        };
        _transaktioner.Add(tran);
    }
//-------------------------------------------------------------------------------------------------------------------
// => i detta fallet blir en readonly funktion(?), håller själva balance privat så att den inte koms åt genom huvudprogrammet iaf
    public List<bankomat.transaktionsHistorik> Transaktioner => _transaktioner;
    public decimal Balance => _balance; //"expression-bodied property"
//-------------------------------------------------------------------------------------------------------------------
    //bara för transaktionshistorik recording grejs
    private const string _uttag = "uttag"; //(egentligen)server-side var.
    private const string _insättning = "insättning"; //(egentligen)server-side var.
//--------------------------------------------------------------------------------------------------------------------
    
public Account(int kontonummer, AccountType kontotyp, string? fullname, int pin, DateTime skapat, decimal balance) {
    _kontonummer = kontonummer;
    _kontotyp    = kontotyp;
    _fullname    = fullname;
    _pin          = pin;
    _skapat      = skapat;
    _balance     = balance;
}

    public void DisplayTransaktionsHistorik() {
        
        Console.Clear();
        Console.WriteLine("Transaktionshistorik för konto:\n");

        foreach (var tran in Transaktioner) {
            Console.WriteLine($"Transaktionsdatum: {tran.tidsstämpel} - Transaktionstyp {tran.transaktionsTyp} - Transaktionsvärde: {tran.belopp:F2}"); // :F2 är (F)ormat specifier för decimaler, :0.00 funkar också
        }

        Console.WriteLine(); //simpel skiljerad (vet ej om är baseline idiomatin eller baseline idiotin)
        DisplayBalance(false);
        genUtil.pressToContinue();
    }
    public void gurkTransaction() { //GURKA

        Console.Clear();
        
        string gurka = "10";
        //int gurka2 = 0;

        //Console.WriteLine($"Gurka #2 har värdet {gurka2}");

        if (int.TryParse(gurka, out int gurka2)) {
            Console.WriteLine($"Gurktransaction completed. Gurka #2 har nu värdet {gurka2}");
        }

        genUtil.pressToContinue();
    }
    public void DisplayBalance(bool clearTerminal) { //if false betyder (oftast) att du återanvänder funktionen's WriteLine del i andra funktioner...
        
        if (clearTerminal) 
            {Console.Clear();}

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Ditt saldo: {Balance:0.00} kr.\n"); // :F2 är (F)ormat specifier för decimaler, :0.00 funkar också
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

            _balance += validAmount;
            //registerTransaktion(thDepo, validAmount);
            RegisterTransaktionsHistorik(_insättning, validAmount);
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
            if (validAmount > _balance) {
                throw new Exception("\nOtillräckligt saldo, försök igen....\n");
            }
            if (validAmount <= 0) {
                throw new Exception("\nOgiltligt belopp, beloppet måste vara större än 0.\n");
            }
            
            _balance -= validAmount;
            //registerTransaktion(thWith, validAmount);
            RegisterTransaktionsHistorik(_uttag, validAmount);
            Console.WriteLine($"\n{validAmount} kr uttaget.\n");
            DisplayBalance(false);
            genUtil.pressToContinue();
        }
}