using CourseProject_backend.Entities;

namespace CourseProject_backend.Interfaces.Helpers
{
    public interface ICSVHepler
    {
        public byte[] GetStream(List<List<string>> data);
    }
}
