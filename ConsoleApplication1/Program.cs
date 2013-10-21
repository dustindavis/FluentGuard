using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentGuard;

namespace FluentGuard.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DoWork(new Person());
            }
            catch (ComponentModelValidationException ex)
            {
                foreach (var err in ex.ValidationErrors)
                {
                    Console.WriteLine(err.ErrorMessage);
                }
            }

            Console.ReadKey();
        }

        public static void DoWork(Person person)
        {
            ModelValidation<Person>.Validate(person)
                .NullOrEmpty("{0} is required", c => c.FirstName, c => c.LastName)
                .Null("{0} is required", c=>c.BirthDate)
                .IsTrue("Must be 21 or older", c=> c < 21, c=>c.Age)
                .ThrowIfErrors();

            Console.WriteLine("This person is cool");
        }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
