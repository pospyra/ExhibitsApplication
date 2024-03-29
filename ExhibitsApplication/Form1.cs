﻿using ExhibitsApplication.Forms;
using ExhibitsApplication.Models;
using ExhibitsApplication.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExhibitsApplication
{
    public partial class Form1 : Form
    {
        private MuseumExhibitParser exhibitParser;
        private ExhibitsStorage exhibitsStorage;
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            exhibitParser = new MuseumExhibitParser();
            exhibitsStorage = ExhibitsStorage.GetInstance();
            this.AutoScroll = true;
        }

        private void DisplayExhibits(List<ExhibitsModel> exhibits)
        {
            // Определение параметров карточек
            int cardWidth = 220;
            int cardHeight = 250;
            int maxCardsPerRow = 5;
            int cardSpacingX = 20;
            int cardSpacingY = 20;

            // Вычисление общей ширины карточек и промежутков между ними
            int totalCardWidth = maxCardsPerRow * (cardWidth + cardSpacingX) - cardSpacingX;
            // Вычисление смещения по горизонтали для центрирования
            int offsetX = (this.Width - totalCardWidth) / 2;

            // Определение начальной координаты Y для первой строки
            int startY = 100;

            int currentCardsInRow = 0;
            int currentX = offsetX;

            foreach (var exhibit in exhibits)
            {
                TableLayoutPanel cardPanel = new TableLayoutPanel();
                cardPanel.Size = new Size(cardWidth, cardHeight);
                cardPanel.BorderStyle = BorderStyle.FixedSingle; // Добавляем рамку карточке
                cardPanel.Location = new Point(currentX, startY);
                cardPanel.Cursor = Cursors.Hand; // Добавляем курсор при наведении на карточку
                cardPanel.Click += (sender, e) => ShowExhibitForm(exhibit);

                // Добавим строку для изображения
                cardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));

                PictureBox pictureBox = new PictureBox();

                pictureBox.Size = new Size(cardWidth, 200); 

                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                if (exhibit.Photo != null)
                {
                    using (MemoryStream ms = new MemoryStream(exhibit.Photo))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
                cardPanel.Controls.Add(pictureBox, 0, 0);


                cardPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                Label cipherLabel = new Label();
                cipherLabel.Text = exhibit.FundCode;
                cipherLabel.AutoSize = true;
                cipherLabel.Margin = new Padding(1);

                cardPanel.Controls.Add(cipherLabel, 0, 2); 

                Label nameLabel = new Label();
                nameLabel.Text = exhibit.Name;
                nameLabel.AutoSize = true;
                nameLabel.Margin = new Padding(1);
                nameLabel.Font = new Font(nameLabel.Font.FontFamily, 11); 

                nameLabel.Click += (sender, e) => ShowExhibitForm(exhibit);

                cardPanel.Controls.Add(nameLabel, 0, 1);
                cardPanel.Controls.Add(cipherLabel, 0, 2);

                nameLabel.Padding = new Padding(2);
                cipherLabel.Padding = new Padding(2);

                this.Controls.Add(cardPanel);

                currentCardsInRow++;

                if (currentCardsInRow < maxCardsPerRow)
                {
                    currentX += cardWidth + cardSpacingX;
                }
                else
                {
                    startY += cardHeight + cardSpacingY; 
                    currentX = offsetX;
                    currentCardsInRow = 0;
                }
            }
        }

        private void ShowExhibitForm(ExhibitsModel exhibit)
        {
            ExhibitsForm exhibitForm = new ExhibitsForm(exhibit);
            exhibitForm.Show();
        }

        private void browseButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word Documents|*.docx;*.doc";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                label1.Visible = false;
                browseButton.Visible = false;

                string filePath = openFileDialog.FileName;
                exhibitParser.ParseWordDocument(filePath);
                DisplayExhibits(exhibitsStorage.GetAllExhibits());

                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
            }
        }

        private void GetGilteredExhibits()
        {
            ClearExhibitCards();
            string name = textBox1.Text;
            var filteredList = exhibitParser.GetExhibitsByFilter(name);
            DisplayExhibits(filteredList);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GetGilteredExhibits();
        }

        private void ClearExhibitCards()
        {
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in this.Controls)
            {
                if (control is Panel)
                {
                    controlsToRemove.Add(control);
                }
            }

            foreach (Control control in controlsToRemove)
            {
                this.Controls.Remove(control);
                control.Dispose(); // Освобождаем ресурсы
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SlideShowForm().ShowDialog();   
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GetGilteredExhibits();

                e.Handled = true;
            }
        }
    }
}
