// See https://aka.ms/new-console-template for more information
using com.sunlw.net.ConCurrent;
using com.sunlw.net.Myevent;
using com.sunlw.net.MyTask;
using com.sunlw.net.reflect;
using com.sunlw.net.Tcp;
using System.Runtime.CompilerServices;
using System.Text;

Console.WriteLine("Hello, World!");

//#region 反射运行测试代码

////反射对象
//ReflectDemo.TestObj();
////反射方法
//ReflectDemo.TestMethond();
//await ReflectDemo.TestAsyncMethond();

//#endregion 反射运行测试代码

//#region 事件测试代码

///*
// *人关注车速,当车速变化会通知所有关注车速的人
// *人也取消关注车速,车速变化后不会通知该人
// */
//Car car = new Car("奔驰");
//var driveCar = new DriveCar(car);
//Person person1 = new Person(driveCar, "张三");
//Person person2 = new Person(driveCar, "李四");
//person1.CareCarSpeed();
//person2.CareCarSpeed();
//driveCar.ChangeSpeed(100);
//driveCar.ChangeSpeed(200);
//person1.NotCareCarSpeed();
//driveCar.ChangeSpeed(speed: 300);
//person1.CareCarSpeed();
//driveCar.ChangeSpeed(speed: 400);

//person1.CareCarCarrypassenger();
//driveCar.ChangeCarrypassenger(true);

//#endregion 事件测试代码

//#region 事件测试代码2

//CarSpeed0 carSpeed0 = new CarSpeed0();
//carSpeed0.Callback += (obj, a) =>
//{
//    Console.WriteLine($"{obj!.ToString()}我要加速了,{a}");
//};
//Person2 person20 = new Person2("王五");
//carSpeed0.Callback += person20.Warning!;

//carSpeed0.Start(new object { }, 500);

//#endregion 事件测试代码2

//#region 事件包装类

///*
//* 为了扩展用
//*/
//CarSpeed carSpeed = new CarSpeed();
//carSpeed.Callback += (obj, a) =>
//{
//    Console.WriteLine($"{obj!.ToString()}我要加速了,{a}");
//};
//Person2 person21 = new Person2("赵六");
//carSpeed.Callback += person21.Warning!;

//carSpeed.Start(new object { }, 200);

//#endregion 事件包装类

//#region struct

//Book book = new();
//book.name = "name";
//var auth = new Auther();
//book.auth = auth;
//auth.name = "张三";
//book.auth = auth;
//Console.WriteLine(book.name);
//Console.WriteLine(book.mark);
//Console.WriteLine(book.page);
//Console.WriteLine(book.auth.name);
//auth.name = "李四";
//Console.WriteLine(book.auth.name);//这个地方是张三,不是李四 struct是值类型

//#endregion struct

//#region 编码测试

///*
// * ASCII码是通用的,UTF8等是在基础上进行了增加
// * A你B9 对应的字节
// * 65 228 189 160 66 57
// * 65 ->A
// * 228 189 160 ->你 (一个汉字3个字节)
// * 66 ->B
// * 57 ->9
// */
//var b_1 = Encoding.UTF8.GetBytes("A你B9");
//var b_2 = Encoding.UTF8.GetString(b_1, 0, b_1.Length);
//var b_3 = Encoding.ASCII.GetString(b_1, 0, b_1.Length);//A???B9 没有的ASCII会解析成?
//var b_4 = (byte)'A'; //可以直接转字节 byte->65
//var b_5 = (byte)125; //byte->125

//#endregion 编码测试

#region 测试Stream

//StreamTest.TestStream();
//await StreamTest.Testpipe();

//await StreamTest.TestTask();

#endregion 测试Stream

#region 测试manualReset

//ManualReset manualReset = new ManualReset();

//manualReset.Start();
////await Task.Delay(30000);

////manualReset.Suspend();
////manualReset.Suspend();

////await Task.Delay(10000);

//manualReset.Continue();
////manualReset.Continue();
////manualReset.Suspend();

#endregion 测试manualReset

//MyValueTaskTest myValueTaskTest = new MyValueTaskTest();

//await myValueTaskTest.Test();

#region 测试并发字典

//TestConcurrent testConcurrent = new TestConcurrent();

//var ren1 = new DaRen(1, 10, "张三");
//var ren2 = new DaRen(2, 12, "李四");
//testConcurrent.dic = new System.Collections.Concurrent.ConcurrentDictionary<int, DaRen>();
////testConcurrent.dic.AddOrUpdate(1, ren1, (k, v) =>
////{
////    return ren1;
////});
//var aa = testConcurrent.dic.GetOrAdd(1, ren1);
//testConcurrent.dic.AddOrUpdate(2, ren2, (k, v) => v);
//Task.Factory.StartNew(async () =>
//{
//    while (true)
//    {
//        aa.ren.Name = "张三";
//        await Task.Delay(2000);
//        Console.WriteLine("线程1:" + aa.ren.Name);
//    }
//});
//Task.Factory.StartNew(async () =>
//{
//    while (true)
//    {
//        aa.ren.Name = "李四";
//        await Task.Delay(2000);
//        Console.WriteLine("线程2:" + aa.ren.Name);
//    }
//});

//Task.Factory.StartNew(async () =>
//{
//    while (true)
//    {
//        Console.WriteLine("线程3:" + aa.ren.Name);
//        await Task.Delay(2000);
//    }
//});

//var ren3 = new DaRen(1, 101, "张三1");
////testConcurrent.dic.AddOrUpdate(1, ren3, (k, v) =>
////{
////    var oldV = v;
////    oldV.ren.Name = "张三修改";
////    return oldV;
////});

//var ren4 = ren3;
//ren4.ren.Name = "111";

//var aa = testConcurrent.dic.TryUpdate(1, ren3, ren4);
//var v = testConcurrent.dic.AddOrUpdate(10, (k) => ren3, (k, v) => v);
//ren3.No = 999;

//testConcurrent.dic.TryGetValue(10, out DaRen daRen);

//var v2 = testConcurrent.dic.AddOrUpdate(10, (k) => ren3, (k, v) =>
//{
//    v.No = 119;
//    return v;
//}
//);

//var b = object.Equals(v, ren3);
//var b2 = object.Equals(v2, ren3);

#endregion 测试并发字典

Console.ReadLine();