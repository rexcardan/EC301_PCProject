using ESAPIX.Constraints;
using ESAPX_StarterUI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESAPX_StarterUI.ViewModels.Tests
{
    [TestClass()]
    public class CTDateConstraintTests
    {
        [TestMethod()]
        public void CTDateConstraintFailsCorrectly()
        {
            //Arrange
            var ctDate = DateTime.Now.AddDays(-61);

            //Act
            var actual = new CTDateConstraint().ConstraintCTDate(ctDate).ResultType;

            //Assert
            var expected = ResultType.ACTION_LEVEL_3;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CTDateConstraintPassesCorrectly()
        {
            //Arrange
            var ctDate = DateTime.Now.AddDays(-59);

            //Act
            var actual = new CTDateConstraint().ConstraintCTDate(ctDate).ResultType;

            //Assert
            var expected = ResultType.PASSED;
            Assert.AreEqual(expected, actual);
        }
    }
}