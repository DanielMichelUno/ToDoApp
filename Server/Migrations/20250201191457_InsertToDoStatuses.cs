using Microsoft.EntityFrameworkCore.Migrations;
using Server.Enums;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InsertToDoStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ToDoStatuses",
                columns: ["Id", "Name"],
                values: new object[,]
                {
                    { (int)ToDoStatuses.ToDo, "To do"  },
                    { (int)ToDoStatuses.InProgress, "In Progress"  },
                    { (int)ToDoStatuses.Done, "Done"  },
                    { (int)ToDoStatuses.Overdue, "Overdue"  }
                }
            );
        }
    }
}
