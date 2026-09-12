public class Menu {
    private const string arrow = "--> ";
    private readonly KontoHanterare _KH;
    public readonly (string label, Action callback)[] mainMenuList; //måste in i constructorn för att kompilatorn inte kan gissa Actiontypen tillräckligt bra... (till skillnad från min första tupleArray..)
    public Menu() {
        _KH = new(); 

        mainMenuList = [
            ("Login To Account.",   () => RequestLoginAccount()),
            ("Create New Account.", () => RequestCreateAccount()),
            ("Exit.",               () => {Console.Clear(); Environment.Exit(0);}),
        ];
    }
    private void StorBankHeader() {
        
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
    private void ValideringsTextFörNamn(string str1, bool ärGiltligt) {
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
    private void SkrivRöttSleep(string str) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(str);
        Console.ResetColor();
        Thread.Sleep(1500);
    }
    private string getName() {
        genUtil.ValideringsResultat<string> resultat;
        //parse deez bad boiz
        string name;
        string surname;
        
        do {
            Console.WriteLine("\nAnge ditt förnamn:");
            resultat = genUtil.testaValideraNamn(Console.ReadLine()); //"Console.ReadLine()" kan anses vara frontend här och testaValidera är backend (I think)... 

            if (!resultat.succee) {
                SkrivRöttSleep(resultat.msg);
            }
        } while (!resultat.succee);
        name = resultat.value;

        do {
            Console.WriteLine("\nAnge ditt efternamn:");
            resultat = genUtil.testaValideraNamn(Console.ReadLine());

            if (!resultat.succee) {
                SkrivRöttSleep(resultat.msg);
            }
        } while (!resultat.succee);
        surname = resultat.value;

        string helaNamnet = name + " " + surname;
        ValideringsTextFörNamn(helaNamnet, true);
        return helaNamnet;
    }
    private int getPIN() { //"getPIN" fyller egentligen mellansteget från frontend till backend, testavalidera har just nu en
                                  //frontend/backend hybrid funktion (testaValidera ska egentligen bara ha logik-checken från 'Enter')
                                  //medans allt annat input grejs som händer i testValidera bör hända i frontend, dvs funktion här i Menu
        genUtil.ValideringsResultat<int> resultat;

        do {
            Console.WriteLine("\nAnge din önskade fyr-siffriga PIN-kod:");
            resultat = genUtil.testaValideraPIN();

            if (!resultat.succee) {
                SkrivRöttSleep(resultat.msg);
            }
        } while (!resultat.succee);

        return resultat.value;
    }
    public string getMejl() { //wrapper
        genUtil.ValideringsResultat<string> resultat;

        do {
            Console.WriteLine("\nAnge din mejladress:");
            resultat = genUtil.testaValideraMejl(Console.ReadLine());

            if (!resultat.succee) {
                SkrivRöttSleep(resultat.msg);
            }
        } while (!resultat.succee);

        return resultat.value;
    }
    public void RequestCreateAccount() {
        Console.Clear();
        StorBankHeader();

    // MOVE THIS PART TO KONTOHANTERARE OR OTHER BACKEND------------------------
        KontoData kd = new();
    
        //userinput
        kd.fullname = getName();
        kd.mejladress = getMejl();
        kd.pin = getPIN();

        //system auto-fill
        kd.kontotyp = AccountType.SPARKONTO;
        kd.balance = 0m;
        kd.kontonummer = 0;

        kd.skapat = DateTime.Now;
    //--------------------------------------------------------------------------

        _KH.CreateAccount(kd);
            
        Console.WriteLine("\nKontot har skapats!\n");
        genUtil.pressToContinue();
        
        /*
        - Skapa ett account via denna datan
        KontoHanteraren.AccountCreate(Kd);
        
        - Spara acconutet i en Dictionary där key:n är mejladressen(username, typ)
            //KontoHanteraren gör detta...

        - Skriv någon metod som tar emot PIN, dubbelkollar den mot kontot för tillträde att logga in
          på kontot genom att injicera det i accountMenu för att komma åt dess funktioner
            //sub-funktion i kontoHanteraren validerar detta, ganska någon privat metod i Account synergize:ar verifikationen där med...
        */

        
        return;
    }
    public void RequestLoginAccount() {
        Console.Clear();
        StorBankHeader();

        //ja du, gissa vad som ska hända här...

        //be användaren skicka in username
            // - backend checkar om de hittar username 
            // > ny meny_segment som efterfrågar valid password (pinkoden i detta fallet)
            // > backend checkar validerar PIN
            // - vid godkänt hämtar backend konto information och menyn tar dig till...
            //...account menyn med account-specifika menyOptions som request:ar accountets metoder


        //test
        foreach (var kn in _KH.AccountListan) {
            Console.WriteLine(kn.Key);
        }

        Console.WriteLine();
        genUtil.pressToContinue();
    }

//OVERLOADED METHOD ----------------------------------------------------------------------------------------------------------------------------------
    //Mom says: We got tryParse at home... (gjord endast för meny listorna/funktionerna)
    private bool tryParseMenuInput(ConsoleKeyInfo tangentbordsknapptryck,(string label, Action<Account> callback)[] menu , out int siffra) {

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
    private bool tryParseMenuInput(ConsoleKeyInfo tangentbordsknapptryck,(string label, Action callback)[] menu , out int siffra) {

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
    public (string label, Action<Account> callback)[] accountMenuList = { //string namn + funktion (typ void pointer grejen i C)
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

    public void accountMenu(Account currentAccount) {
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

    public void mainMenu() {
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