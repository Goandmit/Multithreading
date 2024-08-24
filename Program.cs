int threadNumber = 0;
object access = new object();

string AssignName()
{
    threadNumber++;
    string name = $"Поток {threadNumber}";

    return name;
}

void PrintStart(string name)
{
    lock (access)
    {
        Console.WriteLine($"Начал работу: {name}");
        Console.WriteLine();
    }
}

void PrintWork(string name)
{
    lock (access)
    {
        Console.WriteLine($"Работает: {name}");
        Console.WriteLine();
    }
}

void PrintEnd(string name)
{
    lock (access)
    {
        Console.WriteLine($"Закончил работу: {name}");
        Console.WriteLine();
    }
}

void Print(object? o)
{
    int sleepTime;
    string name = $"{Thread.CurrentThread.Name}";

    if (name == ".NET ThreadPool Worker")
    {
        name = "Поток с асинхронной задачей";
    }

    try
    {
        sleepTime = (o != null) ? sleepTime = Convert.ToInt32(o) : sleepTime = 0;
    }
    catch
    {
        sleepTime = 0;
        Console.WriteLine($"Некорректный входной параметр: {name}");
    }

    PrintStart(name);   

    for (int i = 0; i < 10; i++)
    {
        PrintWork(name);

        Thread.Sleep(sleepTime);
    }

    PrintEnd(name);
}

async Task PrintAsync(object? o)
{   
    await Task.Run(() => Print(o));
}

Thread threadMain = Thread.CurrentThread;
threadMain.Name = "Основной поток";
PrintStart($"{threadMain.Name}");

ParameterizedThreadStart pts = new ParameterizedThreadStart(Print);

Thread threadBackground = new Thread(pts);
threadBackground.Name = "Фоновый поток";
threadBackground.IsBackground = true;
threadBackground.Start(6000);

Thread thread1 = new Thread(pts);
thread1.Name = AssignName();
thread1.Start(1000);

Thread thread2 = new Thread(pts);
thread2.Name = AssignName();
thread2.Start(2000);

Thread thread3 = new Thread(pts);
thread3.Name = AssignName();
thread3.Start(3000);

await PrintAsync(4000);

PrintEnd($"{threadMain.Name}");
Console.ReadKey();
