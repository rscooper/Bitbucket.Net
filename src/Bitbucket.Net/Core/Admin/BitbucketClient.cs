﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bitbucket.Net.Common;
using Bitbucket.Net.Common.Models;
using Bitbucket.Net.Core.Models.Admin;
using Bitbucket.Net.Core.Models.Users;
using Flurl.Http;
using PasswordChange = Bitbucket.Net.Core.Models.Admin.PasswordChange;

namespace Bitbucket.Net.Core
{
    public partial class BitbucketClient
    {
        private IFlurlRequest GetAdminUrl() => GetBaseUrl()
            .AppendPathSegment("/admin");

        private IFlurlRequest GetAdminUrl(string path) => GetAdminUrl()
            .AppendPathSegment(path);

        public async Task<IEnumerable<DeletableGroupOrUser>> GetAdminGroupsAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/groups")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<DeletableGroupOrUser>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<DeletableGroupOrUser> CreateAdminGroupAsync(string name)
        {
            var response = await GetAdminUrl("/groups")
                .SetQueryParam("name", name)
                .PostAsync(new StringContent(""))
                .ConfigureAwait(false);

            return await HandleResponseAsync<DeletableGroupOrUser>(response).ConfigureAwait(false);
        }

        public async Task<DeletableGroupOrUser> DeleteAdminGroupAsync(string name)
        {
            var response = await GetAdminUrl("/groups")
                .SetQueryParam("name", name)
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync<DeletableGroupOrUser>(response).ConfigureAwait(false);
        }

        public async Task<bool> AddAdminGroupUsersAsync(GroupUsers groupUsers)
        {
            var response = await GetAdminUrl("/groups/add-users")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PostJsonAsync(groupUsers)
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserInfo>> GetAdminGroupMoreMembersAsync(string context, string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["context"] = context,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/groups/more-members")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<UserInfo>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserInfo>> GetAdminGroupMoreNonMembersAsync(string context, string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["context"] = context,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/groups/more-none-members")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<UserInfo>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserInfo>> GetAdminUsersAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/users")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<UserInfo>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<bool> CreateAdminUserAsync(string name, string password, string displayName, string emailAddress,
            bool addToDefaultGroup = true, string notify = "false")
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["name"] = name,
                ["password"] = password,
                ["displayName"] = displayName,
                ["emailAddress"] = emailAddress,
                ["addToDefaultGroup"] = BitbucketHelpers.BoolToString(addToDefaultGroup),
                ["notify"] = notify
            };

            var response = await GetAdminUrl("/users")
                .SetQueryParams(queryParamValues)
                .PostAsync(new StringContent(""))
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<UserInfo> UpdateAdminUserAsync(string name = null, string displayName = null, string email = null)
        {
            var data = new
            {
                //TODO: UpdateAdminUserAsync
            };

            var response = await GetAdminUrl("/users")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PutJsonAsync(data)
                .ConfigureAwait(false);

            return await HandleResponseAsync<UserInfo>(response).ConfigureAwait(false);
        }

        public async Task<UserInfo> DeleteAdminUserAsync(string name)
        {
            var response = await GetAdminUrl("/users")
                .SetQueryParam("name", name)
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync<UserInfo>(response).ConfigureAwait(false);
        }

