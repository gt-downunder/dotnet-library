using DotNet.Library.Exceptions;
using DotNet.Library.Extensions;
using DotNet.Library.Services.Abstractions;
using DotNet.Library.Services.Azure.Options;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Library.Services.Azure
{
    public class DevOpsService : IAzureDevOpsService
    {
        private readonly List<DevOpsServiceOptions> _options;
        private readonly IAzureKeyVaultService _azureKeyVaultService;

        public DevOpsService(IOptions<List<DevOpsServiceOptions>> options, IAzureKeyVaultService vault)
        {
            _options = options.Value;
            _azureKeyVaultService = vault;

            if (_options.Any(x => x.PersonalAccessToken.IsNullOrEmpty()))
            {
                throw new TechnicalException("PAT is a required parameter");
            }

            if (_options.Any(x => x.Organization.IsNullOrEmpty()))
            {
                throw new TechnicalException("Organization is a required parameter");
            }
        }

        /// <summary>
        /// Executes a WIQL query
        /// </summary>
        /// <param name="wiql">The query containing the WIQL.</param>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<IList<WorkItem>> GetWorkItemsByWiqlAsync(
            string wiql,
            string teamProjectName,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            if (teamProjectName.IsNullOrEmpty())
            {
                throw new ArgumentException($"{nameof(teamProjectName)} argument cannot be null or empty");
            }

            using WorkItemTrackingHttpClient client = await GetWorkItemTrackingClientAsync(optionsName);

            WorkItemQueryResult results = await client.QueryByWiqlAsync(
                new Wiql { Query = wiql },
                project: teamProjectName,
                cancellationToken: cancellationToken);

            IList<string> queriedFields = [];
            IEnumerable<int> ids = results.WorkItems.Select(x => x.Id);

            foreach (WorkItemFieldReference column in results.Columns)
            {
                queriedFields.Add(column.ReferenceName);
            }

            return await GetWorkItemsByIdAsync(
                ids: ids,
                teamProjectName: teamProjectName,
                fields: queriedFields,
                optionsName: optionsName,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Returns a set of work items
        /// </summary>
        /// <param name="ids">List of IDs to query</param>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="fields">A list of fields to include in the result set</param>
        /// <param name="expand">The expand parameters for work item attributes. Possible options are { None, Relations, Fields, Links, All }.</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<IList<WorkItem>> GetWorkItemsByIdAsync(
            IEnumerable<int> ids,
            string teamProjectName,
            IEnumerable<string> fields = null,
            WorkItemExpand? expand = null,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            if (teamProjectName.IsNullOrEmpty())
            {
                throw new ArgumentException($"{nameof(teamProjectName)} argument cannot be null or empty");
            }

            if (!ids.Any())
            {
                return [];
            }

            using WorkItemTrackingHttpClient client = await GetWorkItemTrackingClientAsync(optionsName);

            IList<WorkItem> results = [];
            int batchNumber = 0;
            const int batchSize = 200;

            // The expand option requires the field parameter to be null
            if (expand.IsNotNull())
            {
                fields = null;
            }

            ids = ids.Distinct();
            do
            {
                // GetWorkItemsAsync() has a limit of 200 work items so we need to batch these calls
                results.AddRange(await client.GetWorkItemsAsync(
                    project: teamProjectName,
                    ids: ids.OrderByDescending(x => x).Skip(batchSize * batchNumber).Take(batchSize),
                    fields: fields,
                    expand: expand,
                    cancellationToken: cancellationToken));

                batchNumber++;
            }
            while (results.Count < ids.Count());

            return results;
        }

        /// <summary>
        /// Fetches Work Item revisions
        /// </summary>
        /// <param name="workItemId">The ID of the work item whose revisions are to be fetched</param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="expand"></param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<IList<WorkItem>> GetWorkItemRevisionsAsync(
            int workItemId,
            int? top = null,
            int? skip = null,
            WorkItemExpand expand = WorkItemExpand.Fields,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using WorkItemTrackingHttpClient client = await GetWorkItemTrackingClientAsync(optionsName);
            return await client.GetRevisionsAsync(workItemId, expand: expand, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get a batch of work item revisions.
        /// </summary>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="fields">A list of fields to return in work item revisions. Omit this parameter to get all reportable fields.</param>
        /// <param name="types">A list of types to filter the results to specific work item types. Omit this parameter to get work item revisions of all work item types.</param>
        /// <param name="continuationToken">Specifies the watermark to start the batch from. Omit this parameter to get the first batch of revisions.</param>
        /// <param name="startDateTime">Date/time to use as a starting point for revisions, all revisions will occur after this date/time. Cannot be used in conjunction with 'continuationToken' parameter.</param>
        /// <param name="includeIdentityRef">Return an identity reference instead of a string value for identity fields.</param>
        /// <param name="includeLatestOnly">Specify if the deleted item should be returned.</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns></returns>
        public async Task<ReportingWorkItemRevisionsBatch> GetReportingRevisionsAsync(
            string teamProjectName,
            IEnumerable<string> fields = null,
            IEnumerable<string> types = null,
            string continuationToken = null,
            DateTime? startDateTime = null,
            bool? includeIdentityRef = null,
            bool? includeLatestOnly = null,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using WorkItemTrackingHttpClient client = await GetWorkItemTrackingClientAsync(optionsName);
            return await client.ReadReportingRevisionsGetAsync(
                project: teamProjectName,
                fields: fields,
                types: types,
                continuationToken: continuationToken,
                startDateTime: startDateTime,
                includeIdentityRef: includeIdentityRef,
                includeLatestOnly: includeLatestOnly,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get a batch of work item links.
        /// </summary>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="linkTypes">A list of types to filter the results to specific link types. Omit this parameter to get work item links of all link types.</param>
        /// <param name="types">A list of types to filter the results to specific work item types. Omit this parameter to get work item links of all work item types.</param>
        /// <param name="continuationToken">Specifies the continuationToken to start the batch from. Omit this parameter to get the first batch of links.</param>
        /// <param name="startDateTime">Date/time to use as a starting point for link changes. Only link changes that occurred after that date/time will be returned. Cannot be used in conjunction with 'continuationToken' parameter.</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<ReportingWorkItemLinksBatch> GetReportingLinksAsync(
            string teamProjectName,
            IEnumerable<string> linkTypes = null,
            IEnumerable<string> types = null,
            string continuationToken = null,
            DateTime? startDateTime = null,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using WorkItemTrackingHttpClient client = await GetWorkItemTrackingClientAsync(optionsName);
            return await client.GetReportingLinksByLinkTypeAsync(
                project: teamProjectName,
                linkTypes: linkTypes,
                types: types,
                continuationToken: continuationToken,
                startDateTime: startDateTime,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Fetches Work Item revisions
        /// </summary>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="team">Team name</param>
        /// <param name="iterationId">ID of the iteration</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<IList<TeamMemberCapacityIdentityRef>> GetTeamCapacitiesAsync(
            string teamProjectName,
            string team,
            Guid iterationId,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using WorkHttpClient client = await GetWorkHttpClientAsync(optionsName);
            return await client.GetCapacitiesWithIdentityRefAsync(
                teamContext: new TeamContext(teamProjectName, team),
                iterationId: iterationId,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get a team's iterations using timeframe filter
        /// </summary>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="team">Team name</param>
        /// <param name="timeframe">A filter for which iterations are returned based on relative time. Only Current is supported currently.</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<IList<TeamSettingsIteration>> GetTeamIterationsAsync(
            string teamProjectName,
            string team,
            string timeframe = null,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using WorkHttpClient client = await GetWorkHttpClientAsync(optionsName);
            return await client.GetTeamIterationsAsync(
                teamContext: new TeamContext(teamProjectName, team),
                timeframe: timeframe,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get Item Metadata and/or Content for a single item.
        /// </summary>
        /// <param name="teamProjectName">The Azure DevOps Team Project to query</param>
        /// <param name="repositoryId">The Id of the repository.</param>
        /// <param name="path">The item path.</param>
        /// <param name="scopePath">The path scope. The default is null.</param>
        /// <param name="recursionLevel">The recursion level of this request. The default is 'none', no recursion.</param>
        /// <param name="includeContentMetadata">Set to true to include content metadata. Default is false.</param>
        /// <param name="latestProcessedChange">Set to true to include the lastest changes. Default is false.</param>
        /// <param name="download">Set to true to download the response as a file. Default is false.</param>
        /// <param name="versionDescriptor">Version descriptor. Default is null.</param>
        /// <param name="includeContent">Set to true to include item content when requesting json. Default is false.</param>
        /// <param name="optionsName">If multiple instances of DevOpsServiceOptions (project connection details) are injected, the unique name of which to use</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        public async Task<Stream> GitGetItemContentsAsync(
            string teamProjectName,
            string repositoryId,
            string path,
            string scopePath = null,
            VersionControlRecursionType? recursionLevel = null,
            bool? includeContentMetadata = null,
            bool? latestProcessedChange = null,
            bool? download = null,
            GitVersionDescriptor versionDescriptor = null,
            bool? includeContent = null,
            string optionsName = null,
            CancellationToken cancellationToken = default)
        {
            using GitHttpClient client = await GetGitHttpClientAsync(optionsName);
            return await client.GetItemContentAsync(
                project: teamProjectName,
                repositoryId: repositoryId,
                path: path,
                scopePath: scopePath,
                recursionLevel: recursionLevel,
                includeContentMetadata: includeContentMetadata,
                latestProcessedChange: latestProcessedChange,
                download: download,
                versionDescriptor: versionDescriptor,
                includeContent: includeContent,
                cancellationToken: cancellationToken);
        }

        private async Task<WorkItemTrackingHttpClient> GetWorkItemTrackingClientAsync(string optionsName)
        {
            DevOpsServiceOptions option = await GetOptionAsync(optionsName);

            return new WorkItemTrackingHttpClient(
                baseUrl: new Uri(GetBaseUrl(option)),
                credentials: new VssBasicCredential(string.Empty, option.PersonalAccessToken));
        }

        private async Task<WorkHttpClient> GetWorkHttpClientAsync(string optionsName)
        {
            DevOpsServiceOptions option = await GetOptionAsync(optionsName);

            return new WorkHttpClient(
                baseUrl: new Uri(GetBaseUrl(option)),
                credentials: new VssBasicCredential(string.Empty, option.PersonalAccessToken));
        }

        private async Task<GitHttpClient> GetGitHttpClientAsync(string optionsName)
        {
            DevOpsServiceOptions option = await GetOptionAsync(optionsName);

            return new GitHttpClient(
                baseUrl: new Uri(GetBaseUrl(option)),
                credentials: new VssBasicCredential(string.Empty, option.PersonalAccessToken));
        }

        private static string GetBaseUrl(DevOpsServiceOptions option) => $"https://dev.azure.com/{option.Organization}";

        private async Task<DevOpsServiceOptions> GetOptionAsync(string optionsName)
        {
            // Default to first element unless specified
            DevOpsServiceOptions option = _options.FirstOrDefault(x => x.Name == optionsName) ?? _options[0];

            option.PersonalAccessToken = _azureKeyVaultService.IsValidSecretUrl(option.PersonalAccessToken)
                ? await _azureKeyVaultService.GetSecretAsync(option.PersonalAccessToken)
                : option.PersonalAccessToken;

            return option;
        }
    }
}
