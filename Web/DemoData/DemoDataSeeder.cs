using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Discount;
using Domain.Entities.Images;
using Domain.Entities.Permission;
using Domain.Entities.Product;
using Domain.Enums;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.DemoData;

/// <summary>
/// Fills an empty database with enough to click through the shop: an administrator, a
/// customer with wallet credit, products with stock and colours, a running discount and
/// the banners. Runs only when Database:SeedDemoData is true (the Docker demo), and does
/// nothing once any product exists. The image files it points at ship in wwwroot.
/// </summary>
public static class DemoDataSeeder
{
    public const string AdminEmail = "admin@shop.local";
    public const string CustomerEmail = "customer@shop.local";

    // The super-admin role created by PermissionSeeds; it holds every permission.
    private const int SuperAdminRoleId = 3;

    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher hasher, IConfiguration configuration, ILogger logger)
    {
        if (await context.Products.AnyAsync())
            return;

        var now = DateTime.Now;
        var adminPassword = configuration["DemoData:AdminPassword"] ?? "Admin#12345";
        var customerPassword = configuration["DemoData:CustomerPassword"] ?? "Customer#12345";

        var admin = NewUser("مدیر", "فروشگاه", AdminEmail, "09120000001", await hasher.EncodePasswordAsync(adminPassword), isAdmin: true, now);
        admin.UserRoleMappings = [new UserRoleMapping { RoleId = SuperAdminRoleId, CreateDate = now }];
        var customer = NewUser("سارا", "محمدی", CustomerEmail, "09120000002", await hasher.EncodePasswordAsync(customerPassword), isAdmin: false, now);
        customer.Address = "تهران، خیابان ولیعصر، پلاک ۱۲";
        context.Users.AddRange(admin, customer);
        await context.SaveChangesAsync();

        context.Transactions.Add(new Transaction
        {
            UserId = customer.Id,
            Price = 150_000_000,
            TransactionType = TransactionType.Deposit,
            IsPay = true,
            CreateDate = now
        });

        SeedCategoryImages(context);

        var gold = new Color { Title = "طلایی", ColorCode = "#d4b896", CreateDate = now };
        var black = new Color { Title = "مشکی", ColorCode = "#222222", CreateDate = now };
        var green = new Color { Title = "سبز", ColorCode = "#3e5a47", CreateDate = now };
        var pink = new Color { Title = "صورتی", ColorCode = "#f3a6c8", CreateDate = now };
        var red = new Color { Title = "قرمز", ColorCode = "#c0392b", CreateDate = now };
        var orange = new Color { Title = "نارنجی", ColorCode = "#f39c12", CreateDate = now };

        var products = new[]
        {
            NewProduct("گوشی موبایل پرو ۲۵۶ گیگابایت", 6, 72_000_000, "1d9e834474564706836d9ce6549c7476.jpg",
                "نمایشگر ۶.۳ اینچی، دوربین سه‌گانه و بدنه تیتانیومی.", now,
                (gold, 5, 0), (black, 3, 2_000_000)),
            NewProduct("گوشی موبایل ۱۲۸ گیگابایت سبز", 6, 38_500_000, "8bf9cb51232e4ff0925fb7fd3cefa6d4.jpg",
                "نمایشگر ۶.۱ اینچی و دوربین دوگانه.", now,
                (green, 8, 0)),
            NewProduct("گوشی موبایل ۱۲۸ گیگابایت صورتی", 6, 45_000_000, "8bfb6cb3092f4febb84b6af99255f0b6.jpg",
                "آخرین عدد موجود در انبار.", now,
                (pink, 1, 0)),
            NewProduct("کفش ورزشی رانینگ", 18, 3_200_000, "507dab9ca63e480abbf3bfcb2172b80e.jpg",
                "زیره سبک و رویه توری تنفس‌پذیر.", now,
                (red, 10, 0)),
            NewProduct("کفش پاشنه‌بلند زنانه", 3, 2_450_000, "023a643777ca45bbb689b594e6b8d923.jpg",
                "چرم مصنوعی با پاشنه ۸ سانتی‌متری.", now,
                (orange, 6, 0)),
            NewProduct("ادوپرفیوم زنانه ۱۰۰ میلی‌لیتر", 15, 6_900_000, "ca261423cbe145be80768d68ee0440db.jpg",
                "رایحه گل‌های سفید و مشک.", now,
                (gold, 4, 0))
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var discount = new Discount
        {
            IsPercentage = true,
            Value = 15,
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CreateDate = now
        };
        context.Discounts.Add(discount);
        await context.SaveChangesAsync();
        context.ProductDiscounts.AddRange(new[] { products[0], products[3], products[4] }
            .Select(p => new ProductDiscount { ProductId = p.Id, DiscountId = discount.Id }));

        context.Banners.AddRange(
            new Banner { Title = "2b0122214a224e1da1d3ade390e2395e.jpg", Link = "/ProductList", ExpirationDate = now.AddYears(1), CreateDate = now },
            new Banner { Title = "849824241b9a4e1abfc3109949751203.jpg", Link = "/ProductList", ExpirationDate = now.AddYears(1), CreateDate = now });

        string[] fixedBanners =
        [
            "fb47dd3233854acba8ba353f500766f1.jpg",
            "22753c916453467cbe7dedc2fe19c4ae.jpg", "7ce177a4da3e48bf8ad4e0d20a195f23.jpg",
            "19ada59f64e54c65a3560be7cf6b8d79.jpg", "fb47dd3233854acba8ba353f500766f1.jpg",
            "4c07e312dc044f8488d99a13cd5b70d5.jpg", "db63397d6d91467394d47563f0c6dc89.jpg"
        ];
        // The top closable strip is left empty: none of the shipped images fits a 60px bar.
        context.BannerFix.AddRange(Enum.GetValues<ImageEnum.Banner>().Where(p => p != ImageEnum.Banner.TopClosable).Select((position, i) => new BannerFix
        {
            Title = fixedBanners[i],
            Link = "/ProductList",
            Position = position,
            CreateDate = now
        }));

        await context.SaveChangesAsync();
        logger.LogInformation("Seeded demo data: admin {Admin}, customer {Customer}, {Count} products.",
            AdminEmail, CustomerEmail, products.Length);
    }

    private static User NewUser(string first, string last, string email, string phone, string passwordHash, bool isAdmin, DateTime now) => new()
    {
        FirstName = first,
        LastName = last,
        Email = email,
        PhoneNumber = phone,
        Password = passwordHash,
        IsAdmin = isAdmin,
        IsEmailActive = true,
        EmailActiveCode = Guid.NewGuid().ToString("N"),
        CreateDate = now
    };

    private static Product NewProduct(string name, int categoryId, int price, string image, string description, DateTime now,
        params (Color Color, int Count, int ExtraPrice)[] variants) => new()
    {
        ProductName = name,
        CategoryId = categoryId,
        Price = price,
        ImageName = image,
        ShortDescription = description,
        Review = description,
        Inventory = variants.Sum(v => v.Count),
        CreateDate = now,
        ProductColors = variants.Select(v => new ProductColor
        {
            Color = v.Color,
            Count = v.Count,
            Price = v.ExtraPrice,
            CreateDate = now
        }).ToList()
    };

    private static void SeedCategoryImages(ApplicationDbContext context)
    {
        var images = new Dictionary<int, string>
        {
            [2] = "16454084e28a4f48a0a967918ac73839.jpg",
            [3] = "a56687a2687c4bd5bbbc496444087f14.jpg",
            [4] = "164117fe898e46c19a947bfc3a2c9509.jpg",
            [6] = "0f3245dc607148cc8c46aa52c0cb4f0a.jpg",
            [7] = "ba665831db6942568b95b46bd2fd7292.jpg",
            [8] = "753b635f22e84d9f8054039c17fcaee5.jpg",
            [10] = "0accc95fc2a24590931a6cf325778a92.jpg",
            [11] = "9e6acb9b15d7452a91a03c028e24ee94.jpg",
            [12] = "b0f2cb13746843d9be6ca2c098aff489.jpg"
        };
        foreach (var category in context.ProductCategories.Where(c => images.Keys.Contains(c.Id)))
            category.ImageName = images[category.Id];
    }
}
