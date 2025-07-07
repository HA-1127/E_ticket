using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_ticket.Migrations
{
    /// <inheritdoc />
    public partial class dataactormovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into ActorsMovies (ActorId, MovieId) values (4, 4); insert into ActorsMovies (ActorId, MovieId) values (13, 14); insert into ActorsMovies (ActorId, MovieId) values (30, 17); insert into ActorsMovies (ActorId, MovieId) values (13, 16); insert into ActorsMovies (ActorId, MovieId) values (13, 6); insert into ActorsMovies (ActorId, MovieId) values (17, 24); insert into ActorsMovies (ActorId, MovieId) values (16, 9); insert into ActorsMovies (ActorId, MovieId) values (12, 2); insert into ActorsMovies (ActorId, MovieId) values (21, 4); insert into ActorsMovies (ActorId, MovieId) values (5, 6); insert into ActorsMovies (ActorId, MovieId) values (18, 2); insert into ActorsMovies (ActorId, MovieId) values (19, 23); insert into ActorsMovies (ActorId, MovieId) values (18, 8); insert into ActorsMovies (ActorId, MovieId) values (16, 16); insert into ActorsMovies (ActorId, MovieId) values (3, 20); insert into ActorsMovies (ActorId, MovieId) values (29, 2); insert into ActorsMovies (ActorId, MovieId) values (18, 19); insert into ActorsMovies (ActorId, MovieId) values (23, 16); insert into ActorsMovies (ActorId, MovieId) values (19, 12); insert into ActorsMovies (ActorId, MovieId) values (15, 10); insert into ActorsMovies (ActorId, MovieId) values (26, 9); insert into ActorsMovies (ActorId, MovieId) values (18, 5); insert into ActorsMovies (ActorId, MovieId) values (5, 7); insert into ActorsMovies (ActorId, MovieId) values (13, 21); insert into ActorsMovies (ActorId, MovieId) values (13, 16); ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("truncate table ActorsMovies");
        }
    }
}
