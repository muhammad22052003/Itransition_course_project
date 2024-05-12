using CourseProject_backend.Entities;

namespace CourseProject_backend.Extensions
{
    public static class MyExtensions
    {
        public static List<TModel> GetForPage<TModel>(this List<TModel> list, int page, int count){
            List<TModel> newCollections = new List<TModel>();

            int startIndex = (page - 1) * 10;

            for (int i = startIndex; i < list.Count && i - startIndex <= count; i++)
            {
                newCollections.Add(list[i]);
            }

            return list;
        }
    }
}
