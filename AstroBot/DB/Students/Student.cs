using System;

namespace AstroBot.DB.Students
{
    class Student 
    {
        public static readonly int NameMaxLength = 15;
        public static readonly int SurnameMaxLength = 20;
        public static readonly int ClassMaxLength = 3;
        public static readonly int TGIdMaxLength = 15;
        public static readonly int VKIdMaxLength = 15;

        public string Name    = "Иван";
        public string Surname = "Иванов";
        public string Class   = "11X";
        public string TGId    = "undefined";
        public string VKId    = "undefined";
    }
}
