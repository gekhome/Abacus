using System;
using System.Collections.Generic;
using System.Linq;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.Models;
using System.Data.Entity;

namespace Abacus.Services
{
    public class UserAdminService : IDisposable
    {
        private AbacusDBEntities entities;

        public UserAdminService(AbacusDBEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<UserAdminViewModel> Read()
        {
            var data = (from d in entities.USER_ADMINS
                        select new UserAdminViewModel
                        {
                            USER_ID = d.USER_ID,
                            USERNAME = d.USERNAME,
                            PASSWORD = d.PASSWORD,
                            FULLNAME = d.FULLNAME,
                            ISACTIVE = d.ISACTIVE ?? false,
                            CREATEDATE = (DateTime)d.CREATEDATE
                        }).ToList();
            return data;
        }

        public void Create(UserAdminViewModel data)
        {
            USER_ADMINS entity = new USER_ADMINS();

            entity.USERNAME = data.USERNAME;
            entity.PASSWORD = data.PASSWORD;
            entity.FULLNAME = data.FULLNAME;
            entity.ISACTIVE = data.ISACTIVE;
            entity.CREATEDATE = data.CREATEDATE;

            entities.USER_ADMINS.Add(entity);
            entities.SaveChanges();

            data.USER_ID = entity.USER_ID;
        }

        public void Update(UserAdminViewModel data)
        {
            USER_ADMINS entity = entities.USER_ADMINS.Find(data.USER_ID);

            entity.USERNAME = data.USERNAME;
            entity.PASSWORD = data.PASSWORD;
            entity.FULLNAME = data.FULLNAME;
            entity.ISACTIVE = data.ISACTIVE;
            entity.CREATEDATE = data.CREATEDATE;

            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Destroy(UserAdminViewModel data)
        {
            USER_ADMINS entity = entities.USER_ADMINS.Find(data.USER_ID);

            if (entity != null)
            {
                entities.Entry(entity).State = EntityState.Deleted;
                entities.USER_ADMINS.Remove(entity);
                entities.SaveChanges();
            }
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}