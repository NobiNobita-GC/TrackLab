namespace TrackLab.Core.IO
{
    public class IOManager
    {
        public static IOManager Instance { get; } = new IOManager();

        private readonly Dictionary<string, List<IOPoint>> _dIs = [];
        private readonly Dictionary<string, List<IOPoint>> _dOs = [];

        private IOManager() { }

        public IReadOnlyDictionary<string, List<IOPoint>> DIs => _dIs;
        public IReadOnlyDictionary<string, List<IOPoint>> DOs => _dOs;

        public void AddDI(string moduleName, IOPoint ioPoint)
        {
            if (ioPoint.Type != IOType.DI)
            {
                throw new ArgumentException($"{ioPoint.Name} 不是 DI");
            }
            if (!_dIs.ContainsKey(moduleName))
            {
                _dIs[moduleName] = [];
            }

            List<IOPoint> ioPoints = _dIs[moduleName];

            if (ioPoints.Any(x => x.Index == ioPoint.Index))
            {
                throw new InvalidOperationException($"DI Index 已存在：{ioPoint.Index}");
            }

            if (ioPoints.Any(x => x.Name == ioPoint.Name))
            {
                throw new InvalidOperationException($"DI Name 已存在：{ioPoint.Name}");
            }

            ioPoints.Add(ioPoint);
        }

        public void AddDO(string moduleName, IOPoint ioPoint)
        {
            if (ioPoint.Type != IOType.DO)
            {
                throw new ArgumentException($"{ioPoint.Name} 不是 DO");
            }
            if (!_dOs.ContainsKey(moduleName))
            {
                _dOs[moduleName] = [];
            }

            List<IOPoint> ioPoints = _dOs[moduleName];

            if (ioPoints.Any(x => x.Index == ioPoint.Index))
            {
                throw new InvalidOperationException($"DO Index 已存在：{ioPoint.Index}");
            }

            if (ioPoints.Any(x => x.Name == ioPoint.Name))
            {
                throw new InvalidOperationException($"DO Name 已存在：{ioPoint.Name}");
            }

            ioPoints.Add(ioPoint);
        }

        public IReadOnlyList<IOPoint> GetDI(string moduleName)
        {
            if (_dIs.TryGetValue(moduleName, out List<IOPoint>? ioPoints))
            {
                return ioPoints;
            }

            return [];
        }

        public IReadOnlyList<IOPoint> GetDO(string moduleName)
        {
            if (_dOs.TryGetValue(moduleName, out List<IOPoint>? ioPoints))
            {
                return ioPoints;
            }

            return [];
        }

        public bool UpdateDI(string moduleName, string ioName, bool value)
        {
            if(!_dIs.TryGetValue(moduleName, out List<IOPoint>? ioPoints))
            {
                return false;
            }
            IOPoint? ioPoint = ioPoints.FirstOrDefault(x => x.Name == ioName);

            if (ioPoint == null)
            {
                return false;
            }

            ioPoint.SetValue(value);

            return true;
        }

        public bool UpdateDO(string moduleName, string ioName, bool value)
        {
            if (!_dOs.TryGetValue(moduleName, out List<IOPoint>? ioPoints))
            {
                return false;
            }

            IOPoint? ioPoint = ioPoints.FirstOrDefault(x => x.Name == ioName);

            if (ioPoint == null)
            {
                return false;
            }

            ioPoint.SetValue(value);

            return true;
        }
    }
}
