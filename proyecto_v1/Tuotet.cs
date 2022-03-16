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
    public partial class Tuotet : Form
    {
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        String sqlQuery;
        MySqlDataReader sqlRd;
        DataSet DS = new DataSet();

        String server = "localhost";
        String username = "root";
        String password = "";
        String database = "proyect_3";
        string ConnectionString = null;
        public Tuotet()
        {
            InitializeComponent();
        }
        /*-----------------------LOAD DATA TO THE GRID----------------------------------------------------*/
        public void upLoadData()
        {

            
            ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                sqlCmd.CommandText = "select tuottet.Tuottet_ID ,tuottet.Tuotte_Nimi, tuottet.Tuote_Varasto ,tuottetryhman.Tuotte_Type ,tuottet.ViivaCode,tuottet.Tuote_Maara from tuottet inner join tuottetryhman on tuottet.FktuottetRyhman_id = tuottetryhman.tuottetRyhman_id ORDER BY tuottet_id ASC;";
              


                sqlRd = sqlCmd.ExecuteReader();
                sqlDt.Load(sqlRd);
                sqlRd.Close();
                sqlConn.Close();
                dataGridView1.DataSource = sqlDt;

            }
            
        }

        /*------------FILLLING COMBOX-----------------*/
      
        public  void fillingCombox(String query, ComboBox combo, string displayMember, string valueMember)
        { 
            
            ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection saConn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, saConn);
                DataTable table = new DataTable();
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(table);
                combo.DisplayMember = Convert.ToString(displayMember);
                combo.ValueMember = Convert.ToString(valueMember);
                combo.DataSource = table;
            }  
        }


        
        private string queryCombox = "SELECT * from tuottetryhman ";
      
        private void Tuotet_Load(object sender, EventArgs e)
        {
            upLoadData();
            //insert info to form tuotet Combobox
            fillingCombox(queryCombox, FktuottetRyhman_idCombox, "tuotte_type", "tuottetRyhman_id");
        }

        /*-----------------------ADD BTN----------------------------------------------------*/
        private void add_btn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                if (txtTuotte_nimi.Text.Trim() == string.Empty && txtPaikka.Text.Trim() == string.Empty && viivaCodetxt.Text.Trim() == string.Empty && txtTuote_maara.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Lisää tietoja");

                }
                else if (FktuottetRyhman_idCombox.Text == "--Choose--")
                {
                    string myStringVariable3 = string.Empty;
                    MessageBox.Show("Select User Type");
                }
                else
                {
                try
                {
                    sqlQuery = "INSERT INTO tuottet (tuotte_nimi, tuote_varasto,viivaCode,tuote_maara,FktuottetRyhman_id) values( '" + this.txtTuotte_nimi.Text + "', '" + this.txtPaikka.Text + "', '" + this.viivaCodetxt.Text + "','" + this.txtTuote_maara.Text + "','" + this.FktuottetRyhman_idCombox.SelectedValue.ToString() + "');";


                    //sqlQuery = "INSERT INTO tuottet (tuotte_nimi ,tuote_varasto  ,viivaCode,tuote_maara,FktuottetRyhman_id) VALUES(@tuotte_nimi ,@tuote_varasto ,@viivaCode,@tuote_maara,@FktuottetRyhman_id);";


                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                    //sqlCmd2.Parameters.AddWithValue("@tuotte_nimi", txtTuotte_nimi.Text);
                    //sqlCmd2.Parameters.AddWithValue("@tuote_varasto ", txtPaikka.Text);
                    //sqlCmd2.Parameters.AddWithValue("@viivaCode", viivaCodetxt.Text);
                    //sqlCmd2.Parameters.AddWithValue("@tuote_maara", txtTuote_maara.Text);
                    //sqlCmd2.Parameters.AddWithValue("@FktuottetRyhman_id", FktuottetRyhman_idCombox.SelectedValue.ToString());

                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                    
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                    MessageBox.Show("Tuottee Added!");


                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Tuottee not Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                upLoadData();
                }

            }
        }

        /*-----------------------EDIT BTN----------------------------------------------------*/
        private void editBtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                try
                {

                    sqlQuery = "UPDATE proyect_3.tuottet set tuottet_id='" + this.txtTuottet_id.Text + "',tuotte_nimi='" + this.txtTuotte_nimi.Text + "',tuote_varasto='" + this.txtPaikka.Text + "',viivaCode='" + this.viivaCodetxt.Text + "',tuote_maara='" + this.txtTuote_maara.Text + "',FktuottetRyhman_id='"+ this.FktuottetRyhman_idCombox.SelectedValue.ToString() + "'WHERE tuottet_id ='" + this.txtTuottet_id.Text + "';";



                   // sqlQuery = "UPDATE proyect_3.tuottet SET tuotte_nimi= @tuotte_nimi , tuote_varasto =@tuote_varasto ,viivaCode=@viivaCode,tuote_maara = @tuote_maara, FktuottetRyhman_id=@FktuottetRyhman_id WHERE tuottet_id= @tuottet_id ;";

                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                    //sqlCmd2.Parameters.AddWithValue("@tuottet_id", txtTuottet_id.Text);
                    //sqlCmd2.Parameters.AddWithValue("@tuotte_nimi", txtTuotte_nimi.Text);
                    //sqlCmd2.Parameters.AddWithValue("@tuote_varasto ", txtPaikka.Text);
                    //sqlCmd2.Parameters.AddWithValue("@viivaCode", viivaCodetxt.Text);
                    //sqlCmd2.Parameters.AddWithValue("@tuote_maara", txtTuote_maara.Text);
                    //sqlCmd2.Parameters.AddWithValue("@FktuottetRyhman_id",FktuottetRyhman_idCombox.SelectedValue.ToString());



                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                  //  sqlCmd2.ExecuteNonQuery();
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                    MessageBox.Show("Tuottee muokattu!");


                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Tuottee ei ollu muokattu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /*-----------------------DELETE BTN----------------------------------------------------*/
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    sqlQuery = "delete from proyect_3.tuottet where tuottet_id ='" + this.txtTuottet_id.Text + "';";

                    //sqlQuery = "delete from proyect_3.opiskelijat where opiskelijat_id = @opiskelijat_id; ";

                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);
                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                    //sqlCmd2.ExecuteNonQuery();
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();


                    MessageBox.Show("Tuottee Poistu!");

                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Tuottee ei ollut poistu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /*-----------------------RESET FORM BTN----------------------------------------------------*/
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            //Clearing the textbox
            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
            }
            txtSearch.Text = "";
            FktuottetRyhman_idCombox.Text = "";
        }
        /*-----------------------SEARCH TUOTTETT----------------------------------------------------*/
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("tuotte_nimi like'%{0}%'", txtSearch.Text);
                dataGridView1.DataSource = dv.ToTable();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /*-----------------------FILTER TUOTTEET----------------------------------------------------*/
        private void txtTuottet_id_TextChanged(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            MySqlConnection sqlConn = new MySqlConnection(ConnectionString);
            /*--------OPEN connection -----------*/
            sqlConn.Open();

           // sqlQuery = "select tuotte_nimi ,tuote_varasto  ,viivaCode,tuote_maara,FktuottetRyhman_id from proyect_3.tuottet tuottetryhman where tuottet_id = @tuottet_id;";

            sqlQuery = "SELECT tuotte_nimi ,tuote_varasto  ,viivaCode,tuote_maara, tuottetryhman.tuotte_type  from tuottet JOIN  tuottetryhman ON tuottet.FktuottetRyhman_id = tuottetryhman.tuottetRyhman_id  where tuottet_id = @tuottet_id;";

            /*----Adding value-----*/
            MySqlCommand sqlCmdS = new MySqlCommand(sqlQuery, sqlConn);

            sqlCmdS.Parameters.AddWithValue("@tuottet_id", txtTuottet_id.Text);
            sqlRd = sqlCmdS.ExecuteReader();
            while (sqlRd.Read())
            {
                txtTuotte_nimi.Text = sqlRd.GetValue(0).ToString();
                txtPaikka.Text = sqlRd.GetValue(1).ToString();
                viivaCodetxt.Text = sqlRd.GetValue(2).ToString();
                txtTuote_maara.Text = sqlRd.GetValue(3).ToString();
                FktuottetRyhman_idCombox.Text = sqlRd.GetValue(4).ToString();
            }
            sqlConn.Close();
        }

      
    }
}
