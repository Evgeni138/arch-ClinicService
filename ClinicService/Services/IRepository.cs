using ClinicService.Models;

namespace ClinicService.Services
{
    public interface IRepository<T, TId>
    {

        int Create(T client);
        int Update(T client);
        int Delete(TId id);

        T GetById(TId id);
        List<T> GetAll();

    }
}
