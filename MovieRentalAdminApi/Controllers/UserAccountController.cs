using Microsoft.AspNetCore.Mvc;
using MovieRentalAdminApi.CrossCutting;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Interfaces;
using MovieRentalAdminApi.Dto;
using MovieRentalAdminApi.Utils;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ITokenFactory _tokenFactory;

        public UserAccountController(IUserAccountRepository userAccountRepository, ITokenFactory tokenFactory)
        {
            _userAccountRepository = userAccountRepository;
            _tokenFactory = tokenFactory;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody]SignUpUserDto user)
        {
            var account = await _userAccountRepository.GetByUserName(user.UserName);
            if (account != null)
                return BadRequest($"There is already an account registered with username: {user.UserName}");
            var _user = new UserAccountEntity().Create(user.UserName, user.Password.ComputeSha256Hash(), user.Role);

            await _userAccountRepository.CreateUser(_user);
            return Ok();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login([FromBody]SignInDto item)
        {
            var account = await _userAccountRepository.GetByUserNameAndPassword(item.UserName, item.Password.ComputeSha256Hash());
            if (account == null)
                return NotFound("User account not found.");

            var token = _tokenFactory.GenerateToken(account.UserName, account.Role);

            if(token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}