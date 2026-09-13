public record AutentiseringData {
    public byte[]? saltLakrits;
    public byte[]? hashKaka;
    public int iterationer; //fördröjningsmetod, är N * hashning - slöar ner brute-force försök
}