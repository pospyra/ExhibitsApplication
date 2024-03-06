using ExhibitsApplication.Models;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExhibitsApplication.Forms
{
    public partial class ExhibitsForm : Form
    {
        private ExhibitsModel exhibit;

        public ExhibitsForm(ExhibitsModel exhibit)
        {
            this.Size = new Size(800, 400);
            InitializeComponent();
            this.exhibit = exhibit;
            ShowInfo();
        }

        private void ShowInfo()
        {
            int startY = 100;
            int dataGridView1Width = 600; // Указываем новую ширину dataGridView1
            int maxTablesPerRow = 3; // Максимальное количество таблиц в строке
            int tableSpacing = 20; // Расстояние между таблицами
            int pictureBoxWidth = 300; // Ширина PictureBox

            int currentTablesInRow = 0;
            int currentX = 20;


            nameLabel.Text = exhibit.Name;
            nameLabel.AutoSize = true;

            dataGridView1.ClearSelection();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Режим заполнения столбцов
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.Columns.Add("Property", "");
            dataGridView1.Columns.Add("Value", "");

            // Установка свойства WrapMode для второго столбца
            dataGridView1.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            dataGridView1.Rows.Add("Инвентарный номер:", exhibit.InventoryNumber);
            dataGridView1.Rows.Add("Название:", exhibit.Name);
            dataGridView1.Rows.Add("Шифр фондовой коллекции:", exhibit.FundCode);
            dataGridView1.Rows.Add("Место и время изготовления:", exhibit.Year);
            dataGridView1.Rows.Add("Количество:", exhibit.Quantity);
            dataGridView1.Rows.Add("Материал, техника изготовления:", exhibit.Material);
            dataGridView1.Rows.Add("Размер:", exhibit.Size);
            dataGridView1.Rows.Add("Состояние сохранности:", exhibit.Condition);
            dataGridView1.Rows.Add("Описание музейного предмета:", exhibit.Description);
            dataGridView1.Rows.Add("Источник поступления:", exhibit.Source);
            dataGridView1.Rows.Add("Дата регистрации:", exhibit.RegistratonDate);

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            this.Controls.Add(dataGridView1);

            // Размещаем PictureBox справа от таблицы

            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Проверяем, что фотография доступна в модели экспоната
            if (exhibit.Photo != null)
            {
                using (MemoryStream ms = new MemoryStream(exhibit.Photo))
                {
                    pictureBox.Image = Image.FromStream(ms);
                }
            }

            this.Controls.Add(pictureBox);

            currentTablesInRow++;

            if (currentTablesInRow < maxTablesPerRow)
            {
                currentX += dataGridView1Width + tableSpacing;
            }
            else
            {
                startY += 400; // Переход на следующую строку
                currentX = 20;
                currentTablesInRow = 0;
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
            //new Form1().ShowDialog();
        }
    }
}
