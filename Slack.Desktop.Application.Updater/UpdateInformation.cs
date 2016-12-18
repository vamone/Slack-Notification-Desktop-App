using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slack.Desktop.Application.Updater
{
    [DebuggerDisplay("Version={Version}, Description={Description}, DownloadUrl={DownloadUrl}, ReleaseAt={ReleaseAt}, FilesCount={Files.Count}")]
    public class UpdateInformation
    {
        public UpdateInformation()
        {
            this.Files = this.Files ?? new List<UpdateFile>();
        }

        public string Description { get; set; }

        public string DownloadUrl { get; set; }

        public ICollection<UpdateFile> Files { get; set; }

        public DateTime ReleaseAt { get; set; }

        public string Version { get; set; }
    }
}