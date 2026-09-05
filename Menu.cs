using System.Security.Cryptography;

static class Menu { //public static så att jag inte behöver construct:a ett menu object för att använda denna klassen
    private const string arrow = "--> ";

    /*
    "tuple" array (det är basically en struct array i C-språket, enda coola skillnaden är hur du nästan instantly kan deklarera den)
    Action<Type> gör att funktions call:et fungerar på vilket skapat objekt ifrån Account klassen, typ
    

    Lambda call:et: 
    
        (currentAccount) => currentAccount.Deposit(Console.ReadLine())

    Är detsamma som att skriva en liten function som såhär:
    
        void SomeName(Account account) {
            account.Deposit();
        }

    */
    public static (string label, Action<Account> callback)[] menuList = { //string namn + funktion (typ void pointer grejen i C)
        ("Deposit", (currentAccount) => currentAccount.Deposit()),
        ("Withdraw", (currentAccount) => currentAccount.Withdraw()),
        ("Balance", (currentAccount) => currentAccount.DisplayBalance(true)),
        ("Transaction", (currentAccount) => currentAccount.DisplayTransactionHistory()),
        ("Gurka", (currentAccount) => currentAccount.gurkTransaction()),
        ("Exit", (generic) => Environment.Exit(0)),
    };

    public static void drawMenu(Account currentAccount) {
        int menuIndex = 0;
        
        do {
            Console.Clear();

        //print menuList strings
            for (int i = 0; i < menuList.Length; i++) {
                if (menuIndex == i) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(arrow);
                    Console.WriteLine($"{i +1}. {menuList[i].label}");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{i +1}. {menuList[i].label}");
                }
            }
            
        //check input for switching between options and activating selected option
            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key) {                             //viktigt att gräva upp .Key i ConsoleKeyInfo variabeln...
                case ConsoleKey.W or ConsoleKey.UpArrow: { //testar bara en variation
                        if (menuIndex > 0){
                            menuIndex--;
                        }
                    }
                    break;
                case ConsoleKey.S:                       //min personliga preferens: "drip filter" stil
                case ConsoleKey.DownArrow: {
                        if (menuIndex < (menuList.Length - 1)) {
                            menuIndex++;
                        }  
                    }
                    break;
                case ConsoleKey.Enter: {
                        menuList[menuIndex].callback(currentAccount);
                    }
                    break;
                default:
                    break;
            }

/*
    ORGINAL, MEN UPPGIFTEN VILLE HA ETT SWITCH-CASE STATEMENT ISTÄLLET SÅ....

            if (key.Key == ConsoleKey.W || key.Key == ConsoleKey.UpArrow) {
                if (menuIndex > 0) {
                    menuIndex--;
                }
            }
            else if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.DownArrow) {
                if (menuIndex < (menuList.Length - 1)) {
                    menuIndex++;
                }
            }
            if (key.Key == ConsoleKey.Enter) {
                menuList[menuIndex].callback(currentAccount);
            }
*/

        } while (true);
    }
}