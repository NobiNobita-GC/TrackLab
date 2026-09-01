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
    }
}
