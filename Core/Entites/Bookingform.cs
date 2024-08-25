using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Bookingform
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "The name can only contain letters and spaces.")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Please enter the car name.")]
        public string CarName { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "CNIC must be exactly 13 digits long.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "CNIC must be exactly 13 digits without Dashes")]
        public string Cnic { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
       [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits long.")]
       [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must contain exactly 11 digits without spaces or other characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } // This will be set from the User.Identity

        [Required(ErrorMessage = "Please enter the car plate number.")]
        public string CarPlateNumber { get; set; }

        [Required(ErrorMessage = "Please enter the car color.")]
        public string CarColor { get; set; }
        [Required(ErrorMessage = "Please enter the pickup location.")]
        [RegularExpression(@"^[^<>@|]+$", ErrorMessage = "The pickup location contains not allowed characters: <, >, @, |.")]
        public string PickupLocation { get; set; }


        [Required(ErrorMessage = "Please enter the pickup date.")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date and time.")]
        public DateTime PickUpDate { get; set; }

        [Required(ErrorMessage = "Please enter the return date.")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date and time.")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Please enter the quantity.")]
        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[^<>@|]+$", ErrorMessage = "The Additional Information  contains not allowed characters: <, >, @, |.")]

        public string SomeInformation { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }
    }

    //public class Bookingform
    //{
    //    public int Id { get; set; }

    //    public string PersonName { get; set; }

    //    public string CarName { get; set; }

    //    public string Cnic { get; set; }

    //    public string PhoneNumber { get; set; }

    //    public string Email { get; set; } 

    //    public string CarPlateNumber { get; set; }

    //    public string CarColor { get; set; }
    //    public string PickupLocation { get; set; }


    //    public DateTime PickUpDate { get; set; }
    //    public DateTime ReturnDate { get; set; }

    //    public int Quantity { get; set; }

    //    public string SomeInformation { get; set; }
    //    public string Status { get; set; }
    //}
}
