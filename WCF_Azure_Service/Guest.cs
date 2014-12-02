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
    public partial class Guest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public byte[] Image { get; set; }

        [DataMember]
        public string QRCode { get; set; }

        [DataMember]
        public int Companions { get; set; }

        [DataMember]
        public bool Atended { get; set; }

        [DataMember]
        public bool? AllCompanionsArrived { get; set; }

        [DataMember]
        public int? AtendedCompanions { get; set; }

        [DataMember]
        public virtual Event Event { get; set; }
    }
}
