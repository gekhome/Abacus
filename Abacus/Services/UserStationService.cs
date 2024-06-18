using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class UserStationService : IDisposable
    {
        private AbacusDBEntities entities;

        public UserStationService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<UserStationViewModel> Read()
        {
            var data = (from d in entities.USER_STATIONS
                        orderby d.USERNAME
                        select new UserStationViewModel
                        {
                            USER_ID = d.USER_ID,
                            USERNAME = d.USERNAME,
                            PASSWORD = d.PASSWORD,
                            STATION_ID = d.STATION_ID ?? 0,
                            ISACTIVE = d.ISACTIVE ?? false
                        }).ToList();
            return data;
        }

        public void Create(UserStationViewModel data)
        {
            USER_STATIONS entity = new USER_STATIONS()
            {
                USERNAME = data.USERNAME,
                PASSWORD = data.PASSWORD,
                STATION_ID = data.STATION_ID,
                ISACTIVE = data.ISACTIVE,
            };
            entities.USER_STATIONS.Add(entity);
            entities.SaveChanges();
        }

        public void Update(UserStationViewModel data)
        {
            USER_STATIONS entity = entities.USER_STATIONS.Find(data.USER_ID);

            entity.USERNAME = data.USERNAME;
            entity.PASSWORD = data.PASSWORD;
            entity.STATION_ID = data.STATION_ID;
            entity.ISACTIVE = data.ISACTIVE;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(UserStationViewModel data)
        {
            USER_STATIONS entity = entities.USER_STATIONS.Find(data.USER_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.USER_STATIONS.Remove(entity);
                entities.SaveChanges();
            }
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}