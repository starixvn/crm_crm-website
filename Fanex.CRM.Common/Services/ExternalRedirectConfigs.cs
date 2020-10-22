namespace Fanex.CRM.Configuration.ExternalRedirectConfigs
{
    using System;
    using System.Configuration;

    public static class ExternalRedirectConfigs
    {
        public readonly static ExternalRedirectConfigsSection _config
            = ConfigurationManager.GetSection("externalRedirectConfigs") as ExternalRedirectConfigsSection;

        public static ExternalRedirectCollection GetConfigs()
        {
            return _config.ExternalRedirects;
        }
    }

    public class ExternalRedirectConfigsSection : ConfigurationSection
    {
        [ConfigurationProperty("externalRedirects")]
        public ExternalRedirectCollection ExternalRedirects
        {
            get { return (ExternalRedirectCollection)this["externalRedirects"]; }
        }
    }

    [ConfigurationCollection(typeof(ExternalRedirectElement))]
    public class ExternalRedirectCollection : ConfigurationElementCollection
    {
        public ExternalRedirectElement this[int index]
        {
            get { return (ExternalRedirectElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExternalRedirectElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExternalRedirectElement)element).Domain;
        }
    }

    public class ExternalRedirectElement : ConfigurationElement
    {
        public Uri DomainUri => new Uri(Domain);

        [ConfigurationProperty("domain", IsRequired = true)]
        public string Domain
        {
            get { return (string)this["domain"]; }
            set { this["domain"] = value; }
        }

        [ConfigurationProperty("includePort", IsRequired = true)]
        public bool IncludePort
        {
            get { return (bool)this["includePort"]; }
            set { this["includePort"] = value; }
        }

        [ConfigurationProperty("externalDomain", IsRequired = true)]
        public string ExternalDomain
        {
            get { return (string)this["externalDomain"]; }
            set { this["externalDomain"] = value; }
        }
    }
}