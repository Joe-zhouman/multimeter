using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using log4net;
using log4net.Core;

namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            Father person = new Son();
            var t = person as Son;
            SetI(ref t);
            Console.WriteLine(person.GetType());
        }
        public static void SetI(ref Son son)
        {
            son.sex = "male";
        }

    }
    
    class Father {
        
    }

    class Son:Father {
        public string sex;
    }
    class Daughter : Father
    {
        public string sex;
    }
    
}