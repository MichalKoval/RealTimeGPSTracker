using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Queries.UserQueries;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.UserUseCase
{
    public class DetailUserQueryHandler : IRequestHandler<DetailUserQuery, DetailUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public DetailUserQueryHandler(
            UserManager<User> userManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<DetailUserResponse> Handle(DetailUserQuery request, CancellationToken cancellationToken)
        {
            // Holds user detail response
            DetailUserResponse userDetailResponse = new DetailUserResponse();

            // Checking if user exists
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user != null)
            {
                userDetailResponse = _mapper.Map<DetailUserResponse>(user);
                userDetailResponse.Success = true;
            }
            else
            {
                userDetailResponse.Success = false;
                userDetailResponse.Errors.Add("You don't have a permission to detail a user");
            }

            return userDetailResponse;
        }
    }
}
