
namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    

    [Serializable]
    [DataContract]
    public class Setting
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int PrivateType { get; set; }

        private int _DisplayOrder = 0;

        [DataMember]        
        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set
            {
                if (value == null)
                    _DisplayOrder = 0;
                else
                {
                    _DisplayOrder = value;
                }
            }
        }

        [DataMember]
        public bool IsSub { get; set; }

        [DataMember]
        public IEnumerable<Setting> Child { get; set; }

        [DataMember]
        [Obsolete("Do not use this property")]
        public List<Setting> Parent { get; set; }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return Equals(entity as Setting);
        }

        public bool Equals(Setting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) || 
                (   
                    other.Value.Equals(Value, StringComparison.OrdinalIgnoreCase)
                    && 
                    other.Type.Equals(Type, StringComparison.OrdinalIgnoreCase)
                );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (Value != null ? Value.ToLower().GetHashCode() : 0);
                result = (result*397) ^ (Type != null ? Type.ToLower().GetHashCode() : 0);
                return result;
            }
        }
    }

    [Serializable]
    [DataContract]
    public class AllSetting
    {

        [DataMember]
        public List<Setting> Title { get; set; }

        [DataMember]
        public List<Setting> Extension { get; set; }

        [DataMember]
        public List<Setting> Communication { get; set; }

        [DataMember]
        public List<Setting> Language { get; set; }

        [DataMember]
        public List<Setting> Category { get; set; }

        [DataMember]
        public List<Setting> SubCategory { get; set; }

        [DataMember]
        public List<Setting> Product { get; set; }

        [DataMember]
        public List<Setting> BetType { get; set; }

        [DataMember]
        public List<Country> Country { get; set; }

        [DataMember]
        public List<Site> Site { get; set; }

        [DataMember]
        public List<Setting> Status { get; set; }

        [DataMember]
        public List<Setting> Response { get; set; }

        [DataMember]
        public List<Setting> Role { get; set; }

    }
}
