﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAirline.Model.AirportModel;
using TheAirline.Model.AirlinerModel;
using TheAirline.Model.AirlineModel;
using TheAirline.Model.AirlinerModel.RouteModel;
using TheAirline.Model.GeneralModel;
using TheAirline.Model.GeneralModel.StatisticsModel;
using TheAirline.Model.GeneralModel.InvoicesModel;
using TheAirline.Model.AirlineModel.SubsidiaryModel;
using TheAirline.Model.PilotModel;
using ProtoBuf;

namespace TheAirline.Model.GeneralModel
{
    [ProtoContract]
    public class RandomEvent
    {
        public enum EventType { Safety, Security, Maintenance, Customer, Employee, Political }
        public enum Focus { Aircraft, Airport, Airline }
        [ProtoMember(1)]
        public EventType Type { get; set; }
        [ProtoMember(2)]
        public Focus focus { get; set; }
        [ProtoMember(3, AsReference=true)]
        public Airline Airline { get; set; }
        [ProtoMember(4)]
        public string EventName { get; set; }
        [ProtoMember(5)]
        public string EventMessage { get; set; }
        [ProtoMember(6,AsReference=true)]
        public FleetAirliner Airliner { get; set; }
        [ProtoMember(7,AsReference=true)]
        public Airport Airport { get; set; }
        [ProtoMember(8,AsReference=true)]
        public Country Country { get; set; }
        [ProtoMember(9,AsReference=true)]
        public Route Route { get; set; }
        [ProtoMember(10)]
        public bool CriticalEvent { get; set; }
        [ProtoMember(11)]
        public DateTime DateOccurred { get; set; }
        [ProtoMember(12)]
        public int CustomerHappinessEffect { get; set; } //0-100
        [ProtoMember(13)]
        public int AircraftDamageEffect { get; set; } //0-100
        [ProtoMember(14)]
        public int AirlineSecurityEffect { get; set; } //0-100
        [ProtoMember(15)]
        public int AirlineSafetyEffect { get; set; } //0-100
        [ProtoMember(16)]
        public int EmployeeHappinessEffect { get; set; } //0-100
        [ProtoMember(17)]
        public int FinancialPenalty { get; set; } //dollar amount to be added or subtracted from airline cash
        [ProtoMember(18)]
        public double PaxDemandEffect { get; set; } //0-2
        [ProtoMember(19)]
        public double CargoDemandEffect { get; set; } //0-2
        [ProtoMember(20)]
        public int EffectLength { get; set; } //should be defined in months
        [ProtoMember(21)]
        public string EventID { get; set; }
        [ProtoMember(22)]
        public int Frequency { get; set; } //frequency per 3 years
        [ProtoMember(23)]
        public DateTime Start { get; set; }
        [ProtoMember(24)]
        public DateTime End { get; set; }
        public RandomEvent(EventType type, Focus focus, string name, string message, bool critical, int custHappiness, int aircraftDamage, int airlineSecurity, int airlineSafety, int empHappiness, int moneyEffect, double paxDemand, double cargoDemand, int length, string id, int frequency, DateTime stat, DateTime end)
        {

            this.DateOccurred = GameObject.GetInstance().GameTime;
            this.CustomerHappinessEffect = 0;
            this.AircraftDamageEffect = 0;
            this.AirlineSecurityEffect = 0;
            this.EmployeeHappinessEffect = 0;
            this.FinancialPenalty = 0;
            this.PaxDemandEffect = 1;
            this.CargoDemandEffect = 1;
            this.EffectLength = 1;
            this.CriticalEvent = false;
            this.EventName = "";
            this.EventMessage = "";
            this.Type = type;

         
            this.EventID = id;
        }

