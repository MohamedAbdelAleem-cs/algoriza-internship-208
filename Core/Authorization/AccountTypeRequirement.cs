using Core.Const;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authorization
{
    public class AccountTypeRequirement:IAuthorizationRequirement
    {
        public AccountType RequiredAccountType { get; }

        public AccountTypeRequirement(AccountType accountType)
        {
            RequiredAccountType = accountType;
        }
    }
}
