namespace ResponseModels;
public class FlatResponse
{
    public ulong Id { get; set; }
    public string Link { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public PriceResponse? LastPrice { get; set; }
    public List<string> Emails { get; set; } = new();
}

public class PriceResponse
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}