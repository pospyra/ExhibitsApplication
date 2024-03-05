﻿using ExhibitsApplication.Models;
using ExhibitsApplication.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExhibitsApplication.Forms
{
    public partial class SlideShowForm : Form
    {
        private List<ExhibitsModel> exhibits;
        private int currentExhibitIndex = 0;
        private DataGridView dataGridView;
        private PictureBox pictureBox;
        private Timer timer;
        public SlideShowForm()
        {
            InitializeComponent();
            GetExhibits();
            InitializeDataGridView();
            InitializePictureBox();
            DisplayCurrentExhibit();

            timer = new Timer();
            timer.Interval = 5000; // Интервал в миллисекундах (5 секунд)
            timer.Tick += Timer_Tick;
        }

        private void GetExhibits()
        {
            exhibits = ExhibitsStorage.GetInstance().GetAllExhibits();
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(20, 20);
            dataGridView.Size = new Size(600, 350);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.Columns.Add("Property", "Property");
            dataGridView.Columns.Add("Value", "Value");
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            this.Controls.Add(dataGridView);
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(200, 300);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Location = new Point(dataGridView.Right + 20, dataGridView.Top);
            this.Controls.Add(pictureBox);
        }

        private void DisplayCurrentExhibit()
        {
            if (exhibits.Count > 0 && currentExhibitIndex >= 0 && currentExhibitIndex < exhibits.Count)
            {
                ExhibitsModel exhibit = exhibits[currentExhibitIndex];
                dataGridView.Rows.Clear();
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

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Переход к предыдущему экспонату
            currentExhibitIndex--;
            if (currentExhibitIndex < 0)
            {
                currentExhibitIndex = exhibits.Count - 1;
            }
            DisplayCurrentExhibit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Переход к следующему экспонату
            currentExhibitIndex++;
            if (currentExhibitIndex >= exhibits.Count)
            {
                currentExhibitIndex = 0;
            }
            DisplayCurrentExhibit();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Переход к следующему экспонату
            currentExhibitIndex++;
            if (currentExhibitIndex >= exhibits.Count)
            {
                currentExhibitIndex = 0;
            }
            DisplayCurrentExhibit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Запуск или остановка таймера при нажатии на кнопку
            if (!timer.Enabled)
            {
                timer.Start();
                button3.Text = "Остановить";
            }
            else
            {
                timer.Stop();
                button3.Text = "Автоперелистывание";
            }
        }
    }
}
