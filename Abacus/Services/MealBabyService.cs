using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MealBabyService : IDisposable
    {
        private AbacusDBEntities entities;

        public MealBabyService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<MealBabyViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ
                        where d.ΒΝΣ == stationId
                        orderby d.ΒΡΕΦΙΚΟ
                        select new MealBabyViewModel
                        {
                            ΒΡΕΦΙΚΟ_ΚΩΔ = d.ΒΡΕΦΙΚΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();
            return (data);
        }

        public void Create(MealBabyViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = new ΓΕΥΜΑΤΑ_ΒΡΕΦΗ()
            {
                ΒΝΣ = stationId,
                ΒΡΕΦΙΚΟ = data.ΒΡΕΦΙΚΟ,
                ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
            };
            entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Add(entity);
            entities.SaveChanges();

            data.ΒΡΕΦΙΚΟ_ΚΩΔ = entity.ΒΡΕΦΙΚΟ_ΚΩΔ;
        }

        public void Update(MealBabyViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Find(data.ΒΡΕΦΙΚΟ_ΚΩΔ);

            entity.ΒΝΣ = stationId;
            entity.ΒΡΕΦΙΚΟ = data.ΒΡΕΦΙΚΟ;
            entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(MealBabyViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Find(data.ΒΡΕΦΙΚΟ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public MealBabyViewModel Refresh(int entityId)
        {
            return entities.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Select(d => new MealBabyViewModel
            {
                ΒΡΕΦΙΚΟ_ΚΩΔ = d.ΒΡΕΦΙΚΟ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
            }).Where(d => d.ΒΡΕΦΙΚΟ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}