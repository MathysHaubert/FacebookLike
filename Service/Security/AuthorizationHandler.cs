using FacebookLike.Service.Security;
using Microsoft.AspNetCore.Components;

namespace FacebookLike.Service;

public class AuthorizationHandler
{
    internal readonly IAuthService _authService;
    private readonly NavigationManager _navigationManager;

    public AuthorizationHandler(IAuthService authService, NavigationManager navigationManager)
    {
        _authService = authService;
        _navigationManager = navigationManager;
    }

    public void EnsureAuthenticated()
    {
        if (_authService.CurrentUser == null)
        {
            var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            
            _navigationManager.NavigateTo(returnUrl.Length > 0 ? $"/login?returnUrl=/{returnUrl}" : "/login");
        }
    }
}