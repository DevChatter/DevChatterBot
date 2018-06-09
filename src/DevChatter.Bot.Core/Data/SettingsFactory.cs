using System;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Games.Roulette;
using System.Linq;
using System.Reflection;

namespace DevChatter.Bot.Core.Data
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly IRepository _repository;
        public SettingsFactory(IRepository repository)
        {
            _repository = repository;
        }

        public T GetSettings<T>() where T : class, new()
        {
            var settings = new T();

            var settingsEntities = _repository.List(CommandSettingsPolicy.BySettingsName(settings.GetType().Name));

            foreach (PropertyInfo propertyInfo in settings.GetType().GetProperties())
            {
                var settingsEntity = settingsEntities.SingleOrDefault(x => x.Key == propertyInfo.Name);
                if (settingsEntity != null)
                {
                    propertyInfo.SetValue(settings, Convert.ChangeType(settingsEntity.Value, propertyInfo.PropertyType));
                }
            }
            return settings;
        }
    }
}
