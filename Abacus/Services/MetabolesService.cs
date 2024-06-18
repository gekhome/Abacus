using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MetabolesService : IDisposable
    {
        private AbacusDBEntities entities;

        public MetabolesService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<PersonnelMetaboliViewModel> Read(int schoolyearId, int personId)
        {
            var data = (from d in entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ == personId
                        orderby d.ΗΜΝΙΑ_ΑΠΟ descending
                        select new PersonnelMetaboliViewModel
                        {
                            ΜΕΤΑΒΟΛΗ_ΚΩΔ = d.ΜΕΤΑΒΟΛΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΗΜΕΡΕΣ = d.ΗΜΕΡΕΣ,
                            ΗΜΝΙΑ_ΑΠΟ = d.ΗΜΝΙΑ_ΑΠΟ,
                            ΗΜΝΙΑ_ΕΩΣ = d.ΗΜΝΙΑ_ΕΩΣ,
                            ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = d.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                            ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ
                        }).ToList();
            return (data);
        }

        public void Create(PersonnelMetaboliViewModel data, int schoolyearId, int personId)
        {
            ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = new ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ()
            {
                ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = personId,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = schoolyearId,
                ΒΝΣ = Common.GetStationFromPersonID(personId),
                ΗΜΝΙΑ_ΑΠΟ = data.ΗΜΝΙΑ_ΑΠΟ,
                ΗΜΝΙΑ_ΕΩΣ = data.ΗΜΝΙΑ_ΕΩΣ,
                ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = data.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = data.ΗΜΝΙΑ_ΑΠΟ.Value.Month,
                ΗΜΕΡΕΣ = Common.WeekDays((DateTime)data.ΗΜΝΙΑ_ΑΠΟ, (DateTime)data.ΗΜΝΙΑ_ΕΩΣ)
            };
            entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
            entities.SaveChanges();

            data.ΜΕΤΑΒΟΛΗ_ΚΩΔ = entity.ΜΕΤΑΒΟΛΗ_ΚΩΔ;
        }

        public void Update(PersonnelMetaboliViewModel data, int schoolyearId, int personId)
        {
            ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);

            entity.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = personId;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = schoolyearId;
            entity.ΒΝΣ = Common.GetStationFromPersonID(personId);
            entity.ΗΜΝΙΑ_ΑΠΟ = data.ΗΜΝΙΑ_ΑΠΟ;
            entity.ΗΜΝΙΑ_ΕΩΣ = data.ΗΜΝΙΑ_ΕΩΣ;
            entity.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = data.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ;
            entity.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = data.ΗΜΝΙΑ_ΑΠΟ.Value.Month;
            entity.ΗΜΕΡΕΣ = Common.WeekDays((DateTime)data.ΗΜΝΙΑ_ΑΠΟ, (DateTime)data.ΗΜΝΙΑ_ΕΩΣ);

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(PersonnelMetaboliViewModel data)
        {
            ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public PersonnelMetaboliViewModel Refresh(int entityId)
        {
            return entities.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Select(d => new PersonnelMetaboliViewModel
            {
                ΜΕΤΑΒΟΛΗ_ΚΩΔ = d.ΜΕΤΑΒΟΛΗ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΗΜΕΡΕΣ = d.ΗΜΕΡΕΣ,
                ΗΜΝΙΑ_ΑΠΟ = d.ΗΜΝΙΑ_ΑΠΟ,
                ΗΜΝΙΑ_ΕΩΣ = d.ΗΜΝΙΑ_ΕΩΣ,
                ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = d.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ
            }).Where(d => d.ΜΕΤΑΒΟΛΗ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}