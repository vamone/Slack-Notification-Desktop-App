using System.Collections.Generic;

namespace Slack.Notification.Service
{
    public class Components
    {
        public Components()
        {
            this.Bots = this.Bots ?? new List<Bot>();
            this.Channels = this.Channels ?? new List<Channel>();
            this.Emojis = this.Emojis ?? new List<Emoji>();
            this.Ims = this.Ims ?? new List<Im>();
        }

        public ICollection<Bot> Bots { get; set; }

        public ICollection<Channel> Channels { get; set; }

        public ICollection<Emoji> Emojis { get; set; }

        public ICollection<Im> Ims { get; set; }

        public ICollection<User> Users { get; set; }
    }
}