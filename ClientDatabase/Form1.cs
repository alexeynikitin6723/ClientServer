using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientDatabase.DatabaseService;

namespace ClientDatabase
{
    public partial class Form1 : Form, IDatabaseServiceCallback
    {
        bool isConnected = false;
        DatabaseServiceClient client;
        int ID;
        public Form1()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new DatabaseServiceClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.Enabled = false;
                button1.Text = "Disconnect";
                isConnected = true;
            }
        }
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.Enabled = true;
                button1.Text = "Connect";
                isConnected = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void ShowCallBack(string[][] data)
        {
            foreach (string[] s in data)
                dataGridView.Rows.Add(s);
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                this.dataGridView.Rows.Clear();
                if (checkBoxRelevance.Checked)
                {
                    client.ShowContract("select * from Contract where datediff(day,LastUpdate,GETDATE())<60", 4);
                }
                else
                {
                    client.ShowContract("select * from Contract", 4);
                }
            }
        }
    }
}
