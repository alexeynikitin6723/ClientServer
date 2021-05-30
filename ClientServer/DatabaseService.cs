using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ClientServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DatabaseService : IDatabaseService
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        SqlConnection sqlConnection = null;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            sqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Application.StartupPath + "\\Database.mdf;Integrated Security=True");
            sqlConnection.Open();
            nextId++;
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
            }
            sqlConnection.Close();
        }

        public void ShowContract(string queryString, int CountCol)
        {
            SqlDataReader dataReader = null;
            List<string[]> data = null;
            try
            {

                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                data = new List<string[]>();
                while (dataReader.Read())
                {
                    data.Add(new string[CountCol]);
                    for (int i = 0; i < CountCol; i++)
                    {
                        data[data.Count - 1][i] = dataReader[i].ToString();
                    }
                }
                dataReader.Close();
            }
            catch
            {

            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            foreach (var item in users)
            {
                item.operationContext.GetCallbackChannel<IDatabaseServiceCallback>().ShowCallBack(data);
            }
        }
    }
}
