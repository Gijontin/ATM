public record KontoData {
    public string? mejladress;
    public int kontonummer;
    public AccountType kontotyp; //sikta på databasdriven/value objects eller polymorfiska typer i framtiden
    public string? fullname;
    public string? pin; //ska bort härifrån i ett riktigt system (sköts av en pundarfunktion(hasher))
    public DateTime skapat;
    public decimal balance; //decimal eftersom både double och float har avrundningsfel
}