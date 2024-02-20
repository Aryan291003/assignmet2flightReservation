using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiApp9.Data
{
    class FlightsService
    {
        public static async Task<List<Flight>> GetFlights()
        {
            return await Task.FromResult(FlightManager.getFlights());
        }

    }
}
