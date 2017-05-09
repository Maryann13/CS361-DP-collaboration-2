using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    //просто абстрактный декоратор, ничего интересного)))))0000))))
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

    //следующие далее конкретные декораторы можно(и может быть даже нужно) реализовать как одиночки
    //...
    //...
    //...
    //но мне лень(:

    //Удаление начальных и конечных пробельных символов ^ F.
    public class RemoveSpaces : OperationDecorator
    {

        public Operation CreateOperation()
        {
            Operation operation = new Operation();
            operation.typename = "RemoveSpaces";
            return operation;
        }
        public string Calculate(string s)
        {
            if (s[0] == ' ')
                s.Remove(0, 1);
            if (s[s.Length - 1] == ' ')
                s.Remove(s.Length - 1, 1);
            return s;
        }
    }

    //Конкатенация строк F && c и F && v.

    public class Concat : OperationDecorator
    {
        //это методы для создания операции конкатенации с константой или же с переменной
        //результат надо подавать в AddOperation
        public Operation CreateOperationConst(string c)
        {
            Operation operation = new Operation();
            operation.typename = "RemoveSpacesC";
            operation.consts.Add(c);
            return operation;
        }

        public Operation CreateOperationVar(string v)
        {
            Operation operation = new Operation();
            operation.typename = "RemoveSpacesV";
            operation.variables.Add(v);
            return operation;
        }


        //variables - это словарь с значениями переменных, если он не подан, то значит функция была вызвана для операции с константой
        public string Calculate(string s, Operation operation, Dictionary<string, string> variables = null)
        {
            if (variables == null)
                return s + operation.consts[0];
            else
            {
                string value = variables[operation.variables[0]];
                return s + value; 
            }
        }
    }

    //todo остальные декораторы
}
