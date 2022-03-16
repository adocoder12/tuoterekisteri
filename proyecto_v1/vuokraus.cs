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
    public partial class vuokraus : Form
    {
       
        
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

        /*-----------------------fILLING THE GRID----------------------------------------------------*/
        public void upLoadData()
        {
           
            
            ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                //sqlCmd.CommandText = "select lainaa.laina_id,kayttajat.etunimi,kayttajat.sukunimi, kayttajat.luoka_ryhma,.kayttajat_role.title,tuottet.tuotte_nimi,lainaa.lainatu_paiva,lainaa.palautu_paiva,lainaa.paiva_maara from kayttajat,kayttajat_role,lainaa,tuottet;";


                sqlCmd.CommandText = "select lainaa.Laina_ID,kayttajat.Etunimi,kayttajat.Sukunimi,kayttajat.Luoka_Ryhma,.kayttajat_role.Title,tuottet.Tuotte_Nimi,lainaa.Lainatu_Paiva,lainaa.Palautu_Paiva,lainaa.Paiva_Maara from lainaa join kayttajat ON lainaa.FkKayttajat = kayttajat.kayttajat_id JOIN kayttajat_role on kayttajat.FkKayttajat_role = kayttajat_role.kayttajat_role_id JOIN tuottet ON lainaa.Fktuottet_id = tuottet.tuottet_id JOIN tuottetryhman on tuottet.FktuottetRyhman_id = tuottetryhman.tuottetRyhman_id  ORDER BY Laina_ID ASC  ;";

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
                combo.DisplayMember = Convert.ToString(displayMember);
                combo.ValueMember = Convert.ToString(valueMember);
                combo.DataSource = table;
            }
        }
      
        public vuokraus()
        {
            InitializeComponent();
        }
        //Combox query
        private string tQueryCombox = "SELECT * from tuottet ";
        private string kQueryCombox = "SELECT * from kayttajat ";

        
      
        




        private void vuokraus_Load(object sender, EventArgs e)
        {
            upLoadData();
            fillingCombox(tQueryCombox, tuottetCombox, "tuotte_nimi", "tuottet_id");
            fillingCombox(kQueryCombox, UserCb, "sukunimi", "kayttajat_id");
            

        }


        /*-----------------------ADD BTN----------------------------------------------------*/
        private void add_btn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    // sqlQuery = "INSERT INTO opiskelijat (etunimi, sukunimi,Fklaina_id,luoka_ryhma) values( '" + this.txtEtunimi.Text + "', '" + this.txtSukunimi.Text + "', '" + this.Fklaina_id.Text + "','" + this.txtLuoka_ryhma.Text + "');";

                    sqlQuery = "INSERT INTO proyect_3.lainaa(Fktuottet_id ,FkKayttajat ,lainatu_paiva ,palautu_paiva,paiva_maara) values(@Fktuottet_id,@FkKayttajat,@lainatu_paiva ,@palautu_paiva,@paiva_maara);";

                    //Adding value
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                    sqlCmd2.Parameters.AddWithValue("@Fktuottet_id", tuottetCombox.SelectedValue.ToString());
                    sqlCmd2.Parameters.AddWithValue("@FkKayttajat", UserCb.SelectedValue.ToString());
                    sqlCmd2.Parameters.Add("@lainatu_paiva", MySqlDbType.DateTime).Value = Convert.ToDateTime(dateLainatu.Text);
                    sqlCmd2.Parameters.Add("@palautu_paiva", MySqlDbType.DateTime).Value = Convert.ToDateTime(datePalautu.Text);
                    sqlCmd2.Parameters.Add("@paiva_maara", MySqlDbType.DateTime).Value = Convert.ToDateTime(datePMaara.Text);

                    //OPEN connection 
                    sqlConn.Open();

                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                    

                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                    MessageBox.Show("Vuokraus Added!");


                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Vuokraus not Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                upLoadData();

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
                    //sqlQuery = "updateproyect_3.opiskelijat set opiskelijat_id='" + this.txtopiskelija_id.Text + "',etunimi='" + this.txtEtunimi.Text + "',sukunimi='" + this.txtSukunimi.Text + "',Fklaina_id='" + this.txtFklaina_id.Text + where opettajat_id='" + this.txtOpettajat_id.Text + "';";

                    sqlQuery = "UPDATE proyect_3.lainaa SET laina_id= @laina_id ,Fktuottet_id = @Fktuottet_id ,FkKayttajat =@FkKayttajat,lainatu_paiva = @lainatu_paiva, palautu_paiva=@palautu_paiva,paiva_maara=@paiva_maara  WHERE laina_id  = @laina_id  ;";

                    /*----Adding value-----*/
                    MySqlCommand sqlCmd2 = new MySqlCommand(sqlQuery, sqlConn);

                    sqlCmd2.Parameters.AddWithValue("@laina_id", txtVuokraja.Text);
                    sqlCmd2.Parameters.AddWithValue("@Fktuottet_id", tuottetCombox.SelectedValue.ToString());
                    sqlCmd2.Parameters.AddWithValue("@FkKayttajat", UserCb.SelectedValue.ToString());
                    sqlCmd2.Parameters.Add("@lainatu_paiva", MySqlDbType.DateTime).Value = Convert.ToDateTime(dateLainatu.Text);
                    sqlCmd2.Parameters.Add("@palautu_paiva", MySqlDbType.DateTime).Value = Convert.ToDateTime(datePalautu.Text);
                    sqlCmd2.Parameters.Add("@paiva_maara", MySqlDbType.DateTime).Value = Convert.ToDateTime( datePMaara.Text);

                    /*--------OPEN connection -----------*/
                    sqlConn.Open();
                    //This is command class which will handle the query and connection object.
                    sqlCmd2.Connection = sqlConn;
                   // sqlCmd2.ExecuteNonQuery();
                    // Here our query will be executed and data saved into the database.
                    sqlRd = sqlCmd2.ExecuteReader();

                    MessageBox.Show("Vuokraus muokattu!");


                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Vuokraus ei ollu muokattu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /*-----------------------Delete BTN----------------------------------------------------*/
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            using (MySqlConnection sqlConn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    sqlQuery = "delete from proyect_3.lainaa where laina_id ='" + this.txtVuokraja.Text + "';";

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


                    MessageBox.Show("Vuokraus Poistu!");

                    sqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Vuokraus ei ollut poistu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /*-----------------------Search Vuokrajat----------------------------------------------------*/
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("Etunimi like'%{0}%'", txtSearch.Text);
                dataGridView1.DataSource = dv.ToTable();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /*-----------------------Filter Vuokrajat----------------------------------------------------*/
        private void txtVuokraja_TextChanged(object sender, EventArgs e)
        {
            string ConnectionString = "server=" + server + ";" + "user id=" + username + ";" + "password=" + password + ";" + "database=" + database;
            MySqlConnection sqlConn = new MySqlConnection(ConnectionString);
            /*--------OPEN connection -----------*/
            sqlConn.Open();

            sqlQuery = "select tuottet.tuotte_nimi   ,Kayttajat.sukunimi,lainatu_paiva ,palautu_paiva,paiva_maara from proyect_3.lainaa INNER JOIN kayttajat ON  FkKayttajat = kayttajat_id INNER JOIN tuottet ON  Fktuottet_id = tuottet_id   where  laina_id = @laina_id;";

            /*----Adding value-----*/
            MySqlCommand sqlCmdS = new MySqlCommand(sqlQuery, sqlConn);

            sqlCmdS.Parameters.AddWithValue("@laina_id", txtVuokraja.Text);
            sqlRd = sqlCmdS.ExecuteReader();
            while (sqlRd.Read()) 
            {
                tuottetCombox.Text = sqlRd.GetValue(0).ToString();
                UserCb.Text = sqlRd.GetValue(1).ToString();
                dateLainatu.Text = sqlRd.GetValue(2).ToString();
                datePalautu.Text = sqlRd.GetValue(3).ToString();
                datePMaara.Text = sqlRd.GetValue(4).ToString();
            }
            sqlConn.Close();
        }
        /*-----------------------RESET BTN----------------------------------------------------*/
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            //Clearing the textbox
            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
            }
            txtSearch.Text = "";
            tuottetCombox.Text = "";
            UserCb.Text = "";
        }
    }
    
}
