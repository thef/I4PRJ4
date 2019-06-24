using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace myWebApp.Hubs.CustomerQueue
{
    public class Customer
    {
        public Customer(string name, string connectionId)
        {
            name_ = name;
            connectionID_ = connectionId;
        }

        public string connectionID_;
        public string name_;
    }

    public class CustomerQueue
    {
        private static Mutex mut_ = new Mutex();
        private Queue<Customer> cq_;

        // TODO: set restrictions on maxsize.
        private int maxSize_ = 0;

        public CustomerQueue(int maxSize)
        {
            maxSize_ = maxSize;
            cq_ = new Queue<Customer>();
        }

        public void Enqueue(string name, string connectionId)
        {
            Customer newCustomer = new Customer(name, connectionId);

            // Synchronization
            mut_.WaitOne();
            cq_.Enqueue(newCustomer);
            mut_.ReleaseMutex();
        }

        public Customer Dequeue()
        {
            mut_.WaitOne();
            Customer nextCustomer = cq_.Dequeue();
            mut_.ReleaseMutex();
            return nextCustomer;
        }
    }
}
