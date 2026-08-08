namespace Service;
using Microsoft.EntityFrameworkCore;
using Alert;
using DBContext;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Signers;

public class Api
{
    public static async Task<IResult> Flat(ILogger<Program> logger, AppDbContext db, RequestModels.Subscribe request)
    {
        logger.LogInformation($"Flat: {request.link}, Email: {request.email}");

        if (!Utility.Validator.IsEmailValid(request.email) || !Utility.Validator.IsValidUrl(request.link)) 
            return Results.BadRequest("Email or link is not valid");
        {
            Models.Flat? flat = await db.Flats.FindAsync(Utility.Hash.GetXxHash64(request.link)); // null exception is created in Validator
            if (flat == null)
            {
                await Parsing.Main(logger, db, request.link, request.email); // null exception is created in Validator
                logger.LogInformation("Flat not exist Parsing...");
            }
            else
            {
                flat.Emails.Add(request.email);
                db.SaveChanges();
                logger.LogInformation("Flat exist adding email");
            }
            return Results.Ok();    
        }
        
    }
    public static async Task<IResult> PatchPrice(ILogger<Program> logger, AppDbContext db, RequestModels.NewPrice request)
    {
        return await Parsing.AddPrice(logger, db, request.link, request.price);
    }
    public static async Task<IResult> Prices (ILogger<Program> logger, AppDbContext db)
    {
            var flats = await db.Flats.Where(f => f != null)
                .Include(f => f.Prices) 
                .ToListAsync();
            
            var response = flats.Select(f => new ResponseModels.FlatResponse
            {
                Id = f.Id,
                Link = f.link,
                Label = f.label,
                Place = f.place,
                LastPrice = f.Prices
                    .Select(p => new ResponseModels.PriceResponse
                    {
                        Amount = p.price,
                        Date = p.TimeChanged 
                    })
                    .LastOrDefault() ?? null,
                Emails = f.Emails
            });
            return Results.Ok(response);
    }
}