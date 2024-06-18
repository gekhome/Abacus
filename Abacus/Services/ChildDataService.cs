using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ChildDataService : IDisposable
    {
        private AbacusDBEntities entities;

        public ChildDataService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<ChildGridViewModel> Read()
        {
            var data = (from d in entities.ΠΑΙΔΙΑ
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new ChildGridViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ
                        }).ToList();
            return (data);
        }

        public IEnumerable<ChildGridViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΠΑΙΔΙΑ
                        where d.ΒΝΣ == stationId
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new ChildGridViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ
                        }).ToList();
            return (data);
        }

        public void Create(ChildGridViewModel data)
        {
            ΠΑΙΔΙΑ entity = new ΠΑΙΔΙΑ()
            {
                ΑΜ = data.ΑΜ,
                ΒΝΣ = data.ΒΝΣ,
                ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
            };
            entities.ΠΑΙΔΙΑ.Add(entity);
            entities.SaveChanges();

            data.CHILD_ID = entity.CHILD_ID;
        }

        public void Create(ChildGridViewModel data, int stationId)
        {
            ΠΑΙΔΙΑ entity = new ΠΑΙΔΙΑ()
            {
                ΑΜ = data.ΑΜ,
                ΒΝΣ = stationId,
                ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
            };
            entities.ΠΑΙΔΙΑ.Add(entity);
            entities.SaveChanges();

            data.CHILD_ID = entity.CHILD_ID;
        }

        public void Update(ChildGridViewModel data)
        {
            ΠΑΙΔΙΑ entity = entities.ΠΑΙΔΙΑ.Find(data.CHILD_ID);

            entity.ΑΜ = data.ΑΜ;
            entity.ΒΝΣ = data.ΒΝΣ;
            entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ;
            entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Update(ChildGridViewModel data, int stationId)
        {
            ΠΑΙΔΙΑ entity = entities.ΠΑΙΔΙΑ.Find(data.CHILD_ID);

            entity.ΑΜ = data.ΑΜ;
            entity.ΒΝΣ = stationId;
            entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ;
            entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ChildGridViewModel data)
        {
            ΠΑΙΔΙΑ entity = entities.ΠΑΙΔΙΑ.Find(data.CHILD_ID);

            try
            {
                if (entity != null)
                {
                    entities.Entry(entity).State = EntityState.Deleted;
                    entities.ΠΑΙΔΙΑ.Remove(entity);
                    entities.SaveChanges();
                }
            }
            catch { }
        }

        public ChildGridViewModel Refresh(int entityId)
        {
            return entities.ΠΑΙΔΙΑ.Select(d => new ChildGridViewModel
            {
                CHILD_ID = d.CHILD_ID,
                ΑΜ = d.ΑΜ,
                ΒΝΣ = d.ΒΝΣ,
                ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                ΟΝΟΜΑ = d.ΟΝΟΜΑ
            }).Where(d => d.CHILD_ID.Equals(entityId)).FirstOrDefault();
        }

        public ChildViewModel GetRecord(int entityId)
        {
            var data = (from d in entities.ΠΑΙΔΙΑ
                        where d.CHILD_ID == entityId
                        select new ChildViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ ?? 0,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΑΦΜ = d.ΑΦΜ,
                            ΑΜΚΑ = d.ΑΜΚΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            AGE = d.AGE ?? 0.0m,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? true,
                            ΕΙΣΟΔΟΣ_ΗΜΝΙΑ = d.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ,
                            ΕΞΟΔΟΣ_ΗΜΝΙΑ = d.ΕΞΟΔΟΣ_ΗΜΝΙΑ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ
                        }
                        ).FirstOrDefault();
            return (data);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}