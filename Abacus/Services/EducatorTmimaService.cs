using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class EducatorTmimaService : IDisposable
    {
        private AbacusDBEntities entities;

        public EducatorTmimaService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<EducatorTmimaViewModel> Read(int personId)
        {
            var data = (from d in entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ
                        where d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ, d.ΤΜΗΜΑ_ΚΩΔ
                        select new EducatorTmimaViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
                        }).ToList();
            return data;
        }

        public void Create(EducatorTmimaViewModel data, int personId, int stationId)
        {
            ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = new ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ()
            {
                ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = personId,
                ΒΝΣ = stationId,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΤΜΗΜΑ_ΚΩΔ = data.ΤΜΗΜΑ_ΚΩΔ,
            };
            entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Add(entity);
            entities.SaveChanges();

            data.RECORD_ID = entity.RECORD_ID;
        }

        public void Update(EducatorTmimaViewModel data, int personId, int stationId)
        {
            ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);

            entity.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = personId;
            entity.ΒΝΣ = stationId;
            entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            entity.ΤΜΗΜΑ_ΚΩΔ = data.ΤΜΗΜΑ_ΚΩΔ;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(EducatorTmimaViewModel data)
        {
            ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public EducatorTmimaViewModel Refresh(int entityId)
        {
            return entities.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Select(d => new EducatorTmimaViewModel
            {
                RECORD_ID = d.RECORD_ID,
                ΒΝΣ = d.ΒΝΣ,
                ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
            }).Where(d => d.RECORD_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}