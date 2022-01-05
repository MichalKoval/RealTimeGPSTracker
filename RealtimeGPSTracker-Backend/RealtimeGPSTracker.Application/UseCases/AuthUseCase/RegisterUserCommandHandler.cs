using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.AuthCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.AuthResponses;
using RealtimeGpsTracker.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.AuthUseCase
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Holds info about success or all the errors related to registration
            RegisterUserResponse registerUserResponse = new RegisterUserResponse { Success = true };

            // Maps User Dto to User entity
            var user = _mapper.Map<User>(request);

            // Setting User unique Guid Id, TODO: Fix to do that automatically
            user.Id = Guid.NewGuid().ToString();

            // Setting User security stamp
            user.SecurityStamp = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                registerUserResponse.Success = false;

                foreach (var error in result.Errors)
                {
                    registerUserResponse.Errors.Add(error.Description);
                }
            }

            return registerUserResponse;
        }
    }
}
