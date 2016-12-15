using System.Collections.Generic;

namespace Slack.Api
{
    public class Components
    {
        public Components()
        {
            this.Profile = new User();
            this.Bots = this.Bots ?? new List<Bot>();
            this.Channels = this.Channels ?? new List<Channel>();
            this.Emojis = this.Emojis ?? new List<EmojiParent>();
            this.Ims = this.Ims ?? new List<Im>();
            this.Users = this.Users ?? new List<User>();
        }

        public User Profile { get; set; }

        public ICollection<Bot> Bots { get; set; }

        public ICollection<Channel> Channels { get; set; }

        public ICollection<EmojiParent> Emojis { get; set; }

        public ICollection<Im> Ims { get; set; }

        public ICollection<User> Users { get; set; }
    }
}