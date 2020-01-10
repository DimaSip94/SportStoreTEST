using SportsStore.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public interface IQueueRequests
    {
        void SaveFile(string text);
    }

    public class QueueRequests : IQueueRequests
    {
        private static ConcurrentQueue<string> queueRequests;
        private static QueueRequests instance;
        public static QueueRequests getInstance()
        {
            if (instance == null)
                instance = new QueueRequests();
            return instance;
        }

        private static bool isStarted = false;

        public void SaveFile(string text)
        {
            if (queueRequests == null) queueRequests = new ConcurrentQueue<string>();
            queueRequests.Enqueue(text);
            if (!isStarted)
            {
                ExecProcessAsync();
            }
        }

        private static async void ExecProcessAsync()
        {
            isStarted = true;
            bool res = await ProccessQueue();
            isStarted = false;
        }

        private async static Task<bool> ProccessQueue()
        {
           bool res = await Task.Run(() => {
               try
               {
                   while (queueRequests.Count > 0)
                   {
                       string q;
                       if (queueRequests.TryDequeue(out q))
                       {
                           workFile(q);
                       }
                       else
                       {
                           Thread.Sleep(5000);
                       }
                   }
                   return true;
               }
               catch (Exception)
               {
                   return false;
               }
            });
            return res;
        }

        private static void workFile(string text)
        {
            Thread.Sleep(5000);
            using (StreamWriter writer = new StreamWriter(text + ".txt", false))
            {
                writer.Write(text);
            }
            
        }
    }
}
