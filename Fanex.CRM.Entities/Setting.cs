// --------------------------------------------------------------------------
// <copyright file="Setting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Setting information.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Setting
    {
        private int _displayOrder = 0;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int PrivateType { get; set; }

        [DataMember]
        public int DisplayOrder
        {
            get
            {
                return _displayOrder;
            }

            set
            {
                _displayOrder = value;
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
        /// Determines whether the specified is equal to this instance.
        /// </summary>
        /// <param name="entity">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>True</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object entity)
        {
            return Equals(entity as Setting);
        }

        /// <summary>
        /// Compare with the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Compare result.</returns>
        public bool Equals(Setting other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) ||
                (other.Value.Equals(Value, StringComparison.OrdinalIgnoreCase) &&
                other.Type.Equals(Type, StringComparison.OrdinalIgnoreCase));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ (Value != null ? Value.ToLower().GetHashCode() : 0);
                result = (result * 397) ^ (Type != null ? Type.ToLower().GetHashCode() : 0);
                return result;
            }
        }
    }
}
