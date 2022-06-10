using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel_accounting
{
    public class Person
    {
        public string FullName { get { return $"{SecondName} {FirstName} {ThirdName}"; } }
        public readonly string FirstName;
        public readonly string SecondName;
        public readonly string ThirdName;
        public readonly string BirthDate;
        public readonly string Sex;
        public readonly string Profession;
        public readonly string Division;
        public readonly string Director;

        public Person(string secondName, string firstName, string thirdName, string birthDate, string sex, string profession
            , string division = "", string director = "")
        {
            SecondName = secondName;
            FirstName = firstName;
            ThirdName = thirdName;
            BirthDate = birthDate;
            Sex = sex;
            Profession = profession;
            Division = division;
            Director = director;
        }
    }
}
