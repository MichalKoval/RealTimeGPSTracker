using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.AuthCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.AuthResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.AuthUseCase
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(
            UserManager<User> userManager,
            IJwtService jwtService
            )
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Holds access_token or all the errors related to login
            LoginUserResponse loginUserResponse = new LoginUserResponse { Success = true };

            // Try to sign in the user
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                loginUserResponse.Success = false;
                loginUserResponse.Errors.Add("User with the user name doesn't exist");
                return loginUserResponse;
            }


            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                loginUserResponse.Success = false;
                loginUserResponse.Errors.Add("User name or password is incorrect");
                return loginUserResponse;
            }

            // FUTURE: Extend user capabilities to have a role based authentication.
            //var roles = await _userManager.GetRolesAsync(user);

            //Generating security access token for a user
            loginUserResponse.AccessToken = _jwtService.GenerateSecurityToken(user.Id.ToString());

            // Returns User Id and security token associated with the User. 
            // Token is stored on client side and is appended to every request coming from client side to the server to recognize that user is logged in.
            // This token is valid for a certain amount of time. After that time relogin is needed to get a new security token.
            return loginUserResponse;
        }
    }
}
