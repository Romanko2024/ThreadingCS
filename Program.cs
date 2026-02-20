﻿using System.Threading;
using System;

namespace threaddemo
{
    class Program
    {
        const int THREAD_COUNT = 20;
        private bool[] canStop = new bool[THREAD_COUNT];

        static void Main(string[] args)
        {
            (new Program()).Start();
        }
        void Start()
        {
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                int index = i;
                Thread t = new Thread(() => Calculator(index));
                t.Start();
            }

            (new Thread(Stoper)).Start();
        }
        void Calculator(int threadNumber)
        {
            long sum = 0;
            long count = 0;
            long step = threadNumber + 1;
            long current = 0;

            do
            {
                sum += current;
                current += step;
                count++;

            } while (!canStop[threadNumber]);

            Console.WriteLine(
                $"Thread {threadNumber + 1}: Sum = {sum}, Count = {count}, Step = {step}"
            );
        }
        public void Stoper()
        {
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                Thread.Sleep(3000);
                canStop[i] = true;
            }
        }
    }
}
