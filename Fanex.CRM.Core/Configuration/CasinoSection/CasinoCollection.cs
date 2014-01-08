// --------------------------------------------------------------------------
// <copyright file="CasinoCollection.cs" company="Nexcel Solutions Vietnam">
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
    /// Casino Collection.
    /// </summary>
    public class CasinoCollection : ConfigurationElementCollection
    {
        public CasinoCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public CasinoElement this[int index]
        {
            get
            {
                return (CasinoElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new CasinoElement this[string alias]
        {
            get
            {
                return (CasinoElement)BaseGet(alias);
            }
        }

        public void Add(CasinoElement casinoElement)
        {
            BaseAdd(casinoElement);
        }

        public void Remove(CasinoElement casinoElement)
        {
            if (BaseIndexOf(casinoElement) >= 0)
            {
                BaseRemove(casinoElement.Alias);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(CasinoElement casinoElement)
        {
            return BaseIndexOf(casinoElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new CasinoElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CasinoElement)element).Alias;
        }
    }
}
