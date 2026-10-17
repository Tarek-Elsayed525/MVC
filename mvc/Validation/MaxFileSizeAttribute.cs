using System.ComponentModel.DataAnnotations;

namespace mvc.Validation
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int mSize;
        public MaxFileSizeAttribute(int maxSize)
        {

            mSize = maxSize;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"the Length of {name}  greater than  [{mSize}]";

        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                if (file.Length <= mSize)
                    return true;
            }
            return false;

        }



    }
}
