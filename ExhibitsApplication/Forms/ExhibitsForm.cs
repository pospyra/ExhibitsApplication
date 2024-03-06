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
            int dataGridViewWidth = 600; // Указываем новую ширину DataGridView
            int maxTablesPerRow = 3; // Максимальное количество таблиц в строке
            int tableSpacing = 20; // Расстояние между таблицами
            int pictureBoxWidth = 300; // Ширина PictureBox

            int currentTablesInRow = 0;
            int currentX = 20;


            Label nameLabel = new Label();
            nameLabel.Text = exhibit.Name;
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(currentX, startY);

            this.Controls.Add(nameLabel); // Добавляем метку на форму

            DataGridView dataGridView = new DataGridView();
            dataGridView.Location = new System.Drawing.Point(currentX, startY + nameLabel.Height); // Учитываем высоту метки
            dataGridView.Size = new System.Drawing.Size(dataGridViewWidth, 350);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Режим заполнения столбцов
            dataGridView.BackgroundColor = Color.White;

            dataGridView.Columns.Add("Property", "");
            dataGridView.Columns.Add("Value", "");

            // Установка свойства WrapMode для второго столбца
            dataGridView.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            dataGridView.Rows.Add("Инвентарный номер:", exhibit.InventoryNumber);
            dataGridView.Rows.Add("Название:", exhibit.Name);
            dataGridView.Rows.Add("Шифр фондовой коллекции:", exhibit.FundCode);
            dataGridView.Rows.Add("Место и время изготовления:", exhibit.Year);
            dataGridView.Rows.Add("Количество:", exhibit.Quantity);
            dataGridView.Rows.Add("Материал, техника изготовления:", exhibit.Material);
            dataGridView.Rows.Add("Размер:", exhibit.Size);
            dataGridView.Rows.Add("Состояние сохранности:", exhibit.Condition);
            dataGridView.Rows.Add("Описание музейного предмета:", exhibit.Description);
            dataGridView.Rows.Add("Источник поступления:", exhibit.Source);
            dataGridView.Rows.Add("Дата регистрации:", exhibit.RegistratonDate);

            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            this.Controls.Add(dataGridView);

            // Размещаем PictureBox справа от таблицы

            // Определяем размеры и размещение PictureBox
            int pictureBoxHeight = dataGridView.Height; // Высота PictureBox будет такой же, как у DataGridView
            int pictureBoxX = currentX + dataGridViewWidth + tableSpacing; // Располагаем справа от DataGridView
            int pictureBoxY = startY + nameLabel.Height; // Учитываем высоту метки

            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(pictureBoxX, pictureBoxY);
            pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight); // Изменяем размер PictureBox
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.Blue; // Просто для демонстрации, замените на вашу фотографию

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
                currentX += dataGridViewWidth + tableSpacing;
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
