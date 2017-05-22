using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
        }
        public Core.Const getConst(string str){
            return new Core.Const(str);
        }
        [TestMethod]
        public void TestConst()
        {
            string str = "df";
            Const v = getConst(str);
            Assert.AreEqual(str, v.Value);
            str = "123";
            v.Value = str;
            Assert.AreEqual(v.Value, str);
            Const v2 = getConst(str);
            Assert.AreEqual(v2.Value, v.Value);
            str = "32";
            Assert.AreEqual(v2.Value, v.Value);
            Assert.AreNotEqual(v2.Value, str);

        }
    }
}
