public class KontoHanterare {
    /*
    KontoH. är en service
        är bara en temporär cache för databas info såsom konto eller en hel dictionary
        hanterar förändringar som händer i listan eller i ett konto och uppdaterar databasen med det
    */
    public Dictionary<string, Account> AccountListan = []; //ska egentligen existera i en persitant databas...
    public void RegisterAccount(Account acn) {
        AccountListan.Add(acn._data.mejladress, acn);
    }
    public void CreateAccount(KontoData kd) {
        
        //validera att all kontodata stämmer och är ifylld

        //skapa ett Account, lägg in kontodatan i det
        Account nytt = new Account(kd);
        //lägg till Account i listan
        RegisterAccount(nytt);
    }
}