        //applies the effects of an event
        public void ExecuteEvents(Airline airline, DateTime time) 
        {
            Random rnd = new Random();
            foreach (RandomEvent rEvent in airline.EventLog)
            {
                if (rEvent.DateOccurred.DayOfYear == time.DayOfYear)
                {
                    rEvent.Airliner.Airliner.Damaged += AircraftDamageEffect;
                    airline.Money += rEvent.FinancialPenalty;
                    airline.Scores.CHR.Add(rEvent.CustomerHappinessEffect);
                    airline.Scores.EHR.Add(rEvent.EmployeeHappinessEffect);
                    airline.Scores.Safety.Add(rEvent.AirlineSafetyEffect);
                    airline.Scores.Security.Add(rEvent.AirlineSecurityEffect);
                    PassengerHelpers.ChangePaxDemand(airline, (rEvent.PaxDemandEffect * rnd.Next(9, 11) / 10));
                }
            }
        }

       

        /*public RandomEvent GenerateRandomEvent()
        {
            //code needed

        }*/

        //adds an event to an airline's event log
        public void AddEvent(Airline airline, RandomEvent rEvent)
        {
            airline.EventLog.Add(rEvent);
        }


        //removes an event from the airlines event log
        public static void RemoveEvent(Airline airline, RandomEvent rEvent)
        {
            airline.EventLog.Remove(rEvent);
        }


        //checks if an event's effects are expired
        public static void CheckExpired(DateTime expDate)
        {
            foreach (Airline airline in Airlines.GetAllAirlines())
            {
                foreach (RandomEvent rEvent in airline.EventLog)
                {
                    expDate = GameObject.GetInstance().GameTime.AddMonths(rEvent.EffectLength);
                    if (expDate < GameObject.GetInstance().GameTime)
                    {
                        PassengerHelpers.ChangePaxDemand(airline, (1 / rEvent.PaxDemandEffect));
                        RemoveEvent(airline, rEvent);
                    }  }  }
        }
    }

    public class RandomEvents
    {
        private static Dictionary<string, RandomEvent> events = new Dictionary<string, RandomEvent>();

        public static void Clear()
        {
            events = new Dictionary<string, RandomEvent>();
        }

        public static void AddEvent(RandomEvent rEvent)
        {
            events.Add(rEvent.EventName, rEvent);
        }

        //gets a single event by name
        public static RandomEvent GetEvent(string name)
        {
            return events[name];
        }


        //gets a list of all events
        public static List<RandomEvent> GetEvents()
        {
            return events.Values.ToList();
        }

        //gets all events of a given type
        public static List<RandomEvent> GetEvents(RandomEvent.EventType type)
        {
            return GetEvents().FindAll((delegate(RandomEvent rEvent) {return rEvent.Type ==type; }));
        }

        //gets x number of random events of a given type
        public static List<RandomEvent> GetEvents(RandomEvent.EventType type, int number, Airline airline)
        {
            Random rnd = new Random();
            Dictionary<int,RandomEvent> rEvents = new Dictionary<int,RandomEvent>();
            List<RandomEvent> tEvents = GetEvents(type);
            int i = 1;
            int j = 0;
            foreach (RandomEvent r in tEvents)
                if (r.Start <= GameObject.GetInstance().GameTime && r.End >= GameObject.GetInstance().GameTime)
                {
                    {
                        r.DateOccurred = MathHelpers.GetRandomDate(GameObject.GetInstance().GameTime, GameObject.GetInstance().GameTime.AddMonths(12));
                        r.Airline = airline;
                        r.Airliner = Helpers.AirlinerHelpers.GetRandomAirliner(airline);
                        r.Route = r.Airliner.Routes[rnd.Next(r.Airliner.Routes.Count())];
                        r.Country = r.Route.Destination1.Profile.Country;
                        r.Airport = r.Route.Destination1;

                        if (r.focus == RandomEvent.Focus.Airline)
                        {
                            r.Airliner = null;
                            r.Airport = null;
                            r.Country = null;
                            r.Route = null;
                        }

                        rEvents.Add(i, r);
                        i++;
                    }
                }

            tEvents.Clear();

            while (j < number)
            {
                int item = rnd.Next(rEvents.Count());
                tEvents.Add(rEvents[item]);
                j++;
            }

            return tEvents;
        }

    }


}
