using ConsoleApp1.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum section
    {
        A, B , C  ,D
    }

    public class Student
    {
        public string name { get; set; }
        public int Id { get; set; }
        public section StudentSection { get; set; }
        //public PocketMoney pocketMoney = null;

        public section AllocatetSection()
        {
            
            return section.A;
           
        }


    }
}
