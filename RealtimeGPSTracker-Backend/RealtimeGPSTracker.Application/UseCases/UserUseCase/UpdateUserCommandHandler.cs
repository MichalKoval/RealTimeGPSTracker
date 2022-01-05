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
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserHubService _userHubService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            UserManager<User> userManager,
            IUserHubService userHubService,
            IMediator mediator,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userHubService = userHubService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Holds user update response
            UpdateUserResponse userUpdateResponse = new UpdateUserResponse { Success = true };
            UpdateUserDetailsResponse userDetailsUpdateResponse = new UpdateUserDetailsResponse { Success = true };
            UpdateUserPasswordResponse userPasswordUpdateResponse = new UpdateUserPasswordResponse { Success = true };

            if (request.UserDetails == null && request.UserPassword == null)
            {
                userUpdateResponse.Success = false;
                userUpdateResponse.Errors.Add("User update request is empty");
                return userUpdateResponse;
            }

            if (request.UserDetails != null)
            {
                request.UserDetails.OwnerId = request.OwnerId;

                // Attempt to update user details
                userDetailsUpdateResponse = await _mediator.Send(request.UserDetails);
            }

            if (request.UserPassword != null)
            {
                request.UserPassword.OwnerId = request.OwnerId;

                // Attemp to update user password
                userPasswordUpdateResponse = await _mediator.Send(request.UserPassword);
            }

            // If error occured in user details or user password update command
            userUpdateResponse.Success = userDetailsUpdateResponse.Success && userPasswordUpdateResponse.Success;

            if (!userUpdateResponse.Success)
            {
                userUpdateResponse.Errors.AddRange(userDetailsUpdateResponse.Errors);
                userUpdateResponse.Errors.AddRange(userPasswordUpdateResponse.Errors);
            }

            return userUpdateResponse;
        }
    }
}
