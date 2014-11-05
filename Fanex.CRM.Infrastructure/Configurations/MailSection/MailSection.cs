// --------------------------------------------------------------------------
// <copyright file="MailSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Mail Section.
    /// </summary>
    public class MailSection : ConfigurationSection
    {
        public enum Setting
        {
            /// <summary>
            /// The server type.
            /// </summary>
            Server,

            /// <summary>
            /// The send type.
            /// </summary>
            Send
        }

        public enum HostName
        {
            /// <summary>
            /// The gmail type.
            /// </summary>
            Gmail,

            /// <summary>
            /// The IBC type.
            /// </summary>
            IBC
        }

        public static MailSection Instance
        {
            get
            {
                var configSection = ConfigurationManager.GetSection("MailSection") as MailSection;
                return configSection;
            }
        }

        [ConfigurationProperty("Active", IsDefaultCollection = false, DefaultValue = "Gmail")]
        public string Active
        {
            get
            {
                var active = (string)base["Active"];
                return active;
            }
        }

        [ConfigurationProperty("Server", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Server
        {
            get
            {
                var configCollection = (MailCollection)base["Server"];
                return configCollection;
            }
        }

        [ConfigurationProperty("Send", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Send
        {
            get
            {
                var configCollection = (MailCollection)base["Send"];
                return configCollection;
            }
        }

        public MailCollection this[Setting name]
        {
            get
            {
                var configCollection = (MailCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return configCollection;
            }
        }
    }
}
