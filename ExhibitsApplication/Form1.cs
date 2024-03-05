using ExhibitsApplication.Models;
using ExhibitsApplication.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExhibitsApplication
{
    public partial class Form1 : Form
    {
        private MuseumExhibitParser exhibitParser;

        public Form1()
        {
            InitializeComponent();
            exhibitParser = new MuseumExhibitParser();
            this.AutoScroll = true;
        }
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word Documents|*.docx;*.doc";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                List<ExhibitsModel> exhibits = exhibitParser.ParseWordDocument(filePath);
                DisplayExhibits(exhibits);
            }
        }
        private void DisplayExhibits(List<ExhibitsModel> exhibits)
        {
            int startY = 20;
            int dataGridViewWidth = 600; // Указываем новую ширину DataGridView

            foreach (var exhibit in exhibits)
            {
                DataGridView dataGridView = new DataGridView();
                dataGridView.Location = new System.Drawing.Point(20, startY);
                dataGridView.Size = new System.Drawing.Size(dataGridViewWidth, 300);
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Режим заполнения столбцов

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

                // Автоматическое изменение высоты ячеек под содержимое
                dataGridView.AutoResizeRows();

                this.Controls.Add(dataGridView);
                startY += 320;
            }
        }



        private void browseButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word Documents|*.docx;*.doc";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                List<ExhibitsModel> exhibits = exhibitParser.ParseWordDocument(filePath);
                DisplayExhibits(exhibits);
            }
        }
    }
}

