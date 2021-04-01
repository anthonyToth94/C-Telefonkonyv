using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telefonkonyv
{
    public partial class Kezelofelulet : Form
    {
        List<Person> listForPerson = new List<Person>();
        Person p = new Person();
        string firstName;
        string lastName;
        string contact;
        string email;
        string address;

        SqlConnection conn = null;
        string connectionString;
        DataTable dt = new DataTable();
        SqlDataAdapter adapter = null;
        SqlDataReader reader = null;
        SqlCommand command = null;
        public Kezelofelulet()
        {
            InitializeComponent();
            SqlConnect(); //connection..
            DataForGV(conn);

        } 

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkValidate(textBox1) == true && checkValidate(textBox2) == true && checkValidate(textBox3) == true && checkValidate(textBox4) == true && checkValidate(textBox5) == true)
            {
                if (Regex.Match(textBox3.Text, "^[0-9]{11}$").Success)
                {
                    if (textBox4.Text.Contains("@"))
                    {
                        Savedata(conn);   //connOpen..  saveData... / connClose
                    }
                    else
                    {
                        MessageBox.Show("Incorrect email address!");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Telephone number!");
                }

                 
            }
            else
            {
                MessageBox.Show("Please fill in the fields!");
            }
        }

        public void SqlConnect()
        {
            //try to connect
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Proba;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                conn = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ConnOpen()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                //MessageBox.Show("Open");
            }
        }
        public void ConnClose()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                //MessageBox.Show("Closed");
            }
        }
        public void Savedata(SqlConnection connection)
        {
            firstName = textBox1.Text;
            lastName = textBox2.Text;
            contact = textBox3.Text;
            email = textBox4.Text;
            address = textBox5.Text;
            try
            {
                ConnOpen();
                string query = "INSERT INTO Adatok(firstName, lastName, contact, email, address) VALUES(@firstName, @lastName, @contact, @email, @address)";
                SqlCommand command = new SqlCommand(query, connection);
                //sql injection!!!
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@contact", contact);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@address", address);
                int row = command.ExecuteNonQuery();
                UpdateListForPerson(firstName, lastName, contact, email, address);
                dt.Clear();
                DataForGV(conn);

                if (row == 1)
                {
                    textBox1.Text = String.Empty;
                    textBox2.Text = String.Empty;
                    textBox3.Text = String.Empty;
                    textBox4.Text = String.Empty;
                    textBox5.Text = String.Empty;
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Failed");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConnClose();
            }
        
        }
        public void UpdateListForPerson(string f, string l, string c, string e, string a)
        {
            p.FirstName = f;
            p.LastName = l;
            p.Contact = c;
            p.Email = e;
            p.Address = a;
            listForPerson.Add(p);
        }
       
        public void DataForGV(SqlConnection connection)
        {
            try
            {
                ConnOpen();
                string query = "SELECT id, firstName, lastName, contact, email, address FROM Adatok";
                command = new SqlCommand(query, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                if(dt.Rows.Count == 1)
                {
                   p.Id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                   listForPerson.Add(p);
                }          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConnClose();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;
            textBox4.Text = String.Empty;
            textBox5.Text = String.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Searchdata(conn);
        }
        public void Searchdata(SqlConnection connection)
        {
            dt.Clear();
            if(textBox6.Text != "")
            {
                try
                {
                    ConnOpen();
                    string query = "SELECT id, firstName, lastName, contact, email, address FROM Adatok WHERE lastName LIKE '%" + textBox6.Text + "%' OR firstName LIKE '%" + textBox6.Text + "%' OR email LIKE '%" + textBox6.Text + "%' OR address LIKE '%" + textBox6.Text + "%' OR contact LIKE '%" + textBox6.Text + "%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ConnClose();
                }
            }
            else
            {
                DataForGV(conn);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteData(conn);
        }
        public void DeleteData(SqlConnection connection)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {                
                try
                {
                    DataGridViewRow row = this.dataGridView1.SelectedRows[0];
                    ConnOpen();
                    string query = "DELETE FROM Adatok WHERE id ='" + row.Cells["Id"].Value + "'";
                    command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    int currentRow = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows.RemoveAt(currentRow);



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public bool checkValidate(TextBox textbox)
        {
            if(textbox.Text == "")
            {
                return false;
            }
            else
                return true;
        }

    }
}
