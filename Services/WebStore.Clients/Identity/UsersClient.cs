using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;

namespace WebStore.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration Configuration) : base(Configuration, WebAPI.Identity.User)
        {
        }

        #region IUserStore<User>
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/User", user, Cancel);

            var creation_success = await response
                .Content
                .ReadAsAsync<bool>(Cancel)
                .ConfigureAwait(false);

            return creation_success
                ? IdentityResult.Success
                : IdentityResult.Failed();

        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/User/Delete", user, Cancel))
                .Content
                .ReadAsAsync<bool>(Cancel).ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken Cancel)
        {
            return await GetAsync<User>($"{Address}/User/Find/{id}", Cancel);
        }
        public async Task<User> FindByNameAsync(string name, CancellationToken Cancel)
        {
            return await GetAsync<User>($"{Address}/User/Normal/{name}", Cancel);

        }
        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/NormalUserName", user, Cancel))
                .Content
                .ReadAsAsync<string>(Cancel)
                .ConfigureAwait(false);
        }
        public async Task<string> GetUserIdAsync(User user, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/UserId", user, Cancel))
                .Content
                .ReadAsAsync<string>(Cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/UserName", user, Cancel))
                .Content
                .ReadAsAsync<string>(Cancel)
                .ConfigureAwait(false);
        }
        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/NormalUserName/{name}", user, Cancel);
            user.NormalizedUserName = await response.Content.ReadAsAsync<string>(Cancel);
        }
        public async Task SetUserNameAsync(User user, string name, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/UserName/{name}", user, Cancel);
            user.UserName = await response.Content.ReadAsAsync<string>(Cancel);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken Cancel)
        {
            return await (await PutAsync($"{Address}/User", user, Cancel))
                .Content
                .ReadAsAsync<bool>(Cancel).ConfigureAwait(false)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        #endregion

        #region IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/Role/{role}", user, Cancel);
        }
        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/Role/Delete/{role}", user, Cancel);
        }
        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/roles", user, Cancel))
                .Content
                .ReadAsAsync<IList<string>>(Cancel);
        }
        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken Cancel)
        {
            return await (await PostAsync($"{Address}/InRole/{role}", user, Cancel))
                .Content
                .ReadAsAsync<bool>(Cancel);
        }
        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken Cancel)
        {
            return await GetAsync<IList<User>>($"{Address}/UsersInRole/{role}", Cancel);
        }


        #endregion

        #region IUserPasswordStore<user>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetPasswordHash", new PasswordHashDTO
            {
                User = user,
                Hash = hash,
            }
            , Cancel)
            .ConfigureAwait(false);

            user.PasswordHash = await response.Content.ReadAsAsync<string>(Cancel).ConfigureAwait(false);
        }
        public async Task<string> GetPasswordHashAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetPasswordHash", user, Cancel);

            return await response.Content.ReadAsAsync<string>(Cancel).ConfigureAwait(false);
        }
        public async Task<bool> HasPasswordAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/HasPasswor", user, Cancel);

            return await response.Content.ReadAsAsync<bool>(Cancel).ConfigureAwait(false);
        }


        #endregion

        #region IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetEmail/{email}", user, Cancel);
            user.Email = await response.Content.ReadAsAsync<string>(Cancel);
        }
        public async Task<string> GetEmailAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetEmail", user, Cancel);
            return await response.Content.ReadAsAsync<string>(Cancel);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetEmailConfirmed", user, Cancel);
            return await response.Content.ReadAsAsync<bool>(Cancel);
        }
        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, Cancel);
            user.EmailConfirmed = await response.Content.ReadAsAsync<bool>(Cancel);
        }
        public async Task<User> FindByEmailAsync(string email, CancellationToken Cancel)
        {
            return await GetAsync<User>($"{Address}/User/FindByEmail/{email}", Cancel);
        }
        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/User/GetNormalizedEmail", user, Cancel);
            return await response.Content.ReadAsAsync<string>(Cancel);
        }
        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/User/SetNormalizedEmail/{email}", user, Cancel);
            user.NormalizedEmail = await response.Content.ReadAsAsync<string>(Cancel);
        }

        #endregion

        #region IUserPhoneStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumber/{phone}", user, Cancel);
            user.PhoneNumber = await response.Content.ReadAsAsync<string>(Cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, Cancel);
            user.PhoneNumberConfirmed = await response.Content.ReadAsAsync<bool>(Cancel);
        }
        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumber", user, Cancel);
            return await response.Content.ReadAsAsync<string>(Cancel);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, Cancel);
            return await response.Content.ReadAsAsync<bool>(Cancel);
        }

        #endregion

        #region IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, Cancel);
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/RemoveLogin/{loginProvider}/{providerKey}", user, Cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetLogins", user, Cancel);
            return await response.Content.ReadAsAsync<IList<UserLoginInfo>>(Cancel);
        }
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken Cancel)
        {
            return await GetAsync<User>($"{Address}/User/FindByLogin/{loginProvider}/{providerKey}", Cancel);
        }

        #endregion

        #region IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetLockoutEndDate", user, Cancel);
            return await response.Content.ReadAsAsync<DateTimeOffset?>(Cancel);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetLockoutEndDate", new SetLockoutDTO { User = user, LockoutEnd = lockoutEnd}, Cancel);
            user.LockoutEnd = await response.Content.ReadAsAsync<DateTimeOffset?>(Cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/IncrementAccessFailedCount", user, Cancel);
            return await response.Content.ReadAsAsync<int>(Cancel);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/ResetAccessFailedCount", user, Cancel);
        }
        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetAccessFailedCount", user, Cancel);
            return await response.Content.ReadAsAsync<int>(Cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetLockoutEnabled", user, Cancel);
            return await response.Content.ReadAsAsync<bool>(Cancel);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetLockoutEnabled/{enabled}", user, Cancel);
            user.LockoutEnabled = await response.Content.ReadAsAsync<bool>(Cancel);
        }

        #endregion

        #region IUseTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/SetTwoFactor/{enabled}", user, Cancel);
            user.TwoFactorEnabled = await response.Content.ReadAsAsync<bool>(Cancel);
        }
        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetTwoFactorEnabled", user, Cancel);
            return await response.Content.ReadAsAsync<bool>(Cancel);
        }

        #endregion

        #region IUserClaimsStore
        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken Cancel)
        {
             await PostAsync($"{Address}/AddClaims", new AddClaimDTO { User = user, Claims = claims}, Cancel);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetClaims", user, Cancel);
            return await response.Content.ReadAsAsync<List<Claim>>(Cancel);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken Cancel)
        {
            var response = await PostAsync($"{Address}/GetUsersForClaimAsync", claim, Cancel);
            return await response.Content.ReadAsAsync<List<User>>(Cancel);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/RemoveClaims", new RemoveClaimDTO { User = user, Claims = claims }, Cancel);
        }

        public async Task ReplaceClaimAsync(User user, Claim Oldclaim, Claim newClaim, CancellationToken Cancel)
        {
            await PostAsync($"{Address}/ReplaceClaim", new ReplaceClaimDTO { User = user, Claim = Oldclaim, NewClaim = newClaim }, Cancel);
        }
        #endregion
    }
}
