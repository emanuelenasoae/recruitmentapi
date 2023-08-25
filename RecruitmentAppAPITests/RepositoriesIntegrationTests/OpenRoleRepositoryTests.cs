using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Frameworks;
using RecruitmentApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RecruitmentAppTests.RepositoriesIntegrationTests
{
    public class OpenRoleRepositoryTests : IClassFixture<TestBase>, IDisposable
    {
        private TestBase _testBase;
        private readonly IDbContextTransaction _dbContextTransaction;

        public OpenRoleRepositoryTests(TestBase testBase)
        {
            _testBase = testBase;
            _dbContextTransaction = _testBase.dbContext.Database.BeginTransaction();
        }

        [Fact]
        public async Task CreateOpenRole_Success()
        {
            //Arrange
            var role = createOpenRole();

            //Act
            _testBase.unitOfWork.OpenRoles.Add(role);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one open role to be added to the db context");
            var createdRole = _testBase.unitOfWork.OpenRoles.GetOpenRoleByIdAsync(role.RoleId);
            Assert.NotNull(createdRole);
        }

        [Fact]
        public async Task RemoveOpenRole_Success()
        {
            //Arrage
            var role = createOpenRole();
            _testBase.unitOfWork.OpenRoles.Add(role);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            _testBase.unitOfWork.OpenRoles.Remove(role);
            int count = await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            Assert.True(count == 1, "Expected one entry to be removed");
            var checkRemoved = await _testBase.unitOfWork.OpenRoles.GetOpenRoleByIdAsync(role.RoleId);
            Assert.Null(checkRemoved);
        }

        [Fact]
        public async Task UpdateOpenRole_Success()
        {
            //Arrange
            var role = createOpenRole();
            _testBase.unitOfWork.OpenRoles.Add(role);
            await _testBase.unitOfWork.SaveChangesAsync();
            // Detach the existing open role entity from the RecruitmentContext so that we can update the new one
            _testBase.dbContext.Entry(role).State = EntityState.Detached;
            OpenRole roleUpdate = new OpenRole()
            {
                RoleId = role.RoleId,
                RoleTitle = "UpdatedOpenRole",
                Seniority = "Senior",
                StartDate = new DateTime(year: 2022, month: DateTime.Now.Month, day: DateTime.Now.Day)
            };

            //Act
            _testBase.unitOfWork.OpenRoles.Update(roleUpdate);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Assert
            var roleNewDetails = await _testBase.unitOfWork.OpenRoles.GetOpenRoleByIdAsync(roleUpdate.RoleId);
            Assert.NotNull(roleNewDetails);
            Assert.Equal(roleNewDetails.RoleTitle, roleUpdate.RoleTitle);
            Assert.Equal(roleNewDetails.Seniority, roleUpdate.Seniority);
            Assert.Equal(roleNewDetails.StartDate, roleUpdate.StartDate);
        }

        [Fact]
        public async Task CreateOpenRole_Fail()
        {
            //Arrange
            var openRole = createOpenRole();
            openRole.RoleId = 999999;
            var openRoleDuplicate = createOpenRole();
            openRoleDuplicate.RoleId = 999999;

            //Act
            _testBase.unitOfWork.OpenRoles.Add(openRole);
            int countInitial = await _testBase.unitOfWork.SaveChangesAsync();
            int countDuplicate;
            try
            {
                _testBase.unitOfWork.OpenRoles.Add(openRoleDuplicate);
                countDuplicate = 1;
            }
            catch (InvalidOperationException)
            {
                countDuplicate = 0;
            }

            //Assert
            Assert.True(countInitial == 1, "First entry needs to reach the db context so that the second entry can validate primary key constraint");
            Assert.True(countDuplicate == 0, "Second entry should not reach the db context since it violates primary key constraint");

        }

        [Fact]
        public async Task GetOpenRoleByExpression()
        {
            //Arrange
            var openRole = createOpenRole();
            _testBase.unitOfWork.OpenRoles.Add(openRole);
            await _testBase.unitOfWork.SaveChangesAsync();

            //Act
            var roleRetrieved = await _testBase.unitOfWork.OpenRoles.FindByAsync(r =>
            r.RoleTitle == openRole.RoleTitle &&
            r.Seniority == openRole.Seniority &&
            r.StartDate!.Value.Year == openRole.StartDate!.Value.Year);

            //Assert
            Assert.NotNull(roleRetrieved);
            Assert.True(roleRetrieved.Equals(openRole), "Couldn't retrieve the open role by expression");
        }

        private OpenRole createOpenRole()
        {
            var openRole = new OpenRole()
            {
                RoleTitle = "OpenRole Test",
                Seniority = "Integration unit test",
                StartDate = new DateTime(year: 9999, month: 6, day: 20)
            };
            return openRole;
        }

        public void Dispose()
        {
            _dbContextTransaction.Rollback();
            _testBase.dbContext.ChangeTracker.Clear();
            _dbContextTransaction.Dispose();
        }
    }
}
