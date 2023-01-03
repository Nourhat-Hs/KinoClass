using APIkino.Data;
using APIkino.Repositories.Contracts;
using KinoClass.Models;
using Microsoft.EntityFrameworkCore;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIkino.Repositories
{
    //husk å ha generere i program.cs
    public class MoviesRepository : IRepository
    {


        private readonly Context _context;
        public MoviesRepository(Context context)
        {
            _context = context;


        }


        public async Task<IEnumerable<Movies>> GetAll()
        {


            //sjekk the logginn
            //? will chacke if it is null or not if it is not null it will get the value 


            return await (from movie in _context.movies
                                select new Movies
                                {
                                    Id = movie.Id,
                                    MovieName= movie.MovieName,
                                    description= movie.description,
                                    price= movie.price,
                                    mengde= movie.mengde,
                             
                                }).ToListAsync();

        }



        public async Task<Movies> Geten(int Id)
        {
            var movie = await _context.movies.FindAsync(Id);
            return movie;
        }

        public async Task<Movies> AddMovie(Movies mr)
        {
            var Movie = new Movies()
            {
                Id = mr.Id,
                MovieName = mr.MovieName,
                description = mr.description,
                price = mr.price,
                mengde = mr.mengde
            };
            //takl to the database using context for store the new movie 
            await _context.movies.AddAsync(Movie);
            await _context.SaveChangesAsync();
            return Movie;
        }

        public async Task<Movies> UpdateMovie(int Id, Movies update)
        {
            var movie = await _context.movies.FindAsync(Id);


            movie.MovieName = update.MovieName;
            movie.description = update.description;
            movie.price = update.price;
            movie.mengde = update.mengde;

            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<Movies> Delete(int Id)
        {
            var movie = await _context.movies.FindAsync(Id);


            _context.Remove(movie);

            await _context.SaveChangesAsync();

            return movie;

        }


       
    }

}
