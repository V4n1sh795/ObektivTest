namespace Alert;
using DBContext;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using DotNetEnv;
using Org.BouncyCastle.Security;

public static class Email {
    private static string _email;
    private static string _app_passwd;

    private static void LoadEnv()
    {
        Env.Load();
        _email = Env.GetString("EMAIL");
        _app_passwd = Env.GetString("APP_PASSWD");
    }
    private static async Task Send(string email, Models.Flat flat)
    {
        // 1. Создаем сообщение
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Имя Отправителя", _email));
        message.To.Add(new MailboxAddress("Имя Получателя", email));
        message.Subject = "Обновление цены на квартиру";

        message.Body = new TextPart("plain") 
        { 
            Text = $"{flat.label} находящаяся на {flat.place} \n {flat.link}" 
        };

        using var client = new SmtpClient();
        try
        {

            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("v4n1shnerush@gmail.com", _app_passwd);
            await client.SendAsync(message);
            Console.WriteLine("Письмо успешно отправлено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
    public static async Task SendAll(ILogger<Program> logger, AppDbContext db, string link)
    {
        if (_email == null)
            LoadEnv();
        Models.Flat? flat = db.Flats.Find(Utility.Hash.GetXxHash64(link));
        if (flat == null)
            logger.LogError("this link doesnt exist");
        else
        {
            foreach (var email in flat.Emails)
            {
                await Send(email, flat);
            }
        }
    }
}