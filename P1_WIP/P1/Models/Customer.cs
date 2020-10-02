using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace P1.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "First name cannot exceed 20 characters")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Last name cannot exceed 20 characters")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
