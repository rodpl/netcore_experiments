using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;


namespace Slow
{
	public class Program
	{
		public static void Main()
		{
			var queue = new ConcurrentQueue<int>();

			var threadPool = new Thread[200];

			for (var i = 0; i < threadPool.Length; i++)
			{
				var thread = threadPool[i] = new Thread(RenegadeContextSwichingThreadProc);
				thread.Start(queue);
			}

			Console.ReadKey();

			Process.GetCurrentProcess().Kill();
		}

		// Renegade thread method
		private static void RenegadeContextSwichingThreadProc(object queueParameter)
		{
			var queue = (ConcurrentQueue<int>)queueParameter;

			while (true)
			{
				int queuedValue;

				if (queue.TryDequeue(out queuedValue))
				{
					Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId + " found: " + queuedValue);
				}

				Thread.Sleep(1);
			}
		}
	}
}
