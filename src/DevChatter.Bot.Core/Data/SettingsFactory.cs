using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using System;
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
                    propertyInfo.SetValue(settings, ConvertValue(propertyInfo, settingsEntity));
                }
            }

            return settings;

            object ConvertValue(PropertyInfo propertyInfo, CommandSettingsEntity settingsEntity)
            {
                if (propertyInfo.PropertyType.IsEnum)
                {
                    return Enum.Parse(propertyInfo.PropertyType, settingsEntity.Value);
                }

                return Convert.ChangeType(settingsEntity.Value, propertyInfo.PropertyType);
            }
        }

        public void CreateDefaultSettingsIfNeeded<T>() where T : class, new()
        {
            var settings = new T();

            var settingsEntities = _repository.List(CommandSettingsPolicy.BySettingsName(settings.GetType().Name));

            foreach (PropertyInfo propertyInfo in settings.GetType().GetProperties())
            {
                if (settingsEntities.All(x => x.Key != propertyInfo.Name))
                {
                    _repository.Create(new CommandSettingsEntity
                    {
                        SettingsTypeName = settings.GetType().Name,
                        Key = propertyInfo.Name,
                        Value = propertyInfo.GetValue(settings).ToString()
                    });
                }
            }
        }
    }
}
