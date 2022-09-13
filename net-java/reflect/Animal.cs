using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace com.sunlw.net.reflect
{
    public class Animal<T> 
        where T : IAnimal
    {
        public string name { get; set; }
        public int? age { get; set; }
        public Animal(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Say()
        {
            var t = typeof(T);
            return $"我是泛型{t.Name},我的名字{name},年龄{(age == null ? "未知" : age.ToString())}";
        }
    }
}
