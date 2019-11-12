using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public static class CommonMethods
    {
        public static int GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999); 
        }

        public static string Generate13UniqueDigits()
        {  
            return DateTime.Now.ToString("yyMMddHHmmssf"); 
        }

        public static string Generate15UniqueDigits()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssf");
        }

        public static string ReplaceWhiteSpaces(string Name)
        {
            return Name.Replace(' ', '-');
        }

        public static string ReplaceHyphen(string Name)
        {
            return Name.Replace('-', ' ');
        } 

        public static string ChangeDateFormat(object Date)
        {
            DateTime dt = Convert.ToDateTime(Date);
            return dt.ToString("f");
        }

        public static string ChangeDateFormatShortDate(object Date)
        {
            DateTime dt = Convert.ToDateTime(Date);
            return dt.ToString("D");
        }
         
        public static string GenerateRandomPassword(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return GenerateRandomPassword(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        public static string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            return filename;
        }

        public static string RenameFileName(string filename)
        {
            if (filename != null || filename != "")
            {
                filename = DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString() + filename;
                filename = filename.Replace(' ', '0').Replace(':', '1').Replace('/', '0');
            } 
            return filename;
        }
    }
}
