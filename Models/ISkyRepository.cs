using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public interface ISkyRepository
    {
        IQueryable<Sky> Skys { get; }

        Task DeleteCityWeather(string city);

        Task AddNewCityWeather(string city);

        void DeleteSky(Sky sk);
        void SaveSkys(Sky[] sks);

        Task<IQueryable<Sky>> GetSkys(string city, int days);

    }
}
