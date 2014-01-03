
namespace Fanex.CRM.Entities.ViewModel
{
    using System.Runtime.Serialization;
    using System;

    [Serializable]
    [DataContract]
    public class FeedbackView
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public string FeedbackDate { get; set; }
        [DataMember]
        public string CreateDate { get; set; }
        [DataMember]
        public int MainCategoryId { get; set; }
        [DataMember]
        public string MainCategory { get; set; }
        [DataMember]
        public int SubCategoryId { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Reviewer { get; set; }
        [DataMember]
        public string ReviewDate { get; set; }
        [DataMember]
        public string RelatedUsername { get; set; }
        [DataMember]
        public string DurationTime { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
