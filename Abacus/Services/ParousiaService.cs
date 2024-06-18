using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ParousiaService : IDisposable
    {
        private AbacusDBEntities entities;

        public ParousiaService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<ChildParousiaViewModel> Read(int tmimaId, DateTime? theDate)
        {
            var data = (from d in entities.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_DATE == theDate
                        orderby d.ΠΑΙΔΙΑ.ΕΠΩΝΥΜΟ, d.ΠΑΙΔΙΑ.ΟΝΟΜΑ
                        select new ChildParousiaViewModel
                        {
                            PAROUSIA_ID = d.PAROUSIA_ID,
                            CHILD_ID = d.CHILD_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            PAROUSIA_DATE = d.PAROUSIA_DATE,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            PRESENCE = d.PRESENCE ?? true,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID
                        }).ToList();
            return data;
        }

        public void Create(ChildParousiaViewModel data, int tmimaId, DateTime? theDate, int schoolyearId, int stationId)
        {
            ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = new ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ()
            {
                STATION_ID = stationId,
                SCHOOLYEARID = schoolyearId,
                TMIMA_ID = tmimaId,
                PAROUSIA_DATE = theDate,
                PAROUSIA_MONTH = theDate.Value.Month,
                CHILD_ID = data.CHILD_ID,
                PRESENCE = data.PRESENCE
            };
            entities.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Add(entity);
            entities.SaveChanges();

            data.PAROUSIA_ID = entity.PAROUSIA_ID;
        }

        public void Update(ChildParousiaViewModel data, int tmimaId, DateTime? theDate, int schoolyearId, int stationId)
        {
            ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = entities.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Find(data.PAROUSIA_ID);

            entity.STATION_ID = stationId;
            entity.SCHOOLYEARID = schoolyearId;
            entity.TMIMA_ID = tmimaId;
            entity.PAROUSIA_DATE = theDate;
            entity.PAROUSIA_MONTH = theDate.Value.Month;
            entity.CHILD_ID = data.CHILD_ID;
            entity.PRESENCE = data.PRESENCE;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ChildParousiaViewModel data)
        {
            ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = entities.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Find(data.PAROUSIA_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public IEnumerable<ParousiesMonthViewModel> ReadMonth(int tmimaId, int monthId)
        {
            var data = (from d in entities.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        select new ParousiesMonthViewModel
                        {
                            ROW_ID = d.ROW_ID,
                            CHILD_ID = d.CHILD_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        }).ToList();
            return data;
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}