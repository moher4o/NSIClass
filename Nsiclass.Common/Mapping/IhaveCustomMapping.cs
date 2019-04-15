using AutoMapper;

namespace Nsiclass.Common.Mapping
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile profile);
    }
}
