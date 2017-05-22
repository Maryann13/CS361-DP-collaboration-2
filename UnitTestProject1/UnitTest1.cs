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
        public System.Collections.Generic.Dictionary<string, string> getTestDictonary()
        {
            System.Collections.Generic.Dictionary<string, string> TestData = new System.Collections.Generic.Dictionary<string, string>();
            TestData.Add("rand", "fail");
            TestData.Add("test", "my");
            TestData.Add("sf", "23");
            TestData.Add("sf6", "sf3");
            TestData.Add("ww", "23");
            TestData.Add("myprogram.us", "www.myprogram.us");
            TestData.Add("www.myprogram.us", "myprogram.us");
            return TestData;
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
            try
            {
                v2.Value = null;
                Assert.Fail();
            }
            catch (Exception e)
            {

            }
            try
            {
                Const g = new Const(null);
                Assert.Fail();
            }
            catch (Exception e) { }
            str = "3rggd";
            v2 = getConst(str);
            Assert.AreEqual(v2.Value, str);
            System.Collections.Generic.Dictionary<string, string> TestData = getTestDictonary();
            Assert.AreEqual(v2.Calculate(TestData),str);
            str="sr";
            v=getConst(str);
            v2=getConst(str);
            Assert.AreEqual(v.Calculate(TestData),str);
            Assert.AreEqual(v2.Calculate(TestData),str);
            Assert.AreEqual(v.Value,str);
            Assert.AreEqual(v2.Value,str);
        }
        [TestMethod]
        public void TestVariable()
        {
            string str = "1234";
            Var v = new Var(str);
            Assert.AreEqual(str, v.Value);
            try
            {
                Assert.AreEqual(str, v.Calculate(getTestDictonary()));
                Assert.Fail();
            }
            catch (Exception e) { }
        
            {
                var tmp = getTestDictonary();
                str = "b9";
                Var g = new Var(str);
                Assert.AreEqual(g.Value, str);
                str = "56";
                g.Value = str;
                tmp.Add(str, "666");
                str = "666";
                Assert.AreEqual(str, g.Calculate(tmp));
                Assert.AreNotEqual(str, g.Value);

            }
        }
        [TestMethod]
        public void TestCharsFreqRemove()
        {
            string str="qwqwertyuiopqwertyqwert";
            Const c = new Const(str);
            CharsFreqRemoveAdapter ad = new CharsFreqRemoveAdapter(c);
            string tmp = ad.Calculate();
            Assert.AreEqual(ad.Calculate(), "wwertywertywert");
        }
        [TestMethod]
        public void TestReplaceSpace()
        {
            ReplaceSpaceDecorator tmp = new ReplaceSpaceDecorator('4');
            Assert.AreEqual(tmp.Calculate(getTestDictonary()), 4);
        }
    }
}
