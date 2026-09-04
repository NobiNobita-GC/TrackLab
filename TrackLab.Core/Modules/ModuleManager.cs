using System.IO;
using System.Text.Json;

namespace TrackLab.Core.Modules
{
    public class ModuleManager
    {
        public static ModuleManager Instance { get; } = new ModuleManager();
       
        private readonly List<ModuleBase> _modules = [];

        private ModuleManager() { }

        public IReadOnlyList<ModuleBase> Modules => _modules;

        public void Add(ModuleBase module)
        {
            bool indexExists = _modules.Any(x => x.Index == module.Index);
            if(indexExists)
            {
                throw new InvalidOperationException($"Module Index 已存在：{module.Index}");
            }

            bool nameExists = _modules.Any(x => x.Name == module.Name);

            if (nameExists)
            {
                throw new InvalidOperationException($"Module Name 已存在：{module.Name}");
            }

            _modules.Add(module);
        }

        public bool InitializeAll()
        {
            bool result = true;

            foreach (ModuleBase module in _modules)
            {
                if (!module.Initialize())
                {
                    result = false;
                }
            }

            return result;
        }

        public ModuleBase? GetByIndex(int index)
        {
            return _modules.FirstOrDefault(x => x.Index == index);
        }

        public ModuleBase? GetByName(string name)
        {
            return _modules.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<ModuleBase> GetByType(ModuleType type)
        {
            return _modules.Where(x => x.Type == type);
        }

        public void Load(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("ModuleConfig.json not found.", configPath);
            }

            string json = File.ReadAllText(configPath);

            List<ModuleConfigEntity>? configs =
                JsonSerializer.Deserialize<List<ModuleConfigEntity>>(json);

            if (configs == null)
            {
                return;
            }

            _modules.Clear();

            foreach (ModuleConfigEntity config in configs)
            {
                Type? moduleType = typeof(ModuleBase).Assembly.GetType(config.ClassName);

                if (moduleType == null)
                {
                    throw new InvalidOperationException($"Module type not found: {config.ClassName}");
                }

                if (!typeof(ModuleBase).IsAssignableFrom(moduleType))
                {
                    throw new InvalidOperationException($"{config.ClassName} is not a ModuleBase.");
                }

                ModuleBase? module =
                    Activator.CreateInstance(moduleType, config.Index, config.Name) as ModuleBase;

                if (module == null)
                {
                    throw new InvalidOperationException($"Failed to create module: {config.Name}");
                }

                Add(module);
            }
        }
    }
}
