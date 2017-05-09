using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    //структура для хранения вместе переменных и констант у конкретной операции
    public struct Operation
    {
        public string typename;
        public List<string> variables { get; set; }
        public List<string> consts { get; set; }
    }
}
