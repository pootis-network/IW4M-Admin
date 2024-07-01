﻿using System.Collections.Generic;

namespace IW4MAdmin.Plugins.Stats.Client.Game
{
    public class WeaponInfo
    {
        public string RawName { get; set; }
        public string Name { get; set; }
        public IList<AttachmentInfo> Attachments { get; set; } = new List<AttachmentInfo>();
    }
}
