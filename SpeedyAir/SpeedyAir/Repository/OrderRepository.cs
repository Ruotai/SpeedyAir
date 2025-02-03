using SpeedyAir.Model;

namespace SpeedyAir.Repository
{
    public interface IOrderRepository
    {
        void InsertOrder(Order order);

        List<Order> GetOrders();
    }

    public class OrderRepository : IOrderRepository
    {
        private List<Order> orders;

        /// <summary>
        /// Repository layer normally connects to database.
        /// For the purpose of this exercise, we will use a list instead.
        /// </summary>
        public OrderRepository()
        {
            orders = new List<Order>();
        }

        public void InsertOrder(Order order)
        {
            orders.Add(order);
        }

        public List<Order> GetOrders()
        {
            return orders;
        }
    }
}
