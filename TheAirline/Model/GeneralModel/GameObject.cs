﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TheAirline.Model.AirlineModel;
using TheAirline.GraphicsModel.PageModel.PageGameModel;

namespace TheAirline.Model.GeneralModel
{
    //the class for the game object
    public class GameObject
    {
        private static GameObject GameInstance;
        public DateTime GameTime { get; set; }
        public DateTime StartDate { get; set; }
        public Airline HumanAirline { get; set; }
        public Airline MainAirline { get; set; }
        public NewsBox NewsBox { get; set; }
        public double FuelPrice { get; set; }
        public long StartMoney { get { return getStartMoney(); } set { ;} }
        public GameTimeZone TimeZone { get; set; }
        public string Name { get; set; }
        public enum DifficultyLevel { Easy, Normal, Hard }
        public DifficultyLevel Difficulty { get; set; }
        public double PassengerDemandFactor { get; set; }
        private GameObject()
        {
            this.PassengerDemandFactor = 100;
            this.GameTime = new DateTime(2007, 12, 31, 10, 0, 0);
            this.TimeZone = TimeZones.GetTimeZones().Find(delegate(GameTimeZone gtz) { return gtz.UTCOffset == new TimeSpan(0, 0, 0); });
            this.Difficulty = DifficultyLevel.Easy;
            this.NewsBox = new NewsBox();
        }

        //return the difficulty level
        

        //returns the start money based on year of start
        
        private long getStartMoney()
        {
            
            double baseStartMoney = 12500000;
            if (this.Difficulty == DifficultyLevel.Easy)
            { baseStartMoney *= 1.5; }
            else if (this.Difficulty == DifficultyLevel.Normal)
            { baseStartMoney *= 1;}
            else if (this.Difficulty == DifficultyLevel.Hard)
            { baseStartMoney *= 0.5;}

            return Convert.ToInt64(GeneralHelpers.GetInflationPrice(baseStartMoney));
        }

        //returns the game instance
        public static GameObject GetInstance()
        {
            if (GameInstance == null)
                GameInstance = new GameObject();
            return GameInstance;
        }

        //restarts the instance
        public static void RestartInstance()
        {
            GameInstance = new GameObject();
        }

    }
}
