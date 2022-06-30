// implement concurrent queue in c#

public class Car {
    public string CarBrand{ get;set;}
}

//public variable
public ConcurrentQueue<Car> queue = new ConcurrentQueue<Car>();

//enqueue
ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
{
    var car = new Car(){ CarBrand = "Apple" };
    queue.Enqueue(car);
}));

//dequeue
//can add thread count to control the maxinum number of items being executed in a queue
new Thread(() =>
{
    Thread.CurrentThread.IsBackground = true;
    var threadCount = 8;
    while (true)
    {
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < threadCount; i++)
        {
            var carToBeProcessed = new Car();
            if (queue.TryDequeue(out carToBeProcessed))
            {
                var task = Task.Run(() =>
                {
                    // function to process with parameter carToBeProcessed
                    processCar(carToBeProcessed);
                });
                tasks.Add(task);
            }
            else
            {
                break;
            }
            
        }
        if (tasks.Count() == 0)
        {
            Thread.Sleep(10000);
        }
        else
        {
            //Task.WaitAll(tasks.ToArray());
            var taskList = Task.Factory.ContinueWhenAll(tasks.ToArray(), (ts) =>
            {
            });
            taskList.Wait();
        }
    }

}).Start();


