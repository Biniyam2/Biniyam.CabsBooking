using ApplicationCore.Entites;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PlaceRepository : EFRepository<Places> , IPlacesRepository
    {
        public PlaceRepository(CabsBookingContext dbContext) : base(dbContext)
        {

        }

     
    }
}
