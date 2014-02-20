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
                DoWork(new Person()
                {
                    Age = 19,
                    Comments = new string[0]
                });
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
                .WhenNullOrEmpty("{0} is required", c => c.FirstName, c => c.LastName)
                .WhenNull("{0} is required", c => c.BirthDate)
                .WhenTrue("Must be 21 or older", c => c < 21, c => c.Age)
                .WhenFalse("Must be 21 or older", c => c >= 21, c => c.Age)
                .WhenNullOrEmpty<int>("No lottory numbers given", c => c.LottoNumbers)
                .WhenNullOrEmpty("No comments provided", c => c.Comments)
                .ThrowWhenOneOrMoreConditionsAreMet();


            Console.WriteLine("This person is cool");
        }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<int> LottoNumbers { get; set; }
        public string[] Comments { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
