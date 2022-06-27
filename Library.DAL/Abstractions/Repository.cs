using Library.Core.Abstractions;
using Library.Core.Abstractions.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Abstractions
{
    public abstract class Repository<TModel> : IRepository<TModel> where TModel : class, IModel
    {
        public virtual TModel? Add(TModel model)
        {
            if (entities.ContainsKey(model.Key))
            {
                return null;
            }
            else
            {
                entities.TryAdd(model.Key, model);
                return model;
            }
        }

        public virtual TModel? Delete(TModel model)
        {
            if (entities.ContainsKey(model.Key))
            {
                entities.TryRemove(model.Key,out TModel? removedItem);
                return removedItem;
            }
            else
            {
                return null;
            }
        }

        public virtual List<TModel> GetAll()
        {
            return entities.Values.ToList();
        }

        public virtual TModel? Update(TModel model)
        {
            if (entities.ContainsKey(model.Key))
            {
                var currentBook = entities[model.Key];

                return currentBook;
            }
            else
            {
                return null;
            }
        }

        public virtual TModel? Get(string key)
        {
            if (entities.ContainsKey(key))
            {
                var currentBook = entities[key];
                return currentBook;
            }
            else
            {
                return null;
            }
        }

        protected ConcurrentDictionary<string, TModel> entities = new ConcurrentDictionary<string, TModel>();
    }
}
