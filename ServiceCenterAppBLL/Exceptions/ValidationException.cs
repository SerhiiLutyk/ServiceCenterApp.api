using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppBLL.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }

        public ValidationException(IEnumerable<string> errors)
            : base("Validation failed: " + string.Join("; ", errors)) { }
    }
}
