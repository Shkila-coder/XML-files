using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Diagnostics;

namespace Kyrsach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //LoadDiscs();
        }

        private BindingList<Discs> data;


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listBox1.SelectedIndex != -1)
            //{
            //    propertyGrid1.SelectedObject = listBox1.SelectedItem;
            //}
        }
        //если в ListBox’e выбран какой-либо элемент, а в нашем случае, имя диска
        //(элементы в нём нумеруются с нуля: 0,1,2,3 и т.д., -1 означает, что не выбран ни один элемент),
        //то в propertyGrid’e должны вывестись элементы, которые закреплены в ListBox’e за конкретным именем.

        private void LoadDiscs()
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load("XMLForKyrsach.xml");
            ////Оператор foreach нужен для циклического обращения к элементам коллекции данных,
            ////которая в нашем случае представлена базой дисков.
            //foreach (XmlNode node in doc.DocumentElement)
            //{
            //    string name = node.Attributes[0].Value;
            //    int fullvolume = int.Parse(node["FullVolume"].InnerText);
            //    int volume = int.Parse(node["Volume"].InnerText);
            //    listBox1.Items.Add(new Discs(name, fullvolume, volume));
            //}
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            int FullVolume = 0;
            int Volume = 0;
            string Name = "";

            if ((tbName.Text == "") || (tbFullVolume.Text == "") || (tbVolume.Text == ""))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка.");
            }
            else
            {
                if (Convert.ToInt32(tbVolume.Text) > Convert.ToInt32(tbFullVolume.Text))
                {
                    MessageBox.Show("Введенный объем памяти больше.", "Ошибка.");
                }
                else
                {
                  Name = "Локальный диск (" + tbName.Text.Trim() + ":)"; //Name
                  FullVolume = Int32.Parse(tbFullVolume.Text); // FullVolume
                  Volume = Int32.Parse(tbVolume.Text); // Volume
                  data.Add(new Discs(Name, Volume, FullVolume));
                  CellValueChanged(null, null);
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e) //сохраняем с формы
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                XMLClass.WriteToXmlFile<BindingList<Discs>>(saveFileDialog1.FileName, data);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Создание пустого Хранилища
            data = new BindingList<Discs>();

            //Создание Объектов и сохранение их в Хранилище
            data.Add(new Discs("Disc A", 1, 100));
            data.Add(new Discs("Disc B", 2, 200));
            data.Add(new Discs("Disc C", 3, 300));

            //Привязка Хранилища к dataGridView1
            dataGridView1.DataSource = data;

            //Настройка dataGridView1
            dataGridView1.Width = 250; //Ширина 

            //Настройка колонок
            var column1 = dataGridView1.Columns[0];
            column1.HeaderText = "Name of Disc"; //текст в шапке
            column1.Width = 100; //ширина колонки
            column1.ReadOnly = true; //значение в этой колонке нельзя править
            column1.Name = "Name"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип колонки

            var column2 = dataGridView1.Columns[1];
            column2.HeaderText = "Volume";
            column2.Width = 50;
            column2.Name = "Volume";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = dataGridView1.Columns[2];
            column3.HeaderText = "Full Volume";
            column2.Width = 50;
            column3.Name = "Full Volume";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            //Назначаем метода-обработчика изменения значения ячейки "Цена"
            dataGridView1.CellValueChanged += CellValueChanged;

            //Принудительный вызов обрабочика для отрисовки цвета строк
            CellValueChanged(null, null);
        }

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || e.ColumnIndex == 1) //Изменилась цена - переопределить цвет
            {
                //Ищем Объект и строку с максимальным кол-вом для памяти
                int maxFullVolume = 0;
                int maxRow = -1;
                for (int i = 0; i < data.Count; i++)
                    if (data[i].FullVolume > maxFullVolume)
                    {
                        maxFullVolume = data[i].FullVolume;
                        maxRow = i;
                    }
                // Изменение цвета строк
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (i == maxRow)
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Beige;
                    else
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;

                }
            }
        }
        
        //Delete button
        private void button3_Click(object sender, EventArgs e)
        {
            int selRowNum = dataGridView1.SelectedCells[0].RowIndex;
            if (MessageBox.Show("Удалить " + dataGridView1[selRowNum, 0].Value + " ?", "Удаление данных", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //Удаляем выбранный объект из Хранилища
                data.RemoveAt(selRowNum);
                //Принудительный вызов обрабочика для перерисовки цвета строк
                CellValueChanged(null, null);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            tbName.Text = data[iRow].Name;
            tbVolume.Text = data[iRow].Volume.ToString();
            tbFullVolume.Text = data[iRow].FullVolume.ToString();
        }

        private void ReadFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlFile = openFileDialog1.FileName;
                data = XMLClass.ReadFromXmlFile<BindingList<Discs>>(xmlFile);
                //data = XMLClass.ReadFromXmlFile<data>(xmlFile);
                //Привязка Хранилища к dataGridView1
                dataGridView1.DataSource = data;

                //Принудительный вызов обрабочика для перерисовки цвета строк
                CellValueChanged(null, null);

                this.Text = "Перечень дисков" + xmlFile;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
