using Manonero.MessageBus.Kafka.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceServices.Settings
{
    internal class AppSetting
    {
        public string BootstrapServers { get; init; }

        public ConsumerSetting[] ConsumerSettings { get; init; }


        public static AppSetting MapValue(IConfiguration configuration)
        {
            var bootstrapServers = configuration[nameof(BootstrapServers)];

            var consumerConfigurations = configuration.GetSection(nameof(ConsumerSettings)).GetChildren();
            var consumerSettings = new List<ConsumerSetting>();


            foreach (var consumerConfiguration in consumerConfigurations)
            {
                var consumerSetting = ConsumerSetting.MapValue(consumerConfiguration, bootstrapServers);
                if (!consumerSettings.Contains(consumerSetting))
                    consumerSettings.Add(consumerSetting);
            }
            var setting = new AppSetting
            {
                BootstrapServers = bootstrapServers,
                ConsumerSettings = consumerSettings.ToArray(),
            };

            return setting;
        }

        public ConsumerSetting GetConsumerSetting(string id)
        => ConsumerSettings.FirstOrDefault(consumerSetting => consumerSetting.Id.Equals(id));
    }
}
