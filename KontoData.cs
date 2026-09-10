public record KontoData {
    string? mejladress;
    public int kontonummer;
    public AccountType kontotyp; //sikta på databasdriven/value objects eller polymorfiska typer i framtiden
    public string? fullname;
    public int pin;
    public DateTime skapat;
    public decimal balance; //decimal eftersom både double och float har avrundningsfel
}