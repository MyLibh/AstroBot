using System;

namespace AstroBot.DB.Students
{
    class Student 
    {
        public static readonly int NameMaxLength = 15;
        public static readonly int RoleMaxLength = 7;
        public static readonly int SurnameMaxLength = 25;
        public static readonly int ClassMaxLength = 3;
        public static readonly int TGIdMaxLength = 15;
        public static readonly int VKIdMaxLength = 15;
        public static readonly int CurrentTaskMaxLength = 4;
        public static readonly int CurrentAnswerMaxLength = 6;
        public static readonly int CurrentTaskCompletedMaxLength = 5;

        public string Name    = "???";
        public ERole  Role    = ERole.Student;
        public string Surname = "???";
        public string Class   = "???";
        public string TGId    = "???";
        public string VKId    = "???";

        public int    CurrentTask          = 0;
        public double CurrentAnswer        = 0;
        public int    CurrentTaskCompleted = 1;

        public enum ERole
        {
            Student,
            Admin,
            Coder
        }
    }
}
