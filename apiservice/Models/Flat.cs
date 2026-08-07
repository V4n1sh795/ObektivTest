
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;



public class Flat
{
    [Key]
    public ulong Id { get; set; }
    public string link { get; set; } = string.Empty;
    public string label { get; set; } = string.Empty;
    public string place { get; set; } = string.Empty;
    public List<Price> Prices { get; set; } = new();
    public List<string> Emails { get; set; } = new List<string>();
}
