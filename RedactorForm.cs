using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personnel_accounting
{
    public partial class RedactorForm : Form
    {
        public static Form myForm;
        public static Person OldPerson;

        public RedactorForm()
        {
            InitializeComponent();
            myForm = this;
            Text = "Редактор";
            Size = new Size(650, 380);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var personInformation = new PersonInformationTable(this, new Point(Width / 2, 0), false, OldPerson);

            var backButton = new Button()
            {
                Location = new Point(Left, Bottom - 95),
                Size = new Size(150, 50),
                Text = "Назад"
            };
            backButton.Click += (s, e) => { this.Close(); };

            var saveButton = new Button()
            {
                Location = new Point(Right - 150, Bottom - 95),
                Size = new Size(150,50),
                Text = "Сохранить",
            };
            saveButton.Click += (s, e) => {
                var newPerson = personInformation.GetPerson();
                if (newPerson != null)
                {
                    if (OldPerson == null)
                    {
                        MainForm.database.CreateRecord(newPerson);
                        MessageBox.Show("Новая запись создана");
                    }
                    else
                    {
                        MainForm.database.EditRecord(OldPerson.FullName, newPerson);
                        MessageBox.Show("Изменения применены");
                    }
                    this.Close();
                }
                else
                    MessageBox.Show("Некорректные данные");
            };

            Controls.Add(backButton);
            Controls.Add(saveButton);
        }
    }
}
