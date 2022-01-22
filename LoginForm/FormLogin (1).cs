using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForm
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public string connectionString = "";
        public string usersTableName = "";
        public DataSet usersDataSet;
        public string logedUserName = "";
        public string logedUserPassword = "";
        public string logedUserFullName = "";
        public string fullNameColumn = "Employee_Name";
        public static bool isLoged = false;

        public void Login()
        {
            UserLogin(tbUserName.Text, tbPassword.Text, fullNameColumn);
            if (isLoged)
            {
                MessageBox.Show(logedUserFullName + " Loged in Sucessfull");
               // this.Close();
            }
            else
                MessageBox.Show("Sorry username or Password in incorrect!!! Please try again");

        }
        public void LogOut()
        {
            isLoged = false;
            UpdateUserDetails();

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        public void UserLogin(string userName, string password, string fullNameColumn)
        {
            try
            {
                string query = "Select top 1 * from " + usersTableName + " where userName = '" + userName + "' and password = '" + password + "'";
                usersDataSet = LoadData(query);
                if (usersDataSet.Tables.Count > 0)
                {
                    if (usersDataSet.Tables[0].Rows.Count > 0)
                    {

                        logedUserName = usersDataSet.Tables[0].Rows[0]["userName"].ToString();
                        logedUserPassword = usersDataSet.Tables[0].Rows[0]["password"].ToString();
                        logedUserFullName = usersDataSet.Tables[0].Rows[0][fullNameColumn].ToString();
                        isLoged = true;

                    }
                    else
                        isLoged = false;
                }
                else
                    isLoged = false;
                UpdateUserDetails();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        } // End Function UserLogin
        public void UpdateUserDetails()
        {
            if (!isLoged)
            {
                logedUserFullName = "";
                logedUserName = "";
                logedUserPassword = "";
            }
        } // End function UpdateUserDetails()
        public DataSet LoadData(string query)
        {

            DataSet dt = new DataSet();
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = query;


            SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(command);
            objSqlDataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }
     
    }
}
