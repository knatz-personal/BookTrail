using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrail.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthor_Author_AuthorId",
                table: "BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthor_Books_BookId",
                table: "BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Books_BookId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genre_GenreId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelf_Users_UserId",
                table: "BookShelf");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelfItem_BookShelf_ShelfId",
                table: "BookShelfItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelfItem_Books_BookId",
                table: "BookShelfItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLog_Books_BookId",
                table: "ReadLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLog_Reviews_ReviewId",
                table: "ReadLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLog_Users_UserId",
                table: "ReadLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_Users_RecommendedById",
                table: "Recommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Books_BookId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Users_UserId",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadLog",
                table: "ReadLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookShelfItem",
                table: "BookShelfItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookShelf",
                table: "BookShelf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenre",
                table: "BookGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthor",
                table: "BookAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author",
                table: "Author");

            migrationBuilder.RenameTable(
                name: "Wishlist",
                newName: "Wishlists");

            migrationBuilder.RenameTable(
                name: "ReadLog",
                newName: "ReadLogs");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "BookShelfItem",
                newName: "BookShelfItems");

            migrationBuilder.RenameTable(
                name: "BookShelf",
                newName: "BookShelves");

            migrationBuilder.RenameTable(
                name: "BookGenre",
                newName: "BookGenres");

            migrationBuilder.RenameTable(
                name: "BookAuthor",
                newName: "BookAuthors");

            migrationBuilder.RenameTable(
                name: "Author",
                newName: "Authors");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_BookId",
                table: "Wishlists",
                newName: "IX_Wishlists_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadLog_ReviewId",
                table: "ReadLogs",
                newName: "IX_ReadLogs_ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadLog_BookId",
                table: "ReadLogs",
                newName: "IX_ReadLogs_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookShelfItem_BookId",
                table: "BookShelfItems",
                newName: "IX_BookShelfItems_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookShelf_UserId",
                table: "BookShelves",
                newName: "IX_BookShelves_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_GenreId",
                table: "BookGenres",
                newName: "IX_BookGenres_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthor_AuthorId",
                table: "BookAuthors",
                newName: "IX_BookAuthors_AuthorId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedById",
                table: "Recommendation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PublishedDate",
                table: "Books",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStarted",
                table: "ReadLogs",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFinished",
                table: "ReadLogs",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadLogs",
                table: "ReadLogs",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookShelfItems",
                table: "BookShelfItems",
                columns: new[] { "ShelfId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookShelves",
                table: "BookShelves",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors",
                columns: new[] { "BookId", "AuthorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelfItems_BookShelves_ShelfId",
                table: "BookShelfItems",
                column: "ShelfId",
                principalTable: "BookShelves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelfItems_Books_BookId",
                table: "BookShelfItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelves_Users_UserId",
                table: "BookShelves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLogs_Books_BookId",
                table: "ReadLogs",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLogs_Reviews_ReviewId",
                table: "ReadLogs",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLogs_Users_UserId",
                table: "ReadLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_Users_RecommendedById",
                table: "Recommendation",
                column: "RecommendedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelfItems_BookShelves_ShelfId",
                table: "BookShelfItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelfItems_Books_BookId",
                table: "BookShelfItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookShelves_Users_UserId",
                table: "BookShelves");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLogs_Books_BookId",
                table: "ReadLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLogs_Reviews_ReviewId",
                table: "ReadLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadLogs_Users_UserId",
                table: "ReadLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_Users_RecommendedById",
                table: "Recommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadLogs",
                table: "ReadLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookShelves",
                table: "BookShelves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookShelfItems",
                table: "BookShelfItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "Wishlist");

            migrationBuilder.RenameTable(
                name: "ReadLogs",
                newName: "ReadLog");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "BookShelves",
                newName: "BookShelf");

            migrationBuilder.RenameTable(
                name: "BookShelfItems",
                newName: "BookShelfItem");

            migrationBuilder.RenameTable(
                name: "BookGenres",
                newName: "BookGenre");

            migrationBuilder.RenameTable(
                name: "BookAuthors",
                newName: "BookAuthor");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Author");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlist",
                newName: "IX_Wishlist_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadLogs_ReviewId",
                table: "ReadLog",
                newName: "IX_ReadLog_ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadLogs_BookId",
                table: "ReadLog",
                newName: "IX_ReadLog_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookShelves_UserId",
                table: "BookShelf",
                newName: "IX_BookShelf_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookShelfItems_BookId",
                table: "BookShelfItem",
                newName: "IX_BookShelfItem_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenre",
                newName: "IX_BookGenre_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthor",
                newName: "IX_BookAuthor_AuthorId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedById",
                table: "Recommendation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateStarted",
                table: "ReadLog",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateFinished",
                table: "ReadLog",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadLog",
                table: "ReadLog",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookShelf",
                table: "BookShelf",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookShelfItem",
                table: "BookShelfItem",
                columns: new[] { "ShelfId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenre",
                table: "BookGenre",
                columns: new[] { "BookId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthor",
                table: "BookAuthor",
                columns: new[] { "BookId", "AuthorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author",
                table: "Author",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthor_Author_AuthorId",
                table: "BookAuthor",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthor_Books_BookId",
                table: "BookAuthor",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Books_BookId",
                table: "BookGenre",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genre_GenreId",
                table: "BookGenre",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelf_Users_UserId",
                table: "BookShelf",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelfItem_BookShelf_ShelfId",
                table: "BookShelfItem",
                column: "ShelfId",
                principalTable: "BookShelf",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookShelfItem_Books_BookId",
                table: "BookShelfItem",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLog_Books_BookId",
                table: "ReadLog",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLog_Reviews_ReviewId",
                table: "ReadLog",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadLog_Users_UserId",
                table: "ReadLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_Users_RecommendedById",
                table: "Recommendation",
                column: "RecommendedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Books_BookId",
                table: "Wishlist",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Users_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
