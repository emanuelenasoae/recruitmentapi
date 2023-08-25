using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace RecruitmentAppTests.RepositoriesIntegrationTests
{
    public class TestBase : IDisposable
    {
        public readonly RecruitmentContext dbContext;
        public readonly IUnitOfWork unitOfWork;

        public TestBase()
        {
            dbContext = new RecruitmentContext();
            unitOfWork = new UnitOfWork(dbContext);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
