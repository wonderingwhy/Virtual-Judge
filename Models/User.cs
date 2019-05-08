using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Models
{
    public class User: IComparable
    {
        public UInt32 UserID { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Nickname { get; set; }
        public String Type { get; set; }
        public List<Item> Items { get; set; }
        public UInt32 ProsSolved { get; set; }
        public TimeSpan Timer { get; set; }
        public UInt32 Rank { get; set; }
        private bool IsValidEnglishChar(Char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            if (ch >= 'a' && ch <= 'z')
            {
                return true;
            }
            if (ch >= 'A' && ch <= 'Z')
            {
                return true;
            }
            return ch == '_';
        }
        private bool IsValidChar(Char ch)
        {
            if ((uint)ch > 127)
            {
                return true;
            }
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            if (ch >= 'a' && ch <= 'z')
            {
                return true;
            }
            if (ch >= 'A' && ch <= 'Z')
            {
                return true;
            }
            return ch == '_';
        }
        private bool IsValidEnglishString(string str)
        {
            if (str.Length < 6 || str.Length > 16)
            {
                return false;
            }
            foreach (Char ch in str.ToCharArray())
            {
                if (IsValidEnglishChar(ch) == false)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsValidString(string str)
        {
            if (str.Length > 10)
            {
                return false;
            }
            foreach (Char ch in str.ToCharArray())
            {
                if (IsValidChar(ch) == false)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckValid()
        {
            return IsValidEnglishString(Username) && IsValidEnglishString(Password) && IsValidString(Nickname);
        }
        public User() {
            Timer = TimeSpan.Zero;
            ProsSolved = 0;
            Items = new List<Item>();
        }
        public User(string u, string p, string n)
        {
            Username = u;
            Password = p;
            Nickname = n;
            Type = "normal";
        }
        public User(uint uid, string p, string n)
        {
            UserID = uid;
            Password = p;
            Nickname = n;
        } 
        public int CompareTo(object obj)
        {
            User u = obj as User;
            try
            {
                if (this.ProsSolved != u.ProsSolved)
                {
                    return this.ProsSolved > u.ProsSolved ? 1 : 0;
                }
                return this.Timer < u.Timer ? 1 : 0;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}
