using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class FeaturePolicyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _policy;
    private readonly string _featureFlag;

    public FeaturePolicyAuthorizeAttribute(string policy, string featureFlag)
    {
        _policy = policy;
        _featureFlag = featureFlag;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var featureManager = context.HttpContext.RequestServices.GetService(typeof(IFeatureManager)) as IFeatureManager;
        var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
        var user = context.HttpContext.User;

        if (featureManager == null || authService == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var isFeatureEnabled = await featureManager.IsEnabledAsync(_featureFlag);
        if (!isFeatureEnabled)
        {
            context.Result = new NotFoundResult(); // lub ForbidResult jeśli wolisz
            return;
        }

        var authResult = await authService.AuthorizeAsync(user, _policy);
        if (!authResult.Succeeded)
        {
            context.Result = new ForbidResult();
        }
    }
}

[FeaturePolicyAuthorize("AdminOnly", "MyCoolFeature")]
public IActionResult SecretStuff()
{
    return Ok("You got in!");
}