using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class DioikitesService : IDisposable
    {
        private AbacusDBEntities entities;

        public DioikitesService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DioikitisViewModel> Read()
        {
            var data = (from d in entities.Δ_ΔΙΟΙΚΗΤΕΣ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DioikitisViewModel
                        {
                            ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΟΙΚΗΤΗΣ = d.ΔΙΟΙΚΗΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();
            return data;
        }

        public void Create(DioikitisViewModel data)
        {
            Δ_ΔΙΟΙΚΗΤΕΣ entity = new Δ_ΔΙΟΙΚΗΤΕΣ()
            {
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΔΙΟΙΚΗΤΗΣ = data.ΔΙΟΙΚΗΤΗΣ,
                ΦΥΛΟ = data.ΦΥΛΟ
            };
            entities.Δ_ΔΙΟΙΚΗΤΕΣ.Add(entity);
            entities.SaveChanges();

            data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = entity.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ;
        }

        public void Update(DioikitisViewModel data)
        {
            Δ_ΔΙΟΙΚΗΤΕΣ entity = entities.Δ_ΔΙΟΙΚΗΤΕΣ.Find(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);

            entity.ΔΙΟΙΚΗΤΗΣ = data.ΔΙΟΙΚΗΤΗΣ;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΦΥΛΟ = data.ΦΥΛΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DioikitisViewModel data)
        {
            Δ_ΔΙΟΙΚΗΤΕΣ entity = entities.Δ_ΔΙΟΙΚΗΤΕΣ.Find(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.Δ_ΔΙΟΙΚΗΤΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DioikitisViewModel Refresh(int entityId)
        {
            return entities.Δ_ΔΙΟΙΚΗΤΕΣ.Select(d => new DioikitisViewModel
            {
                ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΔΙΟΙΚΗΤΗΣ = d.ΔΙΟΙΚΗΤΗΣ,
                ΦΥΛΟ = d.ΦΥΛΟ
            }).Where(d => d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}