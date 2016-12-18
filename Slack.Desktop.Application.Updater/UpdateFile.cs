using System.Diagnostics;

namespace Slack.Desktop.Application.Updater
{
    [DebuggerDisplay("Name={Name}")]
    public class UpdateFile
    {
        public string Name { get; set; }
    }
}