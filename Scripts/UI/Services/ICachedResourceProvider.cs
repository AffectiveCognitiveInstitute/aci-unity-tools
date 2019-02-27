using System.Threading.Tasks;

namespace Aci.Unity.Services
{
    public interface ICachedResourceProvider<T, TParam>
    {
        Task<T> Get(TParam param);
        void Clear();
    }
}
