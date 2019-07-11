using System;
using ConsoleApp1.Banking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest2
    {
        [Test]
        public void TestMethod1()
        {
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;
            NUnit.Framework.Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }
    }
}
