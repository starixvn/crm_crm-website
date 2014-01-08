// --------------------------------------------------------------------------
// <copyright file="CasinoElement.cs" company="Nexcel Solutions Vietnam">
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
    /// Casino Element.
    /// </summary>
    public class CasinoElement : ConfigurationElement
    {
        public CasinoElement()
        {
        }

        [ConfigurationProperty("alias", IsRequired = true, IsKey = true)]
        public string Alias
        {
            get
            {
                return (string)this["alias"];
            }

            set
            {
                this["alias"] = value;
            }
        }

        [ConfigurationProperty("prefix", IsRequired = true)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Prefix
        {
            get
            {
                return (string)this["prefix"];
            }

            set
            {
                this["prefix"] = value;
            }
        }

        [ConfigurationProperty("subfix", IsRequired = true)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Subfix
        {
            get
            {
                return (string)this["subfix"];
            }

            set
            {
                this["subfix"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
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

        [ConfigurationProperty("id", IsRequired = true)]
        ////[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Id
        {
            get
            {
                return (int)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CasinoCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CasinoCollection Stores
        {
            get
            {
                var stores = (CasinoCollection)base["stores"];
                return stores;
            }
        }
    }
}
