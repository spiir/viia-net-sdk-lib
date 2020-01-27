using System;
using Viia.Core.Execptions;

namespace Viia.Core.Config.Configurers
{
    public class OptionsConfigurationBuilder : ConfigurationBuilder
    {
        public OptionsConfigurationBuilder(IRegistrar registrar) : base(registrar) { }

        /// <summary>
        ///     Configures the number of retries to perform in the event that a <see cref="HttpException" /> occurs.
        /// </summary>
        public void SetMaxRetries(int maxRetries)
        {
            RegisterInstance<Action<Options>>(o => o.MaxRetries = maxRetries, true);
        }
    }
}
