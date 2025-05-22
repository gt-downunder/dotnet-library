namespace DotNet.Library.Utilities
{
    public static partial class Constants
    {
        public static class AzureDevOps
        {
            public static class WorkItemStates
            {
                public const string Pending = "Pending";
                public const string Active = "Active";
                public const string Closed = "Closed";
                public const string Resolved = "Resolved";
            }

            public static class WorkItemTypes
            {
                public const string Epic = "Epic";
                public const string Feature = "Feature";
                public const string UserStory = "UserStory";
                public const string Task = "Task";
                public const string Bug = "Bug";
                public const string Release = "Release";
            }
        }
    }
}
