using FitUp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Interface
{
    public interface UserRepoInterface
    {
        IEnumerable<FitUpApplicationUser> GetAll();
        FitUpApplicationUser GetById(string id);
        void Insert(FitUpApplicationUser user);
        void Update(FitUpApplicationUser user);
        void Delete(FitUpApplicationUser user);
    }
}
