using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Абстрактный класс формулы
    public abstract class Formula
    {
        /// <summary>
        /// Вычислить значение формулы
        /// </summary>
        /// <param name="variables">Словарь со значениями переменных</param>
        public abstract string Calculate(Dictionary<string, string> variables = null);
    }
    
    public class Empty : Formula
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return "";
        }
    }
}
