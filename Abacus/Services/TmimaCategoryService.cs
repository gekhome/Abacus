using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class TmimaCategoryService : IDisposable
    {
        private AbacusDBEntities entities;

        public TmimaCategoryService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<TmimaCategoryViewModel> Read()
        {
            var data = (from d in entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ
                        orderby d.CATEGORY_ID
                        select new TmimaCategoryViewModel
                        {
                            CATEGORY_ID = d.CATEGORY_ID,
                            CATEGORY_TEXT = d.CATEGORY_TEXT,
                            AGE_START = d.AGE_START,
                            AGE_END = d.AGE_END
                        }).ToList();
            return data;
        }

        public void Create(TmimaCategoryViewModel data)
        {
            ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = new ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ()
            {
                CATEGORY_TEXT = data.CATEGORY_TEXT,
                AGE_START = data.AGE_START,
                AGE_END = data.AGE_END
            };
            entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Add(entity);
            entities.SaveChanges();

            data.CATEGORY_ID = entity.CATEGORY_ID;
        }

        public void Update(TmimaCategoryViewModel data)
        {
            ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.CATEGORY_ID);

            entity.CATEGORY_TEXT = data.CATEGORY_TEXT;
            entity.AGE_START = data.AGE_START;
            entity.AGE_END = data.AGE_END;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(TmimaCategoryViewModel data)
        {
            ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.CATEGORY_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public TmimaCategoryViewModel Refresh(int entityId)
        {
            return entities.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Select(d => new TmimaCategoryViewModel
            {
                CATEGORY_ID = d.CATEGORY_ID,
                CATEGORY_TEXT = d.CATEGORY_TEXT,
                AGE_START = d.AGE_START,
                AGE_END = d.AGE_END
            }).Where(d => d.CATEGORY_ID.Equals(entityId)).FirstOrDefault();
        }


        public void Dispose()
        {
            entities.Dispose();
        }
    }
}