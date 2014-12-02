namespace WCF_Azure_Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract(IsReference = true)]
    public partial class Event
    {
        
        public Event()
        {
            Guests = new HashSet<Guest>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public virtual ICollection<Guest> Guests { get; set; }
    }
}
