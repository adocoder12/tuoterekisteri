using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace proyecto_v1
{
    public partial class Kayttajat : Form
    {
      
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        String sqlQuery;
        MySqlDataAdapter DtA = new MySqlDataAdapter();
        MySqlDataReader sqlRd;

        DataSet DS = new DataSet();

        String server = "localhost";
        String username = "root";
        String password = "";
        String database = "proyect_3";
        string ConnectionString = null;
        public void upLoadData()
        {
            
            ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                sqlConn.Open();
               sqlQuery = "SELECT Kayttajat_ID,kayttajat.Sukunimi ,kayttajat.Etunimi,kayttajat.Luoka_Ryhma, kayttajat_role.Title from kayttajat INNER JOIN kayttajat_role ON  FkKayttajat_role = kayttajat_role_id ORDER BY kayttajat_id ASC ;";
                MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, sqlConn);
                sqlCmd.Connection = sqlConn;
                sqlRd = sqlCmd.ExecuteReader();
                sqlDt.Load(sqlRd);
                sqlRd.Close();
                sqlConn.Close();
                dataGridView1.DataSource = sqlDt;
            }
        }
        /*-----------------------Filling ComboBox----------------------------------------------------*/
        public void fillingCombox(String query, ComboBox combo, string displayMember, string valueMember)
        {

            ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection saConn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, saConn);
                DataTable table = new DataTable();
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(table);
                combo.DisplayMember = displayMember;
                combo.ValueMember = valueMember;
                combo.DataSource = table;
                
            }
        }
        public Kayttajat()
        {
            InitializeComponent();
        }


        
        private string Comboquery = "SELECT * from  kayttajat_role ;";
        private void student_Load(object sender, EventArgs e)
        {
            upLoadData();
            fillingCombox(Comboquery, comboRooli, "title", "kayttajat_role_id");
        }

        private void student_DoubleClick(object sender, EventArgs e)
        {
            //upLoadData();
        }

        /*------------------------ADDBTN-----------------------------------------------------*/
        private void add_btn_Click(object sender, EventArgs e)
        {
            
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                if (txtEtunimi.Text.Trim() == string.Empty && txtSukunimi.Text.Trim() == string.Empty && txtLuoka_ryhma.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Lisää tietoja");

                }
                else if (comboRooli.Text == "--Choose--")
                {
                    string myStringVariable3 = string.Empty;
                    MessageBox.Show("Select User Type");
                }
                else 
                {
                    try
                    {
                        // sqlQuery = "INSERT INTO kayttajat (etunimi, sukunimi,luoka_ryhma) values( '" + this.txtEtunimi.Text + "', '" + this.txtSukunimi.Text + "', '" + this.txtLuoka_ryhma.Text + "');";

                        sqlQuery = "INSERT INTO proyect_3.kayttajat(etunimi,sukunimi,luoka_ryhma,FkKayttajat_role) values(@etunimi,@sukunimi,@luoka_ryhma,@FkKayttajat_role);";

                        /*----Adding value-----*/
                        MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                       sqlCmd2.Parameters.AddWithValue("@etunimi", txtEtunimi.Text);
                       sqlCmd2.Parameters.AddWithValue("@sukunimi", txtSukunimi.Text);
                       sqlCmd2.Parameters.AddWithValue("@luoka_ryhma", txtLuoka_ryhma.Text);
                       sqlCmd2.Parameters.AddWithValue("@FkKayttajat_role", comboRooli.SelectedValue.ToString());


                        /*--------OPEN connection -----------*/
                        sqlConn.Open();
                        //This is command class which will handle the query and connection object.
                        sqlCmd2.Connection = sqlConn;
                        //sqlCmd2.ExecuteNonQuery();
                        // Here our query will be executed and data saved into the database.
                        sqlCmd2.ExecuteReader(); // tallentaa 

                        MessageBox.Show("Käyttäjät Added!");

                        sqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Käyttäjät not Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    upLoadData();
                    //Clearing the textbox
                    foreach (Control c in panel2.Controls)
                    {
                        if (c is TextBox)
                            ((TextBox)c).Clear();
                    }
                    txtSearch.Text = "";
                }
            }
            

        }

        /*------------------------------------------Edit button------------------------------------*/
        private void editBtn_Click(object sender, EventArgs e)
        {
            
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
               
                try
                {
                    //sqlQuery = "updateproyect_3.kayttajat set kayttajat_id='" + this.txtopiskelija_id.Text + "',etunimi='" + this.txtEtunimi.Text + "',sukunimi='" + this.txtSukunimi.Text + "',Fklaina_id='" + this.txtFklaina_id.Text + "',luoka_ryhma='" + this.txtLuoka_ryhma.Text + "' where kayttajat_id ='" + this.txtopiskelija_id.Text + "';";

                    sqlQuery = "UPDATE kayttajat SET etunimi= @etunimi,sukunimi= @sukunimi,luoka_ryhma=@luoka_ryhma ,FkKayttajat_role = @FkKayttajat_role  WHERE kayttajat_id = @kayttajat_id;";

                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                    sqlCmd2.Parameters.AddWithValue("@kayttajat_id", txtKayttajat_id.Text);
                    sqlCmd2.Parameters.AddWithValue("@etunimi", txtEtunimi.Text);
                    sqlCmd2.Parameters.AddWithValue("@sukunimi", txtSukunimi.Text);
                    sqlCmd2.Parameters.AddWithValue("@luoka_ryhma", txtLuoka_ryhma.Text);
                    sqlCmd2.Parameters.AddWithValue("@FkKayttajat_role", comboRooli.SelectedValue.ToString()); 

                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                   // sqlCmd2.ExecuteNonQuery();
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                    MessageBox.Show("Käyttäjät muokattu!");

                    sqlConn.Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Käyttäjät ei olly muokattu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Clearing the textbox
                foreach (Control c in panel2.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
                txtSearch.Text = "";

               upLoadData();
            }
        }
        /*---------------------DELETE BTN---------------------------*/
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    sqlQuery = "delete from proyect_3.kayttajat where kayttajat_id='" + this.txtKayttajat_id.Text + "';";

                    //sqlQuery = "delete from proyect_3.opiskelijat where opiskelijat_id = @opiskelijat_id; ";

                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);
                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                    sqlCmd2.ExecuteNonQuery();
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                   
                    MessageBox.Show("Käyttäjät Poistu!");

                    sqlConn.Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Käyttäjät not Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //Clearing the textbox
                foreach (Control c in panel2.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
                txtSearch.Text = "";
               
            }
            upLoadData();
        }
        /*------------------------RESET BTN----------------------*/

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            //Clearing the textbox
            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
            }
            txtSearch.Text = "";
           

        }

        /*----------------------------Search User------------------------------------*/
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("sukunimi like'%{0}%'", txtSearch.Text);
                dataGridView1.DataSource = dv.ToTable();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*----------------------------------------------Filtering by ID------------------------------*/

        private void txtKayttajat_id_TextChanged(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            MySqlConnection sqlConn = new MySqlConnection(ConnectionString);
            /*--------OPEN connection -----------*/
            sqlConn.Open();

            sqlQuery = "select etunimi ,sukunimi, luoka_ryhma , kayttajat_role.Title from proyect_3.kayttajat INNER JOIN  kayttajat_role ON  FkKayttajat_role = kayttajat_role_id   where kayttajat_id = @kayttajat_id;";

            /*----Adding value-----*/
            MySqlCommand sqlCmdS = new MySqlCommand(sqlQuery, sqlConn);

            sqlCmdS.Parameters.AddWithValue("@kayttajat_id", txtKayttajat_id.Text);
            sqlRd = sqlCmdS.ExecuteReader();
            while (sqlRd.Read())
            {
                txtEtunimi.Text = sqlRd.GetValue(0).ToString();
                txtSukunimi.Text = sqlRd.GetValue(1).ToString();
                txtLuoka_ryhma.Text = sqlRd.GetValue(2).ToString();
                comboRooli.Text = sqlRd.GetValue(3).ToString();
            }
            sqlConn.Close();
        }
    }
}

