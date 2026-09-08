static class Menu { //public static så att jag inte behöver construct:a ett menu object för att använda denna klassen
    private const string arrow = "--> ";
    private static void StorBankHeader() {
        
        string header = "StorBank - För Dem Som Tål Riktigt Mycket Bank";
        string midPad = "|$$$|$$$|$$$|$$$|$$$|$$$|";

        int midPadLen = midPad.Length * 2;
        int width = Console.WindowWidth;
        int padding = (width - header.Length) / 2;
        int innerWidth = width - midPadLen;
        int pad = Math.Max((innerWidth - header.Length) / 2, 0);

        string centered = 
            midPad + 
            new string(' ', pad) +
            header +
            new string(' ', Math.Max(innerWidth - pad - header.Length, 0)) +
            midPad; //We got "center div'd text" at home

        string line = new string('=', width);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(line);
        Console.WriteLine(centered);
        Console.WriteLine(line);
        Console.WriteLine();
        Console.ResetColor();
    }
    
    private static void inputAccountCreate() {
        /*
        
        SKRIVA JÄVLA BRA MED SÄKERHETSSPÄRRAR SÅ ATT ALL INPUT BLIR KORREKT HÄR, YO...

        //fyll i allt som behövs för en KontoData record och sedan skicka den till factoryn inuti denna metoden
        
        */
    }
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
    //Mom says: We got tryParse at home... (funkar bara i Menu due to return logic, ändra den om du vill ha en mer global util)
    private static bool tryParseMenuInput(ConsoleKeyInfo tangentbordsknapptryck, out int siffra) {

        /*
            GGEZ ASCII KOD TRICK:

            ConsoleKey key = enums

            Enums börjar på ASCII kod 48 (dvs ConsoleKey.D0 = 48)

            Trycker du då 0 så blir det enum 0 som är lika med 48. 48 minus 48 är lika med en int 0
                logiken fortsätter så; Enum 1 = 49, 49 minus 48 är lika med int 1...
        */

        siffra = -1; //sätter ett ogiltligt värde utifall skiten fail:ar (dvs garanterat return.ar false då)

        //Löjligt strul för att få resultatet helt terminal agnostiskt ...Git-Bash producerar annan ASCII om 
        // man kör programmet genom dennes dotnet run tex...
        if (char.IsDigit(tangentbordsknapptryck.KeyChar)) {
            siffra = tangentbordsknapptryck.KeyChar - '0';
            siffra--; //DÖDLIGT VIKTG DETALJ FÖR ATT DET SKA BLI RÄTT MED ARRAY INDEX ÅTKOMSTEN

            //returnerar true om siffra är inom array-spannet
            return siffra >= 0 && siffra <= (menuList.Length - 1); //glöm ej minus ettan för dat array nummerering...
        }
        return false; 
    }
    public static (string label, Action<Account> callback)[] menuList = { //string namn + funktion (typ void pointer grejen i C)
        ("Deposit",     (currentAccount) => currentAccount.Deposit()),
        ("Withdraw",    (currentAccount) => currentAccount.Withdraw()),
        ("Balance",     (currentAccount) => currentAccount.DisplayBalance(true)),
        ("Transaction", (currentAccount) => currentAccount.DisplayTransaktionsHistorik()),
        ("Gurka",       (currentAccount) => currentAccount.gurkTransaction()),
        ("Exit",        (generic)        => {Console.Clear(); Environment.Exit(0);}),
    };

    public static void accountMenu(Account currentAccount) {
        int menuIndex = 0;
        do {
            try {
                //do {
                    Console.Clear();

                    StorBankHeader();
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
                        default: {
                            int direktMenyVal = 0;
                            //riktig hacky custom TryParse variant för att jag vill kunna instant-välja i menyn via nummerisk input också...
                                if (tryParseMenuInput(key, out direktMenyVal)) {
                                    menuList[direktMenyVal].callback(currentAccount);
                                }
                            }
                            break;
                    }
                //} while (true); 
            } catch (Exception ex) {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Thread.Sleep(1000);  
            }
        } while (true); 
    }

    public static void mainMenu() {
        int menuIndex = 0;

        do {
            try {

                Console.Clear();

            //"GUI"

                StorBankHeader();

                for (int i = 0; i < 3; i++) {
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
                switch (key.Key) {
                    default: {
                        int direktMenyVal;
                        if (tryParseMenuInput(key, out direktMenyVal)){
                            //meny[direktMenyVal] för denna funktionen
                        }
                    }
                    break;
                }
            } catch {
                
            }
        } while (true);
    }
}