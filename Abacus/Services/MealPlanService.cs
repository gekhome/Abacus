using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MealPlanService : IDisposable
    {
        private AbacusDBEntities entities;

        public MealPlanService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DiaitologioViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new DiaitologioViewModel
                        {
                            ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                            ΓΕΥΜΑ_ΒΡΕΦΗ = d.ΓΕΥΜΑ_ΒΡΕΦΗ,
                            ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                            ΓΕΥΜΑ_ΠΡΩΙ = d.ΓΕΥΜΑ_ΠΡΩΙ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                            ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
                        }).ToList();
            return (data);
        }

        public void Create(DiaitologioViewModel data, int stationId)
        {
            MealPlanData mpd = GetMealPlanData(stationId, data.ΗΜΕΡΟΜΗΝΙΑ);

            ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = new ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ()
            {
                ΣΤΑΘΜΟΣ_ΚΩΔ = stationId,
                ΗΜΕΡΟΜΗΝΙΑ = data.ΗΜΕΡΟΜΗΝΙΑ,
                ΓΕΥΜΑ_ΒΡΕΦΗ = data.ΓΕΥΜΑ_ΒΡΕΦΗ,
                ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = data.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                ΓΕΥΜΑ_ΠΡΩΙ = data.ΓΕΥΜΑ_ΠΡΩΙ,
                ΤΡΟΦΙΜΟΙ = mpd.ΤΡΟΦΙΜΟΙ,
                ΠΡΟΣΩΠΙΚΟ = mpd.ΠΡΟΣΩΠΙΚΟ,
                ΠΛΗΘΟΣ = mpd.ΠΛΗΘΟΣ,
                ΚΟΣΤΟΣ = mpd.ΚΟΣΤΟΣ,
                ΔΑΠΑΝΗ = mpd.ΔΑΠΑΝΗ
            };
            entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Add(entity);
            entities.SaveChanges();

            data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = entity.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ;
        }

        public void Update(DiaitologioViewModel data, int stationId)
        {
            MealPlanData mpd = GetMealPlanData(stationId, data.ΗΜΕΡΟΜΗΝΙΑ);

            ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Find(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);

            entity.ΣΤΑΘΜΟΣ_ΚΩΔ = stationId;
            entity.ΗΜΕΡΟΜΗΝΙΑ = data.ΗΜΕΡΟΜΗΝΙΑ;
            entity.ΓΕΥΜΑ_ΠΡΩΙ = data.ΓΕΥΜΑ_ΠΡΩΙ;
            entity.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = data.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ;
            entity.ΓΕΥΜΑ_ΒΡΕΦΗ = data.ΓΕΥΜΑ_ΒΡΕΦΗ;
            entity.ΤΡΟΦΙΜΟΙ = mpd.ΤΡΟΦΙΜΟΙ;
            entity.ΠΡΟΣΩΠΙΚΟ = mpd.ΠΡΟΣΩΠΙΚΟ;
            entity.ΠΛΗΘΟΣ = mpd.ΠΛΗΘΟΣ;
            entity.ΚΟΣΤΟΣ = mpd.ΚΟΣΤΟΣ;
            entity.ΔΑΠΑΝΗ = mpd.ΔΑΠΑΝΗ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DiaitologioViewModel data)
        {
            ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Find(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DiaitologioViewModel Refresh(int entityId)
        {
            return entities.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Select(d => new DiaitologioViewModel
            {
                ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                ΓΕΥΜΑ_ΒΡΕΦΗ = d.ΓΕΥΜΑ_ΒΡΕΦΗ,
                ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                ΓΕΥΜΑ_ΠΡΩΙ = d.ΓΕΥΜΑ_ΠΡΩΙ,
                ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
            }).Where(d => d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        private MealPlanData GetMealPlanData(int stationId, DateTime? theDate)
        {
            MealPlanData mpd = new MealPlanData();

            mpd.ΤΡΟΦΙΜΟΙ = 0;
            mpd.ΠΡΟΣΩΠΙΚΟ = 0;
            mpd.ΠΛΗΘΟΣ = 0;
            mpd.ΚΟΣΤΟΣ = 0.0M;
            mpd.ΔΑΠΑΝΗ = 0.0M;

            var data = (from d in entities.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ where d.STATION_ID == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate select d).FirstOrDefault();
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

        public List<SumPersonsTrofeioViewModel> Aggregate(int stationId, DateTime? theDate)
        {
            List<SumPersonsTrofeioViewModel> data = new List<SumPersonsTrofeioViewModel>();

            if (stationId > 0 && theDate != null)
            {
                data = (from d in entities.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ
                        where d.STATION_ID == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ == theDate)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumPersonsTrofeioViewModel
                        {
                            ROWID = d.ROWID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΠΑΙΔΙΑ = d.ΠΑΙΔΙΑ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΑΤΟΜΑ = d.ΑΤΟΜΑ,
                            ΚΟΣΤΟΣ_ΗΜΕΡΑ = d.ΚΟΣΤΟΣ_ΗΜΕΡΑ,
                            ΔΑΠΑΝΗ_ΗΜΕΡΑ = d.ΔΑΠΑΝΗ_ΗΜΕΡΑ,
                            ΥΠΟΛΟΙΠΟ = d.ΥΠΟΛΟΙΠΟ
                        }).ToList();
            }
            return (data);
        }

        public List<SumPersonsTrofeioViewModel> Aggregate(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<SumPersonsTrofeioViewModel> data = new List<SumPersonsTrofeioViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ
                        where d.STATION_ID == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumPersonsTrofeioViewModel
                        {
                            ROWID = d.ROWID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΠΑΙΔΙΑ = d.ΠΑΙΔΙΑ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΑΤΟΜΑ = d.ΑΤΟΜΑ,
                            ΚΟΣΤΟΣ_ΗΜΕΡΑ = d.ΚΟΣΤΟΣ_ΗΜΕΡΑ,
                            ΔΑΠΑΝΗ_ΗΜΕΡΑ = d.ΔΑΠΑΝΗ_ΗΜΕΡΑ,
                            ΥΠΟΛΟΙΠΟ = d.ΥΠΟΛΟΙΠΟ
                        }).ToList();
            }
            return (data);
        }

        public List<sqlMealPlanViewModel> Search(int? stationId, DateTime? theDate1, DateTime? theDate2)
        {
            var data = (from d in entities.sqlΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new sqlMealPlanViewModel
                        {
                            ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                            DAY_NUM = d.DAY_NUM,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                            ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
                        }).ToList();

            return (data);
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}