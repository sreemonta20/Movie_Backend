using System;
namespace Movie.BackEnd.Common.Core
{
    public class ModelFactory<T> where T : class
    {
        //private static Lazy<T> _factoryLazy = new Lazy<T>(
        //    () => (T)Activator.CreateInstance(typeof(T)),
        //    LazyThreadSafetyMode.ExecutionAndPublication);

        //public static T Instance
        //{
        //    get
        //    {
        //        return _factoryLazy.Value;
        //    }
        //}

        /// Static instance. Needs to use lambda expression
        /// to construct an instance (since constructor is private).
        /// </summary>
        private static readonly Lazy<T> sInstance = new Lazy<T>(() => GetInstance());

        /// <summary>
        /// Gets the instance of this singleton.
        /// </summary>
        public static T Instance { get { return sInstance.Value; } }
        //public static List<T> listInstance { get { return sInstance.Value; } }
        /// <summary>
        /// Creates an instance of T via reflection since T's constructor is expected to be private.
        /// </summary>
        /// <returns></returns>
        private static T GetInstance() => Activator.CreateInstance(typeof(T), true) as T;
    }
}
