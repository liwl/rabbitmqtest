using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class VTO
    {
        public string key { get; set; }
        public string value { get; set; }

        public long ticks { get; set; }


    }

    public class TimerQueue
    {
        public Action<VTO> action { get; set; }
        private ConcurrentDictionary<string, Timer> _timerCache;
        private object _locker;

        public TimerQueue()
        {
            _locker = new object();
            _timerCache = new ConcurrentDictionary<string, Timer>();
        }


        public void Push(VTO value)
        {
            lock (_locker)
            {
                Timer timer = new Timer(CheckStatus, value, value.ticks, 0);
                _timerCache.TryAdd(value.key, timer);
            }
        

        }


        public void CheckStatus(object obj)
        {
            lock (_locker)
            {
                //删除定时器
                VTO vto = (VTO) obj;

                Timer timer;
                if (_timerCache.TryGetValue(vto.key, out timer))
                {
                    _timerCache.TryRemove(vto.key, out timer);
                    timer.Dispose();
                }
                //if (action != null)
                //{
                //    action.Invoke(vto);
                //}
                Console.WriteLine("还有" + _timerCache.Count);
                Console.WriteLine("执行" + DateTime.Now);
                Console.WriteLine(vto.key);
            }
        }
    }
}
