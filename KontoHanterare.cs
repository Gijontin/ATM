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
    public bool TryCreateAccount(KontoData kd) {
        if (!ParseValidationKontoData(kd)) {
            return false;
        }

        AutoCompleteKontoData(kd);

        return CreateAccount(kd);
    }
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
        
        AccountListan.Add(acn._data.mejladress, acn);
        SaltAndHash(acn._data);

        //mlg debug
        //Console.WriteLine("\nregKonto lyckades!"); //mlg dedebug
        return true;

        
        return false;
    }
    private void SaltAndHash(KontoData kd) {
        
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
        AccountAuthListan.Add(kd.mejladress, credz);
    }
}