using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class PersonnelService : IDisposable
    {
        private AbacusDBEntities entities;

        public PersonnelService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<PersonnelGridViewModel> Read()
        {
            var data = (from d in entities.ΠΡΟΣΩΠΙΚΟ
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new PersonnelGridViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ ?? 0
                        }).ToList();
            return (data);
        }

        public IEnumerable<PersonnelGridViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΠΡΟΣΩΠΙΚΟ
                        where d.ΒΝΣ == stationId
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new PersonnelGridViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ ?? 0
                        }).ToList();

            return (data);
        }

        public void Create(PersonnelGridViewModel data)
        {
            ΠΡΟΣΩΠΙΚΟ entity = new ΠΡΟΣΩΠΙΚΟ()
            {
                ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ,
                ΒΝΣ = data.ΒΝΣ,
                ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
                ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ
            };
            entities.ΠΡΟΣΩΠΙΚΟ.Add(entity);
            entities.SaveChanges();

            data.PERSONNEL_ID = entity.PERSONNEL_ID;
        }

        public void Create(PersonnelGridViewModel data, int stationId)
        {
            ΠΡΟΣΩΠΙΚΟ entity = new ΠΡΟΣΩΠΙΚΟ()
            {
                ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ,
                ΒΝΣ = stationId,
                ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
                ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ
            };
            entities.ΠΡΟΣΩΠΙΚΟ.Add(entity);
            entities.SaveChanges();

            data.PERSONNEL_ID = entity.PERSONNEL_ID;
        }

        public void Update(PersonnelGridViewModel data)
        {
            ΠΡΟΣΩΠΙΚΟ entity = entities.ΠΡΟΣΩΠΙΚΟ.Find(data.PERSONNEL_ID);

            entity.ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ;
            entity.ΒΝΣ = data.ΒΝΣ;
            entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim();
            entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim();
            entity.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Update(PersonnelGridViewModel data, int stationId)
        {
            ΠΡΟΣΩΠΙΚΟ entity = entities.ΠΡΟΣΩΠΙΚΟ.Find(data.PERSONNEL_ID);

            entity.ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ;
            entity.ΒΝΣ = stationId;
            entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim();
            entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim();
            entity.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(PersonnelGridViewModel data)
        {
            ΠΡΟΣΩΠΙΚΟ entity = entities.ΠΡΟΣΩΠΙΚΟ.Find(data.PERSONNEL_ID);

            try
            {
                if (entity != null)
                {
                    entities.Entry(entity).State = EntityState.Deleted;
                    entities.ΠΡΟΣΩΠΙΚΟ.Remove(entity);
                    entities.SaveChanges();
                }
            }
            catch { }
        }

        public PersonnelGridViewModel Refresh(int entityId)
        {
            return entities.ΠΡΟΣΩΠΙΚΟ.Select(d => new PersonnelGridViewModel
            {
                PERSONNEL_ID = d.PERSONNEL_ID,
                ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                ΒΝΣ = d.ΒΝΣ,
                ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ ?? 0
            }).Where(d => d.PERSONNEL_ID.Equals(entityId)).FirstOrDefault();
        }

        public PersonnelViewModel GetRecord(int personId)
        {
            var data = (from d in entities.ΠΡΟΣΩΠΙΚΟ
                        where d.PERSONNEL_ID == personId
                        select new PersonnelViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΑΦΜ = d.ΑΦΜ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΦΥΛΟ_ΚΩΔ = d.ΦΥΛΟ_ΚΩΔ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΤΟΣ_ΓΕΝΝΗΣΗ = d.ΕΤΟΣ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΚΛΑΔΟΣ = d.ΚΛΑΔΟΣ,
                            ΕΙΔΙΚΟΤΗΤΑ = d.ΕΙΔΙΚΟΤΗΤΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ,
                            ΑΠΟΦΑΣΗ_ΦΕΚ = d.ΑΠΟΦΑΣΗ_ΦΕΚ,
                            ΒΑΘΜΟΣ = d.ΒΑΘΜΟΣ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ,
                            ΑΠΟΧΩΡΗΣΕ = d.ΑΠΟΧΩΡΗΣΕ ?? false,
                            ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ = d.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ,
                            ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ = d.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ
                        }).FirstOrDefault();
            return (data);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}