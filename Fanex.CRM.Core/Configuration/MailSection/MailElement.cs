// --------------------------------------------------------------------------
// <copyright file="MailElement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Mail Element.
    /// </summary>
    public class MailElement : ConfigurationElement
    {
        public MailElement()
        {
        }

        [ConfigurationProperty("host", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }

            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 25)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }

            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }

            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }

            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("auth", IsRequired = false, DefaultValue = true)]
        public bool Auth
        {
            get
            {
                return (bool)this["auth"];
            }

            set
            {
                this["auth"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("from", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string From
        {
            get
            {
                return (string)this["from"];
            }

            set
            {
                this["from"] = value;
            }
        }

        [ConfigurationProperty("to", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string To
        {
            get
            {
                return (string)this["to"];
            }

            set
            {
                this["to"] = value;
            }
        }

        [ConfigurationProperty("bcc", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Bcc
        {
            get
            {
                return (string)this["bcc"];
            }

            set
            {
                this["bcc"] = value;
            }
        }

        [ConfigurationProperty("cc", IsRequired = false)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Cc
        {
            get
            {
                return (string)this["cc"];
            }

            set
            {
                this["cc"] = value;
            }
        }

        [ConfigurationProperty("title", DefaultValue = "", IsRequired = false)]
        public string Title
        {
            get
            {
                return (string)this["title"];
            }
        }

        [ConfigurationProperty("subject", DefaultValue = "", IsRequired = false)]
        public string Subject
        {
            get
            {
                return (string)this["subject"];
            }
        }

        [ConfigurationProperty("mails", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MailCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MailCollection Mails
        {
            get
            {
                var stores = (MailCollection)base["mails"];
                return stores;
            }
        }
    }
}
