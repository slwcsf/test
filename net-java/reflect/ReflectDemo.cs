using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.reflect
{
    /// <summary>
    /// 反射演示
    /// </summary>
    public  static class ReflectDemo
    {

        private const string CatClassName = "com.sunlw.net.reflect.Cat";

        /// <summary>
        /// 反射方法
        /// </summary>
        public static void TestObj(){

            //Assembly 无参构造
            Assembly asm = Assembly.GetExecutingAssembly(); 
            var obj =  asm.CreateInstance(CatClassName, true);
            var cat1 = (Cat)obj!;
            Console.WriteLine(cat1.Say());
            //1.Assembly 有参构造
            var parameters = new object[] { "Tom",3};
            var obj2 = asm.CreateInstance(CatClassName, true, BindingFlags.Default, null, parameters, null, null);
            var cat2 = (Cat)obj2!;
            Console.WriteLine(cat2.Say());

            //2.Activator CreateInstance Unwrap构造
            var objectHandle = Activator.CreateInstance(null, CatClassName);
            var obj3 = objectHandle!.Unwrap();
            var cat3 = (Cat)obj3!;
            Console.WriteLine(cat3.Say());

            //3. GetConstructors构造
            var catType = typeof(Cat);
            var constructors = catType.GetConstructors();
            var constructor1 =constructors.FirstOrDefault(p => p.GetParameters().Length == 0);
            var constructor2 =constructors.FirstOrDefault(p => p.GetParameters().Length == 2);
            var obj4 =constructor1!.Invoke(null);
            var cat4 = (Cat)obj4!;
            Console.WriteLine(cat4.Say());
            var parameters2 = new object[] { "Lili", 5 };
            var obj5 = constructor2!.Invoke(parameters2);
            var cat5 = (Cat)obj5!;
            Console.WriteLine(cat5.Say());

            //4. 泛型对象构造
            var AnimalType = typeof(Animal<IAnimal>);
            var xx = AnimalType.GetGenericTypeDefinition();
            AnimalType = xx.MakeGenericType(typeof(Dog));
            var constructors2 = AnimalType.GetConstructors();
            var constructor3 = constructors2.FirstOrDefault(p => p.GetParameters().Length == 2);
            var parameters3 = new object[] { "jack", 8 };
            var obj6 = constructor3!.Invoke(parameters3);
            var cat6 = (Animal<Dog>)obj6!;
            Console.WriteLine(cat6.Say());


            //MethodInfo
            var catType2 = Type.GetType(CatClassName)!;
            var objectHandle2 = Activator.CreateInstance(catType2);
            var m1 =catType2.GetMethod("HI");
            var m2 =catType2.GetMethod("Say");
            m1!.Invoke(null, null);
            m2!.Invoke(objectHandle2, null);

        }


        public static void TestMethond()
        {
            //反射方法
            Type t = typeof(Cat);
            Cat cat = new Cat("abc", 10); 
            string  result = t.InvokeMember("Say", BindingFlags.InvokeMethod, null, cat!, null) as string;
            Console.WriteLine(result);
        }

        public static async  Task TestAsyncMethond()
        {
            //反射方法
            Type t = typeof(Cat);
            Cat cat = new Cat("abc", 10);
            //反射异步方法
            string result = await (t.InvokeMember("SayAsync", BindingFlags.InvokeMethod, null, cat!, null) as Task<string>);
            Console.WriteLine(result);
        }
    }
}
