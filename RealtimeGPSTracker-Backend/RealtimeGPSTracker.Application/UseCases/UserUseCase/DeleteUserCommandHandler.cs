using MediatR;
using Microsoft.AspNetCore.Identity;
using RealtimeGpsTracker.Core.Commands.UserCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.UseCases.UserUseCase
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserHubService _userHubService;

        public DeleteUserCommandHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            IUserHubService userHubService
            )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _userHubService = userHubService;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Will hold result
            DeleteUserResponse userDeleteResponse = new DeleteUserResponse { Success = true };

            // Checking if user exists
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user != null)
            {
                await _userRepository.DeleteUserAsync(user.Id);
                await _userHubService.SendUpdateMessageToUserGroup(user.Id, "");
            }
            else
            {
                userDeleteResponse.Success = false;
                userDeleteResponse.Errors.Add("You don't have a permission to delete a user");
            }

            return userDeleteResponse;
        }
    }
}

// Getting all user's logins.
//var userLogins = await _userManager.GetLoginsAsync(user);
// Getting all user's roles.
//var userRoles = await _userManager.GetRolesAsync(user);

// Delete must be proceeded as a transaction operation.
//using (var transaction = _context.Database.BeginTransaction())
//{
//    // First removing all user logins.
//    var removeUserLoginResult = IdentityResult.Success;
//    foreach (var userLogin in userLogins)
//    {
//        removeUserLoginResult = await _userManager.RemoveLoginAsync(user, userLogin.LoginProvider, userLogin.ProviderKey);
//        if (removeUserLoginResult != IdentityResult.Success) break;
//    }

//    // Removing all user's roles if the previous operation was successful.
//    var removeUserRoleResult = IdentityResult.Success;
//    if (removeUserLoginResult == IdentityResult.Success)
//    {
//        foreach (var userRole in userRoles)
//        {
//            removeUserRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole);
//            if (removeUserRoleResult != IdentityResult.Success) break;
//        }
//    }
//    else
//    {
//        userDeleteResponse.Success = false;
//        foreach (var error in removeUserLoginResult.Errors)
//        {
//            userDeleteResponse.Errors.Add(error.Description);
//        }
//    }

//    // TODO: FIX Cascade delete: Removing all other data related to the user if previous operations were successful
//    var removeUserDataResult = IdentityResult.Success;
//    if (removeUserRoleResult == IdentityResult.Success)
//    {

//    }
//    else
//    {
//        userDeleteResponse.Success = false;
//        foreach (var error in removeUserRoleResult.Errors)
//        {
//            userDeleteResponse.Errors.Add(error.Description);
//        }
//    }

//    // Removing user itself if previous operations were successful.
//    var removeUserResult = IdentityResult.Success;
//    if (removeUserDataResult == IdentityResult.Success)
//    {
//        // Removing user itself
//        removeUserResult = await _userManager.DeleteAsync(user);
//        if (removeUserResult == IdentityResult.Success)
//            // Commiting if all previous attempts to delete user related data in transaction were successful.
//            transaction.Commit();
//    }
//    else
//    {
//        userDeleteResponse.Success = false;
//        foreach (var error in removeUserDataResult.Errors)
//        {
//            userDeleteResponse.Errors.Add(error.Description);
//        }
//    }
//}
