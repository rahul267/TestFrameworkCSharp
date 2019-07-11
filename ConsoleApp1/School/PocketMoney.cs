using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.School
{
    class PocketMoney
    {
        public float amount { get; set; }

        public bool isEleigibleforPocketMoney(int date)
        {
            if (date > 15)
                return false;
            else
                return true;

        }

        public float recharegPocketMoney()
        {
            if (this.amount == 0)
                amount = amount + 100;
            return amount;
        }

    }
}
