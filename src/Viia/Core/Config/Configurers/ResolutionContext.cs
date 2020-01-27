using System;
using System.Collections.Generic;
using System.Linq;
using Viia.Core.Execptions;

namespace Viia.Core.Config.Configurers
{
    /// <summary>
    ///     Context that is passed into resolver factory methods, allowing them to resolve dependent services
    /// </summary>
    public class ResolutionContext : IDisposable
    {
        //private static Logger _logger;
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();
        private readonly List<ResolutionContext> _childContexts = new List<ResolutionContext>();
        private readonly Dictionary<Type, int> _levels = new Dictionary<Type, int>();
        private readonly HashSet<object> _resolvedObjects = new HashSet<object>();

        private readonly IEnumerable<Resolver> _resolvers;

        /// <summary>
        ///     Constructs the context with the given resolvers available
        /// </summary>
        public ResolutionContext(IEnumerable<Resolver> resolvers)
        {
            _resolvers = resolvers;
        }

        /// <summary>
        ///     Adds a child context
        /// </summary>
        public void AddChildContext(ResolutionContext context)
        {
            _childContexts.Add(context);
        }

        public void Dispose()
        {
            foreach (var disposable in _resolvedObjects.OfType<IDisposable>())
            {
                //_logger.Debug("Disposing {0}", disposable);

                disposable.Dispose();
            }

            foreach (var context in _childContexts)
            {
                context.Dispose();
            }
        }

        /// <summary>
        ///     Resolves an implementation of <typeparamref name="TService" />. Throws a <see cref="ResolutionException" /> if none
        ///     could be found
        /// </summary>
        public TService Get<TService>()
        {
            var result = GetOrDefault<TService>();

            if (Equals(result, default(TService)))
            {
                throw new ResolutionException(typeof(TService), "No appropriate factory method has been registered!");
            }

            return result;
        }

        /// <summary>
        ///     Gets all instances of <typeparamref name="TService" /> that were registered with multi = true
        /// </summary>
        public IEnumerable<TService> GetAll<TService>()
        {
            return _resolvers
                   .OfType<Resolver<TService>>()
                   .Where(r => r.Multi)
                   .Select(r => r.InvokeFactory(this));
        }

        /// <summary>
        ///     Resolves an implementation of <typeparamref name="TService" />. Returns null if none could be found.
        /// </summary>
        public TService GetOrDefault<TService>()
        {
            if (_cache.ContainsKey(typeof(TService)))
            {
                var cachedResult = ( TService ) _cache[typeof(TService)];

                return cachedResult;
            }

            var resolver = _resolvers
                           .OfType<Resolver<TService>>()
                           .Skip(GetLevelFor<TService>())
                           .FirstOrDefault();

            if (resolver == null)
            {
                return default;
            }

            AddToLevel<TService>(1);

            var result = resolver.InvokeFactory(this);

            _cache[typeof(TService)] = result;

            _resolvedObjects.Add(result);

            AddToLevel<TService>(-1);

            return result;
        }

        private void AddToLevel<TService>(int addend)
        {
            var serviceType = typeof(TService);

            if (!_levels.ContainsKey(serviceType))
                _levels[serviceType] = 0;

            _levels[serviceType] += addend;
        }

        private int GetLevelFor<TService>()
        {
            var serviceType = typeof(TService);

            if (!_levels.ContainsKey(serviceType))
                _levels[serviceType] = 0;

            return _levels[serviceType];
        }

        /// <summary>
        ///     Wrapper of a factory method
        /// </summary>
        public abstract class Resolver
        {
            /// <summary>
            ///     Gets whether this is a decorator and will resolve
            /// </summary>
            public bool Decorator { get; }

            /// <summary>
            ///     Gets whether this is a multi-registration, i.e. whether it can be resolved with
            ///     <see cref="ResolutionContext.GetAll{TService}" />
            /// </summary>
            public bool Multi { get; }

            /// <summary>
            ///     Gets the type that can be resolved by this resolver
            /// </summary>
            public Type Type { get; }

            /// <summary>
            ///     Constructs the resolver
            /// </summary>
            protected Resolver(bool decorator, bool multi, Type type)
            {
                Decorator = decorator;
                Multi = multi;
                Type = type;
            }
        }

        /// <summary>
        ///     Typed resolver
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        public class Resolver<TService> : Resolver
        {
            private readonly Func<ResolutionContext, TService> _factory;

            /// <summary>
            ///     Constructs the resolver
            /// </summary>
            public Resolver(Func<ResolutionContext, TService> factory, bool decorator, bool multi)
                : base(decorator, multi, typeof(TService))
            {
                if (factory == null) throw new ArgumentNullException("factory");
                _factory = factory;
            }

            /// <summary>
            ///     Invokes the factory method, resolving the instance
            /// </summary>
            public TService InvokeFactory(ResolutionContext resolutionContext)
            {
                return _factory(resolutionContext);
            }
        }
    }
}
