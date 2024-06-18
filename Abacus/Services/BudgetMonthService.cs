using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class BudgetMonthService : IDisposable
    {
        private AbacusDBEntities entities;

        public BudgetMonthService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<BudgetDataViewModel> Read(int schoolyearId, int monthId)
        {
            var data = (from d in entities.BUDGET_DATA
                        orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.BUDGET_MONTH == monthId
                        select new BudgetDataViewModel
                        {
                            BUDGET_ID = d.BUDGET_ID,
                            STATION_ID = d.STATION_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            CHILDREN_NUM = d.CHILDREN_NUM ?? 0,
                            PERSONNEL_NUM = d.PERSONNEL_NUM ?? 0,
                            BUDGET_MONTH = d.BUDGET_MONTH,
                            BUDGET_CLEAN = d.BUDGET_CLEAN,
                            BUDGET_OTHER = d.BUDGET_OTHER,
                            BUDGET_FOOD = d.BUDGET_FOOD
                        }).ToList();
            return (data);
        }

        public void Create(BudgetDataViewModel data, int schoolyearId, int monthId)
        {
            int days = 22;
            decimal trofeio_atomo = GetTrofeioAtomo(data.STATION_ID);

            BUDGET_DATA entity = new BUDGET_DATA()
            {
                SCHOOLYEAR_ID = schoolyearId,
                BUDGET_MONTH = monthId,
                STATION_ID = data.STATION_ID,
                CHILDREN_NUM = data.CHILDREN_NUM,
                PERSONNEL_NUM = data.PERSONNEL_NUM,
                BUDGET_CLEAN = data.BUDGET_CLEAN,
                BUDGET_OTHER = data.BUDGET_OTHER,
                BUDGET_FOOD = trofeio_atomo * days * (data.CHILDREN_NUM + data.PERSONNEL_NUM)
            };
            entities.BUDGET_DATA.Add(entity);
            entities.SaveChanges();

            data.BUDGET_ID = entity.BUDGET_ID;
        }

        public void Update(BudgetDataViewModel data, int schoolyearId, int monthId)
        {
            int days = 22;
            decimal trofeio_atomo = GetTrofeioAtomo(data.STATION_ID);

            BUDGET_DATA entity = entities.BUDGET_DATA.Find(data.BUDGET_ID);

            entity.SCHOOLYEAR_ID = schoolyearId;
            entity.BUDGET_MONTH = monthId;
            entity.STATION_ID = data.STATION_ID;
            entity.CHILDREN_NUM = data.CHILDREN_NUM;
            entity.PERSONNEL_NUM = data.PERSONNEL_NUM;
            entity.BUDGET_CLEAN = data.BUDGET_CLEAN;
            entity.BUDGET_OTHER = data.BUDGET_OTHER;
            entity.BUDGET_FOOD = trofeio_atomo * days * (data.CHILDREN_NUM + data.PERSONNEL_NUM);

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(BudgetDataViewModel data)
        {
            BUDGET_DATA entity = entities.BUDGET_DATA.Find(data.BUDGET_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.BUDGET_DATA.Remove(entity);
                entities.SaveChanges();
            }
        }

        public BudgetDataViewModel Refresh(int entityId)
        {
            return entities.BUDGET_DATA.Select(d => new BudgetDataViewModel
            {
                BUDGET_ID = d.BUDGET_ID,
                STATION_ID = d.STATION_ID,
                SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                CHILDREN_NUM = d.CHILDREN_NUM ?? 0,
                PERSONNEL_NUM = d.PERSONNEL_NUM ?? 0,
                BUDGET_MONTH = d.BUDGET_MONTH,
                BUDGET_CLEAN = d.BUDGET_CLEAN,
                BUDGET_OTHER = d.BUDGET_OTHER,
                BUDGET_FOOD = d.BUDGET_FOOD
            }).Where(d => d.BUDGET_ID.Equals(entityId)).FirstOrDefault();
        }

        private decimal GetTrofeioAtomo(int? stationId)
        {
            decimal trofeio_atomo = 0.00M;

            var trofeio = (from d in entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ where d.STATION_ID == stationId select d).FirstOrDefault();
            if (trofeio != null)
                trofeio_atomo = (decimal)trofeio.COST_PERSON;

            return trofeio_atomo;
        }

        public string TransferMonth(int schoolyearId, int monthId)
        {
            string msg = "Η μεταφορά των στοιχείων στον επόμενο μήνα ολοκληρώθηκε.";

            var srcdata = (from d in entities.BUDGET_DATA where d.SCHOOLYEAR_ID == schoolyearId && d.BUDGET_MONTH == monthId orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ select d).ToList();
            if (srcdata.Count == 0)
            {
                msg = "Δεν βρέθηκαν δεδομένα προέλευσης για μεταφορά τους στον επόμενο μήνα.";
                return msg;
            }
            int target_month = monthId + 1;
            if (target_month > 12)
            {
                msg = "Δεν μπορεί να γίνει μεταφορά του τελευταίου μήνα (Δεκέμβριος) στον επόμενο.";
                return msg;
            }
            var trgdata = (from d in entities.BUDGET_DATA where d.SCHOOLYEAR_ID == schoolyearId && d.BUDGET_MONTH == target_month select d).ToList();
            if (trgdata.Count > 0)
            {
                msg = "Βρέθηκαν δεδομένα στο μήνα προορισμού. Η μεταφορά ακυρώθηκε.";
                return msg;
            }

            foreach (var item in srcdata)
            {
                BUDGET_DATA entity = new BUDGET_DATA()
                {
                    BUDGET_MONTH = target_month,
                    SCHOOLYEAR_ID = item.SCHOOLYEAR_ID,
                    STATION_ID = item.STATION_ID,
                    CHILDREN_NUM = item.CHILDREN_NUM,
                    PERSONNEL_NUM = item.PERSONNEL_NUM,
                    BUDGET_CLEAN = item.BUDGET_CLEAN,
                    BUDGET_FOOD = item.BUDGET_FOOD,
                    BUDGET_OTHER = item.BUDGET_OTHER
                };
                entities.BUDGET_DATA.Add(entity);
                entities.SaveChanges();
            };
            return msg;
        }

        public string TransferYear(int schoolyearId)
        {
            string msg = "Η μεταφορά των στοιχείων στο επόμενο σχολικό έτος ολοκληρώθηκε.";

            var srcdata = (from d in entities.BUDGET_DATA where d.SCHOOLYEAR_ID == schoolyearId orderby d.BUDGET_MONTH, d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ select d).ToList();
            if (srcdata.Count == 0)
            {
                msg = "Δεν βρέθηκαν δεδομένα προέλευσης για μεταφορά τους στον επόμενο μήνα.";
                return msg;
            }
            var chkdata = (from d in entities.BUDGET_DATA where d.SCHOOLYEAR_ID == schoolyearId orderby d.BUDGET_MONTH descending select d).First();
            if (chkdata.BUDGET_MONTH < 12)
            {
                msg = "Τα δεδομένα προέλευσης δεν περιλαμβάνουν όλους τους μήνες του έτους. Η μεταφορά ακυρώθηκε.";
                return msg;
            }

            int target_year = schoolyearId + 1;

            var syear = (from d in entities.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.SCHOOLYEAR_ID == target_year select d).FirstOrDefault();
            if (syear == null)
            {
                msg = "Το σχολικό έτος προορισμού δεν υπάρχει καταχωρημένο. Η μεταφορά ακυρώθηκε.";
                return msg;
            }
            var trgdata = (from d in entities.BUDGET_DATA where d.SCHOOLYEAR_ID == target_year select d).ToList();
            if (trgdata.Count > 0)
            {
                msg = "Βρέθηκαν δεδομένα στο σχολικό έτος προορισμού. Η μεταφορά ακυρώθηκε.";
                return msg;
            }

            foreach (var item in srcdata)
            {
                BUDGET_DATA entity = new BUDGET_DATA()
                {
                    SCHOOLYEAR_ID = target_year,
                    BUDGET_MONTH = item.BUDGET_MONTH,
                    STATION_ID = item.STATION_ID,
                    CHILDREN_NUM = item.CHILDREN_NUM,
                    PERSONNEL_NUM = item.PERSONNEL_NUM,
                    BUDGET_CLEAN = item.BUDGET_CLEAN,
                    BUDGET_FOOD = item.BUDGET_FOOD,
                    BUDGET_OTHER = item.BUDGET_OTHER
                };
                entities.BUDGET_DATA.Add(entity);
                entities.SaveChanges();
            };
            return msg;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}