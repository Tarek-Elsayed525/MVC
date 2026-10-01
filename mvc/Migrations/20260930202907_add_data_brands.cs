using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc.Migrations
{
    /// <inheritdoc />
    public partial class add_data_brands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Brands (Name, Description, Status) values ('Apple', 'Fusce consequat.', 0);insert into Brands (Name, Description, Status) values ('hp', 'Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.', 1);insert into Brands (Name, Description, Status) values ('Dell', 'Duis bibendum, felis sed interdum venenatis.', 0);insert into Brands (Name, Description, Status) values ('Oppo', 'Donec ut mauris eget massa tempor convallis.', 0);insert into Brands (Name, Description, Status) values ('LG', 'Nullam sit amet turpis elementum ligula vehicula consequat.', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Brands where Name in ('Apple', 'hp', 'Dell', 'Oppo', 'LG');");
        }
    }
}
