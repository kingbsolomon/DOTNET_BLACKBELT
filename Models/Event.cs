using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeltExam.Validations;

namespace BeltExam.Models
{
    public class Event
    {
        [Key]
        public int EventId{get;set;}

        [Required(ErrorMessage="Title is required")]
        public string Title{get;set;}

        [Required(ErrorMessage="Start Date and Time is required")]
        [Display(Name="Start Date/Time:")]
        [PastDate]

        public DateTime Start{get;set;}

        [Required(ErrorMessage="Duration is required")]
        [Display(Name="Duration:")]
        public int Duration{get;set;}

        [Required(ErrorMessage="Please Select Hours, Minutes, Days")]
        [Display(Name="Duration String:")]
        public string DurationString {get;set;}


        [Required(ErrorMessage="Description is required")]
        [MinLength(2)]
        public string Description{get;set;}

        //Foreign
        public int UserId {get;set;}
        public User Creator {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public DateTime EndDate {get;set;}

        public List<RSVP> Attendees {get;set;}









        
    }
}