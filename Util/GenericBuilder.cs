namespace Util
{
    public class GenericBuilder<T> where T : class, new()
    {
        private readonly T _instance;
        private readonly Dictionary<string, Action<T, object>> _setter;

        public GenericBuilder() {
            _instance = new T();
            _setter = new Dictionary<string, Action<T, object>>();
        }

        public GenericBuilder<T> WithProperty(String propertyName, Action<T, object> setter)
        {
            _setter[propertyName] = setter;
            return this;
        }

        public GenericBuilder<T> Set(String propertyName, object val)
        {
            if (_setter.ContainsKey(propertyName))
            {
                _setter[propertyName](_instance, val);
            }
            else
                throw new ArgumentException($"Property {propertyName} is not registed");
            return this;
        }

        public T Build()
        {
            return _instance;
        }
    }
}
