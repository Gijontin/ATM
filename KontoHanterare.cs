using System.Security.Cryptography;

public class KontoHanterare {
    /*
    KontoH. är en service
        är bara en temporär cache för databas info såsom konto eller en hel dictionary
        hanterar förändringar som händer i listan eller i ett konto och uppdaterar databasen med det
    */
    public Dictionary<string, Account> AccountListan = []; //ska egentligen existera "längre bak" i en databas som ansvarar för datans persistance...
                                                           //specifikt i AccountRepositorium.cs i mitt fall..
    public Dictionary<string, AutentiseringData> AccountAuthListan = [];
    public record LoginData<T>(bool godkänt, T account, KontoData Konto); //format delat med Frontend, MEN: byt KontoData till usrname/pass record
// LOGIN LOGIC ------------------------------------------------------------------------------------
    //FRONTEND ACCESS
    public LoginData<Account> TryLoginAccount(KontoData loginKN) { //byt till usrname/pass record
        if (genUtil.testaValideraMejl(loginKN.mejladress).succee) {    //validering + cleanup
            loginKN.pin = genUtil.testaValideraPIN(loginKN.pin).value; //cleanup dat shid också dådå
        }

        LoginData<Account> kd = new LoginData<Account>(false, null, null);

        if (FindAccount(loginKN)) {
            kd = new(true, FetchAccountData(loginKN), null);
            return kd;
        } else {
            return kd = new(false, null, null);
        }
    }
    //BACKEND ---
    private bool FindAccount(KontoData loginKN) { //byt KontoData till usrname/pass record

        if (string.IsNullOrWhiteSpace(loginKN.mejladress) || string.IsNullOrWhiteSpace(loginKN.pin)) {
            return false;
        }

        AutentiseringData temp = new();

        if (AccountAuthListan.TryGetValue(loginKN.mejladress, out temp)) {
            return genUtil.testaValideraHash(loginKN.pin, temp).succee;
        }

        return false;
    }
    private Account FetchAccountData(KontoData kd) {
        AccountListan.TryGetValue(kd.mejladress, out Account tempAcc);
        return tempAcc;
    }
// CREATE LOGIC -----------------------------------------------------------------------------------
    //FRONTEND ACCESS
    public bool TryCreateAccount(KontoData kd) {
        if (!ParseValidationKontoData(kd)) {
            return false;
        }

        AutoCompleteKontoData(kd);

        return CreateAccount(kd); //eftersom jag skrev om de flesta från void till bool kan jag nog använda denna såhär..
    }
    //BACKEND ---
    private bool ParseValidationKontoData(KontoData kd) {
        
        //userinput
        if (!genUtil.testaValideraFulltNamn(kd.fullname).succee ||
            !genUtil.testaValideraMejl(kd.mejladress).succee ||
            !genUtil.testaValideraPIN(kd.pin).succee)
        {
            return false;
        }

        //Console.WriteLine("\nparsevalid lyckades!"); //mlg dedebug
        return true;
    }
    private void AutoCompleteKontoData(KontoData kd) {
        //system auto-fill - fixa: görs mer i backend 
        kd.kontotyp = AccountType.SPARKONTO;
        kd.balance = 0m;
        kd.kontonummer = 0;

        kd.skapat = DateTime.Now;
        //Console.WriteLine("\nautocomp lyckades!"); //mlg dedebug
    }
    private bool CreateAccount(KontoData kd) {
        
        //validera att all kontodata stämmer och är ifylld

        //skapa ett Account, lägg in kontodatan i det
        Account nytt = new Account(kd);
        
        //lägg till Account i listan
        if (!RegisterAccount(nytt)) {
            return false;
        }

        //Console.WriteLine("\ncreateaccount lyckades!"); //mlg dedebug
        return true;
    }
    private bool RegisterAccount(Account acn) {
        
        if (!string.IsNullOrWhiteSpace(acn._data.mejladress) && AccountListan.TryAdd(acn._data.mejladress, acn)){
            SaltAndHash(acn._data);
            //mlg debug
            //Console.WriteLine("\nregKonto lyckades!"); //mlg dedebug
            return true;
        }

        return false;
    }
    private void SaltAndHash(KontoData kd) {

        if (string.IsNullOrWhiteSpace(kd.pin)) {
            return;
        }

        //gen salt
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt); //bättre än äldre rng sätt, enligt AI...

        //combine pin and salt then hash
            //okay så detta är det "minsta klyddet" alternativet för vad som är inbyggt i C#... >_>
        using var pbkdf2 = new Rfc2898DeriveBytes(kd.pin, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        //create AutData
        AutentiseringData credz = new AutentiseringData {
            saltLakrits = salt,
            hashKaka = hash,
            iterationer = 100_000
        };

        //store in Dic
        if (!string.IsNullOrWhiteSpace(kd.mejladress)) {
            AccountAuthListan.Add(kd.mejladress, credz);
        }
    }
//-------------------------------------------------------------------------------------------------
}