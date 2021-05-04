﻿using Microsoft.EntityFrameworkCore;
using Reddit_NewsLetter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit_NewsLetter.Services
{
    public class UserRepository : IUser
    {
        private readonly RedditDb db;



        public UserRepository(RedditDb db)
        {
            this.db = db;

        }
        public IEnumerable<UserModel> GetUsers
        {
            get
            {
                return db.User.AsQueryable().OrderBy(s => s.UserId);
            }
        }
        public async Task<UserModel> GetUser(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }

            return await db.User.AsQueryable().Where(s => s.UserId == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<UserModel> AddUser(UserModel user)
        {
            if(user == null) 
            {
                throw new NullReferenceException(nameof(user));
            }
            user.UserId = Guid.NewGuid();
            await db.User.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
        
        public async Task<UserModel> UpdateUser(UserModel updateduser,Guid id)
        {
            var query = await GetUser(id);
            if(query == null) 
            {
                throw new System.NullReferenceException(nameof(query));
            }
            updateduser.UserId = query.UserId;
            var user = db.User.Attach(updateduser);
            user.State = EntityState.Modified;
            await db.SaveChangesAsync();
            return updateduser;
        }

        public async Task<int> UserSubscription(UserModel user)
        {   if(user.Toogle == true) 
            {
                user.Toogle = false;
            }
            else
            {
                user.Toogle = true;
            }
            var updateduser = db.User.Attach(user);
            updateduser.State = EntityState.Modified;
           return await db.SaveChangesAsync();

        }
    }
}
