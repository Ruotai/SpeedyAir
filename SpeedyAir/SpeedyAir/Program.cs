using SpeedyAir.Repository;
using SpeedyAir.Service;

namespace SpeedyAir;

internal class Program
{
    private static ScheduleService ScheduleService { get; set; }
    private static OrderService OrderService { get; set; }

    private static ScheduleRepository ScheduleRepository { get; set; }
    private static OrderRepository OrderRepository { get; set; }

    static void Main(string[] args)
    {
        bool exit = false;
        Initialize();

        Console.WriteLine("Welcome to SpeedyAir");
        Console.WriteLine("Select your action:");
        Console.WriteLine("1. Load flight schedule;");
        Console.WriteLine("2. Display loaded flight schedule;");
        Console.WriteLine("3. Load orders;");
        Console.WriteLine("4. Display scheduled orders;");
        Console.WriteLine("0. Exit;");

        while (!exit)
        {
            var response = Console.ReadLine();
            var option = int.Parse(response);
            
            if (option == 0)
            {
                exit = true;
            }
            else if (option == 1)
            {
                ScheduleService.LoadFlightSchedule("../../../../../flight-schedule.txt");
                Console.WriteLine("Flight schedule loaded");
            }
            else if (option == 2)
            {
                var schedule = ScheduleService.PrintFlightSchedule();
                Console.WriteLine(schedule);
            }
            else if (option == 3)
            {
                OrderService.LoadOrders("../../../../../coding-assigment-orders.json");
                Console.WriteLine("Orders loaded");
            }
            else if (option == 4)
            {
                var orders = OrderService.PrintOrders();
                Console.WriteLine(orders);
            }
        }
    }

    private static void Initialize()
    {
        ScheduleRepository = new ScheduleRepository();
        OrderRepository = new OrderRepository();

        ScheduleService = new ScheduleService(ScheduleRepository);
        OrderService = new OrderService(OrderRepository, ScheduleRepository);
    }
}