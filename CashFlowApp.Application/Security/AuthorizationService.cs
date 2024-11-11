using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Application.Security
{
    public class AuthorizationService
    {
        private readonly Dictionary<string, List<string>> _userPermissions;

        public AuthorizationService()
        {
            _userPermissions = new Dictionary<string, List<string>>
            {
                { "Admin", new List<string> { "AddTransaction", "ViewBalance", "DeleteTransaction" } },
                { "User", new List<string> { "AddTransaction", "ViewBalance" } }
            };
        }

        public bool Authorize(string role, string action)
        {
            return _userPermissions.ContainsKey(role) && _userPermissions[role].Contains(action);
        }
    }
}
