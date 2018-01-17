#Fluent Guard#

Fluent Guard is a validation framework with a fluent interface. It's meant to be quick and easy to validate multiple properties on a model with as little code as possible.

### Example Usage ###

	public void DoWork(Person person)
        {
            ModelValidation<Person>.Validate(person)
                .WhenNullOrEmpty("{0} is required", c => c.FirstName, c => c.LastName)
                .WhenNull("{0} is required", c=>c.BirthDate)
                .WhenTrue("Must be 21 or older", c=> c < 21, c=>c.Age)
                .ThrowIfErrors();

            Console.WriteLine("This person is cool");
        } 

