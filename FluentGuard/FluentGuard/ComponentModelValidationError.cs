using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentGuard
{
   
    public class ComponentModelValidationError : IComponentModelValidationError
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }

        public ComponentModelValidationError(string name, string message)
        {
            Name = name;
            ErrorMessage = message;
        }

    }

}
