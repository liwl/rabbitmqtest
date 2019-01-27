using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TimerQueue timerQueue = new TimerQueue();
            timerQueue.action = cc;
            for (int i = 0; i < 100; i++)
            {
                timerQueue.Push(new VTO()
                {
                    key = "第"+i+"个任务",
                    value = "任务值",
                    ticks = 1000 * (new Random()).Next(1,100),
                });
                Console.WriteLine("第"+i+"添加时间:" + DateTime.Now);
            }
          
            //timerQueue.Push(new VTO()
            //{
            //    key = "第3个任务",
            //    value = "任务值",
            //    ticks = 1000 * 7,

            //});
            //timerQueue.Push(new VTO()
            //{
            //    key = "第2个任务",
            //    value = "任务值",
            //    ticks = 1000 * 10,

            //});
         
            Console.ReadLine();



        }

        public static void cc(VTO vto)
        {
            Console.WriteLine("回调"+vto.key);
        }
    }
}
