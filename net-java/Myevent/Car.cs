using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.Myevent
{
    public class Car
    {
        // public delegate string Myd(int speed);
        public string brand { get; set; }

        public Car(string brand)
        {
            this.brand = brand;
        }
    }

    public class DriveCar
    {
        public int speed;
        public Car car;

        public DriveCar(Car car)
        {
            this.car = car;
        }

        public event Func<int, string>? speedChangeEvent;

        public event EventHandler<bool> carrypassengerEventHanler;

        //{
        //    add
        //    {
        //        value!.Invoke(this, true);
        //        Console.WriteLine($"这是个啥");
        //    }
        //    remove { }
        //}

        public void ChangeSpeed(int speed)
        {
            Console.WriteLine($"车:我开{car.brand}车的速度是{speed}");
            if (speedChangeEvent != null)
            {
                speedChangeEvent!.Invoke(speed);
            }
        }

        public void ChangeCarrypassenger(bool isCarrypassenger)
        {
            Console.WriteLine($"车:我开{car.brand}车要拉人了");
            carrypassengerEventHanler!.Invoke(this, isCarrypassenger);
        }
    }

    public class Person
    {
        public string name { get; set; }
        private DriveCar carSpeed;

        public Person(DriveCar carSpeed, string name)
        {
            this.carSpeed = carSpeed;
            this.name = name;
            //this.carSpeed.speedChangeEvent += Warning;
        }

        public void NotCareCarSpeed()
        {
            Console.WriteLine($"{name}:我不关注车速了");
            this.carSpeed.speedChangeEvent -= Warning;
        }

        public void CareCarSpeed()
        {
            Console.WriteLine($"{name}:我要关注车速");
            this.carSpeed.speedChangeEvent += Warning;
        }

        public string Warning(int speed)
        {
            Console.WriteLine($"{name}:这个车的速度是{speed},我要注意了");
            return $"车的速度是{speed},我要注意了";
        }

        public void CareCarCarrypassenger()
        {
            Console.WriteLine($"{name}:我要关注车是不是拉人了");
            this.carSpeed.carrypassengerEventHanler += CarCarrypassenger;
        }

        public void CarCarrypassenger(object? sender, bool e)
        {
            Console.WriteLine($"{sender!.ToString()}拉人了?={e}");
        }
    }

    public class Person2
    {
        public string name { get; set; }

        public Person2(string name)
        {
            this.name = name;
            //this.carSpeed.speedChangeEvent += Warning;
        }

        public void Warning(object sender, int speed)
        {
            Console.WriteLine($"{sender},{name}:这个车的速度是{speed},我要注意了");
        }
    }

    public struct CarSpeedWapper<T>
    {
        private event EventHandler<T>? _event;

        private Delegate[]? _handlers;

        public void AddHandler(EventHandler<T>? handler)
        {
            _event += handler;
            _handlers = null;
        }

        public void RemoveHandler(EventHandler<T>? handler)
        {
            _event -= handler;
            _handlers = null;
        }

        public void Invoke(object sender, T parameter)
        {
            var handlers = _handlers;
            if (handlers is null)
            {
                handlers = _event?.GetInvocationList();
                if (handlers is null)
                {
                    return;
                }

                _handlers = handlers;
            }
            foreach (EventHandler<T> action in handlers)
            {
                try
                {
                    action(sender, parameter);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }

    public class CarSpeed
    {
        public event EventHandler<int> Callback
        {
            add => _callbackWrapper.AddHandler(value);
            remove => _callbackWrapper.RemoveHandler(value);
        }

        private CarSpeedWapper<int> _callbackWrapper;

        public void Start(object sender, int speed)
        {
            _callbackWrapper.Invoke(sender, speed);
        }
    }

    public class CarSpeed0
    {
        public event EventHandler<int> Callback;

        public void Start(object sender, int speed)
        {
            Callback.Invoke(sender, speed);
        }
    }

    public struct Book
    {
        public string name { get; set; }
        public string mark { get; set; }
        public int page { get; set; }

        public Auther auth { get; set; }
    }

    public struct Auther
    {
        public string name { get; set; }
        public int age { get; set; }
    }
}