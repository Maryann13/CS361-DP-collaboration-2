using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class Component
    {
        public abstract void AddOperation(Operation operation);
        public List<Operation> operations { get; set; }
    }

    //наша главная формула
    public class Formula : Component
    {
        //инициализация формулы константой
        public void InitWithConst(string c)
        {
            if (operations.Count == 0)
            {
                Operation init = new Operation(OperationType.Const);
                init.consts.Add(c);
                operations.Add(init);
            }
        }
        //инициализация формулы переменной
        public void InitWithVar(string var)
        {
            if (operations.Count == 0)
            {
                Operation init = new Operation(OperationType.Var);
                init.consts.Add(var);
                operations.Add(init);
            }
        }
        public override void AddOperation(Operation operation)
        {
            //не делать ничего, вся работа будет в AddOperation у декоратора конкретной операции
        }

        public string Calculate()
        {
            //todo
            //для каждой из операций будет вызываться функция Calculate из соответствующего декоратора
            //кроме первой(которая задается инициализаций const или var)
            return "";
        }
    }
}
