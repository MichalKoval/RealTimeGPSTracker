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
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Holds info about success or all the errors related to refresh a security token
            RefreshTokenResponse refreshTokenResponse = new RefreshTokenResponse { Success = true };

            // TODO: Implement refreshing token mechanism !!!

            return refreshTokenResponse;
        }
    }
}
