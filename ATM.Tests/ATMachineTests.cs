using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ATM.Tests
{
    [TestClass]
    public class ATMachineTests
    {
        private IATMachine _ATMachine;

        [TestInitialize]
        public void Initialize()
        {
            _ATMachine = new ATMachine("Manufacturer","Serial-Number");
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
