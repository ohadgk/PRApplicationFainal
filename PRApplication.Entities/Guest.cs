using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PRApplication.Entities
{

    public class Guest
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string FirstName{ get; set; }

        [Required, StringLength(30)]
        public string LastName { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public byte[] Image { get; set; }

        public string QRCode { get; set; }

        public int Companions { get; set; }

        public int? AtendedCompanions  { get; set; }

        public bool Atended { get; set; }

        public bool? AllCompanionsArrived { get; set; }
    }
}
