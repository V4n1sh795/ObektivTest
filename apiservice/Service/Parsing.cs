namespace Service;
using HtmlAgilityPack;
using DBContext;


public static class Parsing
{
    public static async Task Main(ILogger<Program> logger, AppDbContext db,  string link, string email)
    {
        var web = new HtmlWeb();
        HtmlDocument doc = await web.LoadFromWebAsync(link);

        string html = doc.DocumentNode.OuterHtml;
        string price_xpath = "/html/body/main/section[4]/div/div[3]/div/div[1]/div[2]/div[2]/div[2]/form/ul/li[1]/label/div/div/span[1]";
        string label_xpath = "/html/body/main/section[4]/div/div[3]/div/div[1]/div[2]/div[2]/div[1]/div[1]/h3";
        string place_xpath = "/html/body/main/section[4]/div/div[3]/div/div[1]/div[2]/div[2]/div[1]/div[2]/p";

        string price = await ElemByXpath(price_xpath, doc);
        string label = await ElemByXpath(label_xpath, doc);
        string place = await ElemByXpath(place_xpath, doc);

        logger.LogInformation($"Price - {price}, Label - {label}, Place - {place}");

        Models.Flat flat = new Models.Flat
        {
            Id = Utility.Hash.GetXxHash64(link),
            link = link,
            label = label,
            place = place,
        };
        flat.Emails.Add(email);
        flat.Prices.Add(new Models.Price(price));
        await db.Flats.AddAsync(flat);
        await db.SaveChangesAsync();
    }
    public static async Task<IResult> AddPrice(ILogger<Program> logger, AppDbContext db, string link, string price)
    {
        Models.Flat? flat = db.Flats.Find(Utility.Hash.GetXxHash64(link));
        if (flat == null || !Utility.Validator.IsValidUrl(link))
            return Results.BadRequest("This flat is doesnt exist, or link is not valid");
        else
        {
            await Alert.Email.SendAll(logger, db, link, price);
            flat.Prices.Add(new Models.Price(price));
            await db.SaveChangesAsync();
            return Results.Ok();
        }
    }
    private static async Task<string> ElemByXpath(string xpath, HtmlDocument doc)
    {
        HtmlNode? node = doc.DocumentNode.SelectSingleNode(xpath);
        if (node != null)
        {
            string text = node.InnerText.Trim();
            return text;
        }
        else
            return "";
    }
}