using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Middleware
{
    class AuthService
    {
        
        public void userLogin(string login, string pass)
        {
            //Comment récupérer Login/Password et la connexion a la bdd

            userContext user = new userContext();

            user.Users.Find();

            string commandLoginString = "SELECT login FROM users WHERE login = @login";
            SqlParameter paramLogin = new SqlParameter("@login", SqlDbType.Text);
            paramLogin.Value = login;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandLoginString, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramLogin);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    //return reader;
                };
            }


            // Check si le mot de passe correspond
            string commandPassString = "SELECT login FROM users WHERE login = @login AND pass = @pass";
            SqlParameter paramLogin2 = new SqlParameter("@login", SqlDbType.Text);
            SqlParameter paramPass = new SqlParameter("@pass", SqlDbType.Text);
            paramLogin2.Value = login;
            paramPass.Value = pass;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandPassString, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramLogin2);
                    cmd.Parameters.Add(paramPass);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    //return reader;
                };
            }

        }
        
        
        
        
    }
}
