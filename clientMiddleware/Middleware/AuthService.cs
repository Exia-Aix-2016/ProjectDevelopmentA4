using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Middleware
{
    class AuthService
    {
        
        public userLogin()
        {
            //Comment récupérer Login/Password et la connexion a la bdd
            
            string commandString = "SELECT login FROM users WHERE login = @login";
            SqlParameter paramLogin = new SqlParameter("@login", SqlDbType.Text);
            paramLogin.Value = login;

            Object oUser = AuthService.QueryUser();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandString, conn))
                {
                    connection.CommandType = CommandType.Text;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    //return reader;
                };
            }


            // Check si le mot de passe correspond
            string commandString = "SELECT login FROM users WHERE login = @login AND pass = @pass";
            SqlParameter paramLogin = new SqlParameter("@login", SqlDbType.Text);
            SqlParameter paramPass = new SqlParameter("@pass", SqlDbType.Text);
            paramLogin.Value = login;
            paramPass.Value = pass;

            Object oUser = AuthService.QueryUser();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandString, conn))
                {
                    connection.CommandType = CommandType.Text;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    //return reader;
                };
            }

        }
        
        
        
        
    }
}
