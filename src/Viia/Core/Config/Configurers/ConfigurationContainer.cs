using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viia.Core.Config.Configurers
{
    public class ConfigurationContainer : IRegistrar
    {
        private readonly List<ResolutionContext.Resolver> _resolvers = new List<ResolutionContext.Resolver>();

        public ResolutionContext CreateContext()
        {
            return new ResolutionContext(_resolvers);
        }

        public void Decorate<TService>(Func<ResolutionContext, TService> serviceFactory)
        {
            Register(serviceFactory, true, false);
        }

        public bool HasService<TService>(bool checkForPrimary = false)
        {
            return checkForPrimary
                       ? _resolvers
                         .OfType<ResolutionContext.Resolver<TService>>()
                         .Any(s => !s.Decorator && !s.Multi)
                       : _resolvers
                         .OfType<ResolutionContext.Resolver<TService>>()
                         .Any();
        }

        public void Register<TService>(Func<ResolutionContext, TService> serviceFactory)
        {
            Register(serviceFactory, false, false);
        }

        public void RegisterInstance<TService>(TService instance, bool multi = false)
        {
            Register(c => instance, false, multi);
        }

        internal void InsertResolversInto(ConfigurationContainer otherContainer)
        {
            otherContainer._resolvers.AddRange(_resolvers);
        }

        private string Format<TService>(List<ResolutionContext.Resolver<TService>> agg)
        {
            var primary = agg.Where(r => !r.Decorator)
                             .ToList();

            var decorators = agg.Where(r => r.Decorator)
                                .ToList();

            var builder = new StringBuilder();

            if (primary.Any())
            {
                builder.AppendLine(@"    Primary:");
                builder.AppendLine(string.Join(System.Environment.NewLine, primary.Select(p => $"        {p.Type}")));
            }

            if (decorators.Any())
            {
                builder.AppendLine(@"    Decorators:");
                builder.AppendLine(string.Join(System.Environment.NewLine, decorators.Select(p => $"        {p.Type}")));
            }

            return builder.ToString();
        }

        private void Register<TService>(Func<ResolutionContext, TService> serviceFactory, bool decorator, bool multi)
        {
            var havePrimaryResolverAlready = HasService<TService>(true);

            if (!decorator && !multi && havePrimaryResolverAlready)
            {
                var message = $"Attempted to register factory method for {typeof(TService)} as non-decorator,"
                              + " but there's already a primary resolver for that service! There"
                              + " can be only one primary resolver for each service type (but" + " any number of decorators)";

                throw new InvalidOperationException(message);
            }

            if (multi && havePrimaryResolverAlready)
            {
                var message = $"Attempted to register factory method for {typeof(TService)} as multi, but there's"
                              + " already a primary resolver for that service! When doing multi-registrations,"
                              + " it is important that they're all configured to be multi-registrations!";

                throw new InvalidOperationException(message);
            }

            var resolver = new ResolutionContext.Resolver<TService>(serviceFactory, decorator, multi);

            if (decorator)
            {
                _resolvers.Insert(0, resolver);
                return;
            }

            _resolvers.Add(resolver);
        }
    }
}
