using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppBLL.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
            : base("Access denied.") { }

        public AccessDeniedException(string message)
            : base(message) { }
    }
}

