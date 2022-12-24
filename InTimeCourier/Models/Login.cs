using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    public class Login
    {
        public int UserId { get; set; }
        [StringLength(maximumLength:20,ErrorMessage ="Username length should not less than 6 or more than 20 character.",MinimumLength =6)]
        [Required(ErrorMessage ="Please enter your username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 30, ErrorMessage = "Username length should not less than 6 or more than 30 character.", MinimumLength = 6)]
        public string Password { get; set; }
        public string TokenId { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}