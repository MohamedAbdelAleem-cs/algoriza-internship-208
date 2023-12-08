using Core.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authorization
{
    public class AccountTypeHandler:AuthorizationHandler<AccountTypeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountTypeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountTypeRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }
            var accountTypeClaim = context.User.FindFirst("AccountType")?.Value;
            if (Enum.TryParse(accountTypeClaim, out AccountType userAccountType))
            {
                if (userAccountType == requirement.RequiredAccountType)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

