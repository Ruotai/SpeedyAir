using Newtonsoft.Json;
using SpeedyAir.Model;
using SpeedyAir.Repository;
using System.Text;

namespace SpeedyAir.Service
{
    public class OrderService
    {
        private IOrderRepository _orderRepository;
        private IScheduleRepository _scheduleRepository;

        public OrderService(IOrderRepository orderRepository, IScheduleRepository scheduleRepository)
        {
            _orderRepository = orderRepository;
            _scheduleRepository = scheduleRepository;
        }

        public void LoadOrders(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                var text = reader.ReadToEnd();
                var orderPairs = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(text);

                foreach (var order in orderPairs)
                {
                    string orderNumber = order.Key;
                    string destination = order.Value.First().Value;

                    ProcessOrder(orderNumber, destination);
                }
            }
        }

        private void ProcessOrder(string orderNumber, string destination)
        {
            var nextAvailableFlight = _scheduleRepository.GetFirstAvailableFlight(destination);

            if (nextAvailableFlight != null)
            {
                _scheduleRepository.ReduceFlightCapacity(nextAvailableFlight.Number);
            }

            Order order = new Order()
            {
                Number = orderNumber,
                Destination = destination,
                Flight = nextAvailableFlight
            };
            _orderRepository.InsertOrder(order);
        }

        public string PrintOrders()
        {
            var orders = _orderRepository.GetOrders();

            StringBuilder builder = new StringBuilder();
            foreach (var order in orders)
            {
                if (order.Flight != null)
                {
                    builder.Append($"order: {order.Number}, flightNumber: {order.Flight.Number}, departure: {order.Flight.Origin}, arrival: {order.Flight.Destination}, day: {order.Flight.Day}\n");
                }
                else
                {
                    builder.Append($"order: {order.Number}, flightNumber: not scheduled\n");
                }
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}
