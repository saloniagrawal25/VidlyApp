using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VidlyApp.Models
{
    public class Min18YrsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;
            if(customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
            {
                return ValidationResult.Success;
            }
            if(customer.Birthdate == null)
            {
                return new ValidationResult("Birthday is required.");
            }

            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return age >= 18 ? ValidationResult.Success : new ValidationResult("Customer should be 18 years of age");
        }
    }
};