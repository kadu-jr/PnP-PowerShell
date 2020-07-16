﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public partial class TeamTab
    {
        private string _displayName;
        #region Public Members

        /// <summary>
        /// Defines the Display Name of the Channel
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = HttpUtility.UrlDecode(value);
            }
        }

        [JsonProperty("teamsApp@odata.bind")]
        public string TeamsApp { get; set; }
        /// <summary>
        /// App definition identifier of the tab
        /// </summary>
        public string TeamsAppId { get; set; }

        /// <summary>
        /// Allows to remove an already existing Tab
        /// </summary>
        public bool Remove { get; set; }

        /// <summary>
        /// Defines the Configuration for the Tab
        /// </summary>
        public TeamTabConfiguration Configuration { get; set; }

        /// <summary>
        /// Declares the ID for the Tab
        /// </summary>
        public string Id { get; set; }

        #endregion
    }
}
