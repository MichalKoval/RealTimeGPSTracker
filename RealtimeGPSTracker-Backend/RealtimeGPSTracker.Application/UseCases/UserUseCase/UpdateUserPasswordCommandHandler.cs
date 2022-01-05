using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.UserCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.UserUseCase
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdateUserPasswordResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserHubService _userHubService;
        private readonly IMapper _mapper;

        public UpdateUserPasswordCommandHandler(
            UserManager<User> userManager,
            IUserHubService userHubService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userHubService = userHubService;
            _mapper = mapper;
        }

        public async Task<UpdateUserPasswordResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // Holds user update response
            UpdateUserPasswordResponse userPasswordUpdateResponse = new UpdateUserPasswordResponse();

            // Checking if user exists
            var user = await _userManager.FindByIdAsync(request.OwnerId);
            
            if (user != null)
            {
                // Attempt to update the user password
                
                // Generating user password reset token
                var userPasswordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                if (userPasswordResetToken != null)
                {
                    // Reseting password
                    var userUpdatePasswordResult = await _userManager.ResetPasswordAsync(user, userPasswordResetToken, request.Password);

                    if (userUpdatePasswordResult != IdentityResult.Success)
                    {
                        userPasswordUpdateResponse.Success = false;
                        foreach (var error in userUpdatePasswordResult.Errors)
                        {
                            userPasswordUpdateResponse.Errors.Add(error.Description);
                        }
                    }
                    else
                    {
                        await _userHubService.SendUpdateMessageToUserGroup(user.Id, "");
                        userPasswordUpdateResponse.Success = true;
                    }
                }
                else
                {
                    userPasswordUpdateResponse.Success = false;
                    userPasswordUpdateResponse.Errors.Add("There was an error in attempt to update a password");
                }
            }
            else
            {
                userPasswordUpdateResponse.Success = false;
                userPasswordUpdateResponse.Errors.Add("You don't have a permission to update user password");
            }

            return userPasswordUpdateResponse;
        }
    }
}
