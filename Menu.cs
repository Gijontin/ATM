using System.Security.Cryptography;

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
        
        Console.Clear();

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
    private string getPINput() {
        //char[] input = []; //nah klydd, C# har massa färdiga funktioner för strings så...
        
        string input = "";

        Console.WriteLine("\nAnge din önskade fyr-siffriga PIN kod.");
        
        while (true) {
            var key = Console.ReadKey(intercept: true);

        //ENTER
            if (key.Key == ConsoleKey.Enter) {
                if (input.Length != 4) { //Remember: order on if checks matter...
                    genUtil.SkrivFärgPaus("\nPIN-koden måste bestå av ett fyr-siffrigt antal", ConsoleColor.Red);
                    continue;
                }
                if (!input.All(char.IsDigit)) {
                    genUtil.SkrivFärgPaus("\nPIN-koden kan endast bestå av siffror", ConsoleColor.Red);
                    continue;
                } else {
                    return input.Trim();
                }
            }
        //BACKSPACE
            if (key.Key == ConsoleKey.Backspace && input.Length > 0) {
                input = input[..^1];
                Console.Write("\b \b"); //tur att man höll på med text-adventure i C asså...
                continue; //skip the other if-checks cuz behövs ej om du tryckt backspace
            }
        //SIFFRA
            if (char.IsDigit(key.KeyChar)) {

                if (input.Length < 4 && key.Key != ConsoleKey.Backspace) {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
        } 
    }
    private string getName() {
        string? fullname;
        do {
            Console.WriteLine("Ange ditt för- och efternamn:");
            fullname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fullname)) {
                genUtil.SkrivFärgPaus("\nDu måste fylla i ett namn.", ConsoleColor.Red);
                StorBankHeader();
                continue; //undviker att splitta en null string
            }

            fullname.Trim(); //kraschar om jag försöker trimma null ))))

            string[] split = fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length < 2) {
                genUtil.SkrivFärgPaus("\nEfternamn saknas.", ConsoleColor.Red);
                StorBankHeader();
                continue; //VIKTIG, skippar return
            }
            
            return fullname;

        } while (true);
    }
    public string getMejl() {
        string? mejl;

        do {
            Console.WriteLine("\nAnge din mejladress:");
            mejl = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(mejl)) {
                genUtil.SkrivFärgPaus("\nDu måste fylla i en mejladress.", ConsoleColor.Red);
                StorBankHeader();
                continue;
            }

            mejl.Trim(); //efter null-check, att försöka trimma null blir krasch...

            if (!mejl.Contains('@')) {
                genUtil.SkrivFärgPaus("\nMejladressen måste innehålla ett snabel-a.", ConsoleColor.Red);
                StorBankHeader();
                continue;
            }

            int kanelbullensUtpost = mejl.IndexOf('@');
            if (kanelbullensUtpost == mejl.Length - 1) {
                genUtil.SkrivFärgPaus("\nMejladressen saknar domän.", ConsoleColor.Red);
                StorBankHeader();
                continue;
            }

            string domän = mejl.Substring(kanelbullensUtpost + 1);
            if (!domän.Contains('.')) {
                genUtil.SkrivFärgPaus("\nDomänen saknar en punkt.", ConsoleColor.Red);
                StorBankHeader();
                continue;
            }

            return mejl;

        } while (true);
    }
    public void RequestCreateAccount() {
        Console.Clear();
        StorBankHeader();

        KontoData kd = new();
    
        //userinput
        kd.fullname = getName();
        kd.mejladress = getMejl();
        kd.pin = getPINput();

        if (!_KH.TryCreateAccount(kd)) {
            Console.WriteLine("\n\nNågot blev fel, försök igen...");
        } else {
            Console.WriteLine("\n\nKontot har skapats!\n");
        }
            
        
        genUtil.pressToContinue();

        return;
    }
    public void RequestLoginAccount() {
        Console.Clear();
        StorBankHeader();

        //ja du, gissa vad som ska hända här...

        //ta username och lösen
        KontoData kd = new(); //byt KontoData till usrname/pass record
            kd.mejladress = getMejl();
            kd.pin = getPINput();

        accountMenu(_KH.TryLoginAccount(kd).account);
        //skicka detta till den backend fnuktion som sköter detta för valideringcheck
            //om usrname finns och pin:en blir korrekt hash:ad hämta då kontot och skicka dess data
            //tillbaka till en menu som visar kontot

            //vet ej riktigt om frontend-menyn i sig sedan ska ha requests om ändringar på kontot så som dep/with etc till backend
            //eller om dessa funktion-requests måste inkapsuleras ännu bättre i backend...


//debug test (om konto skapas och hålls i cache) -----------------------------
        foreach (var kn in _KH.AccountListan) {
            Console.WriteLine(kn.Key);
        }
        Console.WriteLine();

        //test2
        foreach (var kh in _KH.AccountAuthListan) {
            if (kh.Value.saltLakrits != null && kh.Value.hashKaka != null){
                Console.WriteLine(Convert.ToBase64String(kh.Value.saltLakrits));
                Console.WriteLine(Convert.ToBase64String(kh.Value.hashKaka));
                Console.WriteLine();
            }
        }
        Console.WriteLine();
//-----------------------------------------------------------------------------

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