using System.Text;
using System.Text.RegularExpressions;
using SpeedyAir.Model;
using SpeedyAir.Repository;

namespace SpeedyAir.Service
{
    public class ScheduleService
    {
        private IScheduleRepository _repository;

        public ScheduleService(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public void LoadFlightSchedule(string filePath)
        {
            int day = 1;

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string text = reader.ReadLine();

                    if (text.StartsWith("Day"))
                    {
                        var data = text.Split(' ');
                        var dayValue = data[1].Replace(":", string.Empty);
                        day = int.Parse(dayValue);
                    }

                    if (text.StartsWith("Flight"))
                    {
                        var data = text.Split(' ');

                        var numberValue = data[1].Replace(":", string.Empty);
                        int number = int.Parse(numberValue);

                        var originValue = data.First(x => Regex.IsMatch(x, "^\\([A-Z]{3}\\)$"));
                        string origin = originValue.Replace("(", string.Empty).Replace(")", string.Empty);

                        var destinationValue = data.Last(x => Regex.IsMatch(x, "^\\([A-Z]{3}\\)$"));
                        string destination = destinationValue.Replace("(", string.Empty).Replace(")", string.Empty);

                        Flight flight = new Flight()
                        {
                            Day = day,
                            Number = number,
                            Origin = origin,
                            Destination = destination,
                            Capacity = 20
                        };

                        _repository.UpsertSchedule(flight);
                    }
                }
            }
        }

        public string PrintFlightSchedule()
        {
            var schedules = _repository.GetSchedules().OrderBy(x => x.Number);

            StringBuilder builder = new StringBuilder();
            foreach (var flight in schedules)
            {
                builder.Append($"Flight: {flight.Number}, departure: {flight.Origin}, arrival: {flight.Destination}, day: {flight.Day}\n");
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

    }
}
