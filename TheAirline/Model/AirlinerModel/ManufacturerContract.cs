﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheAirline.Model.AirlineModel;
using TheAirline.Model.GeneralModel;

namespace TheAirline.Model.AirlinerModel
{
    //the contract for an airline for a manufacturer
    [ProtoContract]
    public class ManufacturerContract
    {
        [ProtoMember(1,AsReference=true)]
        public Manufacturer Manufacturer { get; set; }
        [ProtoMember(2)]
        public DateTime SigningDate { get; set; }
        [ProtoMember(3)]
        public int Length { get; set; }
        [ProtoMember(4)]
        public DateTime ExpireDate { get; set; }
        [ProtoMember(5)]
        public double Discount { get; set; } //in percent
        [ProtoMember(6)]
        public int Airliners { get; set; } //the number of airliners to purchase in that period
        [ProtoMember(7)]
        public int PurchasedAirliners { get; set; }
        public ManufacturerContract(Manufacturer manufacturer, DateTime date, int length, double discount)
        {
            this.Manufacturer = manufacturer;
            this.Airliners = length;
            this.SigningDate = date;
            this.Length = length;
            this.Discount = discount;
            this.ExpireDate = date.AddYears(this.Length);
            this.PurchasedAirliners = 0;
        }
        //returns the termination fee for the contract
        public double getTerminationFee()
        {
            return GeneralHelpers.GetInflationPrice(this.Length * 1000000);
        }
        //the discount for airliners ordered under a contract
        public double getDiscount(int airliners)
        {
            airliners = PurchasedAirliners;
            if (Length <= 3)
            { this.Discount = (PurchasedAirliners / 2) + 1; }
            else if (Length <= 5)
            { this.Discount = (PurchasedAirliners / 2) + 2; }
            else if (Length <= 7)
            { this.Discount = (PurchasedAirliners / 2) + 4; }
            else if (Length <= 15)
            { this.Discount = (PurchasedAirliners / 2) + 7; }
            else
                this.Discount = 1;

            this.Discount = Discount;
            return Discount;

           }     
    }
}
