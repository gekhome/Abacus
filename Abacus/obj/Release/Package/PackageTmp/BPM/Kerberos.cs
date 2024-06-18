using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;

namespace Abacus.BPM
{
    public class Kerberos
    {
        AbacusDBEntities db = new AbacusDBEntities();
        Common c = new Common();

        public const int TICKET_TIMEOUT_MINUTES = 280;

        /// <summary>
        /// Υπολογίζει τις εργάσιμες ημέρες μεταξύ δύο ημερομηνιών,
        /// δηλ. χωρίς τα Σαββατοκύριακα.
        /// </summary>
        /// <param name="initial_date"></param>
        /// <param name="final_date"></param>
        /// <returns name="daycount"></returns>
        public int WorkingDays(DateTime initial_date, DateTime final_date)
        {
            int daycount = 0;

            DateTime date1 = initial_date;
            DateTime date2 = final_date;

            while (date1 <= date2)
            {
                switch (date1.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        break;
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        daycount++;
                        break;
                    default:
                        break;
                }
                date1 = date1.AddDays(1);
            }
            return daycount;
        }


        #region ΚΑΝΟΝΕΣ ΔΙΑΓΡΑΦΗΣ ΠΑΡΑΜΕΤΡΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        public bool CanDeleteMealMorning(int recordId)
        {
            var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΠΡΩΙ == recordId select d).ToList();
            if (data.Count > 0)
                return false;
            else
                return true;
        }

