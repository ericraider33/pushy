using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace PushyCommon
{
    public class LoginCookieInfo
    {
        public const String KEY_LOGIN_INFO = "LoginInfo";

        public String userName { get; set; }
        public String[] roles { get; set; }
        public String timeZoneId { get; set; }
        public int ticketTimeoutMinutes { get; set; }

        public ClaimsIdentity generateIdentity()
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(nameof(userName), userName));
            if (roles != null)
            {
                foreach (String role in roles)
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            identity.AddClaim(new Claim(nameof(ticketTimeoutMinutes), ticketTimeoutMinutes.ToString()));
            identity.AddClaim(new Claim(nameof(timeZoneId), timeZoneId));

            return identity;
        }

        public static LoginCookieInfo fromClaims(IEnumerable<Claim> claims)
        {
            LoginCookieInfo result = new LoginCookieInfo();

            List<String> roles = new List<String>();
            foreach (Claim claim in claims)
            {
                switch (claim.Type)
                {
                    case nameof(userName):
                        result.userName = claim.Value;
                        break;
                    case ClaimTypes.Role:
                        roles.Add(claim.Value);
                        break;
                    case nameof(ticketTimeoutMinutes):
                        result.ticketTimeoutMinutes = int.Parse(claim.Value);
                        break;
                    case nameof(timeZoneId):
                        result.timeZoneId = claim.Value;
                        break;
                }
            }

            result.roles = roles.ToArray();
            if (result.userName == null)
                throw new Exception();

            return result;
        }

        public override string ToString()
        {
            return $"{nameof(userName)}: {userName}, " +
                   $"{nameof(roles)}: {roles}, " +
                   $"{nameof(ticketTimeoutMinutes)}: {ticketTimeoutMinutes}";
        }

        public static LoginCookieInfo getLoginCookieInfo(ClaimsPrincipal mvcUser)
        {
            if (mvcUser == null)
                return null;

            // Checks if not authenticated
            if (!mvcUser.Identities.Any(i => i.IsAuthenticated))
                return null;

            return fromClaims(mvcUser.Claims);
        }
    }
}