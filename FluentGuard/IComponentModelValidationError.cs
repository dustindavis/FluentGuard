using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentGuard
{
    public interface IComponentModelValidationError
    {
        string ErrorMessage { get; set; }
        string Name { get; set; }
    }
}
