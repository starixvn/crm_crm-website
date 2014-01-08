// --------------------------------------------------------------------------
// <copyright file="CasinoSection.cs" company="Nexcel Solutions Vietnam">
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
    /// Casino Section.
    /// </summary>
    public class CasinoSection : ConfigurationSection
    {
        public enum SystemConfigCollection
        {
            /// <summary>
            /// The systems type.
            /// </summary>
            Systems,

            /// <summary>
            /// The communications type.
            /// </summary>
            Communications
        }

        public static CasinoSection Instance
        {
            get
            {
                var configSection = ConfigurationManager.GetSection("CasinoSection") as CasinoSection;
                return configSection;
            }
        }

        [ConfigurationProperty("systems", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CasinoCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CasinoCollection Systems
        {
            get
            {
                var configCollection = (CasinoCollection)base["systems"];
                return configCollection;
            }
        }

        [ConfigurationProperty("communications", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CasinoCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CasinoCollection Communications
        {
            get
            {
                var configCollection = (CasinoCollection)base["communications"];
                return configCollection;
            }
        }

        public CasinoCollection this[SystemConfigCollection name]
        {
            get
            {
                var configCollection = (CasinoCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return configCollection;
            }
        }
    }
}
