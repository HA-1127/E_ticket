using System.ComponentModel.DataAnnotations;

namespace E_ticket.validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class customlengthvalidationattribute : ValidationAttribute
    {
       private readonly int _length;
        public customlengthvalidationattribute(int length)
        {
            _length = length;
        }
        public override bool IsValid(object? value)
        {
            if (value is string v)
            {
                if (v.Length < -_length)
                {
                    return true;
                }
            }
            return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
