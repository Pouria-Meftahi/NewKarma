using AutoMapper;
using NewKarma.Models.Domain;
using NewKarma.Models.View;

namespace NewKarma.Helper
{
    public class Helper : Profile
    {
        public Helper()
        {
            CreateMap<Car, VmCar>();
        }
    }
}
