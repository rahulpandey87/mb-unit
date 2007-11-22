using System;
using System.Collections;

using MbUnit.Framework;

namespace MbUnit.Demo.Haacked
{
    public class MyClass
    {
        Hashtable _values = new Hashtable();

        public MyClass()
        {
            _values.Add("keyOne", 1);
            _values.Add("keyTwo", 7);
            _values.Add("keyThree", 10);
        }

        public int GetValue(string key)
        {
            return (int)_values[key];
        }

        public int SumIt(string[] keys)
        {
            int total = 0;
            foreach (string key in keys)
            {
                total += (int)_values[key];
                _values[key] = total;
                //Maybe we do some other
                //interesting things here.
            }
            return total;
        }
    }

    [TestFixture]
    public class MyClassFixture
    {
        [Factory]
        public string[] GetKeys()
        {
            string[] keys = new string[3];
            keys[0] = "keyOne";
            keys[1] = "keyTwo";
            keys[2] = "keyThree";

            return keys;
        }

        [CombinatorialTest]
        public void Sumit(
            [UsingFactories("GetKeys")] string key1,
            [UsingFactories("GetKeys")] string key2
            )
        {
            MyClass mine = new MyClass();
            string[] keys = { key1, key2 };
            int sum = mine.GetValue(key1) + mine.GetValue(key2);
            Assert.AreEqual(sum, mine.SumIt(keys));
        }



        [CombinatorialTest]
        public void Sumit(
            [UsingFactories("GetKeys")] string key1,
            [UsingFactories("GetKeys")] string key2,
            [UsingFactories("GetKeys")] string key3
           )
        {
            MyClass mine = new MyClass();
            string[] keys = { key1, key2, key3};
            int sum = 
                mine.GetValue(key1) 
                + mine.GetValue(key2) 
                + mine.GetValue(key3);
            Assert.AreEqual(sum, mine.SumIt(keys));
        }
    }
}