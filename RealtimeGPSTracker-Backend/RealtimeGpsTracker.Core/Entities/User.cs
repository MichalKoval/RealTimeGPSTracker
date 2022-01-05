using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RealtimeGpsTracker.Core.Entities
{
    /// <summary>
    /// Extends Identity user model.
    /// </summary>
    public class User : IdentityUser
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[PersonalData]
        //public string new Id { get; set; }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

	    public virtual ICollection<GpsDevice> GpsDevices { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}
