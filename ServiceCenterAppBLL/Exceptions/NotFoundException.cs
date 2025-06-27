﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceCenterAppBLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} with identifier '{key}' was not found.") { }

        public NotFoundException(string message) : base(message) { }
    }
}
