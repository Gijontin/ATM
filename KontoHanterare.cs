public class KontoHanterare {
    /*
    KontoH. är en service
        är bara en temporär cache för databas info såsom konto eller en hel dictionary
        hanterar förändringar som händer i listan eller i ett konto och uppdaterar databasen med det
    */
    public Dictionary<string, Account> AccountListan = []; //ska egentligen existera "längre bak" i en databas som ansvarar för datans persistance...
                                                           //specifikt i AccountRepositorium.cs i mitt fall..
    
    public bool TryCreateAccount(KontoData kd) {
        if (!ParseValidationKontoData(kd)) {
            return false;
        }

        AutoCompleteKontoData(kd);
        CreateAccount(kd);

        return true;
    }
    private void RegisterAccount(Account acn) {
        AccountListan.Add(acn._data.mejladress, acn);
    }
    private void AutoCompleteKontoData(KontoData kd) {
        //system auto-fill - fixa: görs mer i backend 
        kd.kontotyp = AccountType.SPARKONTO;
        kd.balance = 0m;
        kd.kontonummer = 0;

        kd.skapat = DateTime.Now;
    }
    private bool ParseValidationKontoData(KontoData kd) {
        
        //userinput
        if (!genUtil.testaValideraFulltNamn(kd.fullname).succee ||
            !genUtil.testaValideraMejl(kd.mejladress).succee ||
            !genUtil.testaValideraPIN(kd.pin).succee)
        {
            return false;
        }

        return true;
    }
    private void CreateAccount(KontoData kd) {
        
        //validera att all kontodata stämmer och är ifylld

        //skapa ett Account, lägg in kontodatan i det
        Account nytt = new Account(kd);
        //lägg till Account i listan
        RegisterAccount(nytt);
    }
}