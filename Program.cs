﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace threaddemo
{
    class Program
    {
        const int THREAD_COUNT = 4;
        private bool[] canStop = new bool[THREAD_COUNT];

        static void Main(string[] args)
        {
            new Program().Start();
        }
        void Start()
        {
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                int index = i;
                new Thread(() => Calculator(index)).Start();
            }

            new Thread(Stoper).Start();
        }
        void Calculator(int index)
        {
            long sum = 0;
            long count = 0;
            long step = index + 1;
            long current = 0;

            while (!Volatile.Read(ref canStop[index]))
            {
                sum += current;
                current += step;
                count++;
            }

            Console.WriteLine(
                $"[Thread {index + 1}] Sum: {sum}, Count: {count}, Step: {step}"
            );
        }
        void Stoper()
        {
            Random rand = new Random();
            var stopSchedule = new List<(int Index, int Delay)>();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                int delay = rand.Next(1, 11) * 1000;
                Console.WriteLine($"Initial plan: Thread {i + 1} will stop at {delay}ms");
                stopSchedule.Add((i, delay));
            }

            var sortedSchedule = stopSchedule.OrderBy(x => x.Delay).ToList();

            int elapsed = 0;
            foreach (var item in sortedSchedule)
            {
                int waitTime = item.Delay - elapsed;
                if (waitTime > 0)
                {
                    Thread.Sleep(waitTime);
                }

                Volatile.Write(ref canStop[item.Index], true);
                elapsed += waitTime;
                
                Console.WriteLine($"---> Manager sent permission to Thread {item.Index + 1} at {item.Delay}ms");
            }
        }
    }
}