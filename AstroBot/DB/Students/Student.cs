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

        public string Name    = "???";
        public string Surname = "???";
        public string Class   = "???";
        public string TGId    = "???";
        public string VKId    = "???";
    }
}
