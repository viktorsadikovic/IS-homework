using Domain.DomainModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IProjectionService
    {
        List<Projection> GetAllProjections();
        Projection GetDetailsForProjection(Guid? id);
        void CreateNewProjection(MovieProjectionViewModel p);
        void UpdeteExistingProjection(Projection p);
        void DeleteProjection(Guid id);

    }
}
