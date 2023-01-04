
using APIkino.Data;
using APIkino.Extantions;
using APIkino.Repositories;
using APIkino.Repositories.Contracts;
using com.sun.corba.se.spi.activation;
using com.sun.xml.@internal.bind.v2.model.core;
using KinoClass.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Convert = System.Convert;

namespace APIkino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        //dont forget to register the repository in program.cs
        private readonly IShoping shoping;
        private readonly IRepository repository;

        public UserController(IShoping shoping,
                            IRepository repository,
                           ILogger<UserController> logger)
        {
            this.shoping = shoping;
            this.repository = repository;
            _logger = logger;

        }

         
        [HttpGet("CurrentUser")]
        public int GetLoggetInUserId()
        {
            int id = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            return id;
        }


    


        [HttpGet("GetItems")]
       
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> CartItems()
        {
            
            try
            {
                int userId = GetLoggetInUserId();
                // here we returned cartItem object
                var cartItems = await this.shoping.CartItems(userId);

                if (cartItems == null)
                {
                    // if ther is no items in the in the shoping cart
                    return NoContent();
                }

                //since we are getting an CartItemDTO item
                //to the client from the actionMethod
                //but we dont have all the data in the cart Item
                //that we return from the Repository so
                //we want so we need to get it from _repository
                var movies = await this.repository.GetAll();

                //we can make convertion to get the cartItemDto (in extention)


                //if there is no movies in the sysem
                if (movies == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var UserCartItems = cartItems.ConvertToDto(movies);
               
                return Ok(UserCartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "the GET call to /api/user fieled");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);

            }
            

        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<CartItemDTO>> GetItem( int Id)
        {
            try
            {
                var cartItem = await this.shoping.GetItem(Id);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var movie = await this.repository.Geten(cartItem.MovieId);
                if (movie == null)
                {
                    _logger.LogError("the GET call to /api/user/{Id} fieled", Id);
                    return NotFound();


                }
                var cartItemDTO = cartItem.convertionDTO(movie);
                return Ok(cartItemDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "the GET call to /api/user/{Id} fieled", Id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error getting data from the database"
                     );

            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> AddItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var AddMovie = await this.shoping.AddItem(cartItemToAddDto);
                if (AddMovie == null)
                {
                    return BadRequest("some thing went wrong");
                }
                var movie = await this.repository.Geten(AddMovie.MovieId);
                if (movie == null)
                {
                    _logger.LogError("the Add call to /api/user fieled");
                    throw new Exception($"Something went wrong when attempting to" +
                        $" retrieve movies (movieId:({cartItemToAddDto.MovieId})");
                }
                var MovieDto = AddMovie.convertionDTO(movie);

               
                

                return CreatedAtAction(nameof(AddItem), new { id = MovieDto.Id }, MovieDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "the GET call to /api/user fieled");
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error getting data from the database"
                     );
            }
        }

        [HttpDelete("{Id:int}")]
        

        public async Task<ActionResult<CartItemDTO>> DeleteItem( int id)
        {
            try
            {
                var cartItem = await this.shoping.DeleteItem(id);
                if (cartItem == null)
                {
                    _logger.LogError("the delete call to /api/user fieled");
                    return NotFound("feil i shoping");
                }

                var movie = await this.repository.Geten(cartItem.Id);
                if (movie == null)
                {
                    _logger.LogError("the Delete call to /api/user fieled");
                    return NotFound(" feil i movie");
                }
                var cartItemDto = cartItem.convertionDTO(movie);
                return Ok(cartItemDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "the Delete call to /api/user fieled");
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error getting data from the database"
                     );

            }
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDTO>> UpdateItem(int Id,CartItemMengdeUpdate cartUpdate)
        {
            try
            {
                var Item = await this.shoping.UpdateItem(Id, cartUpdate);
                if (Item == null)
                {
                    _logger.LogError("the update call to /api/user fieled");
                    return NotFound("feil is shoping");
                }
                var movie = await this.repository.Geten(Item.MovieId);
                if (movie == null)
                {
                    _logger.LogError("the Delete call to /api/user fieled");
                    return NotFound("");
                }
                var cartItemDto = Item.convertionDTO(movie);
                return Ok(cartItemDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "the update call to /api/user fieled");
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error getting data from the database"
                     );
            }
        }
        
        
    }
}
