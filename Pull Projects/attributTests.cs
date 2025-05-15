using Xunit;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;

public class FeaturePolicyAuthorizeAttributeTests
{
    private static AuthorizationFilterContext CreateAuthContext(IServiceProvider serviceProvider, ClaimsPrincipal user = null)
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext { RequestServices = serviceProvider, User = user ?? new ClaimsPrincipal() }
        };

        var filters = new List<IFilterMetadata>();
        var context = new AuthorizationFilterContext(actionContext, filters);

        return context;
    }

    [Fact]
    public async Task FeatureDisabled_AllowsAccess()
    {
        // Arrange
        var featureManagerMock = new Mock<IFeatureManager>();
        featureManagerMock.Setup(fm => fm.IsEnabledAsync("MyFeature")).ReturnsAsync(false);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IFeatureManager))).Returns(featureManagerMock.Object);

        var attribute = new FeaturePolicyAuthorizeAttribute("TestPolicy", "MyFeature");

        var context = CreateAuthContext(serviceProvider.Object);

        // Act
        await attribute.OnAuthorizationAsync(context);

        // Assert
        Assert.Null(context.Result); // dostęp dozwolony
    }

    [Fact]
    public async Task FeatureEnabled_And_AuthorizationFails_ReturnsForbid()
    {
        // Arrange
        var featureManagerMock = new Mock<IFeatureManager>();
        featureManagerMock.Setup(fm => fm.IsEnabledAsync("MyFeature")).ReturnsAsync(true);

        var authServiceMock = new Mock<IAuthorizationService>();
        authServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "TestPolicy"))
                       .ReturnsAsync(AuthorizationResult.Failed());

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IFeatureManager))).Returns(featureManagerMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(IAuthorizationService))).Returns(authServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("type", "value") }));

        var attribute = new FeaturePolicyAuthorizeAttribute("TestPolicy", "MyFeature");
        var context = CreateAuthContext(serviceProvider.Object, user);

        // Act
        await attribute.OnAuthorizationAsync(context);

        // Assert
        Assert.IsType<ForbidResult>(context.Result);
    }

    [Fact]
    public async Task FeatureEnabled_And_AuthorizationSucceeds_AllowsAccess()
    {
        // Arrange
        var featureManagerMock = new Mock<IFeatureManager>();
        featureManagerMock.Setup(fm => fm.IsEnabledAsync("MyFeature")).ReturnsAsync(true);

        var authServiceMock = new Mock<IAuthorizationService>();
        authServiceMock.Setup(a => a.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "TestPolicy"))
                       .ReturnsAsync(AuthorizationResult.Success());

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IFeatureManager))).Returns(featureManagerMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(IAuthorizationService))).Returns(authServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var attribute = new FeaturePolicyAuthorizeAttribute("TestPolicy", "MyFeature");
        var context = CreateAuthContext(serviceProvider.Object, user);

        // Act
        await attribute.OnAuthorizationAsync(context);

        // Assert
        Assert.Null(context.Result); // dostęp dozwolony
    }
}