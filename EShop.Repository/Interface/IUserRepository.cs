using EShop.Domain.DomainModels;
using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository.Interface
{
   public interface IUserRepository
    {
        IEnumerable<EShopApplicationUser> GetAll();

        EShopApplicationUser Get(string id);

        void Insert(EShopApplicationUser entity);

        void Update(EShopApplicationUser entity);

        void Delete(EShopApplicationUser entity);
    }
}
