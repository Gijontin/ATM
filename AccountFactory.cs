public class AccountFactory {
    public Account SkapaAccount(AccountType kontotyp, string fullname, int pin, decimal startsaldo) {
        
        int kontonummer = 0000;   // din egen metod
        DateTime skapat = DateTime.Now;

        var konto = new Account(
                kontonummer,
                kontotyp,
                fullname,
                pin,
                skapat,
                startsaldo
            );

        return konto;
    }
}