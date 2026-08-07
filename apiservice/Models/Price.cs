using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

public class Price
{
    [Key]
    public int Id { get; set; }
    public int price { get; set; }
    public DateTime TimeChanged { get; set; } = DateTime.Now;

    public ulong FlatId { get; set; }
    public Flat? Flat { get; set; }

    private Price() { }
    public Price(string str_price)
    {
        Utility.ParseValues.Price(str_price, out int price);
        this.price = price;
    }
}