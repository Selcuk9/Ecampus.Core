using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotEcampus.UI.DataBase
{
    public interface IDataBase
    {
        public string GetUserById(long? id);
        public void AddUser();
        public void RemoveById(long? id);
    }
}
