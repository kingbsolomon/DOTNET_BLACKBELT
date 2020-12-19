using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId {get;set;}
        public int UserId {get;set;}
        public int EventId {get;set;}
        public User Participant {get;set;}
        public Event Activity {get;set;}
    }
}