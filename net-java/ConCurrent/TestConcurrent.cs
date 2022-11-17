using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sunlw.net.ConCurrent
{
    public class TestConcurrent
    {
        public ConcurrentDictionary<int, DaRen> dic { get; set; }
    }

    public class DaRen
    {
        public DaRen(int no, int id, string name)
        {
            No = no;
            ren = new Ren();
            ren.Id = id;
            ren.Name = name;
        }

        public int No { get; set; }
        public Ren ren { get; set; }
    }

    public class Ren
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}