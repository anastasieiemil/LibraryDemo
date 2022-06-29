using Library.Core.Abstractions;
using Library.Core.Abstractions.Repositories;
using Newtonsoft.Json;
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
                entities.TryAdd(model.Key, JsonConvert.SerializeObject(model));
                return model;
            }
        }

        public virtual TModel? Delete(TModel model)
        {
            if (entities.ContainsKey(model.Key))
            {
                if(entities.TryRemove(model.Key, out string removedItem))
                {
                    return model;
                }
                else
                {
                    return null;

                }
            }
            else
            {
                return null;
            }
        }

        public virtual List<TModel> GetAll()
        {
            return entities.Values.Select(x => JsonConvert.DeserializeObject<TModel>(x))
                                         .ToList();
        }

        public virtual TModel? Update(TModel model)
        {
            if (entities.ContainsKey(model.Key))
            {
                var currentBook = entities[model.Key];
                return JsonConvert.DeserializeObject<TModel>(currentBook);
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
                return JsonConvert.DeserializeObject<TModel>(currentBook);
            }
            else
            {
                return null;
            }
        }

        public int Count()
        {
            return entities.Count;
        }

        protected ConcurrentDictionary<string, string> entities = new ConcurrentDictionary<string, string>();
    }
}
