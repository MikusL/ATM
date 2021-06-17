using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ATM.Tests
{
    [TestClass]
    public class AtMachineTests
    {
        private IAtMachine _atMachine;

        [TestInitialize]
        public void Initialize()
        {
            _atMachine = new AtMachine("Manufacturer","Serial-Number");
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
