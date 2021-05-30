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
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IDatabaseService" в коде и файле конфигурации.
    [ServiceContract(CallbackContract =typeof(IDatabaseServiceCallback))]
    public interface IDatabaseService
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void ShowContract(string queryString, int CountCol);
    }

    public interface IDatabaseServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ShowCallBack(List<string[]> data);
    }
}
