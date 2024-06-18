using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MealMorningService : IDisposable
    {
        private AbacusDBEntities entities;

        public MealMorningService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<MealMorningViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ
                        where d.ΒΝΣ == stationId
                        orderby d.ΠΡΩΙΝΟ
                        select new MealMorningViewModel
                        {
                            ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();
            return (data);
        }

        public void Create(MealMorningViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = new ΓΕΥΜΑΤΑ_ΠΡΩΙ()
            {
                ΒΝΣ = stationId,
                ΠΡΩΙΝΟ = data.ΠΡΩΙΝΟ,
                ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
            };
            entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Add(entity);
            entities.SaveChanges();

            data.ΠΡΩΙΝΟ_ΚΩΔ = entity.ΠΡΩΙΝΟ_ΚΩΔ;
        }

        public void Update(MealMorningViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Find(data.ΠΡΩΙΝΟ_ΚΩΔ);

            entity.ΒΝΣ = stationId;
            entity.ΠΡΩΙΝΟ = data.ΠΡΩΙΝΟ;
            entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(MealMorningViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Find(data.ΠΡΩΙΝΟ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public MealMorningViewModel Refresh(int entityId)
        {
            return entities.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Select(d => new MealMorningViewModel
            {
                ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
            }).Where(d => d.ΠΡΩΙΝΟ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}