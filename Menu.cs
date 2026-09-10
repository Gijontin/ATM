using System.Text.RegularExpressions;

static class Menu {
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
    private static void ValideringsTextFörNamn(string str1, bool ärGiltligt) {
        Console.WriteLine();
        Console.Write("Namnet: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{str1} ");
        Console.ResetColor();
        Console.Write("är ");

        if (ärGiltligt) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("giltligt!\n\n");
            Console.ResetColor();  
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ogiltligt!");
            Console.ResetColor();
            Console.Write(" Försök igen...\n\n");
            
        }
        genUtil.pressToContinue();
    }
    private static string getName() {
        Console.Clear();
        StorBankHeader();
        //parse deez bad boiz
        string name       = "";
        string surname    = "";
        string helaNamnet = "";

        do {
            try {
            //FED name INPUT + CHECK
                Console.WriteLine("Ange ditt förnamn:");
                if (!genUtil.testaValideraNamn(Console.ReadLine(), out name)){
                    Console.WriteLine("Något blev fel, försök igen");
                }
                
            //FED surname INPUT + CHECK
                Console.WriteLine("\nAnge ditt efternamn:");
                if (!genUtil.testaValideraNamn(Console.ReadLine(), out surname)) {
                    Console.WriteLine("Något blev fel, försök igen");
                }

            //FED helaNamnet CHECK
                helaNamnet = name + " " + surname;
                    if (Regex.IsMatch(helaNamnet, "[^\\p{L} ]")) { //om siffror och symboler som oftast inte finns i namn ingår så kasta error...
                        ValideringsTextFörNamn(helaNamnet, false);
                        throw new Exception("Otillåtna tecken, försök igen..."); 
                    }

            //VALID RETURN
                ValideringsTextFörNamn(helaNamnet, true);

                return helaNamnet;
            } 
            catch (Exception ex){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Thread.Sleep(2000); //fika på det
                
                Console.Clear();
                StorBankHeader();
            }
        } while (true);
    }

    private static int makePIN(string input) {
        int pinkod;

        if (int.TryParse(input, out pinkod)) {
            return pinkod;
        }
        
        return 0;
    }
    private static void inputAccountCreate() {

        KontoData kd = new();

        kd.fullname = getName();
        while (!genUtil.testaValideraPIN(out kd.pin)); //ja, jag är fast i C tänk...
        /*
        
        SKRIVA JÄVLA BRA MED SÄKERHETSSPÄRRAR SÅ ATT ALL INPUT BLIR KORREKT HÄR, YO...

        //fyll i allt som behövs för en KontoData record och sedan skicka den till factoryn inuti denna metoden
            public int kontonummer;
            public AccountType kontotyp; //sikta på databasdriven/value objects eller polymorfiska typer i framtiden
            public string? fullname;
            public int pin;
            public DateTime skapat;
            public decimal balance; //decimal eftersom både double och float har avrundningsfel
        */
    }

//OVERLOADED METHOD ----------------------------------------------------------------------------------------------------------------------------------
    //Mom says: We got tryParse at home... (gjord endast för meny listorna/funktionerna)
    private static bool tryParseMenuInput(ConsoleKeyInfo tangentbordsknapptryck,(string label, Action<Account> callback)[] menu , out int siffra) {

        /*
            GGEZ ASCII KOD TRICK:
            -ConsoleKey key = enums
            -Enums börjar på ASCII kod 48 (dvs ConsoleKey.D0 = 48)
            -Trycker du då 0 så blir det enum 0 som är lika med 48. 48 minus 48 är lika med en int 0
                logiken fortsätter så; Enum 1 = 49, 49 minus 48 är lika med int 1...
        */

        siffra = -1; //sätter ett ogiltligt värde utifall skiten fail:ar (dvs garanterat return.ar false då)

        //Löjligt strul för att få resultatet helt terminal agnostiskt ...Git-Bash producerar annan ASCII om 
        // man kör programmet genom dennes dotnet run tex...
        if (char.IsDigit(tangentbordsknapptryck.KeyChar)) {
            siffra = tangentbordsknapptryck.KeyChar - '0';
            siffra--; //DÖDLIGT VIKTG DETALJ FÖR ATT DET SKA BLI RÄTT MED ARRAY INDEX ÅTKOMSTEN

            //returnerar true om siffra är inom array-spannet
            return siffra >= 0 && siffra <= (menu.Length - 1); //glöm ej minus ettan för dat array nummerering...
        }
        return false; 
    }
    private static bool tryParseMenuInput(ConsoleKeyInfo tangentbordsknapptryck,(string label, Action callback)[] menu , out int siffra) {

        /*
            GGEZ ASCII KOD TRICK:
            -ConsoleKey key = enums
            -Enums börjar på ASCII kod 48 (dvs ConsoleKey.D0 = 48)
            -Trycker du då 0 så blir det enum 0 som är lika med 48. 48 minus 48 är lika med en int 0
                logiken fortsätter så; Enum 1 = 49, 49 minus 48 är lika med int 1...
        */

        siffra = -1; //sätter ett ogiltligt värde utifall skiten fail:ar (dvs garanterat return.ar false då)

        //Löjligt strul för att få resultatet helt terminal agnostiskt ...Git-Bash producerar annan ASCII om 
        // man kör programmet genom dennes dotnet run tex...
        if (char.IsDigit(tangentbordsknapptryck.KeyChar)) {
            siffra = tangentbordsknapptryck.KeyChar - '0';
            siffra--; //DÖDLIGT VIKTG DETALJ FÖR ATT DET SKA BLI RÄTT MED ARRAY INDEX ÅTKOMSTEN

            //returnerar true om siffra är inom array-spannet
            return siffra >= 0 && siffra <= (menu.Length - 1); //glöm ej minus ettan för dat array nummerering...
        }
        return false; 
    }
