using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel_accounting
{
    public class InformationBox
    {
        private TextBox textBox;
        public string Text { get => textBox.Text; set { textBox.Text = value; } }

        private Label description;
        public string Description { get => description.Text; set { description.Text = value; } }

        public int Left { get => description.Left; }
        public int Right { get => description.Right; }
        public int Bottom { get => description.Bottom; }

        public InformationBox(Form form, Point location, Size size, string informationDescription, bool isReadOnly)
        {
            textBox = new TextBox()
            {
                Location = location,
                Size = size,
                ReadOnly = isReadOnly,
            };
            description = new Label()
            {
                Text = informationDescription,
                Location = new Point(textBox.Left, textBox.Bottom),
                Size = size,
            };

            form.Controls.Add(textBox);
            form.Controls.Add(description);
        }
    }
}
