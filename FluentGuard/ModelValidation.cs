using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentGuard
{
    public class ModelValidation<T>
    {
        List<ComponentModelValidationError> _errors = new List<ComponentModelValidationError>();
        public IReadOnlyCollection<ComponentModelValidationError> Errors { get { return _errors.AsReadOnly(); } }

        private int _expressionCount;
        private T _model;
        public ModelValidation(T model)
        {
            _model = model;
            _expressionCount = 0;
        }

        public ModelValidation<T> WhenFalse<W>(string Message, Func<W, bool> predicate, params Expression<Func<T, W>>[] property)
        {
            RunExpression(Message, c => !predicate(c(_model)), property);
            return this;
        }

        public ModelValidation<T> WhenTrue<W>(string Message, Func<W, bool> predicate, params Expression<Func<T, W>>[] property)
        {
            RunExpression(Message, c => predicate(c(_model)), property);
            return this;
        }

        public ModelValidation<T> WhenNull(string Message, params Expression<Func<T, object>>[] property)
        {
            RunExpression(Message, (c) => c(_model) == null, property);
            return this;
        }

        public ModelValidation<T> WhenNull<W>(string Message, params Expression<Func<T, Nullable<W>>>[] property) where W : struct
        {
            RunExpression(Message, c => !c(_model).HasValue, property);
            return this;
        }

        public ModelValidation<T> WhenNullOrEmpty(string Message, params Expression<Func<T, string>>[] property)
        {
            RunExpression(Message, (c) => String.IsNullOrEmpty(c(_model)), property);
            return this;
        }

        public ModelValidation<T> WhenNullOrEmpty<W>(string Message, params Expression<Func<T, IEnumerable<W>>>[] property)
        {
            RunExpression(Message, (c) => c(_model) == null || c(_model).Count() == 0, property);
            return this;
        }

        public ModelValidation<T> WhenNullOrEmpty(string Message, params Expression<Func<T, IEnumerable>>[] property)
        {
            Func<IEnumerable, bool> hasAtLeastOneItem = new Func<IEnumerable,bool>((x) => {
                var res = x.GetEnumerator().MoveNext(); //TODO: Better way?
                return res;
            });

            RunExpression(Message, (c) => c(_model) == null || !hasAtLeastOneItem(c(_model)), property);
            return this;
        }

        private void RunExpression<T, U>(string Message, Func<Func<T, U>, bool> predicate, params Expression<Func<T, U>>[] property)
        {
            foreach (var exp in property)
            {
                _expressionCount++;
                var f = exp.Compile();

                if (predicate(f))
                {
                    string propName = GetPropertyNameFromExpression(exp);
                    _errors.Add(new ComponentModelValidationError(propName, string.Format(Message, propName)));
                }
            }
        }

        private string GetPropertyNameFromExpression<T>(Expression<T> exp)
        {
            return (((MemberExpression)exp.Body).Member as PropertyInfo).Name;
        }

        public void ThrowWhenAllConditionsAreMet()
        {
            if (_errors.Count == _expressionCount)
            {
                throw new ComponentModelValidationException(_errors);
            }
        }

        public void ThrowWhenNoConditionsAreMet()
        {
            if (_errors.Count == 0)
            {
                throw new ComponentModelValidationException(_errors);
            }
        }

        public void ThrowWhenOneOrMoreConditionsAreMet()
        {
            ThrowIfErrors();
        }

        public void ThrowIfErrors()
        {
            if (_errors.Count > 0)
            {
                throw new ComponentModelValidationException(_errors);
            }
        }

        public static ModelValidation<T> Validate<T>(T model)
        {

            return new ModelValidation<T>(model);
        }
    }
}
