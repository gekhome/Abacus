using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class GenikoiService : IDisposable
    {
        private AbacusDBEntities entities;

        public GenikoiService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<DirectorGeneralViewModel> Read()
        {
            var data = (from d in entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DirectorGeneralViewModel
                        {
                            ΓΕΝΙΚΟΣ_ΚΩΔ = d.ΓΕΝΙΚΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΓΕΝΙΚΟΣ = d.ΓΕΝΙΚΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();
            return data;
        }

        public void Create(DirectorGeneralViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = new Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ()
            {
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΓΕΝΙΚΟΣ = data.ΓΕΝΙΚΟΣ,
                ΦΥΛΟ = data.ΦΥΛΟ
            };
            entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Add(entity);
            entities.SaveChanges();

            data.ΓΕΝΙΚΟΣ_ΚΩΔ = entity.ΓΕΝΙΚΟΣ_ΚΩΔ;
        }

        public void Update(DirectorGeneralViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Find(data.ΓΕΝΙΚΟΣ_ΚΩΔ);

            entity.ΓΕΝΙΚΟΣ = data.ΓΕΝΙΚΟΣ;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΦΥΛΟ = data.ΦΥΛΟ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(DirectorGeneralViewModel data)
        {
            Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Find(data.ΓΕΝΙΚΟΣ_ΚΩΔ);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public DirectorGeneralViewModel Refresh(int entityId)
        {
            return entities.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Select(d => new DirectorGeneralViewModel
            {
                ΓΕΝΙΚΟΣ_ΚΩΔ = d.ΓΕΝΙΚΟΣ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΓΕΝΙΚΟΣ = d.ΓΕΝΙΚΟΣ,
                ΦΥΛΟ = d.ΦΥΛΟ
            }).Where(d => d.ΓΕΝΙΚΟΣ_ΚΩΔ == entityId).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}