using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Абстрактный декоратор операций
    public abstract class OperationDecorator : Component
    {
        protected Component component;
        public void SetComponent(Component component)
        {
            this.component = component;
        }
        public override void AddOperation(Operation operation)
        {
            component.operations.Add(operation);
        }
    }

    // Удаление начальных и конечных пробельных символов ^ F
    public class RemoveSpaces : OperationDecorator
    {

        public Operation CreateOperation()
        {
            Operation operation = new Operation(OperationType.RemoveSpaces);
            return operation;
        }
        public string Calculate(string s)
        {
            return s.Trim();
        }
    }

    // Конкатенация строк F && c и F && v

    public class Concat : OperationDecorator
    {
        //это методы для создания операции конкатенации с константой или же с переменной
        //результат надо подавать в AddOperation
        public Operation CreateOperationConst(string c)
        {
            Operation operation = new Operation(OperationType.ConcatC);
            operation.consts.Add(c);
            return operation;
        }

        public Operation CreateOperationVar(string v)
        {
            Operation operation = new Operation(OperationType.ConcatV);
            operation.variables.Add(v);
            return operation;
        }


        //variables - это словарь с значениями переменных, если он не подан,
        //то значит функция была вызвана для операции с константой
        public string Calculate(string s, Operation operation, Dictionary<string, string> variables = null)
        {
            if (variables == null)
                return s + operation.consts[0];
            else
                return s + variables[operation.variables[0]]; 
        }
    }

    //todo остальные декораторы
}
