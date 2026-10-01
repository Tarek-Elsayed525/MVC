using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc.Migrations
{
    /// <inheritdoc />
    public partial class add_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into [dbo].[Categories] (Name, Description, Status) values ('Mobiles', 'Nulla facilisi.', 0);insert into [dbo].[Categories] (Name, Description, Status) values ('Laptops', 'Aliquam quis turpis eget elit sodales scelerisque.', 1);insert into [dbo].[Categories] (Name, Description, Status) values ('Tablets', 'Cras non velit nec nisi vulputate nonummy.', 1);insert into [dbo].[Categories] (Name, Description, Status) values ('Acssories', 'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.', 0);insert into [dbo].[Categories] (Name, Description, Status) values ('Cameras', 'Pellentesque ultrices mattis odio.', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from [dbo].[Categories] where Name in ('Mobiles', 'Laptops', 'Tablets', 'Acssories', 'Cameras');");
        }
    }
}
