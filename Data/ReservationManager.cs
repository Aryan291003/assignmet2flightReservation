using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp9.Data
{
    internal class ReservationManager
    {

        /**
         * The location of the reservation file.
         */
        //public const string RESERVATIONS_BINARY = "res/reservations.bin";
        //private static string Reservation_TXT = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Resources\Files\reservation.csv");
        private static string Reservation_TXT = @"C:\Assignment2_Updated\MauiApp9\Resources\Files\reservation.csv";


        private static Random random = new Random();
        /**
         * Holds the Reservation objects.
         */
        private static List<Reservation> reservations= new List<Reservation>();

        public ReservationManager()
        {
             
            this.PopulateReservation();
        }

        /**
         * Makes a reservation.
         * @param flight Flight to book reservation for.
         * @param name Name of person (cannot be null or empty).
         * @param citizenship Citizenship of person (cannot be null or empty).
         * @return Created reservation instance.
         * @throws NoMoreSeatsException Thrown if flight is booked up.
         * @throws InvalidNameException Thrown if name is null or empty.
         * @throws InvalidCitizenshipException Thrown if citizenship is null or empty.
         * @throws NullFlightException Thrown if flight is null
         */
        /*public Reservation MakeReservation(Flight flight, string name, string citizenship)
        { // throws NoMoreSeatsException, InvalidNameException, InvalidCitizenshipException, NullFlightException {
            if (this.GetAvailableSeats(flight) == 0)
                throw new Exception();//NoMoreSeatsException
            if (flight == null)
                throw new NullFlightException();
            string reservationCode = GenerateReservationCode(flight);

            Reservation reservation = new Reservation(reservationCode, flight, name, citizenship);

            this.reservations.Add(reservation);

            this.persist();

            return reservation;
        }*/

        /**
         * Finds reservations containing either reservation code or airline.
         * @param reservationCode Reservation code to search for.
         * @param airline Airline to search for.
         * @param name Travelers name to search for.
         * @return Any matching Reservation objects.
         */
        public List<Reservation> FindReservations(string reservationCode, string airline, string name)
        {
            List<Reservation> found = new List<Reservation>();

            foreach (Reservation reservation in reservations)
            {
                if (reservation.Code.Contains(reservationCode) && reservation.Airline.Contains(airline) && reservation.Name.Contains(name))
                    found.Add(reservation);
            }

            return found;
        }

        /**
         * Finds reservation with the exact reservation code.
         * @param reservationCode Reservation code.
         * @return Reservation object or null if none found.
         */
        public Reservation FindReservationByCode(string reservationCode)
        {
            foreach (Reservation reservation in reservations)
            {
                if (reservation.Code.Equals(reservationCode))
                    return reservation;
            }

            return null;
        }

        /**
         * Gets the number of available seats for a flight.
         * @param flight Flight instance.
         * @return Number of available seats.
         */
        private int GetAvailableSeats(Flight flight)
        {
            int usedSeats = 0;

            foreach (Reservation reservation in reservations)
            {
                /*if (reservation.Active && reservation.Code.Equals(flight.Code))
                {
                    usedSeats++;
                }*/
            }

            return flight.Seats - usedSeats;
        }

        public string GenerateResCode()
        {
            return GenerateReservationCode();
        }

        /**
         * 
         * 
         * Gets reservation code using a flight.
         * @param flight Flight instance.
         * @return Reservation code.
         */
        public  string GenerateReservationCode()
        {
            /*char letter = flight.isDomestic() ? 'D' : 'I';

            Random rand = new Random();

            return letter.ToString() + (rand.Next(9999) + 1000).ToString();*/
            string reservationCode;

            do
            {
                char letter = (char)('A' + random.Next(26));
                string numbers = random.Next(1000, 10000).ToString();
                reservationCode = letter + numbers;
            } while (IsCodeGenerated(reservationCode, Reservation_TXT));

            return reservationCode;
        }
        private static bool IsCodeGenerated(string reservationCode, string Reservation_TXT)
        {
            if (!File.Exists(reservationCode))
            {
                return false;
            }

            List<string> existingCode = File.ReadAllLines(Reservation_TXT).ToList();

            return existingCode.Contains(reservationCode);
        }

        /**
         * Populates reservations with Reservation objects from Random Access File.
         */
        private void PopulateReservation()
        {
            Reservation newReservation;
            if (reservations.Count > 0)
            {
                foreach (string line in File.ReadLines(Reservation_TXT))
                {
                    string[] parts = line.Split(",");
                    string reservationCode = parts[0];
                    string flightCode = parts[1];
                    string airline = parts[2];
                    double cost = double.Parse(parts[3]);
                    string name = parts[4];
                    string citizenship = parts[5];
                    string status = parts[6];

                    newReservation = new Reservation(reservationCode, flightCode, airline, cost, name, citizenship, status);
                    reservations.Add(newReservation);
                }
            }
        }
        public static List<Reservation> GetReservations()  // Return type is  List<User> 
        {
            return reservations;
        }
        public void UpdateReservationFile(List<Reservation> reservations)
        {

            //File.WriteAllText(Reservation_TXT, string.Empty);//line to empty the list

            foreach (var res in reservations)
            {
                File.AppendAllText(Reservation_TXT, $"{res.Code},{res.FlightCode},{res.Airline},{res.Cost},{res.Name},{res.Citizenship},{res.Active}\n");
                

            }
        }

        /**
         * Saves Reservation objects to a file.
         */
        /*public void persist()
       {
            try
             {
                 RandomAccessFile raf = new RandomAccessFile(RESERVATIONS_BINARY, "rw");

                 raf.SetLength(0);

                 foreach (Reservation reservation in this.reservations)
                 {
                     raf.WriteBoolean(reservation.Active);
                     raf.WriteUTF(reservation.Code);
                     raf.WriteUTF(reservation.FlightCode);
                     raf.WriteUTF(string.Format("%-20s", reservation.Airline));
                     raf.WriteDouble(reservation.Cost);
                     raf.WriteUTF(string.Format("%-75s", reservation.Name));
                     raf.WriteUTF(string.Format("%-75s", reservation.Citizenship));
                 }

                 raf.Close();
             }
             catch (Exception ex)
             {
                 // ex.printStackTrace();
             }
         }
    }
        */

        /*[Serializable]
        internal class NullFlightException : Exception
        {
            public NullFlightException()
            {
            }

            public NullFlightException(string message) : base(message)
            {
            }

            public NullFlightException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NullFlightException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }*/
    }
}
