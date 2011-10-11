﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheAirline.Model.AirlinerModel.RouteModel
{
    /*! Route airliner class.
    * This class is used for an airliner class onboard of a route airliner
    * The class needs parameters for type of class and the fare price
    */
    public class RouteAirlinerClass
    {
        public double FarePrice { get; set; }
        public RouteFacility FoodFacility { get; set; }
        public RouteFacility DrinksFacility { get; set; }
        public int CabinCrew { get; set; }
        public AirlinerClass.ClassType Type { get; set; }
        public RouteAirlinerClass(AirlinerClass.ClassType type, double fareprice)
        {
            this.Type = type;
            this.FarePrice = fareprice;
        }

    }
}
