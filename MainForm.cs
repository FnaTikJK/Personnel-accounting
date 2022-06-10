namespace Personnel_accounting
{
    public partial class MainForm : Form
    {
        public static string PathToProgram;
        public static Form myForm;
        public static DataBase database;

        private static ComboBox fullnameWithProfessionBox, professionBox, divisionBox;
        private static PersonInformationTable informationTable;
        public MainForm()
        {
            InitializeComponent();
            myForm = this;
            Text = "Учёт персонала";
            Size = new Size(800, 575);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            PathToProgram = string.Join('\\', Directory.GetCurrentDirectory().Split('\\').SkipLast(3));


            AddFields();
        }

        private void AddFields()
        {
            database = new DataBase("Database.mdf");

            var fullnamesWithProffesion = new List<string>() { "" };
            fullnamesWithProffesion.AddRange(database.GetRow("fullname").MergeListsByIndex(database.GetRow("profession")));
            fullnamesWithProffesion.Sort();
            var professions = new List<string>() { "" };
            professions.AddRange(database.GetDistinctedRow("profession"));
            var divisions = new List<string>() { "" };
            divisions.AddRange(database.GetDistinctedRow("division"));

            var filtersDescription = new Label()
            {
                Text = "Фильтры",
                Location = new Point(0, 0),
                Size = new Size(150, 25),
            };

            professionBox = new ComboBox()
            {
                Text = "<Не выбрано>",
                Location = new Point(filtersDescription.Left, filtersDescription.Bottom),
                Size = new Size(250,25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = professions,
            };
            var professionDescription = new Label()
            {
                Text = "Должность",
                Location = new Point(professionBox.Left, professionBox.Bottom),
                Size = professionBox.Size
            };

            divisionBox = new ComboBox()
            {
                Text = "<Не выбрано>",
                Location = new Point(professionBox.Right, professionBox.Location.Y),
                Size = professionBox.Size,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource= divisions,
            };
            var divisionDescription = new Label()
            {
                Text = "Подразделение",
                Location = new Point(divisionBox.Left, divisionBox.Bottom),
                Size = divisionBox.Size,
            };

            fullnameWithProfessionBox = new ComboBox() {
                Text = "<Не выбрано>",
                Location = new Point(professionDescription.Left, professionDescription.Bottom),
                Size = new Size(500, 50),
                DataSource = fullnamesWithProffesion,
                DroppedDown = true,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
            };
            var fullnameDescription = new Label()
            {
                Text = "ФИО работника",
                Location = new Point(fullnameWithProfessionBox.Left, fullnameWithProfessionBox.Location.Y+fullnameWithProfessionBox.Height),
                Size = new Size(150, fullnameWithProfessionBox.Height),
            };

            var filterButton = new Button()
            {
                Location = new Point(divisionBox.Right, divisionBox.Top),
                Size = new Size(150, 50),
                Text = "Отфильтровать",
            };
            filterButton.Click += (s, e) =>
            {
                informationTable.Clear();
                if (professionBox.Text.Length > 0 || divisionBox.Text.Length > 0)
                {
                    var filtredByProfession = database.GetFiltredRow("fullname", "profession", professionBox.Text)
                            .MergeListsByIndex(database.GetFiltredRow("profession", "profession", professionBox.Text));
                    var filtredByDivision = database.GetFiltredRow("fullname", "division", divisionBox.Text)
                            .MergeListsByIndex(database.GetFiltredRow("profession", "division", divisionBox.Text));
                    List<string> result = new List<string>() { "" };
                    if (professionBox.Text.Length > 0 && divisionBox.Text.Length > 0)
                        result.AddRange(filtredByProfession.Except(filtredByProfession.Except(filtredByDivision)).ToList());
                    else if (professionBox.Text.Length > 0)
                        result.AddRange(filtredByProfession);
                    else
                        result = filtredByDivision;
                    fullnameWithProfessionBox.DataSource = result;
                    
                }
                else
                {
                    UpdateDataSource();
                    MessageBox.Show("фильтры не выбраны");
                }
            };

            var selectButton = new Button()
            {
                Text = "Выбрать",
                Location = new Point(fullnameDescription.Left, fullnameDescription.Bottom),
                Size = new Size(100, 50),
            };
            
            informationTable = new PersonInformationTable(this, new Point(this.Width / 2, selectButton.Bottom),true);

            fullnameWithProfessionBox.TextChanged += (s, e) => informationTable.Clear();

            selectButton.Click += (s, e) =>
            {
                if (fullnameWithProfessionBox.Text != "<Не выбрано>" && fullnameWithProfessionBox.Text.Trim().Length > 0)
                {
                    var person = database.GetInformationAboutPerson(string.Join(" ", fullnameWithProfessionBox.Text.Split().Take(3)));
                    informationTable.FillTable(person);
                }
                else
                    MessageBox.Show("Некорректные данные");
            };

            var createNewRecordButton = new Button()
            {
                Location = new Point(myForm.Width - 170, selectButton.Top),
                Size = new Size(150, 50),
                Text = "Создать новую запись",
            };
            createNewRecordButton.Click += (s, e) =>
            {
                RedactorForm.OldPerson = null;
                new RedactorForm().ShowDialog();
            };

            var editButton = new Button()
            {
                Location = new Point(0,myForm.Height - 100),
                Size = new Size(150,50),
                Text = "Редактировать",
            };
            editButton.Click += (s,e) =>
            {
                if (informationTable.SecondName != "")
                {
                    RedactorForm.OldPerson = informationTable.GetPerson();
                    new RedactorForm().ShowDialog();
                }
                else
                    MessageBox.Show("Запись не выбрана");
            };

            var deleteButton = new Button()
            {
                Location = new Point(myForm.Width-170, editButton.Top),
                Size = new Size(150,50),
                Text = "Удалить",
            };
            deleteButton.Click += (s, e) =>
            {
                if (informationTable.GetPerson() != null)
                {
                    database.DeleteRecord(informationTable.FullName);
                    MessageBox.Show("Запис удалена");
                }
                else
                    MessageBox.Show("Запись не выбрана");
            };

            Controls.Add(filtersDescription);
            Controls.Add(filterButton);
            Controls.Add(professionBox);
            Controls.Add(professionDescription);
            Controls.Add(divisionBox);
            Controls.Add(divisionDescription);
            Controls.Add(fullnameWithProfessionBox);
            Controls.Add(fullnameDescription);
            Controls.Add(selectButton);
            Controls.Add(editButton);
            Controls.Add(deleteButton);
            Controls.Add(createNewRecordButton);
        }

        public static void UpdateDataSource()
        {
            var fullnamesWithProffesion = new List<string>() { "" };
            fullnamesWithProffesion.AddRange(database.GetRow("fullname").MergeListsByIndex(database.GetRow("profession")));
            fullnamesWithProffesion.Sort();
            var professions = new List<string>() { "" };
            professions.AddRange(database.GetDistinctedRow("profession"));
            var divisions = new List<string>() { "" };
            divisions.AddRange(database.GetDistinctedRow("division"));

            fullnameWithProfessionBox.DataSource = fullnamesWithProffesion;
            professionBox.DataSource = professions;
            divisionBox.DataSource = divisions;

            informationTable.Clear();
        }
    }
}