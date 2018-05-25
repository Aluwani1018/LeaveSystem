using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeaveSystem.Presentation.Helpers
{
    public static class Utilities
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }
        public static string[] GetRoles(ClaimsPrincipal identity)
        {
            return identity.Claims
                .Where(c => c.Type == OpenIdConnectConstants.Claims.Role)
                .Select(c => c.Value)
                .ToArray();
        }
        private static void AddErrors(IEnumerable<string> result,ref ModelStateDictionary ModelState)
        {
            foreach (var error in result)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}
