﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheAirline.Model.GeneralModel.CountryModel.TownModel;
using TheAirline.Model.GeneralModel;
using ProtoBuf;

namespace TheAirline.Model.PilotModel
{
    //the class for the profile of a pilot
    [ProtoContract]
    public class PilotProfile
    {
        [ProtoMember(1)]
        public string Firstname { get; set; }
        [ProtoMember(2)]
        public string Lastname { get; set; }
        public string Name { get { return string.Format("{0} {1}", this.Firstname, this.Lastname); } set { ;} }
        [ProtoMember(3)]
        public int Age { get { return MathHelpers.GetAge(this.Birthdate); } set { ;} }
        [ProtoMember(4)]
        public Town Town { get; set; }
        [ProtoMember(5)]
        public DateTime Birthdate { get; set; }
        public PilotProfile(string firstname, string lastname, DateTime birthdate, Town town)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Town = town;
            this.Birthdate = birthdate;
  
        }
    }
}
