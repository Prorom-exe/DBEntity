using DBEntity.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBEntity
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        DBContext db = new DBContext();



        private void Form1_Load(object sender, EventArgs e)
        {
            //operations for combobox-s
            comboBox1.ValueMember = "CategoryID";
            comboBox1.DisplayMember = "CategoryName";
            var category = db.Categories.ToList();
            category.Insert(0, new Category() { CategoryName = "Все типы", CategoryID = 0 }); ;
            comboBox1.DataSource = category;

            comboBox2.ValueMember = "Id";
            comboBox2.DisplayMember = "PropCars";





            //create category
            Category category1 = new Category { CategoryName = "Двигатель" };
            Category category2 = new Category { CategoryName = "ГСМ" };
            db.Categories.Add(category1);
            db.Categories.Add(category2);
            db.SaveChanges();


            //Создаем два объекта Person




            Item item1 = new Item

            { ItemName = "Помпа", Price = 700, PropCars = "Ваз", Category = category1 };
            Item item3 = new Item

            { ItemName = "Поршень", Price = 2500, PropCars = "KIA", Category = category1 };

            Item item2 = new Item

            { ItemName = "Лукойл", Price = 1000, PropCars = "", Category = category2 };
            Item item4 = new Item

            { ItemName = "Shell", Price = 1600, PropCars = "KIA", Category = category2 };

            // Добавляем их в бд

            db.Items.Add(item3);
            db.Items.Add(item1);
            db.Items.Add(item2);
            db.Items.Add(item4);

            db.SaveChanges();

            //Получаем объекты из бд и выводим на dataGridView1

            dataGridView1.DataSource = db.Items.Select(m => new { m.Id, m.Category.CategoryName, m.ItemName, m.Price, m.PropCars }).ToList();
            dataGridView2.DataSource = db.Korzina.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string ItemName, Price;


            DataGridViewRow curRow = dataGridView1.CurrentRow;

            ItemName = curRow.Cells["ItemName"].Value.ToString();
            Price = curRow.Cells["Price"].Value.ToString();



            Korzina tovar = new Korzina();

            tovar.ItemName = ItemName;
            tovar.Price = Convert.ToDecimal(Price);
            tovar.Kol = Convert.ToInt32(numericUpDown1.Value);

            db.Korzina.Add(tovar);
            db.SaveChanges();
            SumTovars();

            // В dataGridView2 отображаем новых данных
            dataGridView2.DataSource = db.Korzina.ToList();
            numericUpDown1.Value = 1;

        }
        void SumTovars()
        {
            decimal Sum = 0;
            foreach (var tovar in db.Korzina)
            {
                Sum = Sum + (decimal)(tovar.Price * tovar.Kol);
            }
            textBox1.Text = Convert.ToString(Sum);
        }





        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow curRow = dataGridView2.CurrentRow;
            int ID = (int)curRow.Cells["Id"].Value;

            // Берем ссылку на текущую запись
            Korzina tovar = db.Korzina.Find(ID);
            db.Korzina.Remove(tovar);
            db.SaveChanges();

            // В dataGridView2 отображаем новых данных
            dataGridView2.DataSource = db.Korzina.ToList();
            SumTovars();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            var categoryId = (int)comboBox1.SelectedValue;
            if (categoryId == 0)
            {
                dataGridView1.DataSource = db.Items.Select(m => new { m.Id, m.Category.CategoryName, m.ItemName, m.Price, m.PropCars }).ToList();
            }
            else
            {

                dataGridView1.DataSource = db.Items.Where(p => p.Category.CategoryID == categoryId).Select(m => new { m.Id, m.Category.CategoryName, m.ItemName, m.Price, m.PropCars }).ToList();
                var prop = db.Items.Where(p => p.Category.CategoryID == categoryId).Select(i => i.PropCars).ToList();
                prop.Insert(0, "Все авто");
                comboBox2.DataSource = prop;
                
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            var categoryId = (int)comboBox1.SelectedValue;
            var propCars = Convert.ToString(comboBox2.SelectedValue);
            if (propCars == "Все авто")
            {
                dataGridView1.DataSource = db.Items.Where(p => p.Category.CategoryID == categoryId).Select(m => new { m.Id, m.Category.CategoryName, m.ItemName, m.Price, m.PropCars }).ToList();
            }
            else
            {
                dataGridView1.DataSource = db.Items.Where(p => p.PropCars == propCars & p.Category.CategoryID == categoryId).Select(m => new { m.Id, m.Category.CategoryName, m.ItemName, m.Price, m.PropCars }).ToList();
            }
        }
        
    }
}
 
