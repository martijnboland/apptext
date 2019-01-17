using System;

namespace AppText.Shared.Validation
{
    /// <summary>
    /// Don't let the <see cref="Validator{T}"/> validate the property with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreValidationAttribute : Attribute
    {}
}