        public async Task<bool> AddAdminUserGroupsAsync(UserGroups userGroups)
        {
            var response = await GetAdminUrl("/users/add-groups")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PostJsonAsync(userGroups)
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAdminUserCaptcha(string name)
        {
            var response = await GetAdminUrl("/users/captcha")
                .SetQueryParam("name", name)
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAdminUserCredentialsAsync(PasswordChange passwordChange)
        {
            var response = await GetAdminUrl("/users/credentials")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PutJsonAsync(passwordChange)
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<IEnumerable<DeletableGroupOrUser>> GetAdminUserMoreMembersAsync(string context, string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["context"] = context,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/users/more-members")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<DeletableGroupOrUser>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<DeletableGroupOrUser>> GetAdminUserMoreNonMembersAsync(string context, string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["context"] = context,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/users/more-none-members")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<DeletableGroupOrUser>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<bool> RemoveAdminUserFromGroupAsync(string userName, string groupName)
        {
            var data = new
            {
                context = userName,
                itemName = groupName
            };

            var response = await GetAdminUrl("/users/remove-group")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PostJsonAsync(data)
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<UserInfo> RenameAdminUserAsync(UserRename userRename)
        {
            var response = await GetAdminUrl("users/rename")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PostJsonAsync(userRename)
                .ConfigureAwait(false);

            return await HandleResponseAsync<UserInfo>(response).ConfigureAwait(false);
        }

        public async Task<Cluster> GetClusterAsync()
        {
            return await GetAdminUrl("/cluster")
                .GetJsonAsync<Cluster>()
                .ConfigureAwait(false);
        }

        public async Task<LicenseDetails> GetLicenseAsync()
        {
            return await GetAdminUrl("/license")
                .GetJsonAsync<LicenseDetails>()
                .ConfigureAwait(false);
        }

        public async Task<LicenseDetails> UpdateLicenseAsync(LicenseInfo licenseInfo)
        {
            var response = await GetAdminUrl("/license")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PostJsonAsync(licenseInfo)
                .ConfigureAwait(false);

            return await HandleResponseAsync<LicenseDetails>(response).ConfigureAwait(false);
        }

        public async Task<MailServerConfiguration> GetMailServerAsync()
        {
            return await GetAdminUrl("/mail-server")
                .GetJsonAsync<MailServerConfiguration>()
                .ConfigureAwait(false);
        }

        public async Task<MailServerConfiguration> UpdateMailServerAsync(MailServerConfiguration mailServerConfiguration)
        {
            var response = await GetAdminUrl("/mail-server")
                .ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
                .PutJsonAsync(mailServerConfiguration)
                .ConfigureAwait(false);

            return await HandleResponseAsync<MailServerConfiguration>(response).ConfigureAwait(false);
        }

        public async Task<bool> DeleteMailServerAsync()
        {
            var response = await GetAdminUrl("/mail-server")
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<string> GetMailServerSenderAddressAsync()
        {
            var response = await GetAdminUrl("/mail-server/sender-address")
                .GetAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response, s => s).ConfigureAwait(false);
        }

        public async Task<string> UpdateMailServerSenderAddressAsync(string senderAddress)
        {
            var response = await GetAdminUrl("/mail-server/sender-address")
                .PutJsonAsync(senderAddress)
                .ConfigureAwait(false);

            return await HandleResponseAsync(response, s => s).ConfigureAwait(false);
        }

        public async Task<bool> DeleteMailServerSenderAddressAsync()
        {
            var response = await GetAdminUrl("/mail-server/sender-address")
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GroupPermission>> GetAdminGroupPermissionsAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/permissions/groups")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<GroupPermission>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<bool> UpdateAdminGroupPermissionsAsync(Permissions permission, string name)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["permission"] = permission,
                ["name"] = name
            };

            var response = await GetAdminUrl("/permissions/groups")
                .SetQueryParams(queryParamValues)
                .PutJsonAsync(new StringContent(""))
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAdminGroupPermissionsAsync(string name)
        {
            var response = await GetAdminUrl("/permissions/groups")
                .SetQueryParam("name", name)
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<IEnumerable<DeletableGroupOrUser>> GetAdminGroupPermissionsNoneAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/permissions/groups/none")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<DeletableGroupOrUser>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserPermission>> GetAdminUserPermissionsAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/permissions/users")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<UserPermission>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<bool> UpdateAdminUserPermissionsAsync(Permissions permission, string name)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["permission"] = permission,
                ["name"] = name
            };

            var response = await GetAdminUrl("/permissions/users")
                .SetQueryParams(queryParamValues)
                .PutJsonAsync(new StringContent(""))
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAdminUserPermissionsAsync(string name)
        {
            var response = await GetAdminUrl("/permissions/users")
                .SetQueryParam("name", name)
                .DeleteAsync()
                .ConfigureAwait(false);

            return await HandleResponseAsync(response).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAdminUserPermissionsNoneAsync(string filter = null,
            int? maxPages = null,
            int? limit = null,
            int? start = null)
        {
            var queryParamValues = new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["start"] = start,
                ["filter"] = filter
            };

            return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
                    await GetAdminUrl("/permissions/users/none")
                        .SetQueryParams(qpv)
                        .GetJsonAsync<BitbucketResult<User>>()
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }
}
