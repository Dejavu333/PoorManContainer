using System.Text.Json;

namespace PoorManContainer
{
    public static class Cont
    {
        //----------------------------
        // properties, fields
        //----------------------------
        private static Dictionary<Type, Type> _registeredTypes = null;
        private static readonly string _configSrcPath = "./PoorManContainer.json";

        //----------------------------
        // methods
        //----------------------------
        private static void _Register(Type interfaceType, Type implementationType)
        {
            _registeredTypes.Add(interfaceType, implementationType);
        }

        private static void _ReadConfiguration()
        {
            string json = File.ReadAllText(_configSrcPath);
            var configuration = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var noSuchRecords = new List<string>();
            foreach (var item in configuration)
            {
                Type interfaceType = Type.GetType(item.Key);
                Type implementationType = Type.GetType(item.Value);
                if (interfaceType is null) noSuchRecords.Add(item.Key);
                if (implementationType is null) noSuchRecords.Add(item.Value);
                if (interfaceType is null || implementationType is null) continue;
                _Register(interfaceType, implementationType);
            }
            if (noSuchRecords.Count > 0) throw new NoSuchRecordExists("\nThe following records are not present in the project:\n" + string.Join(", ", noSuchRecords));
        }

        private static void Configure()
        {
            if (_registeredTypes != null) return;
            _registeredTypes = new Dictionary<Type, Type>();
            _ReadConfiguration();
        }

        public static T New<T>(params object[] args)
        {
            Configure(); // lazy loading
            Type interfaceType = typeof(T);
            Type implementationType = _registeredTypes[interfaceType];
            return (T)Activator.CreateInstance(implementationType, args);
        }
    }


    public class NoSuchRecordExists : Exception
    {
        public NoSuchRecordExists() { }
        public NoSuchRecordExists(string message) : base(message) { }
    }
}
