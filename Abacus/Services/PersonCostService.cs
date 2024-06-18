using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class PersonCostService : IDisposable
    {
        private AbacusDBEntities entities;

        public PersonCostService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<PersonCostDayViewModel> Read(int schoolyearId)
        {
            var data = (from d in entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ
                        where d.SCHOOLYEARID == schoolyearId
                        orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ
                        select new PersonCostDayViewModel
                        {
                            ID = d.ID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            COST_PERSON = d.COST_PERSON
                        }).ToList();

            return (data);
        }

        public void Create(PersonCostDayViewModel data, int schoolyearId)
        {
            ΤΡΟΦΕΙΟ_ΑΤΟΜΟ entity = new ΤΡΟΦΕΙΟ_ΑΤΟΜΟ()
            {
                SCHOOLYEARID = schoolyearId,
                STATION_ID = data.STATION_ID,
                COST_PERSON = data.COST_PERSON
            };
            entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Add(entity);
            entities.SaveChanges();

            data.ID = entity.ID;
        }

        public void Update(PersonCostDayViewModel data, int schoolyearId)
        {
            ΤΡΟΦΕΙΟ_ΑΤΟΜΟ entity = entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Find(data.ID);

            entity.SCHOOLYEARID = schoolyearId;
            entity.STATION_ID = data.STATION_ID;
            entity.COST_PERSON = data.COST_PERSON;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(PersonCostDayViewModel data)
        {
            ΤΡΟΦΕΙΟ_ΑΤΟΜΟ entity = entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Find(data.ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public PersonCostDayViewModel Refresh(int entityId)
        {
            return entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Select(d => new PersonCostDayViewModel
            {
                ID = d.ID,
                SCHOOLYEARID = d.SCHOOLYEARID,
                STATION_ID = d.STATION_ID,
                COST_PERSON = d.COST_PERSON
            }).Where(d => d.ID.Equals(entityId)).FirstOrDefault();
        }

        public string Transfer(int schoolyearId)
        {
            string msg = "";

            var srcData = (from d in entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ where d.SCHOOLYEARID == schoolyearId orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ select d).ToList();
            if (srcData.Count == 0)
            {
                msg = "Δεν βρέθηκαν δεδομένα του επιλεγμένου έτους για μεταφορά. Η διαδικασία ακυρώθηκε.";
                return msg;
            }
            int target_year = schoolyearId + 1;

            var syearRecord = (from d in entities.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.SCHOOLYEAR_ID == target_year select d).FirstOrDefault();
            if (syearRecord == null)
            {
                msg = "Το σχολκό έτος προορισμού δεν υπάρχει καταχωρημένο στα σχολικά έτη. Η διαδικασία ακυρώθηκε.";
                return msg;
            }

            var trgData = (from d in entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ where d.SCHOOLYEARID == target_year select d).ToList();
            if (trgData.Count > 0)
            {
                msg = "Βρέθηκαν δεδομένα στο σχολικό έτος προορισμού. Η διαδικασία ακυρώθηκε.";
                return msg;
            }
            // Good to go
            foreach (var item in srcData)
            {
                ΤΡΟΦΕΙΟ_ΑΤΟΜΟ target = new ΤΡΟΦΕΙΟ_ΑΤΟΜΟ()
                {
                    SCHOOLYEARID = target_year,
                    STATION_ID = item.STATION_ID,
                    COST_PERSON = item.COST_PERSON
                };
                entities.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ.Add(target);
                entities.SaveChanges();
            };
            msg = "Η μεταφορά των δεδομένων ολοκληρώθηκε.";
            return msg;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}