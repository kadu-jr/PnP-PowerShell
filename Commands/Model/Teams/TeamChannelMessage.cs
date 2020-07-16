﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamChannelMessage
    {
        public string Id { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? DeletedDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }
        public string Importance { get; set; } = "normal";
        public TeamChannelMessageBody Body { get; set; } = new TeamChannelMessageBody();

        public TeamChannelMessageFrom From { get; set; } = new TeamChannelMessageFrom();
    }

    public class TeamChannelMessageFrom
    {
        public User User { get; set; } = new User();
    }

    public class TeamChannelMessageBody
    {

        [JsonProperty("contentType")]
        private string contentType { get; set; }

        [JsonIgnore]
        public TeamChannelMessageContentType ContentType
        {
            get
            {
                if (Enum.TryParse<TeamChannelMessageContentType>(contentType, out TeamChannelMessageContentType ct))
                {
                    return ct;
                }
                else
                {
                    return TeamChannelMessageContentType.Text;
                }
            }
            set
            {
                contentType = value.ToString().ToLower();
            }
        }

        public string Content { get; set; }

    }

    public enum TeamChannelMessageContentType
    {
        Text,
        Html
    }
}
