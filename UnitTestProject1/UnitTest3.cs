using System;
using ConsoleApp1;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class StudentTest
    {
        Student Rahul  ;

        [SetUp]
        public void  initializeStudent()
        {
             Rahul = new Student() ;
            Rahul.name = "Rahul";
            Rahul.Id = 1;
            Rahul.StudentSection = section.A ;
           }

        [Test]
        public void isNameValid( )
        {
            Assert.AreEqual(Rahul.name, "Rahul", "Names are not equal"); 
        }

        [Test]
        public void isSectionValid()
        {
            Assert.AreEqual(Rahul.AllocatetSection(), section.A, "Section  are not equal");
        }
    }
}
