using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Console = System.Console;

namespace Slow
{
	public class Program
	{
		public static void Main()
		{
			var queue = new ConcurrentQueue<int>(Enumerable.Range(0, 10000));

			for (var i = 0; i < Environment.ProcessorCount; i++)
			{
                System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolWorkItemMethod, queue);
			}

			Console.ReadKey();
		}

		// Renegade thread method
		private static void ThreadPoolWorkItemMethod(object queueObject)
		{
			var queue = (ConcurrentQueue<int>)queueObject;

            int value;
            if (queue.TryDequeue(out value)) {
                Console.WriteLine("Thread pool thread " + Thread.CurrentThread.ManagedThreadId + " dequeued value " + value);
                System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolWorkItemMethod, queue);
            }
		}
	}
}
