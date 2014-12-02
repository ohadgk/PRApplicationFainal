using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PRApplication.Entities
{

    public class Event
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        public ICollection<Guest> Guests { get; set; }

        
    }
}
