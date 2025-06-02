using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Attributes
{
    /// <summary>
    /// Custom validation attribute for Iranian phone numbers.
    /// Validates that the phone number starts with "09" and has a total length of 11 digits.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IranianPhoneNumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IranianPhoneNumberAttribute"/> class.
        /// Sets a default error message.
        /// </summary>
        public IranianPhoneNumberAttribute()
        {
            // Default error message in Persian
            ErrorMessage = "شماره موبایل معتبر نیست. شماره باید با 09 شروع شود و 11 رقم باشد.";
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The PhoneAttribute already handles null/empty strings if it's present,
            // but it's good practice to check here as well if this attribute is used standalone.
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // If you want to allow empty values and let [Required] handle it,
                // you can return ValidationResult.Success here.
                // For this specific case, an Iranian phone number implies it's not empty.
                // However, if [Required] is already there, this might be redundant for null/empty.
                // Let's assume for now that a non-null, non-empty value is expected for this specific validation.
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            string phoneNumber = value.ToString();

            // Regular expression to check if the phone number starts with "09" and is followed by 9 digits,
            // making a total of 11 digits.
            // ^ : asserts position at start of the string
            // 09 : matches "09" literally
            // \d{9} : matches exactly nine digits (0-9)
            // $ : asserts position at the end of the string
            var regex = new Regex(@"^09\d{9}$");

            if (regex.IsMatch(phoneNumber))
            {
                return ValidationResult.Success; // Validation passed
            }

            // Validation failed, return the error message.
            // The ErrorMessage property can be overridden when applying the attribute.
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}