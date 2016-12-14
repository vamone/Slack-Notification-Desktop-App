using System.Collections.Generic;

namespace Slack.Api
{
    public class Components
    {
        public Components(ICollection<Bot> bots, ICollection<Channel> channels, ICollection<EmojiParent> emojis,
            ICollection<Im> ims, ICollection<User> users)
        {
            this.Bots = bots ?? new List<Bot>();
            this.Channels = channels ?? new List<Channel>();
            this.Emojis = emojis ?? new List<EmojiParent>();
            this.Ims = ims ?? new List<Im>();
            this.Users = users ?? new List<User>();
        }

        public User Profile { get; set; }

        public ICollection<Bot> Bots { get; set; }

        public ICollection<Channel> Channels { get; set; }

        public ICollection<EmojiParent> Emojis { get; set; }

        public ICollection<Im> Ims { get; set; }

        public ICollection<User> Users { get; set; }
    }
}