using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentGuard
{
    public class ComponentModelValidationException : Exception
    {
        public ComponentModelValidationException(IEnumerable<IComponentModelValidationError> validationErrors)
            : base("Error validationg model")
        {
            ValidationErrors = validationErrors;
        }

        public IEnumerable<IComponentModelValidationError> ValidationErrors { get; set; }
    }

}
