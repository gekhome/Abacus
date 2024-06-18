using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ChildEgrafiService : IDisposable
    {
        private AbacusDBEntities entities;

        public ChildEgrafiService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<ChildTmimaViewModel> Read(int childId)
        {
            var data = (from d in entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ
                        where d.ΠΑΙΔΙ_ΚΩΔ == childId
                        orderby d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        select new ChildTmimaViewModel
                        {
                            ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                            ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
                        }).ToList();
            return data;
        }

        public void Create(ChildTmimaViewModel data, int childId, int stationId)
        {
            ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = new ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ()
            {
                ΠΑΙΔΙ_ΚΩΔ = childId,
                ΒΝΣ = stationId,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΤΜΗΜΑ = data.ΤΜΗΜΑ,
                ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = data.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                ΗΜΝΙΑ_ΠΕΡΑΣ = data.ΗΜΝΙΑ_ΠΕΡΑΣ
            };
            entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Add(entity);
            entities.SaveChanges();

            data.ΕΓΓΡΑΦΗ_ΚΩΔ = entity.ΕΓΓΡΑΦΗ_ΚΩΔ;
        }

        public void Update(ChildTmimaViewModel data, int childId, int stationId)
        {
            ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Find(data.ΕΓΓΡΑΦΗ_ΚΩΔ);

            entity.ΠΑΙΔΙ_ΚΩΔ = childId;
            entity.ΒΝΣ = stationId;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΤΜΗΜΑ = data.ΤΜΗΜΑ;
            entity.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = data.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ;
            entity.ΗΜΝΙΑ_ΠΕΡΑΣ = data.ΗΜΝΙΑ_ΠΕΡΑΣ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ChildTmimaViewModel data)
        {
            ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Find(data.ΕΓΓΡΑΦΗ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public ChildTmimaViewModel Refresh(int entityId)
        {
            return entities.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Select(d => new ChildTmimaViewModel
            {
                ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΤΜΗΜΑ = d.ΤΜΗΜΑ,
                ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
            }).Where(d => d.ΕΓΓΡΑΦΗ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}