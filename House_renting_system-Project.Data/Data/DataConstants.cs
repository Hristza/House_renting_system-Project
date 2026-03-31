using System;
using System.Collections.Generic;
using System.Text;

namespace House_renting_system_Project.Data.Data
{
    public class DataConstants
    {
        public class House
        {
        public const int TitleMaxLength = 50;
        public const int AddressMaxLength = 150;
        public const int DescriptionMaxLength = 500;
        }
        public class Category
        {
            public const int NameMaxLength = 50;
        }
        public class Agent
        {
            public const int PhoneNumberMaxLength = 15;
        }
    }
}
