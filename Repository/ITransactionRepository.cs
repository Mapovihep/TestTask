using TestTask.Models;

namespace TestTask.Repository
{
    public interface IRepository<T>
    {
        public Task<Guid> Add(T obj);
        public Task<T> GetById(Guid id);
        public Task Update(T obj);
        public Task Delete(T obj);

    }
}
