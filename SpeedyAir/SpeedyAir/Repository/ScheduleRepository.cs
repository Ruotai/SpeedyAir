using SpeedyAir.Model;

namespace SpeedyAir.Repository
{
    public interface IScheduleRepository
    {
        void UpsertSchedule(Flight schedule);

        List<Flight> GetSchedules();

        Flight GetFirstAvailableFlight(string destination);

        void ReduceFlightCapacity(int number);
    }

    public class ScheduleRepository : IScheduleRepository
    {
        private List<Flight> schedules;

        /// <summary>
        /// Repository layer normally connects to database.
        /// For the purpose of this exercise, we will use a list instead.
        /// </summary>
        public ScheduleRepository()
        {
            schedules = new List<Flight>();
        }

        public void UpsertSchedule(Flight flight)
        {
            var duplicate = schedules.FirstOrDefault(x => x.Number.Equals(flight.Number));
            if (duplicate != null)
            {
                schedules.Remove(duplicate);
            }

            schedules.Add(flight);
        }

        public List<Flight> GetSchedules()
        {
            return schedules;
        }

        public Flight GetFirstAvailableFlight(string destination)
        {
            return schedules.FirstOrDefault(x => x.Destination.Equals(destination) && x.Capacity > 0);
        }

        public void ReduceFlightCapacity(int number)
        {
            var flight = schedules.FirstOrDefault(x => x.Number.Equals(number));
            if (flight.Capacity > 0)
            {
                flight.Capacity -= 1;
            }
        }
    }
}
