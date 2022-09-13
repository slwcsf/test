// See https://aka.ms/new-console-template for more information
using com.sunlw.net.reflect;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        //反射对象
        ReflectDemo.TestObj();
        //反射方法
        ReflectDemo.TestMethond();
        await ReflectDemo.TestAsyncMethond();
    }
}