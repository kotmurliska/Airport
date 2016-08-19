using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Airport
{
    public class FlightsSchedule
    {
        private const string DatePattern = "dd.MM.yyyy HH:mm:ss";

        private FlightInfo[] fligths;

        public FlightsSchedule()
        {
            FillFlights();
        }

        private void FillFlights()
        {
            fligths = new FlightInfo[7]
            {
                new FlightInfo {Number = "AC-231", Destination = "London", DeparturePlace = "Moscow", AirLine = "WizAir", Departure = DateTime.Now.AddDays(1), Arrival = DateTime.Now.AddDays(1).AddHours(4), Gate ="D-09", Status = FlightStatus.checkIn},
                new FlightInfo {Number = "AC-236", Destination = "Barselona", DeparturePlace = "Kharkov", AirLine = "MAU" , Departure = DateTime.Now.AddDays(2), Arrival = DateTime.Now.AddDays(2).AddHours(4), Gate ="D-11", Status = FlightStatus.inFlight},
                new FlightInfo {Number = "AC-178", Destination = "Madrid", DeparturePlace = "Warshava", AirLine = "WizAir", Departure = DateTime.Now.AddDays(-1), Arrival = DateTime.Now.AddHours(1), Gate ="M-09", Status = FlightStatus.delayed},
                new FlightInfo {Number = "AC-155", Destination = "Los Angeles", DeparturePlace = "Toronto", AirLine = "Low cost", Departure = DateTime.Now.AddDays(1), Arrival = DateTime.Now.AddDays(1).AddHours(8), Gate ="A-19", Status = FlightStatus.arrived},
                new FlightInfo {Number = "AC-88", Destination = "New York", DeparturePlace = "Mexico", AirLine = "low cost", Departure = DateTime.Now.AddMinutes(30), Arrival = DateTime.Now.AddHours(3), Gate ="K-01", Status = FlightStatus.canceled},                
                null,
                null
            };
        }

        internal void AddFlight()
        {
            Console.WriteLine(@"Do you want to add more flights? Y\N");

            var choice = Console.ReadLine();

            switch (choice.ToUpper())
            {
                case "Y":
                    try
                    {

                        Console.Clear();

                        var flight = CreateFlight();

                        int count = FlightsCount();
                        if (count < fligths.Length)
                        {
                            fligths[count] = flight;
                        }
                        else
                        {
                            Array.Resize<FlightInfo>(ref fligths, fligths.Length + 1);
                            fligths[count] = flight;
                        }
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Flight {0} was added successfully, to continue press any key", flight.ToString());
                        Console.ResetColor();
                        Console.ReadLine();
                    }
                    catch
                    {
                        DrawConsoleHeader("Data format was incorrect to press any key to continue", ConsoleColor.Red);                        
                        Console.ReadLine();
                    }
                    break;
                default:
                    break;

            }
        }

        private int FlightsCount()
        {
            int res = 0;
            foreach (var item in fligths)
            {
                if (item != null)
                    res++;
            }
            return res;
        }

        internal void EditFlight()
        {
            Console.Clear();
            //the output of all the information on flights в отдельный метод + стилизовать в таблицу

            DrawFlights();


            try
            {
                Console.WriteLine("Enter the number of flight to edit");
                var choice = Console.ReadLine();

                FlightInfo flight = null;
                
                for (int i = 0; i < fligths.Length; i++)
                {
                    if (fligths[i] != null && fligths[i].Number == choice)
                    {
                        flight = fligths[i];
                        break;
                    }
                }

                if (flight == null)
                {
                    DrawConsoleHeader("We could not find a flight, do you want to continue? Press any button",ConsoleColor.Red);
                    Console.ReadLine();
                    return;
                }

                Console.Clear();   
          
                DrawConsoleHeader(string.Format("Flight: {0} has been found. Press any key to continue",choice), ConsoleColor.Green);
                DrawFlights(new FlightInfo[] { flight });
                Console.ReadLine();

                Console.WriteLine("Please enter new number or press enter if you don't want to change");
                var number = Console.ReadLine();
                if (!String.IsNullOrEmpty(number))
                    flight.Number = number;

                Console.WriteLine("Please enter new destination or press enter if you don't want to change");
                var destination = Console.ReadLine();
                if (!String.IsNullOrEmpty(destination))
                    flight.Destination = destination;

                Console.WriteLine("Please enter new departure place or press enter if you don't want to change");
                var depPlace = Console.ReadLine();
                if (!String.IsNullOrEmpty(depPlace))
                    flight.DeparturePlace = depPlace;

                Console.WriteLine("Please enter new airline or press enter if you don't want to change");
                var line = Console.ReadLine();
                if (!String.IsNullOrEmpty(line))
                    flight.AirLine = line;

                var depTime = SetDate("Please enter new the departure date and time or press enter (use this format dd.MM.yyyy HH:mm:ss )", false);
                if (depTime != null)
                    flight.Departure = (DateTime)depTime;


                var arrTime = SetDate("Please enter new the departure date and time or press enter (use this format dd.MM.yyyy HH:mm:ss )", false);
                if (arrTime != null)
                    flight.Arrival = (DateTime)arrTime;

                Console.WriteLine("Please enter new gate or press enter if you don't want to change");
                var gate = (Console.ReadLine());
                if (!String.IsNullOrEmpty(gate))
                    flight.Gate = gate;

                Console.WriteLine(@"Please enter new status or press enter if you don't want to change 
(use this status checkIn, gateClosed, arrived, departedAt, unknown, canceled, expectedAt, delayed, inFlight)");
                var status = (Console.ReadLine());
                if (!String.IsNullOrEmpty(status))
                    flight.Status = (FlightStatus)Enum.Parse(typeof(FlightStatus), status);

                DrawConsoleHeader("Data format was incorrect to press any key to continue", ConsoleColor.Green);                
                Console.ReadLine();
            }
            catch
            {
                DrawConsoleHeader("Data format was incorrect to press any key to continue", ConsoleColor.Red);                
                Console.ReadLine();
            }
        }

        internal void RemoveFlight()
        {
            Console.WriteLine(@"Do you want to remove flights? Y\N");

            var choice = Console.ReadLine();

            Console.Clear();

            DrawFlights();

            switch (choice.ToUpper())
            {
                case "Y":
                        
                        Console.WriteLine("Enter the number of flight to delete");
                        string num = Console.ReadLine();

                        FlightInfo[] newFligths = new FlightInfo[fligths.Length - 1];
                        int index = 0;
                        for (int i = 0; i < fligths.Length; i++)
                        {
                            if (fligths[i] != null && fligths[i].Number == num)
                            {
                                index = i;
                            }
                        }

                        for (int i = 0; i < fligths.Length; i++)
                        {
                            if (i < index)
                            {
                                newFligths[i] = fligths[i];
                            }
                            else if (i > index)
                                newFligths[i - 1] = fligths[i];
                        }
                        fligths = newFligths;
                    
                    Console.Clear();
                    break;

                default:
                    break;
            }
        }

        private FlightInfo CreateFlight()
        {
            var flight = new FlightInfo();

            Console.WriteLine("Please, enter the number of flight");
            flight.Number = Console.ReadLine();

            Console.WriteLine("Please, enter the destination of flight");
            flight.Destination = Console.ReadLine();

            Console.WriteLine("Please, enter the departure place of flight");
            flight.DeparturePlace = Console.ReadLine();

            Console.WriteLine("Please, enter the airline of flight");
            flight.AirLine = Console.ReadLine();

            flight.Departure = SetDate("Please enter the departure date and time (use this format dd.MM.yyyy HH:mm:ss )") ?? DateTime.Now;

            flight.Arrival = SetDate("Please enter the arrival date and time (use this format dd.MM.yyyy HH:mm:ss )") ?? DateTime.Now;

            Console.WriteLine("Please, enter the gate of flight");
            flight.Gate = Console.ReadLine();

            Console.WriteLine("Please, enter the status of flight");
            flight.Status = (FlightStatus)Enum.Parse(typeof(FlightStatus), Console.ReadLine());

            return flight;
        }

        internal void SearchFlight(string number, DateTime? arrival, string destination, string depPlace)
        {
            Console.Clear();

            FlightInfo[] fc = new FlightInfo[fligths.Length];

            for (int i = 0; i < fligths.Length; i++)
            {
                if (!string.IsNullOrEmpty(number) && fligths[i].Number == number)
                {
                    fc[i] = fligths[i];
                    break;
                }
                else if (arrival != null && fligths[i].Arrival == arrival)
                {
                    fc[i] = fligths[i];
                    break;
                }
                else if (!string.IsNullOrEmpty(destination) && fligths[i].Destination == destination)
                {
                    fc[i] = fligths[i];
                    break;
                }
                else if (!string.IsNullOrEmpty(depPlace) && fligths[i].DeparturePlace == depPlace)
                {
                    fc[i] = fligths[i];
                    break;
                }
            }

            if (fc.Count() == 0)
                DrawConsoleHeader("Sorry, flights were not found", ConsoleColor.Red);
            else
                DrawFlights(fc);

            DrawConsoleHeader("Press any key to continue", ConsoleColor.Green);            
            Console.ReadLine();
                         
            
        }

        internal void SearchFlightByTime()
        {
            DrawConsoleHeader("List of nearest flights:", ConsoleColor.Green);           

            FlightInfo[] fl = new FlightInfo[0];

            for (int i = 0; i < fligths.Length; i++)
            {

                if (fligths[i] != null && 
                    ((fligths[i].Arrival <= DateTime.Now.AddHours(1) && fligths[i].Arrival >= DateTime.Now) 
                    || (fligths[i].Departure <= DateTime.Now.AddHours(1) && fligths[i].Departure >= DateTime.Now)))
                {
                    Array.Resize<FlightInfo>(ref fl, fl.Length + 1);
                    fl[fl.Length-1] = fligths[i];                                            
                }
            }

            DrawFlights(fl);

            DrawConsoleHeader("To continue press any key...", ConsoleColor.Green);           
            Console.ReadLine();
        }

        public DateTime? SetDate(string message, bool isNeedDate = true)
        {
            var isCorrect = true;

            Console.WriteLine(message);

            DateTime dateTime;
            DateTime? resultTime = null;

            do
            {
                var dateStr = Console.ReadLine();

                if (!DateTime.TryParseExact(dateStr, DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {

                    if (isNeedDate)
                    {
                        DrawConsoleHeader("You've entered wrong date.Try again", ConsoleColor.Red);                        
                    }
                    isCorrect = false;

                }
                else
                {
                    resultTime = dateTime;
                    isCorrect = true;
                }

            } while (!isCorrect && isNeedDate);

            return resultTime;
        }

        public void DrawFlights()
        {
            DrawFlights(this.fligths);
        }

        public void DrawFlights(FlightInfo[] flightsCollection)
        {                     

            Console.ForegroundColor = ConsoleColor.White;

            var ct = new ConsoleTableCreater { TextAlignment = ConsoleTableCreater.AlignText.ALIGN_RIGHT };

            ct.SetHeaders(new[] { "Number", "Destination", "Departure place", "AirLine", "Departure time", "Arrival time", "Gate", "Status of flight" });

            foreach (var flight in flightsCollection)
            {
                if (flight == null) continue;

                ct.AddRow(new List<string> { flight.Number, flight.Destination, flight.DeparturePlace, flight.AirLine, flight.Departure.ToString(), flight.Arrival.ToString(), flight.Gate, flight.Status.ToString() });
            }

            ct.PrintTable();
        }

        public void DrawConsoleHeader(string text, ConsoleColor color)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
