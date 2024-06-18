using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MealNoonService : IDisposable
    {
        private AbacusDBEntities entities;

        public MealNoonService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<MealNoonViewModel> Read(int stationId)
        {
            var data = (from d in entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ
                        where d.ΒΝΣ == stationId
                        orderby d.ΜΕΣΗΜΕΡΙΑΝΟ
                        select new MealNoonViewModel
                        {
                            ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();
            return (data);
        }

        public void Create(MealNoonViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = new ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ()
            {
                ΒΝΣ = stationId,
                ΜΕΣΗΜΕΡΙΑΝΟ = data.ΜΕΣΗΜΕΡΙΑΝΟ,
                ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
            };
            entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Add(entity);
            entities.SaveChanges();

            data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = entity.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ;
        }

        public void Update(MealNoonViewModel data, int stationId)
        {
            ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Find(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);

            entity.ΒΝΣ = stationId;
            entity.ΜΕΣΗΜΕΡΙΑΝΟ = data.ΜΕΣΗΜΕΡΙΑΝΟ;
            entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(MealNoonViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Find(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public MealNoonViewModel Refresh(int entityId)
        {
            return entities.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Select(d => new MealNoonViewModel
            {
                ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                ΒΝΣ = d.ΒΝΣ,
                ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
            }).Where(d => d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}