using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class ProgrammaService : IDisposable
    {
        private AbacusDBEntities entities;

        public ProgrammaService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<ProgrammaDayViewModel> Read(DateTime? theDate, int stationId)
        {
            var data = (from d in entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.STATION_ID == stationId && d.PROGRAMMA_DATE == theDate
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new ProgrammaDayViewModel
                        {
                            STATION_ID = d.STATION_ID,
                            PROGRAMMA_ID = d.PROGRAMMA_ID,
                            PROGRAMMA_DATE = d.PROGRAMMA_DATE,
                            PROGRAMMA_MONTH = d.PROGRAMMA_MONTH,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            PERSON_ID = d.PERSON_ID,
                            HOUR_START = d.HOUR_START,
                            HOUR_END = d.HOUR_END
                        }).ToList();
            return data;
        }

        public void Create(ProgrammaDayViewModel data, DateTime? theDate, int schoolyearId, int stationId)
        {
            ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
            {
                PROGRAMMA_DATE = theDate,
                STATION_ID = stationId,
                PERSON_ID = data.PERSON_ID,
                HOUR_START = data.HOUR_START,
                HOUR_END = data.HOUR_END,
                PROGRAMMA_MONTH = theDate.Value.Month,
                SCHOOLYEARID = schoolyearId
            };
            entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(entity);
            entities.SaveChanges();

            data.PROGRAMMA_ID = entity.PROGRAMMA_ID;
        }

        public void Update(ProgrammaDayViewModel data, DateTime? theDate, int schoolyearId, int stationId)
        {
            ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(data.PROGRAMMA_ID);

            entity.PROGRAMMA_DATE = theDate;
            entity.STATION_ID = stationId;
            entity.PERSON_ID = data.PERSON_ID;
            entity.HOUR_START = data.HOUR_START;
            entity.HOUR_END = data.HOUR_END;
            entity.PROGRAMMA_MONTH = theDate.Value.Month;
            entity.SCHOOLYEARID = schoolyearId;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(ProgrammaDayViewModel data)
        {
            ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(data.PROGRAMMA_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Remove(entity);
                entities.SaveChanges();
            }
        }

        public IEnumerable<ProgrammaDayViewModel> Read(DateTime? theDate1, DateTime? theDate2, int stationId)
        {
            List<ProgrammaDayViewModel> data = new List<ProgrammaDayViewModel>();

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.STATION_ID == stationId && d.PROGRAMMA_DATE >= theDate1 && d.PROGRAMMA_DATE <= theDate2
                        orderby d.PROGRAMMA_DATE, d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new ProgrammaDayViewModel
                        {
                            STATION_ID = d.STATION_ID,
                            PROGRAMMA_ID = d.PROGRAMMA_ID,
                            PROGRAMMA_DATE = d.PROGRAMMA_DATE,
                            PROGRAMMA_MONTH = d.PROGRAMMA_MONTH,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            PERSON_ID = d.PERSON_ID,
                            HOUR_START = d.HOUR_START,
                            HOUR_END = d.HOUR_END
                        }).ToList();
            }
            return data;
        }

        public void Create(ProgrammaDayViewModel data, int schoolyearId, int stationId)
        {
            ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
            {
                PROGRAMMA_DATE = data.PROGRAMMA_DATE,
                STATION_ID = stationId,
                PERSON_ID = data.PERSON_ID,
                HOUR_START = data.HOUR_START,
                HOUR_END = data.HOUR_END,
                PROGRAMMA_MONTH = data.PROGRAMMA_DATE.Value.Month,
                SCHOOLYEARID = schoolyearId
            };
            entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(entity);
            entities.SaveChanges();

            data.PROGRAMMA_ID = entity.PROGRAMMA_ID;
        }

        public void Update(ProgrammaDayViewModel data, int schoolyearId, int stationId)
        {
            ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = entities.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(data.PROGRAMMA_ID);

            entity.PROGRAMMA_DATE = data.PROGRAMMA_DATE;
            entity.PERSON_ID = data.PERSON_ID;
            entity.HOUR_START = data.HOUR_START;
            entity.HOUR_END = data.HOUR_END;
            entity.STATION_ID = stationId;
            entity.PROGRAMMA_MONTH = data.PROGRAMMA_DATE.Value.Month;
            entity.SCHOOLYEARID = schoolyearId;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}