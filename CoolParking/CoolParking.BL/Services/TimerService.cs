using System;
using System.Timers;
using CoolParking.BL.Interfaces;

namespace CoolParking.BL.Services
{
    public class TimerService : ITimerService
    {

        private Timer timerObj;
        public double Interval
        {
            get
            {
                return timerObj.Interval / 1000;
            }
            set
            {
                timerObj.Interval = value * 1000;
            }
        }

        public event ElapsedEventHandler Elapsed { add { timerObj.Elapsed += value; } remove { timerObj.Elapsed -= value; } }

        public TimerService()
        {
            timerObj = new Timer();
        }

        public void Dispose()
        {
            timerObj.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            timerObj.Start();
        }

        public void Stop()
        {
            timerObj.Stop();
        }
    }
}