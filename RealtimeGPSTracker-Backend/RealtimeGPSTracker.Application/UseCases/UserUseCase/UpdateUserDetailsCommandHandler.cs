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
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand, UpdateUserDetailsResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserHubService _userHubService;
        private readonly IMapper _mapper;

        public UpdateUserDetailsCommandHandler(
            UserManager<User> userManager,
            IUserHubService userHubService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userHubService = userHubService;
            _mapper = mapper;
        }

        public async Task<UpdateUserDetailsResponse> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            // Holds user update response
            UpdateUserDetailsResponse userUpdateResponse = new UpdateUserDetailsResponse();

            // Checking if user exists
            var user = await _userManager.FindByIdAsync(request.OwnerId);
            
            if (user != null)
            {
                // Mapping changes to user
                user = _mapper.Map<UpdateUserDetailsCommand, User>(request, user);

                // Attempt to update the user
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (userUpdateResult != IdentityResult.Success)
                {
                    userUpdateResponse.Success = false;
                    foreach (var error in userUpdateResult.Errors)
                    {
                        userUpdateResponse.Errors.Add(error.Description);
                    }
                }
                else
                {
                    await _userHubService.SendUpdateMessageToUserGroup(user.Id, "");
                    userUpdateResponse.Success = true;
                }
            }
            else
            {
                userUpdateResponse.Success = false;
                userUpdateResponse.Errors.Add("You don't have a permission to update user information");
            }

            return userUpdateResponse;
        }
    }
}
