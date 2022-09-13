using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace com.sunlw.net.reflect
{
    public class Cat : IAnimal
    {
        public string name { get; set; }
        public int? age { get; set; }
        public Cat(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public Cat()
        {
            this.name = "无名氏";
        }

        public string Say()
        {
            return $"我的名字{name},年龄{(age == null ? "未知" : age.ToString())}";
        }

        public async Task<string> SayAsync()
        {
            //实际的异步方法
            await Task.Delay(10);
            return await Task.FromResult($"我是异步方法调用的,我的名字{name},年龄{(age == null ? "未知" : age.ToString())}");
        }
    }

}
