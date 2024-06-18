using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;
using Abacus.Notification;


namespace Abacus.BPM
{

    public class Common
    {
        private AbacusDBEntities db = new AbacusDBEntities();

        /*
         * ------------------------------------------------------------------
         * Content  : Date functions
         * Author   : Giorgos Kyriakakis
         * Date     : 14-06-2015
         * Note:    : The functions must be declared static to expose them
         *            to all modules of the application.
         * ------------------------------------------------------------------
         */

        #region String Functions (equivalent to VB)

        public static string Right(string text, int numberCharacters)
        {
            return text.Substring(numberCharacters > text.Length ? 0 : text.Length - numberCharacters);
        }

        public static string Left(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }
        public static int Len(string text)
        {
            int _length;
            _length = text.Length;
            return _length;
        }
        public static byte Asc(string src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(src + "")[0]);
        }
        public static char Chr(byte src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetChars(new byte[] { src })[0]);
        }
        public static bool isNumber(string param)
        {
            Regex isNum = new Regex("[^0-9]");
            return !isNum.IsMatch(param);
        }

        #endregion

        #region Date Functions

        public static int WeekDay(int day, int month, int year)
        {
            string strDate = day.ToString() + "/" + month.ToString() + "/" + year.ToString();
            DateTime theDate;
            int weekday;

            if (DateTime.TryParse(strDate, out theDate) == true)
            {
                theDate = Convert.ToDateTime(strDate);
                weekday = (int)theDate.DayOfWeek;
            }
            else weekday = -1;

            return (weekday);
        }

        /// <summary>
        /// Μετατρέπει τον αριθμό του μήνα σε λεκτικό
        /// στη γενική πτώση.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string monthToGRstring(int m)
        {
            string stGRmonth = "";

            switch (m)
            {
                case 1: stGRmonth = "Ιανουαρίου"; break;
                case 2: stGRmonth = "Φεβρουαρίου"; break;
                case 3: stGRmonth = "Μαρτίου"; break;
                case 4: stGRmonth = "Απριλίου"; break;
                case 5: stGRmonth = "Μαϊου"; break;
                case 6: stGRmonth = "Ιουνίου"; break;
                case 7: stGRmonth = "Ιουλίου"; break;
                case 8: stGRmonth = "Αυγούστου"; break;
                case 9: stGRmonth = "Σεπτεμβρίου"; break;
                case 10: stGRmonth = "Οκτωβρίου"; break;
                case 11: stGRmonth = "Νοεμβρίου"; break;
                case 12: stGRmonth = "Δεκεμβρίου"; break;
                default: break;
            }
            return stGRmonth;
        }

        /// <summary>
        /// Ελέγχει αν η αρχική ημερομηνία είναι μικρότερη
        /// ή ίση με την τελική ημερομηνία.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public bool ValidStartEndDates(DateTime dateStart, DateTime dateEnd)
        {
            bool result;

            if (dateStart > dateEnd)
                result = false;
            else
                result = true;
            return result;
        }

        /// <summary>
        /// Ελέγχει αν δύο ημερομηνίες ανήκουν στο ίδιο έτος.
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public bool DatesInSameYear(DateTime date1, DateTime date2)
        {
            bool result;

            if (date1.Year != date2.Year)
                result = false;
            else
                result = true;
            return result;
        }

        /// <summary>
        /// Ελέγχει αν δύο ημερομηνίες είναι μέσα στο ίδιο Σχ. Έτος
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="schoolYearID"></param>
        /// <returns></returns>
        public bool DatesInSchoolYear(DateTime dateStart, DateTime dateEnd, int schoolYearID)
        {
            bool result = true;

            var schoolYear = (from s in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ
                              where s.SCHOOLYEAR_ID == schoolYearID
                              select new { s.ΗΜΝΙΑ_ΕΝΑΡΞΗ, s.ΗΜΝΙΑ_ΛΗΞΗ }).FirstOrDefault();

            if (dateStart < schoolYear.ΗΜΝΙΑ_ΕΝΑΡΞΗ || dateEnd > schoolYear.ΗΜΝΙΑ_ΛΗΞΗ)
                result = false;

            return result;
        }

        /// <summary>
        /// Ελέγχει αν το σχολικό έτος έχει τη μορφή ΝΝΝΝ-ΝΝΝΝ
        /// και αν τα έτη είναι συμβατά με τις ημερομηνίες
        /// έναρξης και λήξης.
        /// </summary>
        /// <param name="syear"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public bool VerifySchoolYear(string syear, DateTime d1, DateTime d2)
        {

            if (syear.IndexOf('-') == -1)
            {
                //ShowAdminMessage("Το σχολικό έτος πρέπει να είναι στη μορφή έτος1-έτος2.");
                return false;
            }

            string[] split = syear.Split(new Char[] { '-' });
            string sy1 = Convert.ToString(split[0]);
            string sy2 = Convert.ToString(split[1]);

            if (!isNumber(sy1) || !isNumber(sy2))
            {
                //ShowAdminMessage("Τα έτη δεν είναι αριθμοί.");
                return false;
            }
            else
            {
                int y1 = Convert.ToInt32(sy1);
                int y2 = Convert.ToInt32(sy2);

                if (y2 - y1 > 1 || y2 - y1 <= 0)
                {
                    //UserFunctions.ShowAdminMessage("Τα έτη δεν είναι σωστά.");
                    return false;
                }
                if (d1.Year != y1 || d2.Year != y2)
                {
                    //UserFunctions.ShowAdminMessage("Οι ημερομηνίες δεν συμφωνούν με τα έτη.");
                    return false;
                }
            }
            // at this point everything is ok
            return true;
        }

        /// <summary>
        /// Ελέγχει αν το χολικό έτος μορφής ΝΝΝΝ-ΝΝΝΝ υπάρχει ήδη.
        /// </summary>
        /// <param name="syear"></param>
        /// <returns></returns>
        public bool SchoolYearExists(int syear)
        {
            AbacusDBEntities db = new AbacusDBEntities();

            var syear_recs = (from s in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ
                              where s.SCHOOLYEAR_ID == syear
                              select s).Count();

            if (syear_recs != 0)
            {
                //ShowAdminMessage("Το σχολικό έτος υπάρχει ήδη.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Υπολογίζει τα έτη (στρογγυλοποιημένα) μεταξύ δύο ημερομηνιών
        /// </summary>
        /// <param name="sdate">αρχική ημερομηνία</param>
        /// <param name="edate">τελική ημερομηνία</param>
        /// <returns></returns>
        public int YearsDiff(DateTime sdate, DateTime edate)
        {
            TimeSpan ts = edate - sdate;
            int days = ts.Days;

            double _years = days / 365;

            int years = Convert.ToInt32(Math.Ceiling(_years));

            return years;
        }

        public int DaysDiff(DateTime sdate, DateTime edate)
        {
            int ndays = 1 + Convert.ToInt32((edate - sdate).TotalDays);

            return ndays;
        }

        public int WeekDays(DateTime d0, DateTime d1)
        {
            int ndays = 1 + Convert.ToInt32((d1 - d0).TotalDays);

            int nsaturdays = (ndays + Convert.ToInt32(d0.DayOfWeek)) / 7;

            int result = ndays - 2 * nsaturdays - (d0.DayOfWeek == DayOfWeek.Sunday ? 1 : 0) + (d1.DayOfWeek == DayOfWeek.Saturday ? 1 : 0);

            return result;
        }
        

        public int GetMonthNumber(DateTime? _date)
        {
            int _month = 0;
            if (_date != null)
            {
                _month = _date.Value.Month;
            }
            return (_month);
        }

        private int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        #endregion

        #region AFM validation

        /// ------------------------------------------------------------------------
        /// CheckAFM: Ελέγχει αν ένα ΑΦΜ είναι σωστό
        /// Το ΑΦΜ που θα ελέγξουμε
        /// true = ΑΦΜ σωστό, false = ΑΦΜ Λάθος
        /// Αυτή είναι η χρησιμοποιούμενη μεθοδος.
        /// Προσθήκη: Αποκλεισμός όταν όλα τα ψηφία = 0 (ο αλγόριθμος τα δέχεται!)
        /// Ημ/νια: 12/3/2013
        /// ------------------------------------------------------------------------
        public bool CheckAFM(string cAfm)
        {
            int nExp = 1;
            // Ελεγχος αν περιλαμβάνει μόνο γράμματα
            try { long nAfm = Convert.ToInt64(cAfm); }

            catch (Exception) { return false; }

            // Ελεγχος μήκους ΑΦΜ
            if (string.IsNullOrWhiteSpace(cAfm))
            {
                return false;
            }

            cAfm = cAfm.Trim();
            int nL = cAfm.Length;
            if (nL != 9) return false;

            // Έλεγχος αν όλα τα ψηφία είναι 0
            var count = cAfm.Count(x => x == '0');
            if (count == cAfm.Length) return false;

            //Υπολογισμός αν το ΑΦΜ είναι σωστό

            int nSum = 0;
            int xDigit = 0;
            int nT = 0;

            for (int i = nL - 2; i >= 0; i--)
            {
                xDigit = int.Parse(cAfm.Substring(i, 1));
                nT = xDigit * (int)(Math.Pow(2, nExp));
                nSum += nT;
                nExp++;
            }

            xDigit = int.Parse(cAfm.Substring(nL - 1, 1));

            nT = nSum / 11;
            int k = nT * 11;
            k = nSum - k;
            if (k == 10) k = 0;
            if (xDigit != k) return false;

            return true;

        }

        #endregion

        #region Converters

        public string DayNumberToText(int number)
        {
            string s;
            string suffix1 = "ημέρα";
            string suffix = "ημέρες";
            string[] tens = new string[] { "είκοσι ", "τριάντα ", "σαράντα ", "πενήντα ", "εξήντα ", "εβδομήντα ", "οδόντα ", "ενενήντα ", "εκατό ", "εκατόν " };
            string[] digits = new string[] { "μηδέν ", "μία ", "δύο ", "τρεις ", "τέσσερις ", "πέντε ", "έξι ", "επτά ", "οκτώ ", "εννέα " };
            string[] teens = new string[] { "δέκα ", "έντεκα ", "δώδεκα ", "δεκατρείς ", "δεκατέσσερις ", "δεκαπέντε ", "δεκαέξι", "δεκαεπτά ", "δεκαοκτώ ", "δεκαεννέα " };

            if (number == 0) s = digits[0] + suffix;
            else if (number == 1) s = digits[1] + suffix1;
            else if (number == 2) s = digits[2] + suffix;
            else if (number == 3) s = digits[3] + suffix;
            else if (number == 4) s = digits[4] + suffix;
            else if (number == 5) s = digits[5] + suffix;
            else if (number == 6) s = digits[6] + suffix;
            else if (number == 7) s = digits[7] + suffix;
            else if (number == 8) s = digits[8] + suffix;
            else if (number == 9) s = digits[9] + suffix;
            else if (number == 10) s = teens[0] + suffix;
            else if (number == 11) s = teens[1] + suffix;
            else if (number == 12) s = teens[2] + suffix;
            else if (number == 13) s = teens[3] + suffix;
            else if (number == 14) s = teens[4] + suffix;
            else if (number == 15) s = teens[5] + suffix;
            else if (number == 16) s = teens[6] + suffix;
            else if (number == 17) s = teens[7] + suffix;
            else if (number == 18) s = teens[8] + suffix;
            else if (number == 19) s = teens[9] + suffix;
            else if (number == 20) s = tens[0] + suffix;
            else if (number == 21) s = tens[0] + digits[1] + suffix;
            else if (number == 22) s = tens[0] + digits[2] + suffix;
            else if (number == 23) s = tens[0] + digits[3] + suffix;
            else if (number == 24) s = tens[0] + digits[4] + suffix;
            else if (number == 25) s = tens[0] + digits[5] + suffix;
            else if (number == 26) s = tens[0] + digits[6] + suffix;
            else if (number == 27) s = tens[0] + digits[7] + suffix;
            else if (number == 28) s = tens[0] + digits[8] + suffix;
            else if (number == 29) s = tens[0] + digits[9] + suffix;
            else if (number == 30) s = tens[1] + suffix;
            else if (number == 31) s = tens[1] + digits[1] + suffix;
            else if (number == 32) s = tens[1] + digits[2] + suffix;
            else if (number == 33) s = tens[1] + digits[3] + suffix;
            else if (number == 34) s = tens[1] + digits[4] + suffix;
            else if (number == 35) s = tens[1] + digits[5] + suffix;
            else if (number == 36) s = tens[1] + digits[6] + suffix;
            else if (number == 37) s = tens[1] + digits[7] + suffix;
            else if (number == 38) s = tens[1] + digits[8] + suffix;
            else if (number == 39) s = tens[1] + digits[9] + suffix;
            else if (number == 40) s = tens[2] + suffix;
            else if (number == 41) s = tens[2] + digits[1] + suffix;
            else if (number == 42) s = tens[2] + digits[2] + suffix;
            else if (number == 43) s = tens[2] + digits[3] + suffix;
            else if (number == 44) s = tens[2] + digits[4] + suffix;
            else if (number == 45) s = tens[2] + digits[5] + suffix;
            else if (number == 46) s = tens[2] + digits[6] + suffix;
            else if (number == 47) s = tens[2] + digits[7] + suffix;
            else if (number == 48) s = tens[2] + digits[8] + suffix;
            else if (number == 49) s = tens[2] + digits[9] + suffix;
            else if (number == 50) s = tens[3] + suffix;
            else if (number == 51) s = tens[3] + digits[1] + suffix;
            else if (number == 52) s = tens[3] + digits[2] + suffix;
            else if (number == 53) s = tens[3] + digits[3] + suffix;
            else if (number == 54) s = tens[3] + digits[4] + suffix;
            else if (number == 55) s = tens[3] + digits[5] + suffix;
            else if (number == 56) s = tens[3] + digits[6] + suffix;
            else if (number == 57) s = tens[3] + digits[7] + suffix;
            else if (number == 58) s = tens[3] + digits[8] + suffix;
            else if (number == 59) s = tens[3] + digits[9] + suffix;
            else if (number == 60) s = tens[4] + suffix;
            else if (number == 61) s = tens[4] + digits[1] + suffix;
            else if (number == 62) s = tens[4] + digits[2] + suffix;
            else if (number == 63) s = tens[4] + digits[3] + suffix;
            else if (number == 64) s = tens[4] + digits[4] + suffix;
            else if (number == 65) s = tens[4] + digits[5] + suffix;
            else if (number == 66) s = tens[4] + digits[6] + suffix;
            else if (number == 67) s = tens[4] + digits[7] + suffix;
            else if (number == 68) s = tens[4] + digits[8] + suffix;
            else if (number == 69) s = tens[4] + digits[9] + suffix;
            else if (number == 70) s = tens[5] + suffix;
            else if (number == 71) s = tens[5] + digits[1] + suffix;
            else if (number == 72) s = tens[5] + digits[2] + suffix;
            else if (number == 73) s = tens[5] + digits[3] + suffix;
            else if (number == 74) s = tens[5] + digits[4] + suffix;
            else if (number == 75) s = tens[5] + digits[5] + suffix;
            else if (number == 76) s = tens[5] + digits[6] + suffix;
            else if (number == 77) s = tens[5] + digits[7] + suffix;
            else if (number == 78) s = tens[5] + digits[8] + suffix;
            else if (number == 79) s = tens[5] + digits[9] + suffix;
            else if (number == 80) s = tens[6] + suffix;
            else if (number == 81) s = tens[6] + digits[1] + suffix;
            else if (number == 82) s = tens[6] + digits[2] + suffix;
            else if (number == 83) s = tens[6] + digits[3] + suffix;
            else if (number == 84) s = tens[6] + digits[4] + suffix;
            else if (number == 85) s = tens[6] + digits[5] + suffix;
            else if (number == 86) s = tens[6] + digits[6] + suffix;
            else if (number == 87) s = tens[6] + digits[7] + suffix;
            else if (number == 88) s = tens[6] + digits[8] + suffix;
            else if (number == 89) s = tens[6] + digits[9] + suffix;
            else if (number == 90) s = tens[7] + suffix;
            else if (number == 91) s = tens[7] + digits[1] + suffix;
            else if (number == 92) s = tens[7] + digits[2] + suffix;
            else if (number == 93) s = tens[7] + digits[3] + suffix;
            else if (number == 94) s = tens[7] + digits[4] + suffix;
            else if (number == 95) s = tens[7] + digits[5] + suffix;
            else if (number == 96) s = tens[7] + digits[6] + suffix;
            else if (number == 97) s = tens[7] + digits[7] + suffix;
            else if (number == 98) s = tens[7] + digits[8] + suffix;
            else if (number == 99) s = tens[7] + digits[9] + suffix;
            else if (number == 100) s = tens[8] + suffix;

            else s = "";

            return (s);
        }

        #endregion

        #region Custom Abacus Functions

        public float Max(params float[] values)
        {
            return Enumerable.Max(values);
        }
        public float Min(params float[] values)
        {
            return Enumerable.Min(values);
        }

        public bool ValidateBirthdate(ΠΑΙΔΙΑ child)
        {
            if (child.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ == null) return false;

            DateTime _birthdate = (DateTime)child.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ;

            string refDate = "01/01/" + _birthdate.Year.ToString();

            DateTime childDate;
            childDate = DateTime.ParseExact(refDate, "dd/MM/yyyy", null);
            if (!ValidBirthDate(childDate)) return false;
            else return true;
        }
        public bool ValidBirthDate(DateTime birthdate)
        {
            bool result = true;
            int maxAge = 6;
            int minAge = 0;

            DateTime minDate = DateTime.Today.Date.AddYears(-maxAge);
            DateTime maxDate = DateTime.Today.Date.AddYears(-minAge);

            if (birthdate >= minDate && birthdate <= maxDate)
                result = true;
            else
                result = false;
            return result;
        }

        public int CalculatePersonAge(PersonnelViewModel data)
        {
            int _age = 0;
            int Yob = (int)data.ΕΤΟΣ_ΓΕΝΝΗΣΗ;
            if (Yob > 0)
            {
                _age = DateTime.Now.Year - Yob;
            }
            return _age;
        }

        public AgeYearMonth CalculateAgeInYearsMonths(DateTime DateOfBirth)
        {
            AgeYearMonth theAge = new AgeYearMonth();
                        
            DateTime Now = DateTime.Now;
            int _Years = new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1;

            DateTime _DOBDateNow = DateOfBirth.AddYears(_Years);
    
            int _Months = 0;    
            for (int i = 1; i <= 12; i++)    
            {    
                if (_DOBDateNow.AddMonths(i) == Now)    
                {    
                    _Months = i;    
                    break;    
                }    
                else if (_DOBDateNow.AddMonths(i) >= Now)    
                {    
                    _Months = i - 1;    
                    break;    
                }    
            }    
            int _Days = Now.Subtract(_DOBDateNow.AddMonths(_Months)).Days;

            theAge.Years = _Years;
            theAge.Months = _Months;

            return theAge;    
        }      

        public string CalculateChildAge(ChildViewModel child)
        {
            AgeYearMonth ChildAge = new AgeYearMonth();

            DateTime BirthDate;

            if (child.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ != null)
            {
                BirthDate = ((DateTime)child.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ).Date;
                ChildAge = CalculateAgeInYearsMonths(BirthDate);
            }
            return (ChildAge.Years.ToString() + " έτη και " + ChildAge.Months.ToString() + " μήνες");
        }

        public decimal ChildAgeDecimal(DateTime? birthdate, DateTime? refdate)
        {
            decimal age = 0.0m;

            if (birthdate == null) return age;  // if no valid birthdate abort calculation

            DateTime today = DateTime.Today;

            if (refdate != null) today = (DateTime)refdate;
            // get the last birthday
            DateTime birthDate = (DateTime)birthdate;

            int years = today.Year - birthDate.Year;
            DateTime last = birthDate.AddYears(years);
            if (last > today)
            {
                last = last.AddYears(-1);
                years--;
            }
            // get the next birthday
            DateTime next = last.AddYears(1);
            // calculate the number of days between them
            double yearDays = (next - last).Days;
            // calcluate the number of days since last birthday
            double days = (today - last).Days;
            // calculate exaxt age
            double exactAge = (double)years + (days / yearDays);

            age = (decimal)exactAge;

            return (age);
        }

        public string GetSchoolYearText(int syearId)
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ
                        where d.SCHOOLYEAR_ID == syearId
                        select d).FirstOrDefault();

            string syearText = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            return (syearText);
        }

        public string GetStationUsername(int stationId)
        {
            string username = "";

            var data2 = (from d in db.USER_STATIONS where d.STATION_ID == stationId select d).FirstOrDefault();
            if (data2 != null) username = data2.USERNAME;

            return (username);
        }

        public Guid GetFileGuidFromName(string filename, int uploadId)
        {
            Guid file_id = new Guid();

            var fileData = (from d in db.DOCUPLOADS_FILES where d.FILENAME == filename && d.UPLOAD_ID == uploadId select d).FirstOrDefault();
            if (fileData != null) file_id = fileData.ID;

            return (file_id);
        }

        public Tuple<int, int> GetUploadInfo(int uploadId)
        {
            int station_id = 0;
            int syear_id = 0;

            var upload = (from d in db.DOCUPLOADS where d.UPLOAD_ID == uploadId select d).FirstOrDefault();
            if (upload != null)
            {
                station_id = (int)upload.STATION_ID;
                syear_id = (int)upload.SCHOOLYEAR_ID;
            }

            var data = Tuple.Create(station_id, syear_id);
            return (data);
        }

        public string BuildTmimaName(TmimaViewModel data)
        {
            string tmima_name = "";
            string auto_part = "";

            var data1 = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.SCHOOLYEAR_ID == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ select d).FirstOrDefault();
            var data2 = (from d in db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ where d.CATEGORY_ID == data.ΚΑΤΗΓΟΡΙΑ select d).FirstOrDefault();
            var data3 = (from d in db.ΣΥΣ_ΑΑ_ΛΑΤΙΝΙΚΟΙ where d.AA_ID == data.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ select d).FirstOrDefault();
            if (data1 != null && data2 != null && data3 != null)
            {
                auto_part = "(" + data1.ΣΧΟΛΙΚΟ_ΕΤΟΣ + ") - " + data2.CATEGORY_TEXT + "-" + data3.AA_TEXT;
            }
            else auto_part = "(????-????)-?-?";

            if (!String.IsNullOrEmpty(data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ)) tmima_name = auto_part + " - " + data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ;
            else tmima_name = auto_part;

            return tmima_name;
        }

        public int GetStationFromfChildID(int childid)
        {
            int stationId = 0;

            var data = (from d in db.ΠΑΙΔΙΑ where d.CHILD_ID == childid select d).FirstOrDefault();
            if (data != null)
            {
               stationId = (int)data.ΒΝΣ;
            }
            return (stationId);
        }

        public int GetStationFromfPersonID(int personId)
        {
            int stationId = 0;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.PERSONNEL_ID == personId select d).FirstOrDefault();
            if (data != null)
            {
                stationId = (int)data.ΒΝΣ;
            }
            return (stationId);
        }

        public ΤΜΗΜΑ GetTmimaData(int tmimaId)
        {
            var data = (from d in db.ΤΜΗΜΑ where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId select d).FirstOrDefault();

            return (data);
        }

        public bool IsEducator(int personId)
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.PERSONNEL_ID == personId select d).FirstOrDefault();
            if (data != null)
            {
                if (data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1) return true;
                else return false;
            }
            else return false;
        }

        public string GeneratePassword()
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 1000);
            return String.Format("{0:000}", random);
        }


        /// <summary>
        /// Υπολογίζει τις ημέρες λογιστικού έτους μεταξύ δύο ημερομηνιών,
        /// προσομειώνοντας τη συνάρτηση Days360 του Excel.
        /// </summary>
        /// <param name="initial_date"></param>
        /// <param name="final_date"></param>
        /// <returns name="meres"></returns>
        public int Days360(DateTime initial_date, DateTime final_date)
        {
            DateTime date1 = initial_date;
            DateTime date2 = final_date;

            var y1 = date1.Year;
            var y2 = date2.Year;
            var m1 = date1.Month;
            var m2 = date2.Month;
            var d1 = date1.Day;
            var d2 = date2.Day;

            DateTime tempDate = date1.AddDays(1);
            if (tempDate.Day == 1 && date1.Month == 2)
            {
                d1 = 30;
            }
            if (d2 == 31 && d1 == 30)
            {
                d2 = 30;
            }

            double meres = (y2 - y1) * 360 + (m2 - m1) * 30 + (d2 - d1);
            meres = (meres / 30) * 25;
            meres = Math.Ceiling(meres);

            return Convert.ToInt32(meres);
        }

        public bool DateInCurrentSchoolYear(DateTime _date)
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.ΤΡΕΧΟΝ_ΕΤΟΣ == true select d).FirstOrDefault();
            if (data == null) return false;
            if (_date >= data.ΗΜΝΙΑ_ΕΝΑΡΞΗ && _date <= data.ΗΜΝΙΑ_ΛΗΞΗ) return true;
            else return false;
        }

        public bool DateInSelectedSchoolYear(DateTime _date, int syearId)
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.SCHOOLYEAR_ID == syearId select d).FirstOrDefault();
            if (data == null) return false;
            if (_date >= data.ΗΜΝΙΑ_ΕΝΑΡΞΗ && _date <= data.ΗΜΝΙΑ_ΛΗΞΗ) return true;
            else return false;
        }

        #endregion

        #region ΣΥΝΑΡΤΗΣΕΙΣ ΜΗΝΙΑΙΩΝ ΜΕΤΑΒΟΛΩΝ

        // ----------------------------------------------------------
        // TODO GETTER FOR METABOLES MONTH DAYS FROM srcABSENCE_DAYS
        // Stores total days of absence for each employeee and month
        // Date : 04/03/2020
        // ----------------------------------------------------------

        public int GenerateAbsenceDays(int personId, int schoolyearId, int monthId)
        {
            int days = 0;

            var src = (from d in db.srcABSENCE_DAYS
                       where d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ == personId && d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ == monthId
                       select d).FirstOrDefault();

            if (src != null)
            {
                days = src.ΜΕΡΕΣ_ΑΠΟΥΣΙΑ == null ? 0 : (int)src.ΜΕΡΕΣ_ΑΠΟΥΣΙΑ;
            }

            return (days);
        }

        public int GenerateMetabolesYear(int personId, int schoolyearId, int monthId)
        {
            int year = 0;
            var srcData = (from d in db.srcMETABOLES_REPORT
                           where d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ == personId && d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ == monthId
                           select d).FirstOrDefault();

            if (srcData != null) year = (int)srcData.ΕΤΟΣ;
            else
            {
                var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.SCHOOLYEAR_ID == schoolyearId select d).FirstOrDefault();
                if (data != null)
                {
                    if (monthId >= 9 && monthId <= 12) year = data.ΗΜΝΙΑ_ΕΝΑΡΞΗ.Value.Year;
                    else if (monthId >= 1 && monthId <= 8) year = data.ΗΜΝΙΑ_ΛΗΞΗ.Value.Year;
                }
            }
            return (year);
        }

        public string GenerateMetabolesText(int personId, int schoolyearId, int monthId)
        {
            string prefix_text = "Εργάστηκε κανονικά.";
            string metaboles_text = "";

            var srcData = (from d in db.srcMETABOLES_REPORT
                           where d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ == personId && d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ == monthId
                           select d).ToList();
            if (srcData.Count == 0) metaboles_text = prefix_text;
            else
            {
                // build text with metaboles.
                foreach (var item in srcData)
                {
                    string duration = "(" + item.ΗΜΕΡΕΣ + ") " + DayNumberToText((int)item.ΗΜΕΡΕΣ) + ", ";
                    metaboles_text += item.METABOLI_TEXT + " " + duration + " από " +
                                      item.ΗΜΝΙΑ_ΑΠΟ.Value.Date.ToString("dd/MM/yyyy") + " έως " + item.ΗΜΝΙΑ_ΕΩΣ.Value.Date.ToString("dd/MM/yyyy") + ". ";
                }
                //metaboles_text = prefix_text + " " + metaboles_text;
            }
            return (metaboles_text);
        }


        #endregion

        #region Getters

        public MealPlanData GetMealPlanData(int stationId, DateTime? theDate)
        {
            MealPlanData mpd = new MealPlanData();

            mpd.ΤΡΟΦΙΜΟΙ = 0;
            mpd.ΠΡΟΣΩΠΙΚΟ = 0;
            mpd.ΠΛΗΘΟΣ = 0;
            mpd.ΚΟΣΤΟΣ = 0.0M;
            mpd.ΔΑΠΑΝΗ = 0.0M;

            var data = (from d in db.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ where d.STATION_ID == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate select d).FirstOrDefault();
            if (data != null)
            {
                mpd.ΤΡΟΦΙΜΟΙ = (int)data.ΠΑΙΔΙΑ;
                mpd.ΠΡΟΣΩΠΙΚΟ = (int)data.ΠΡΟΣΩΠΙΚΟ;
                mpd.ΠΛΗΘΟΣ = (int)data.ΑΤΟΜΑ;
                mpd.ΚΟΣΤΟΣ = data.ΚΟΣΤΟΣ_ΗΜΕΡΑ ?? 0.0M;
                mpd.ΔΑΠΑΝΗ = data.ΔΑΠΑΝΗ_ΗΜΕΡΑ ?? 0.0M;
            }
            return (mpd);
        }

        public int GetFirstHour()
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ orderby d.HOUR_TEXT ascending select d).First();
            if (data != null) return data.HOUR_ID;
            else return 1;
        }

        public int GetLastHour()
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ orderby d.HOUR_TEXT descending select d).First();
            if (data != null) return data.HOUR_ID;
            else return 1;
        }

        public decimal GetVATvalue(int vat)
        {
            var data = (from d in db.ΦΠΑ_ΤΙΜΕΣ where d.FPA_ID == vat select d).FirstOrDefault();
            if (data != null) return (decimal)data.FPA_VALUE;
            else return 0.0M;
        }

        public decimal GetVATvalueFromProduct(int? productId)
        {
            decimal result = 0.0M;

            var data = (from d in db.ΠΡΟΙΟΝΤΑ where d.ΠΡΟΙΟΝ_ΚΩΔ == productId select d).FirstOrDefault();
            if (data != null)
            {
                int vat = data.ΠΡΟΙΟΝ_ΦΠΑ != null ? (int)data.ΠΡΟΙΟΝ_ΦΠΑ : 0;
                result = GetVATvalue(vat);
            }
            return result;
        }

        public int GetCurrentSchoolYear()
        {
            int CurrentSchoolYear = 1;
            var syear = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.ΤΡΕΧΟΝ_ΕΤΟΣ == true select d).FirstOrDefault();
            if (syear != null) CurrentSchoolYear = syear.SCHOOLYEAR_ID;

            return (CurrentSchoolYear);
        }

        public int GetGenderFromName(string firstname)
        {
            int gender = 2;

            if (firstname[firstname.Length - 1] == 'Σ' || firstname[firstname.Length - 1] == 'Λ'
                || firstname[firstname.Length - 1] == 'Ν' || firstname[firstname.Length - 1] == 'Μ'
                || firstname[firstname.Length - 1] == 'Ξ' || firstname[firstname.Length - 1] == 'Ρ' || firstname[firstname.Length - 1] == 'Β') gender = 1;

            //int pos1 = firstname.IndexOf("Σ", firstname.Length-2, 1);
            //int pos2 = firstname.IndexOf("Λ", firstname.Length - 2, 1);
            //int pos3 = firstname.IndexOf("Ν", firstname.Length - 2, 1);

            //if (pos1 >= 0 || pos1 >= 0 || pos3 >= 0) gender = 1;
            return (gender);
        }


        #endregion


    }   // class Common

    // View Engine extension of Primus
    public class AbacusViewEngine : RazorViewEngine
    {
        public AbacusViewEngine()
        {
            string[] locations = new string[] {  
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Station/Children/{0}.cshtml",
                "~/Views/Station/Documents/{0}.cshtml",
                "~/Views/Station/Personnel/{0}.cshtml",
                "~/Views/Station/Printouts/{0}.cshtml",
                "~/Views/Station/Programma/{0}.cshtml",
                "~/Views/Station/Expenses/{0}.cshtml",          
                "~/Views/Station/Setup/{0}.cshtml",          
                "~/Views/Station/Statistika/{0}.cshtml",          

                "~/Views/Admin/{1}/{0}.cshtml",
                "~/Views/Admin/Documents/{0}.cshtml",
                "~/Views/Admin/Children/{0}.cshtml",
                "~/Views/Admin/Personnel/{0}.cshtml",
                "~/Views/Admin/Printouts/{0}.cshtml",
                "~/Views/Admin/Programma/{0}.cshtml",
                "~/Views/Admin/Expenses/{0}.cshtml",
                "~/Views/Admin/Statistika/{0}.cshtml",

                "~/Views/Shared/PartialViews/{0}.cshtml",
                "~/Views/Shared/EditorTemplates/{0}.cshtml",
                "~/Views/Shared/Layouts/{0}.cshtml"
                };

            this.ViewLocationFormats = locations;
            this.PartialViewLocationFormats = locations;
            this.MasterLocationFormats = locations;
        }
    }

}   // namespace