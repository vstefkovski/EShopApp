using EShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();

        Order GetOrderDetails(BaseEntity model);


    }
}
