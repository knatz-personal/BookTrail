using Bogus;
using BookTrail.Data;
using BookTrail.Data.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Extensions;

namespace BookTrail.Seeder
{
    public class SeederService
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ApplicationDbContext _context;
        private readonly Faker _faker;
        private readonly SeederOptions _seederOptions;

        public SeederService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
            _seederOptions = new SeederOptions();
            _configuration.GetSection(SeederOptions.ConfigurationSection).Bind(_seederOptions);

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in appsettings.json");
            }

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.EnableDetailedErrors()
                .EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(GetType()
                    .Assembly
                    .GetName()
                    .Name);
            });

            _context = new ApplicationDbContext(optionsBuilder.Options);
            _faker = new Faker();
        }

        public async Task SeedAsync(string[] args)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(_seederOptions.Database.TimeoutMinutes));

            await ResetDatabaseIfRequested(args);
            await TestDatabaseConnection();

            await SeedIdentityData();
            await SeedFakerData();

            await _context.SaveChangesAsync();
            Console.WriteLine("Seeding complete!");
            Console.ReadLine();
        }

        private async Task ResetDatabaseIfRequested(string[] args)
        {
            bool shouldReset = args.Contains("--reset");
            if (shouldReset)
            {
                Console.WriteLine("Resetting database tables...");
                await _context.Database.ExecuteSqlRawAsync(
                    """
                    TRUNCATE TABLE 
                    "BookAuthor", 
                    "BookGenre", 
                    "BookShelfItem", 
                    "BookShelf", 
                    "Reviews", 
                    "ReadLog", 
                    "Recommendation", 
                    "Wishlist", 
                    "Books", 
                    "Genre", 
                    "Author", 
                    "Users",
                    "Roles", 
                    "UserRoles" 
                    RESTART IDENTITY CASCADE
                    """);
                Console.WriteLine("Reset complete.");
            }
        }

        private async Task TestDatabaseConnection()
        {
            Console.WriteLine("Testing database connection...");
            try
            {
                await _context.Database.CanConnectAsync();
                Console.WriteLine("Database connection successful!");
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to connect to database. Connection string: {_configuration.GetConnectionString("DefaultConnection")}",
                    ex);
            }
        }

        private async Task SeedIdentityData()
        {
            PasswordHasher<User> hasher = new();

            User fixedAdmin = new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@booktrail.com",
                NormalizedEmail = "ADMIN@BOOKTRAIL.COM",
                EmailConfirmed = true,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null!, "AdminPass123!"),
                CreatedAt = DateTime.UtcNow,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            User fixedUser = new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Regular",
                LastName = "User",
                Email = "user@booktrail.com",
                NormalizedEmail = "USER@BOOKTRAIL.COM",
                EmailConfirmed = true,
                UserName = "user",
                NormalizedUserName = "USER",
                PasswordHash = hasher.HashPassword(null!, "UserPass123!"),
                CreatedAt = DateTime.UtcNow,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            IdentityRole<Guid> adminRole = new() { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" };
            IdentityRole<Guid> userRole = new() { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" };
            IdentityUserRole<Guid> adminUserRole = new() { UserId = fixedAdmin.Id, RoleId = adminRole.Id };

            await _context.Users.AddRangeAsync(fixedAdmin, fixedUser);
            await _context.Roles.AddRangeAsync(adminRole, userRole);
            await _context.UserRoles.AddAsync(adminUserRole);
        }

        private async Task SeedFakerData()
        {
            List<User> users = new Faker<User>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Bio, f => f.Lorem.Sentence())
                .RuleFor(u => u.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.EmailConfirmed, _ => true)
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.NormalizedUserName, (_, u) => u.UserName?.ToUpper())
                .RuleFor(u => u.NormalizedEmail, (_, u) => u.Email?.ToUpper())
                .RuleFor(u => u.PasswordHash, f => f.Random.Hash())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.PhoneNumberConfirmed, _ => true)
                .RuleFor(u => u.TwoFactorEnabled, _ => false)
                .RuleFor(u => u.LockoutEnabled, _ => false)
                .RuleFor(u => u.AccessFailedCount, _ => 0)
                .Generate(_seederOptions.SeedQuantities.Users);
            await _context.Users.AddRangeAsync(users);

            List<Genre> genres = new Faker<Genre>()
                .RuleFor(g => g.Id, _ => Guid.NewGuid())
                .RuleFor(g => g.Name, f => f.Lorem.Word())
                .RuleFor(g => g.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(g => g.IsDeleted, _ => false)
                .Generate(_seederOptions.SeedQuantities.Genres);
            await _context.Genre.AddRangeAsync(genres);

            List<Author> authors = new Faker<Author>()
                .RuleFor(a => a.Id, _ => Guid.NewGuid())
                .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                .RuleFor(a => a.LastName, f => f.Name.LastName())
                .RuleFor(a => a.Bio, f => f.Lorem.Paragraph())
                .RuleFor(a => a.BirthDate, f => DateOnly.FromDateTime(f.Date.Past(50, DateTime.Now.AddYears(-20)).Utc()))
                .RuleFor(a => a.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(a => a.IsDeleted, _ => false)
                .RuleFor(a => a.ProfileImage, f => f.Internet.Avatar())
                .Generate(_seederOptions.SeedQuantities.Authors);
            await _context.Author.AddRangeAsync(authors);

            List<Book> books = new Faker<Book>()
                .RuleFor(b => b.Id, _ => 0)
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
                .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                .RuleFor(b => b.PublishedDate, f => DateOnly.FromDateTime(f.Date.Past(10).Utc()))
                .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
                .RuleFor(b => b.CoverImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(b => b.PageCount, f => f.Random.Int(100, 900))
                .RuleFor(b => b.Language, f => f.Locale)
                .RuleFor(b => b.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(b => b.IsDeleted, _ => false)
                .Generate(_seederOptions.SeedQuantities.Books);
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            List<Review> reviews = books.Take(_seederOptions.SeedQuantities.Reviews).Select(b => new Review
            {
                BookId = b.Id,
                UserId = _faker.PickRandom(users).Id,
                Rating = _faker.Random.Short(1, 5),
                Content = _faker.Lorem.Paragraph(2),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

            List<ReadLog> readLogs = books.Take(_seederOptions.SeedQuantities.Reviews).Select(b => new ReadLog
            {
                BookId = b.Id,
                UserId = _faker.PickRandom(users).Id,
                DateStarted = _faker.Date.Past().Utc(),
                DateFinished = _faker.Date.Recent(30).Utc(),
                Rating = _faker.Random.Short(1, 5)
            }).ToList();

            List<BookShelf> shelves = users.Select(u => new BookShelf
            {
                UserId = u.Id,
                ShelfName = _faker.Random.Word() + " Shelf",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

            await _context.BookShelf.AddRangeAsync(shelves);
            await _context.SaveChangesAsync();

            List<BookShelfItem> shelfItems = shelves.SelectMany(shelf =>
                _faker.PickRandom(books, 3).Select(book =>
                    new BookShelfItem { ShelfId = shelf.Id, BookId = book.Id, DateAdded = DateTime.UtcNow })).ToList();

            List<Recommendation> recommendations = books.Take(20).Select(b => new Recommendation
            {
                BookId = b.Id,
                UserId = _faker.PickRandom(users).Id,
                RecommendedById = _faker.PickRandom(users).Id,
                Reason = _faker.Lorem.Sentence(),
                DateRecommended = DateTime.UtcNow,
                Status = _faker.Random.Enum<ERecommendationStatus>(),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

            List<Wishlist> wishlists = users.SelectMany(u => _faker.PickRandom(books, 3).Select(b => new Wishlist
            {
                UserId = u.Id, BookId = b.Id, DateAdded = DateTime.UtcNow
            })).ToList();

            List<BookGenre> bookGenres = books.SelectMany(b =>
                _faker.PickRandom(genres, 2).Select(g => new BookGenre { BookId = b.Id, GenreId = g.Id })).ToList();

            List<BookAuthor> bookAuthors = books.SelectMany(b =>
                _faker.PickRandom(authors, 2).Select(a => new BookAuthor { BookId = b.Id, AuthorId = a.Id })).ToList();


            await _context.Reviews.AddRangeAsync(reviews);
            await _context.BookShelfItem.AddRangeAsync(shelfItems);
            await _context.ReadLog.AddRangeAsync(readLogs);
            await _context.Recommendation.AddRangeAsync(recommendations);
            await _context.Wishlist.AddRangeAsync(wishlists);
            await _context.BookGenre.AddRangeAsync(bookGenres);
            await _context.BookAuthor.AddRangeAsync(bookAuthors);
        }
    }
}