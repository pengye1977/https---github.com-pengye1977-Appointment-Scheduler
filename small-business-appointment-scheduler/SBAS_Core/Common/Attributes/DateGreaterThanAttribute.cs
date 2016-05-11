// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-04-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="DateGreaterThanAttribute.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This was originaally going to be used for service type but this was moved to be int he service type table.</summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The Attributes namespace.
/// </summary>
namespace SBAS_Core.Common.Attributes
{
    /// <summary>
    /// Class DateGreaterThanAttribute.
    /// </summary>
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error MSG template
        /// </summary>
        private const string DefaultErrorMsgTemplate = "{0} should be after {1}";
        /// <summary>
        /// Gets or sets the other field.
        /// </summary>
        /// <value>The other field.</value>
        public string OtherField { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateGreaterThanAttribute"/> class.
        /// </summary>
        public DateGreaterThanAttribute()
        {
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMsgTemplate;
            }
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? date = value != null ? (DateTime?)value : null;
            var otherValue = validationContext.ObjectType.GetProperty(OtherField).GetValue(validationContext.ObjectInstance);
            DateTime? otherDate = otherValue != null ? (DateTime?)otherValue : null;

            TimeSpan diff = date.Value.Subtract(otherDate.Value);

            if (date.HasValue && otherDate.HasValue && diff.TotalMinutes >= 0)
            {
                return new ValidationResult(String.Format(ErrorMessage, validationContext.DisplayName, OtherField));
            }

            return ValidationResult.Success;

        }

    }
}
