using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Library.Services.Abstractions
{
    public interface IAzureDevOpsService
    {
        Task<IList<WorkItem>> GetWorkItemsByWiqlAsync(
            string wiql,
            string teamProjectName,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<IList<WorkItem>> GetWorkItemsByIdAsync(
            IEnumerable<int> ids,
            string teamProjectName,
            IEnumerable<string> fields = null,
            WorkItemExpand? expand = null,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<IList<WorkItem>> GetWorkItemRevisionsAsync(
            int workItemId,
            int? top = null,
            int? skip = null,
            WorkItemExpand expand = WorkItemExpand.Fields,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<ReportingWorkItemRevisionsBatch> GetReportingRevisionsAsync(
            string teamProjectName,
            IEnumerable<string> fields = null,
            IEnumerable<string> types = null,
            string continuationToken = null,
            DateTime? startDateTime = null,
            bool? includeIdentityRef = null,
            bool? includeLatestOnly = null,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<ReportingWorkItemLinksBatch> GetReportingLinksAsync(
            string teamProjectName,
            IEnumerable<string> linkTypes = null,
            IEnumerable<string> types = null,
            string continuationToken = null,
            DateTime? startDateTime = null,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<IList<TeamMemberCapacityIdentityRef>> GetTeamCapacitiesAsync(
            string teamProjectName,
            string team,
            Guid iterationId,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<IList<TeamSettingsIteration>> GetTeamIterationsAsync(
            string teamProjectName,
            string team,
            string timeframe = null,
            string optionsName = null,
            CancellationToken cancellationToken = default);

        Task<Stream> GitGetItemContentsAsync(
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
            CancellationToken cancellationToken = default);
    }
}
