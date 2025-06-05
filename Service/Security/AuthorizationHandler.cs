using FacebookLike.Service.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FacebookLike.Service;

public class AuthorizationHandler
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    public AuthorizationHandler(AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
    {
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
    }

    public async Task EnsureAuthenticated()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        if (!(authState.User.Identity?.IsAuthenticated ?? false))
        {
            var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            _navigationManager.NavigateTo(returnUrl.Length > 0 ? $"/login?returnUrl=/{returnUrl}" : "/login");
        }
    }
}