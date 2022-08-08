using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutDownSQLServerProcess
{
    public class ShutDownProcess
    {
        private string connectionString;

        private string serverName;

        private string databaseName;

        public ShutDownProcess(string databaseName = "master", string serverName = "(localdb)\\MSSQLLocalDB")
        {
            connectionString = "Server=" + serverName + "; Database="+databaseName+"; Trusted_Connection = True; MultipleActiveResultSets = true";
        }

        public string ServerName
        {
            get => serverName;
            set => serverName = value;
        }
        public string DatabaseName
        {
            get => databaseName;
            set => databaseName = value;
        }

        public string ConnectionString
        {
            get => connectionString;
            set => connectionString = value;
        }

        public void UpdateConnectionString()
        {
            connectionString = "Server=" + serverName + ";Database=" + databaseName + "; Trusted_Connection = True; MultipleActiveResultSets = true";
        }

        public void ShowSQLServerUserProcess()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT* FROM sys.dm_exec_sessions WHERE is_user_process = 1", con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine(String.Format("|{0,10}|{1,20}|{2,10}|{3,30}|{4,15}|{5,10}|{6,10}|", "Session_id",
                                    "Login_time", "Host_name", "Program_name", "Host_process_id", "LoginName", "Status"));
                            Console.WriteLine(String.Format("|{0,10}|{1,20}|{2,10}|{3,30}|{4,15}|{5,10}|{6,10}|", new string('-', 10),
                                    new string('-', 20), new string('-', 10), new string('-', 30), new string('-', 15),
                                    new string('-', 10), new string('-', 10)));
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("|{0,10}|{1,20}|{2,10}|{3,30}|{4,15}|{5,10}|{6,10}|", reader.GetValue(0).ToString(),
                                    reader.GetValue(1).ToString(), reader.GetValue(2).ToString(),
                                    reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(8).ToString(),
                                    reader.GetValue(11).ToString()));
                                Console.WriteLine(String.Format("|{0,10}|{1,20}|{2,10}|{3,30}|{4,15}|{5,10}|{6,10}|", new string('-', 10),
                                    new string('-', 20), new string('-', 10), new string('-', 30), new string('-', 15),
                                    new string('-', 10), new string('-', 10)));
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("An error was found: \n" + exp.Message);
            }
        }

        public void ShowSQLServerAllProcess()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_who", con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,10}|{3,10}|{4,10}|{5,10}|", "SPID",
                                    "ECID", "Status", "LoginName", "Hostname", "DBName"));
                            Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,10}|{3,10}|{4,10}|{5,10}|", new string('-', 5),
                                    new string('-', 5), new string('-', 10), new string('-', 10), new string('-', 10),
                                    new string('-', 10), new string('-', 10)));
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,10}|{3,10}|{4,10}|{5,10}|", reader.GetValue(0).ToString(),
                                    reader.GetValue(1).ToString(), reader.GetValue(2).ToString().TrimEnd(),
                                    reader.GetValue(3).ToString().TrimEnd(), reader.GetValue(4).ToString().TrimEnd(),
                                    reader.GetValue(6).ToString().TrimEnd()));
                                Console.WriteLine(String.Format("|{0,5}|{1,5}|{2,10}|{3,10}|{4,10}|{5,10}|", new string('-', 5),
                                        new string('-', 5), new string('-', 10), new string('-', 10), new string('-', 10),
                                        new string('-', 10), new string('-', 10)));
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine("An error was found: \n" + exp.Message);
            }
        }

        public void KillSQLServerUserProcess(short spid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("KILL " + spid, con))
                    {
                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("An error was found: \n" + exp.Message);
            }
        }

        public void KillSQLServerAllProcess()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT* FROM sys.dm_exec_sessions WHERE is_user_process = 1", con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                using (SqlCommand cmd2 = new SqlCommand("KILL " + reader.GetValue(0).ToString(), con))
                                {
                                    cmd2.ExecuteReader();
                                }
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception exp)
            {
                Console.WriteLine("An error was found: \n" + exp.Message);
            }
        }

        public void StopSQLServerAllProcess()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("ALTER DATABASE prueba SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception exp)
            {
                Console.WriteLine("An error was found: \n" + exp.Message);
            }
        }
    }
}
