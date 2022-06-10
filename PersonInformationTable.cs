using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel_accounting
{
    public class PersonInformationTable
    {
        public string FullName { get { return $"{SecondName} {FirstName} {ThirdName}"; } }
        public string SecondName { get => secondName.Text; set { secondName.Text = value; } }
        private InformationBox secondName;
        public string FirstName { get => firstName.Text; set { firstName.Text = value; } }
        private InformationBox firstName;
        public string ThirdName { get => thirdName.Text; set { thirdName.Text = value; } }
        private InformationBox thirdName;
        public string BirthDate { get => birthDate.Text; set { birthDate.Text = value; } }
        private InformationBox birthDate;
        public string Sex { get => sex.Text; set { sex.Text = value; } }
        private InformationBox sex;
        public string Profession { get => profession.Text; set { profession.Text = value; } }
        private InformationBox profession;
        public string Division { get => division.Text; set { division.Text = value; } }
        private InformationBox division;
        public string Director { get => director.Text; set { director.Text = value; } }
        private InformationBox director;

        private Size informationSize;

        public PersonInformationTable(Form form, Point startLocation, bool isReadOnly, Person person = null)
        {
            informationSize = new Size(form.Width / 2, 25);
            var description = new Label()
            {
                Text = "Информация",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Times New Roman", 13),
                Location = new Point(startLocation.X-informationSize.Width/2, startLocation.Y),
                Size = informationSize,
            };

            form.Controls.Add(description);

            secondName = new InformationBox(form,new Point(0, description.Bottom), informationSize, "Фамилия", isReadOnly);
            firstName = new InformationBox(form,new Point(0, secondName.Bottom), informationSize, "Имя", isReadOnly);
            thirdName = new InformationBox(form,new Point(0, firstName.Bottom), informationSize, "Отчество", isReadOnly);
            birthDate = new InformationBox(form,new Point(0, thirdName.Bottom), informationSize, "Дата рождения", isReadOnly);
            sex = new InformationBox(form,new Point(0, birthDate.Bottom), informationSize, "Пол", isReadOnly);

            profession = new InformationBox(form,new Point(form.Width - informationSize.Width, description.Bottom)
                , informationSize, "Должность", isReadOnly);
            division = new InformationBox(form,new Point(profession.Left, profession.Bottom), informationSize, "Подразделение",isReadOnly);
            director = new InformationBox(form,new Point(profession.Left, division.Bottom), informationSize, "ФИО руководителя", isReadOnly);

            FillTable(person);
        }

        public void FillTable(Person person)
        {
            if (person != null)
            {
                SecondName = person.SecondName;
                FirstName = person.FirstName;
                ThirdName = person.ThirdName;
                BirthDate = person.BirthDate;
                Sex = person.Sex;
                Profession = person.Profession;
                Division = person.Division;
                Director = person.Director;
            }
        }
        
        public void Clear()
        {
            SecondName = "";
            FirstName = "";
            ThirdName = "";
            BirthDate = "";
            Sex = "";
            Profession = "";
            Division = "";
            Director = "";
        }

        public Person GetPerson()
        {
            var person =  new Person(this.SecondName.Trim(), this.FirstName.Trim(), this.ThirdName.Trim()
                        , this.BirthDate.Trim(), this.Sex.Trim(), this.Profession.Trim()
                        , this.Division.Trim(), this.Director.Trim());
            if (IsCorrectPerson(person))
                return person;
            return null;
        }

        private bool IsCorrectPerson(Person person)
        {
            return (person.SecondName.Length > 0 && person.FirstName.Length > 0 && person.ThirdName.Length > 0
                && person.BirthDate.Length > 0 && person.Sex.Length > 0 && person.Profession.Length > 0)
                && (person.Profession.Length>0 && person.Division.Length>0 || person.Profession == "director" && person.Director =="" && person.Division == "");
        }
    }
}