        public bool CanDeleteMealNoon(int recordId)
        {
            var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ == recordId select d).ToList();
            if (data.Count > 0)
                return false;
            else
                return true;
        }

        public bool CanDeleteMealBaby(int recordId)
        {
            var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ where d.ΓΕΥΜΑ_ΒΡΕΦΗ == recordId select d).ToList();
            if (data.Count > 0)
                return false;
            else
                return true;
        }

        public bool CanDeleteApoxorisiType(int apoxorisiId)
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ == apoxorisiId select d).ToList();
            if (data.Count > 0)
                return false;
            else
                return true;
        }

        public bool CanDeleteProduct(int productId)
        {
            var data1 = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ where d.ΠΡΟΙΟΝ == productId select d).ToList();
            var data2 = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ where d.ΠΡΟΙΟΝ == productId select d).ToList();
            var data3 = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ where d.ΠΡΟΙΟΝ == productId select d).ToList();
            if (data1.Count > 0 || data2.Count > 0 || data3.Count > 0) return false;
            else return true;
        }

        public bool CanDeleteVATvalue(int vatId)
        {
            var data = (from d in db.ΠΡΟΙΟΝΤΑ where d.ΠΡΟΙΟΝ_ΦΠΑ == vatId select d).ToList();
            if (data.Count > 0) return false;
            else return true;
        }

        public bool CanDeleteExtraCategory(int categoryId)
        {
            var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ where d.ΚΑΤΗΓΟΡΙΑ == categoryId select d).ToList();
            if (data.Count > 0) return false;
            else return true;
        }

        public bool CanDeleteCategory(int categoryId)
        {
            var data = (from d in db.ΠΡΟΙΟΝΤΑ where d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ == categoryId select d).ToList();
            if (data.Count > 0) return false;
            else return true;
        }

        public bool CanDeleteSchoolYear(int schoolyearId)
        {
            return false;
        }

        public bool CanDeleteHour(int hourId)
        {
            return false;
        }

        public bool CanDeletePeriferiaki()
        {
            return false;
        }

        public bool CanDeleteEidikotita(int eidikotitaId)
        {
            return false;
        }

        public bool CanDeleteTmima(int tmimaId)
        {
            return false;
        }

        public bool CanDeleteTmimaCategory(int categoryId)
        {
            return false;
        }

        public bool CanDeletePersonnelType(int prosopikoId)
        {
            return false;
        }

        public bool CanDeleteMetaboliType(int prosopikoId)
        {
            return false;
        }

        public bool CanDeleteChild(int childId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ where d.ΠΑΙΔΙ_ΚΩΔ == childId select d).ToList();
            if (data.Count > 0) return false;
            else return true;
        }

        public bool CanDeletePerson(int personId)
        {
            var data1 = (from d in db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ where d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId select d).ToList();
            var data2 = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.PERSON_ID == personId select d).ToList();

            if (data1.Count > 0 || data2.Count > 0) return false;
            else return true;
        }

        public bool CanDeleteDiaxiristis(int adminId)
        {
            return false;
        }

        public bool CanDeleteOperator(int operatorId)
        {
            var data1 = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ where d.ADMIN_ID == operatorId select d).ToList();
            var data2 = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ where d.ADMIN_ID == operatorId select d).ToList();
            if (data1.Count == 0 && data2.Count == 0) 
                return true;
            else 
                return false;
        }

        public bool CanDeleteUpload(int uploadId)
        {
            var data = (from d in db.DOCUPLOADS_FILES where d.UPLOAD_ID == uploadId select d).ToList();
            if (data.Count > 0) return false;
            else return true;
        }

        #endregion  ΚΑΝΟΝΕΣ ΔΙΑΓΡΑΦΗΣ ΠΑΡΑΜΕΤΡΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        #region ΓΕΝΙΚΟΙ ΕΛΕΓΧΟΙ ΕΠΙΚΥΡΩΣΗΣ ΔΕΔΟΜΕΝΩΝ

        public bool existsUsername(string username)
        {
            var userAdmins = db.USER_ADMINS.Where(u => u.USERNAME == username).FirstOrDefault(); ;
            var userStations = db.USER_STATIONS.Where(u => u.USERNAME == username).FirstOrDefault();

            return (userAdmins != null || userStations != null);
        }

        public bool ValidatePrimaryKeyChild(int am, int stationId)
        {
            var existingdata = db.ΠΑΙΔΙΑ.Where(s => s.ΑΜ == am && s.ΒΝΣ == stationId).ToList();
            if (existingdata.Count == 0) return true;
            else return false;
        }

        public bool ValidatePrimaryKeyPerson(int am, int stationId)
        {
            if (am == 0)
                return true;

            var existingdata = db.ΠΡΟΣΩΠΙΚΟ.Where(s => s.ΜΗΤΡΩΟ == am && s.ΒΝΣ == stationId).ToList();
            if (existingdata.Count == 0) return true;
            else return false;
        }

        public bool IsValidAM(PersonnelGridViewModel p)
        {
            if (p.ΜΗΤΡΩΟ == 0 && p.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ != 5)
                return false;
            return true;
        }

        public string ValidatePersonFields(ΠΡΟΣΩΠΙΚΟ p)
        {
            string errMsg = "";

            bool apoxorisiError1 = p.ΑΠΟΧΩΡΗΣΕ == true && (p.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ == null || p.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ == null);
            bool apoxorisiError2 = p.ΑΠΟΧΩΡΗΣΕ == false && (p.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ > 0 || p.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ != null);

            if (!c.CheckAFM(p.ΑΦΜ)) 
                errMsg += "-> Το ΑΦΜ δεν είναι έγκυρο. ";
            if (p.ΜΗΤΡΩΟ == 0 && p.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ != 5)
                errMsg += "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.";
            if (apoxorisiError1) 
                errMsg += "-> Έχει τσεκαριστεί αποχώρηση, οπότε πρέπει να συμπληρωθεί αιτία και ημ/νία αποχώρησης.";
            if (apoxorisiError2) 
                errMsg += "-> Δεν έχει τσεκαριστεί αποχώρηση, οπότε αιτία και ημ/νία αποχώρησης πρέπει να είναι κενά.";

            return (errMsg);
        }

        public bool MealsMorningExist(int stationId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count > 0) 
                return true;
            else
                return false;
        }

        public bool MealsNoonExist(int stationId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count > 0)
                return true;
            else
                return false;
        }

        public bool MealsBabyExist(int stationId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count > 0)
                return true;
            else
                return false;
        }

        public bool CanPopulateMeals()
        {
            var data1 = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ select d).ToList();
            var data2 = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ select d).ToList();
            var data3 = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ select d).ToList();
            if (data1.Count > 0 && data2.Count > 0 && data3.Count > 0)
                return true;
            else
                return false;
        }

        #endregion


    }
}