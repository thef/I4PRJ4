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
        public Customer(string name, string groupName, string connectionId)
        {
            name_ = name;
            groupName_ = groupName;
            connectionID_ = connectionId;
        }

        private string connectionID_;
        private string name_;
        private string groupName_;
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
        }

        public void Enqueue(string name, string groupName, string connectionId)
        {

            Customer newCustomer = new Customer(name, groupName, connectionId);

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
