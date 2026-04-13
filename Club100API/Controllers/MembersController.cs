using Club100API.Repositories;
using Club100API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Club100API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersRepository _repo;

        public MembersController(IMembersRepository repo)
        {
            _repo = repo;
        }
        // GET: api/<MembersController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        //public ActionResult<IEnumerable<Member>> GetAll()
        //{
        //    return Ok(_repo.GetAll());
        //}

        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetAll()
        {
            throw new Exception("Saboteur test - deliberate break");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("filter")]

        // [Authorize(Roles = "User, Admin")]

        public ActionResult<IEnumerable<Member>> GetMembersByCountAndName(
            [FromQuery] int? minCount,
            [FromQuery] int? maxCount,
            [FromQuery] string? nameFilter)
        {
            try
            {
                IEnumerable<Member> result = _repo.GetMembersByCountAndName(minCount
                    , maxCount, nameFilter);
                if (result == null || result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<CatsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        // [Authorize(Roles = "User, Admin")]
        public ActionResult<Member> Get(int id)
        {
            Member? member = _repo.GetById(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        // POST api/<CatsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        // [Authorize(Roles = "Admin")]
        public ActionResult<Member> Post([FromBody] Member newMember)
        {
            Member theMember = _repo.Add(newMember);
            string uri = Url.RouteUrl(RouteData.Values) + "/" + theMember.Id;
            return Created(uri, theMember);
        }

        // PUT api/<CatsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Member> Put(int id, [FromBody] Member value)
        {
            Member? updatedMember = _repo.Update(id,  value);
            if (updatedMember == null) { return NotFound("No such member, id: " + id); }
            return Ok(updatedMember);
        }

        // DELETE api/<CatsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Member> Delete(int id)
        {
            Member? removedmember = _repo.Delete(id);
            if (removedmember == null) { return NotFound("No such member, id: " + id); }
            return Ok(removedmember);
        }

        /*[HttpOptions]
        public void Options()
        {
        }*/
    }
}
