﻿using MyScope.Core.Models;
using MyShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;
        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }
        public void Commit()
        {
            cache[className] = items;
        }
        public void Insert(T TEntity)
        {
            items.Add(TEntity);
        }
        public void Update(T TEntity)
        {
            T tToUpdate = items.Find(i => i.Id == TEntity.Id);
            if (tToUpdate != null)
            {
                tToUpdate = TEntity;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }
        public T Find(string id)
        {
            T tToFind = items.Find(i => i.Id == id);
            if (tToFind != null)
            {
                return tToFind;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string id)
        {
            T tToDelete = items.Find(i => i.Id == id);
            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }
    }
}
