using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum OperationType { Const, Var, RemoveSpaces, ConcatC, ConcatV };

    //структура для хранения вместе переменных и констант у конкретной операции
    public struct Operation
    {
        public OperationType Type { get; }
        public List<string> variables { get; set; }
        public List<string> consts { get; set; }

        public Operation(OperationType type) : this()
        {
            Type = type;
        }
    }
}