//------------------------------------------------------------------------------------------------------------------------------------------------------
    public static (string label, Action<Account> callback)[] accountMenuList = { //string namn + funktion (typ void pointer grejen i C)
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
        ("Deposit",     (currentAccount) => currentAccount.Deposit()),
        ("Withdraw",    (currentAccount) => currentAccount.Withdraw()),
        ("Balance",     (currentAccount) => currentAccount.DisplayBalance(true)),
        ("Transaction", (currentAccount) => currentAccount.DisplayTransaktionsHistorik()),
        ("Gurka",       (currentAccount) => currentAccount.gurkTransaction()),
        ("Exit",        (generic)        => {Console.Clear(); Environment.Exit(0);}),
    };

    public static (string label, Action callback)[] mainMenuList = {
        ("Login To Account.",   () => Console.Clear()),
        ("Create New Account.", () => getName()),
        ("Exit.",               () => {Console.Clear(); Environment.Exit(0);}),
    };
    public static void accountMenu(Account currentAccount) {
        int menuIndex = 0;
        do {
            try {
                    Console.Clear();

                    StorBankHeader();
                //print menuList strings
                    for (int i = 0; i < accountMenuList.Length; i++) {
                        if (menuIndex == i) {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(arrow);
                            Console.WriteLine($"{i +1}. {accountMenuList[i].label}");
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{i +1}. {accountMenuList[i].label}");
                        }
                    }
                
                //MENU TOGGLE/ACTIVATION

                    ConsoleKeyInfo key = Console.ReadKey();

                    switch (key.Key) {                             //viktigt att gräva upp .Key i ConsoleKeyInfo variabeln...
                        case ConsoleKey.W or ConsoleKey.UpArrow: { 
                                if (menuIndex > 0){
                                    menuIndex--;
                                } else if (menuIndex <= 0) {
                                    //loop around to the BOTTOM of the menuList
                                    menuIndex = (accountMenuList.Length - 1);
                                }
                            }
                            break;
                        case ConsoleKey.S:                       //min personliga preferens: "drip filter" stil
                        case ConsoleKey.DownArrow: {
                                if (menuIndex < (accountMenuList.Length - 1)) {
                                    menuIndex++;
                                }  else if (menuIndex >= (accountMenuList.Length - 1)) {
                                    //loop around to the TOP of the menuList
                                    menuIndex = 0;
                                }
                            }
                            break;
                        case ConsoleKey.Enter: {
                                accountMenuList[menuIndex].callback(currentAccount);
                            }
                            break;
                        default: {
                            int direktMenyVal = 0;
                            if (tryParseMenuInput(key, accountMenuList, out direktMenyVal)) { //hacky af, probably error prone...
                                accountMenuList[direktMenyVal].callback(currentAccount);
                            }
                            }
                            break;
                    }
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

            //"GUI" shid
                StorBankHeader();

                for (int i = 0; i < 3; i++) {
                    if (menuIndex == i) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(arrow);
                        Console.WriteLine($"{i +1}. {mainMenuList[i].label}");
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{i +1}. {mainMenuList[i].label}");
                    }
                }

            //MENU TOGGLE/ACTIVATION

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key) {
                    case ConsoleKey.W or ConsoleKey.UpArrow: {
                            if (menuIndex > 0){
                                menuIndex--;
                            } else if (menuIndex <= 0) {
                                //loop around to the BOTTOM of menuList
                                menuIndex = (mainMenuList.Length - 1);
                            }
                        }
                        break;
                    case ConsoleKey.S or ConsoleKey.DownArrow: {
                            if (menuIndex < (mainMenuList.Length - 1)) {
                                menuIndex++;
                            }  else if (menuIndex >= (mainMenuList.Length - 1)) {
                                //loop around to the TOP of menuList
                                menuIndex = 0;
                            }                       
                        }
                        break;
                    case ConsoleKey.Enter: {
                            mainMenuList[menuIndex].callback();
                        }
                        break;
                    default: {
                        int direktMenyVal;
                        if (tryParseMenuInput(key, mainMenuList, out direktMenyVal)){ //hacky af, probably error prone...
                            mainMenuList[direktMenyVal].callback();
                        }
                    }
                    break;
                }
            } catch {
                
            }
        } while (true);
    }
}