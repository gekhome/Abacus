using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class MetabolesReportService : IDisposable
    {
        private AbacusDBEntities entities;

        public MetabolesReportService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public List<MetabolesReportViewModel> Read(int stationId, int schoolyearId, int monthId)
        {
            var data = (from d in entities.METABOLES_REPORT
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId
                        select new MetabolesReportViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            BNS = d.BNS,
                            EMPLOYEE_ID = d.EMPLOYEE_ID,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_DAYS = d.METABOLI_DAYS,
                            METABOLI_YEAR = d.METABOLI_YEAR,
                            METABOLI_MONTH = d.METABOLI_MONTH,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();
            return (data);
        }

        public void Create(MetabolesReportViewModel data, int stationId, int schoolyearId, int monthId)
        {
            METABOLES_REPORT entity = new METABOLES_REPORT()
            {
                BNS = stationId,
                SCHOOL_YEAR = schoolyearId,
                METABOLI_MONTH = monthId,
                EMPLOYEE_ID = data.EMPLOYEE_ID,
                METABOLI_YEAR = data.METABOLI_YEAR,
                METABOLI_TEXT = data.METABOLI_TEXT,
                METABOLI_DAYS = data.METABOLI_DAYS
            };
            entities.METABOLES_REPORT.Add(entity);
            entities.SaveChanges();

            data.RECORD_ID = entity.RECORD_ID;
        }

        public void Update(MetabolesReportViewModel data, int stationId, int schoolyearId, int monthId)
        {
            METABOLES_REPORT entity = entities.METABOLES_REPORT.Find(data.RECORD_ID);

            entity.BNS = stationId;
            entity.SCHOOL_YEAR = schoolyearId;
            entity.METABOLI_MONTH = monthId;
            entity.EMPLOYEE_ID = data.EMPLOYEE_ID;
            entity.METABOLI_YEAR = data.METABOLI_YEAR;
            entity.METABOLI_TEXT = data.METABOLI_TEXT;
            entity.METABOLI_DAYS = data.METABOLI_DAYS;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(MetabolesReportViewModel data)
        {
            METABOLES_REPORT entity = entities.METABOLES_REPORT.Find(data.RECORD_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.METABOLES_REPORT.Remove(entity);
                entities.SaveChanges();
            }
        }

        public MetabolesReportViewModel Refresh(int entityId)
        {
            return entities.METABOLES_REPORT.Select(d => new MetabolesReportViewModel
            {
                RECORD_ID = d.RECORD_ID,
                BNS = d.BNS,
                EMPLOYEE_ID = d.EMPLOYEE_ID,
                SCHOOL_YEAR = d.SCHOOL_YEAR,
                METABOLI_DAYS = d.METABOLI_DAYS,
                METABOLI_YEAR = d.METABOLI_YEAR,
                METABOLI_MONTH = d.METABOLI_MONTH,
                METABOLI_TEXT = d.METABOLI_TEXT
            }).Where(d => d.RECORD_ID.Equals(entityId)).FirstOrDefault();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}